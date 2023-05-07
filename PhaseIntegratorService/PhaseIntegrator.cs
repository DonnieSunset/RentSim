using Protocol;
using PhaseIntegratorService.DTOs;
using PhaseIntegratorService.Clients;
using Domain;
using Microsoft.Extensions.DependencyInjection;

namespace PhaseIntegratorService
{
    public class PhaseIntegrator : IPhaseIntegrator
    {
        //LifeAssumptions lifeAssumptions;

        //IFinanceMathClient myFinanceMathClient;
        //ISavingPhaseClient mySavingPhaseClient;
        //IRentPhaseClient myRentPhaseClient;
        //IStopWorkPhaseClient myStopWorkPhaseClient;

        public PhaseIntegrator(/*LifeAssumptions lifeAssumptions*/)
        {
            //lifeAssumptions = lifeAssumptions;

            //myFinanceMathClient = serviceProvider.GetService<IFinanceMathClient>();
            //mySavingPhaseClient = serviceProvider.GetService<ISavingPhaseClient>();
            //myRentPhaseClient = serviceProvider.GetService<IRentPhaseClient>();
            //myStopWorkPhaseClient = serviceProvider.GetService<IStopWorkPhaseClient>();
        }

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

                StartCapitalCash = new CAmount(lifeAssumptions.cash),
                StartCapitalStocks = new CAmount(lifeAssumptions.stocks),
                StartCapitalMetals = new CAmount(lifeAssumptions.metals),

                GrowthRateCash = lifeAssumptions.cashGrowthRate,
                GrowthRateStocks = lifeAssumptions.stocksGrowthRate,
                GrowthRateMetals = lifeAssumptions.metalsGrowthRate,

                SaveAmountPerMonthCash = lifeAssumptions.cashSaveAmountPerMonth,
                SaveAmountPerMonthStocks = lifeAssumptions.stocksSaveAmountPerMonth,
                SaveAmountPerMonthMetals = lifeAssumptions.metalsSaveAmountPerMonth,
            };

            var savingPhaseResult = await savingPhaseClient.GetSavingPhaseSimulationAsync(savingPhaseInput);
            savingPhaseClient.LogSavingPhaseResult(savingPhaseResult, protocolWriter);
            //result_totalSavings = savingPhaseResult.FinalSavings;
            result.SavingPhaseServiceResult = savingPhaseResult;

            // Later Needs
            var laterNeedsResult = new LaterNeedsResultDTO
            {
                NeedsMinimum_AgeStopWork_WithInflation_PerMonth = await financeMathClient.GetAmountWithInflationAsync(lifeAssumptions.ageCurrent, ageStopWork, lifeAssumptions.needsCurrentAgeMinimal_perMonth, lifeAssumptions.inflationRate),
                NeedsComfort_AgeStopWork_WithInflation_PerMonth = await financeMathClient.GetAmountWithInflationAsync(lifeAssumptions.ageCurrent, ageStopWork, lifeAssumptions.needsCurrentAgeComfort_perMonth, lifeAssumptions.inflationRate),
                NeedsMinimum_AgeRentStart_WithInflation_PerMonth = await financeMathClient.GetAmountWithInflationAsync(lifeAssumptions.ageCurrent, lifeAssumptions.ageRentStart, lifeAssumptions.needsCurrentAgeMinimal_perMonth, lifeAssumptions.inflationRate),
                NeedsComfort_AgeRentStart_WithInflation_PerMonth = await financeMathClient.GetAmountWithInflationAsync(lifeAssumptions.ageCurrent, lifeAssumptions.ageRentStart, lifeAssumptions.needsCurrentAgeComfort_perMonth, lifeAssumptions.inflationRate)
            };
            result.LaterNeedsResult = laterNeedsResult;

