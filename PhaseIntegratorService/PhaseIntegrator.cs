using Domain;
using PhaseIntegratorService.Clients;
using PhaseIntegratorService.DTOs;
using Protocol;

namespace PhaseIntegratorService
{
    public class PhaseIntegrator : IPhaseIntegrator
    {
        public async Task<PhaseIntegratorServiceResultDTO> SimulateGoodCase(
            LifeAssumptions lifeAssumptions,
            IFinanceMathClient financeMathClient,
            ISavingPhaseClient savingPhaseClient,
            IStopWorkPhaseClient stopWorkPhaseClient,
            IRentPhaseClient rentPhaseClient
            )
        { 
            for (int assumedStopWorkAge = lifeAssumptions.ageCurrent; assumedStopWorkAge <= lifeAssumptions.ageRentStart; assumedStopWorkAge++) 
            {
                try
                {
                    PhaseIntegratorServiceResultDTO result = await SimulateGoodCaseWithAssumedStopWorkAge(
                        lifeAssumptions,
                        assumedStopWorkAge,
                        financeMathClient,
                        savingPhaseClient,
                        stopWorkPhaseClient,
                        rentPhaseClient
                        );

                    if (result.Result.Type == ResultDTO.ResultType.Success 
                        && result.StopWorkPhaseServiceResult.MonthlyDepositRate >= result.LaterNeedsResult.NeedsComfort_AgeStopWork_WithInflation_PerMonth
                        )
                    {
                        return result;
                    }
                }
                catch
                { }
            }

            var badResult = new PhaseIntegratorServiceResultDTO();
            badResult.Result.Type = ResultDTO.ResultType.Failure;
            badResult.Result.Message = "Unable to calculate stopWorkAge.";

            return badResult;
        }

