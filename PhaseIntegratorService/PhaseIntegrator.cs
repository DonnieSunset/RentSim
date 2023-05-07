using Protocol;
using PhaseIntegratorService.DTOs;
using PhaseIntegratorService.Clients;
using Domain;
using Microsoft.Extensions.DependencyInjection;

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
            //todo: muss raus
            int ageStopWork = 63;

            var result = new PhaseIntegratorServiceResultDTO();
            var protocolWriter = new MemoryProtocolWriter();

            // Saving Phase
            var savingPhaseInput = new SavingPhaseServiceInputDTO
            {
                AgeFrom = lifeAssumptions.ageCurrent,
                AgeTo = ageStopWork,

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
            savingPhaseClient.LogSavingPhaseResult(savingPhaseResult, protocolWriter);

            result.SavingPhaseServiceResult = savingPhaseResult;

            // Later Needs
            var laterNeedsResult = new LaterNeedsResultDTO
            {
                NeedsMinimum_AgeStopWork_WithInflation_PerMonth = await financeMathClient.GetAmountWithInflationAsync(lifeAssumptions.ageCurrent, ageStopWork, lifeAssumptions.NeedsCurrentAgeMinimal_perMonth, lifeAssumptions.InflationRate),
                NeedsComfort_AgeStopWork_WithInflation_PerMonth = await financeMathClient.GetAmountWithInflationAsync(lifeAssumptions.ageCurrent, ageStopWork, lifeAssumptions.NeedsCurrentAgeComfort_perMonth, lifeAssumptions.InflationRate),
                NeedsMinimum_AgeRentStart_WithInflation_PerMonth = await financeMathClient.GetAmountWithInflationAsync(lifeAssumptions.ageCurrent, lifeAssumptions.ageRentStart, lifeAssumptions.NeedsCurrentAgeMinimal_perMonth, lifeAssumptions.InflationRate),
                NeedsComfort_AgeRentStart_WithInflation_PerMonth = await financeMathClient.GetAmountWithInflationAsync(lifeAssumptions.ageCurrent, lifeAssumptions.ageRentStart, lifeAssumptions.NeedsCurrentAgeComfort_perMonth, lifeAssumptions.InflationRate)
            };
            result.LaterNeedsResult = laterNeedsResult;

            // State Rent
            var stateRentResult = new StateRentResultDTO
            {
                AssumedStateRent_Net_PerMonth = await rentPhaseClient.ApproxStateRent(lifeAssumptions.ageCurrent, lifeAssumptions.NetStateRentFromCurrentAge_perMonth, lifeAssumptions.ageRentStart, lifeAssumptions.NetStateRentFromRentStartAge_perMonth, ageStopWork),
                AssumedStateRent_Gross_PerMonth = await rentPhaseClient.ApproxStateRent(lifeAssumptions.ageCurrent, lifeAssumptions.GrossStateRentFromCurrentAge_perMonth, lifeAssumptions.ageRentStart, lifeAssumptions.GrossStateRentFromRentStartAge_perMonth, ageStopWork),
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
            rentPhaseClient.LogRentPhaseResult(rentPhaseResult, protocolWriter);

            // Stop Work Phase
            decimal rentStartAmount = protocolWriter.Protocol
                .Single(x => x.Age == lifeAssumptions.ageRentStart)
                .TotalYearBegin;

            var stopWorkPhaseInput = new StopWorkPhaseServiceInputDTO
            {
                AgeFrom = ageStopWork,
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
            ResultRowValidator.ValidateAll(protocolWriter.Protocol, lifeAssumptions.ageCurrent, ageStopWork, lifeAssumptions.ageEnd);

            result.Protocol = protocolWriter.Protocol;
            return result;
        }
    }
}