            // State Rent
            var stateRentResult = new StateRentResultDTO
            {
                AssumedStateRent_Net_PerMonth = await rentPhaseClient.ApproxStateRent(lifeAssumptions.ageCurrent, lifeAssumptions.netStateRentFromCurrentAge_perMonth, lifeAssumptions.ageRentStart, lifeAssumptions.netStateRentFromRentStartAge_perMonth, ageStopWork),
                AssumedStateRent_Gross_PerMonth = await rentPhaseClient.ApproxStateRent(lifeAssumptions.ageCurrent, lifeAssumptions.grossStateRentFromCurrentAge_perMonth, lifeAssumptions.ageRentStart, lifeAssumptions.grossStateRentFromRentStartAge_perMonth, ageStopWork),
            };
            //rentAtStopWork_gross = stateRentResult.assumedStateRent_Gross_PerMonth;
            //rentAtStopWork_net = stateRentResult.assumedStateRent_Net_PerMonth;
            result.StateRentResult = stateRentResult;

            // Rent Phase
            var rentPhaseInput = new RentPhaseServiceInputDTO()
            {
                AgeFrom = lifeAssumptions.ageRentStart,
                AgeTo = lifeAssumptions.ageEnd,

                StartCapitalCash = new CAmount(savingPhaseResult.FinalSavingsCash),
                StartCapitalStocks = new CAmount(savingPhaseResult.FinalSavingsStocks),
                StartCapitalMetals = new CAmount(savingPhaseResult.FinalSavingsMetals),

                GrowthRateCash = lifeAssumptions.cashGrowthRate,
                GrowthRateStocks = lifeAssumptions.stocksGrowthRate,
                GrowthRateMetals = lifeAssumptions.metalsGrowthRate,

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

                GrowthRateCash = lifeAssumptions.cashGrowthRate,
                GrowthRateStocks = lifeAssumptions.stocksGrowthRate,
                GrowthRateMetals = lifeAssumptions.metalsGrowthRate,

                EndCapitalTotal = rentStartAmount
            };

            var stopWorkPhaseResult = await stopWorkPhaseClient.GetStopWorkPhaseSimulationAsync(stopWorkPhaseInput);
            stopWorkPhaseClient.LogStopWorkPhaseResult(stopWorkPhaseResult, protocolWriter);

            //result_MonthlyRateStopWorkPhase = protocolWriter.Protocol.Single(x => x.Age == ageStopWork).TotalDeposits / 12m;
            //result_MonthlyRateRentPhase = protocolWriter.Protocol.Single(x => x.Age == lifeAssumptions.ageRentStart).TotalDeposits / 12m;

            // Re-balancing after Stop-Work phase

            var lastStopWorkPhase = protocolWriter.Protocol.Single(x => x.Age == lifeAssumptions.ageRentStart - 1);
            var firstRentPhase = protocolWriter.Protocol.Single(x => x.Age == lifeAssumptions.ageRentStart);

            var cashDiff = lastStopWorkPhase.Cash.YearEnd - firstRentPhase.Cash.YearBegin;
            protocolWriter.Log(lifeAssumptions.ageRentStart - 1, new TransactionDetails { cashDeposit = -cashDiff });

            var stocksDiff = lastStopWorkPhase.Stocks.YearEnd - firstRentPhase.Stocks.YearBegin;
            protocolWriter.Log(lifeAssumptions.ageRentStart - 1, new TransactionDetails { stockDeposit = -stocksDiff });

            var metalsDiff = lastStopWorkPhase.Metals.YearEnd - firstRentPhase.Metals.YearBegin;
            protocolWriter.Log(lifeAssumptions.ageRentStart - 1, new TransactionDetails { metalDeposit = -metalsDiff });

            //protocolWriter.RecalcYearBeginEntries();

            // validation

            //resultRows = protocolWriter.Protocol;

            ResultRowValidator.ValidateAll(protocolWriter.Protocol, lifeAssumptions.ageCurrent, ageStopWork, lifeAssumptions.ageEnd);

            result.Protocol = protocolWriter.Protocol;

            return result;
        }
    }
}