        public async Task<PhaseIntegratorServiceResultDTO> SimulateGoodCaseWithAssumedStopWorkAge(
            LifeAssumptions lifeAssumptions,
            int assumedStopWorkAge,
            IFinanceMathClient financeMathClient,
            ISavingPhaseClient savingPhaseClient,
            IStopWorkPhaseClient stopWorkPhaseClient,
            IRentPhaseClient rentPhaseClient
            )
        {
            var result = new PhaseIntegratorServiceResultDTO();
            PhaseIntegratorServiceResultDTO errorResult;

            var protocolWriter = new MemoryProtocolWriter();

            // Saving Phase
            var savingPhaseInput = new SavingPhaseServiceInputDTO
            {
                AgeFrom = lifeAssumptions.ageCurrent,
                AgeTo = assumedStopWorkAge,

                StartCapitalCash = new CAmount(lifeAssumptions.Cash),
                StartCapitalStocks = new CAmount(lifeAssumptions.Stocks),
                StartCapitalMetals = new CAmount(lifeAssumptions.Metals),

                GrowthRateCash = lifeAssumptions.CashGrowthRate,
                GrowthRateStocks = lifeAssumptions.StocksGrowthRate,
                GrowthRateMetals = lifeAssumptions.MetalsGrowthRate,

                SaveAmountPerMonthCash = lifeAssumptions.CashSaveAmountPerMonth,
                SaveAmountPerMonthStocks = lifeAssumptions.StocksSaveAmountPerMonth,
                SaveAmountPerMonthMetals = lifeAssumptions.MetalsSaveAmountPerMonth,
            };

            var savingPhaseResult = await savingPhaseClient.GetSavingPhaseSimulationAsync(savingPhaseInput);
            ThrowIfNotSuccess(savingPhaseResult.Result, "Error in SavingPhase: ");
            result.SavingPhaseServiceResult = savingPhaseResult;
            savingPhaseClient.LogSavingPhaseResult(savingPhaseResult, protocolWriter);

            // Later Needs
            var laterNeedsResult = new LaterNeedsResultDTO
            {
                NeedsMinimum_AgeStopWork_WithInflation_PerMonth = await financeMathClient.GetAmountWithInflationAsync(lifeAssumptions.ageCurrent, assumedStopWorkAge, lifeAssumptions.NeedsCurrentAgeMinimal_perMonth, lifeAssumptions.InflationRate),
                NeedsComfort_AgeStopWork_WithInflation_PerMonth = await financeMathClient.GetAmountWithInflationAsync(lifeAssumptions.ageCurrent, assumedStopWorkAge, lifeAssumptions.NeedsCurrentAgeComfort_perMonth, lifeAssumptions.InflationRate),
                NeedsMinimum_AgeRentStart_WithInflation_PerMonth = await financeMathClient.GetAmountWithInflationAsync(lifeAssumptions.ageCurrent, lifeAssumptions.ageRentStart, lifeAssumptions.NeedsCurrentAgeMinimal_perMonth, lifeAssumptions.InflationRate),
                NeedsComfort_AgeRentStart_WithInflation_PerMonth = await financeMathClient.GetAmountWithInflationAsync(lifeAssumptions.ageCurrent, lifeAssumptions.ageRentStart, lifeAssumptions.NeedsCurrentAgeComfort_perMonth, lifeAssumptions.InflationRate)
            };
            result.LaterNeedsResult = laterNeedsResult;

            // State Rent
            var stateRentResult = new StateRentResultDTO
            {
                AssumedStateRent_Net_PerMonth = await rentPhaseClient.ApproxStateRent(lifeAssumptions.ageCurrent, lifeAssumptions.NetStateRentFromCurrentAge_perMonth, lifeAssumptions.ageRentStart, lifeAssumptions.NetStateRentFromRentStartAge_perMonth, assumedStopWorkAge),
                AssumedStateRent_Gross_PerMonth = await rentPhaseClient.ApproxStateRent(lifeAssumptions.ageCurrent, lifeAssumptions.GrossStateRentFromCurrentAge_perMonth, lifeAssumptions.ageRentStart, lifeAssumptions.GrossStateRentFromRentStartAge_perMonth, assumedStopWorkAge),
            };
            result.StateRentResult = stateRentResult;

            // Rent Phase
            var rentPhaseInput = new RentPhaseServiceInputDTO()
            {
                AgeFrom = lifeAssumptions.ageRentStart,
                AgeTo = lifeAssumptions.ageEnd,

                StartCapitalCash = new CAmount(savingPhaseResult.FinalSavingsCash),
                StartCapitalStocks = new CAmount(savingPhaseResult.FinalSavingsStocks),
                StartCapitalMetals = new CAmount(savingPhaseResult.FinalSavingsMetals),

                GrowthRateCash = lifeAssumptions.CashGrowthRate,
                GrowthRateStocks = lifeAssumptions.StocksGrowthRate,
                GrowthRateMetals = lifeAssumptions.MetalsGrowthRate,

                TotalRateNeeded_PerYear = -(laterNeedsResult.NeedsComfort_AgeRentStart_WithInflation_PerMonth - stateRentResult.AssumedStateRent_Net_PerMonth) * 12
            };

            var rentPhaseResult = await rentPhaseClient.GetRentPhaseSimulationAsync(rentPhaseInput);
            ThrowIfNotSuccess(rentPhaseResult.Result, "Error in RentPhase: ");
            result.RentPhaseServiceResult = rentPhaseResult;
            rentPhaseClient.LogRentPhaseResult(rentPhaseResult, protocolWriter);

            // Stop Work Phase
            decimal rentStartAmount = protocolWriter.Protocol
                .Single(x => x.Age == lifeAssumptions.ageRentStart)
                .TotalYearBegin;

            var stopWorkPhaseInput = new StopWorkPhaseServiceInputDTO
            {
                AgeFrom = assumedStopWorkAge,
                AgeTo = lifeAssumptions.ageRentStart,

                StartCapitalCash = new CAmount(savingPhaseResult.FinalSavingsCash),
                StartCapitalStocks = new CAmount(savingPhaseResult.FinalSavingsStocks),
                StartCapitalMetals = new CAmount(savingPhaseResult.FinalSavingsMetals),

                GrowthRateCash = lifeAssumptions.CashGrowthRate,
                GrowthRateStocks = lifeAssumptions.StocksGrowthRate,
                GrowthRateMetals = lifeAssumptions.MetalsGrowthRate,

                EndCapitalTotal = rentStartAmount
            };

            var stopWorkPhaseResult = await stopWorkPhaseClient.GetStopWorkPhaseSimulationAsync(stopWorkPhaseInput);
            ThrowIfNotSuccess(stopWorkPhaseResult.Result, "Error in StopWorkPhase: ");
            result.StopWorkPhaseServiceResult = stopWorkPhaseResult;
            stopWorkPhaseClient.LogStopWorkPhaseResult(stopWorkPhaseResult, protocolWriter);

            // Re-balancing after Stop-Work phase

            var lastStopWorkPhase = protocolWriter.Protocol.Single(x => x.Age == lifeAssumptions.ageRentStart - 1);
            var firstRentPhase = protocolWriter.Protocol.Single(x => x.Age == lifeAssumptions.ageRentStart);

            var cashDiff = lastStopWorkPhase.Cash.YearEnd - firstRentPhase.Cash.YearBegin;
            protocolWriter.Log(lifeAssumptions.ageRentStart - 1, new TransactionDetails { cashDeposit = -cashDiff });

            var stocksDiff = lastStopWorkPhase.Stocks.YearEnd - firstRentPhase.Stocks.YearBegin;
            protocolWriter.Log(lifeAssumptions.ageRentStart - 1, new TransactionDetails { stockDeposit = -stocksDiff });

            var metalsDiff = lastStopWorkPhase.Metals.YearEnd - firstRentPhase.Metals.YearBegin;
            protocolWriter.Log(lifeAssumptions.ageRentStart - 1, new TransactionDetails { metalDeposit = -metalsDiff });

            // validation
            ResultRowValidator.ValidateAll(protocolWriter.Protocol, lifeAssumptions.ageCurrent, assumedStopWorkAge, lifeAssumptions.ageRentStart, lifeAssumptions.ageEnd);

            result.Protocol = protocolWriter.Protocol;
            result.Result.Type = ResultDTO.ResultType.Success;
            return result;
        }

        public void ThrowIfNotSuccess(ResultDTO result, string errorMsg)
        { 
            if (result.Type != ResultDTO.ResultType.Success) 
            {
                throw new Exception(String.Join(", ", errorMsg, result.Message, result.Details));
            }
        }
    }
}
