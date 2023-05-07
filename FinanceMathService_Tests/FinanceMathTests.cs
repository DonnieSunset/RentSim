using FinanceMathService.DTOs;
using NUnit.Framework;
using FinanceMathService;
using Protocol;
using Domain;

namespace FinanceMathService_Tests
{
    [TestFixture]
    public class FinanceMathTests
    {
        const double Tolerance = 0.001;

        [Test]
        public void StartCapitalByNumericalSparkassenformel_DefaultInput_SingleAssetsResultToZero()
        {
            var fMath = new FinanceMath();
            var proto = new MemoryProtocolWriter();
            var input = new StartCapitalByNumericalSparkassenformelInputDTO()
            {
                AgeFrom = 67,
                AgeTo = 80,

                GrowthRateCash = 0,
                GrowthRateMetals = 1,
                GrowthRateStocks = 5,

                StartCapitalCash = new FinanceMathService.DTOs.CAmount { FromDeposits = 44000 },
                StartCapitalStocks = new FinanceMathService.DTOs.CAmount { FromDeposits = 399073, FromInterests = 401978.61m },
                StartCapitalMetals = new FinanceMathService.DTOs.CAmount { FromDeposits = 21200, FromInterests = 4668.03m },

                TotalRateNeeded_PerYear = -51276.5878325744m
            };
            
            SimulationResultDTO result = fMath.StartCapitalByNumericalSparkassenformel(input);
            LogRentPhaseResult(result, proto);

            Assert.That(proto.Protocol.Last().Age, Is.EqualTo(input.AgeTo-1));
            Assert.Multiple(() =>
            {
                Assert.That(proto.Protocol.Last().Cash.YearEnd, Is.EqualTo(0).Within(Tolerance), "Cash YearEnd.");
                Assert.That(proto.Protocol.Last().Stocks.YearEnd, Is.EqualTo(0).Within(Tolerance), "Stocks YearEnd.");
                Assert.That(proto.Protocol.Last().Metals.YearEnd, Is.EqualTo(0).Within(Tolerance), "Metals YearEnd.");
            });
        }


        private void LogRentPhaseResult(SimulationResultDTO result, IProtocolWriter protocolWriter)
        {
            var firstEntry = result.Entities.First();
            protocolWriter.LogBalanceYearBegin(firstEntry.Age, result.FirstYearBeginValues.Cash, result.FirstYearBeginValues.Stocks, result.FirstYearBeginValues.Metals);

            foreach (var entity in result.Entities)
            {
                protocolWriter.Log(entity.Age, new TransactionDetails { cashDeposit = entity.Deposits.Cash, cashInterests = entity.Interests.Cash, cashTaxes = entity.Taxes.Cash });
                protocolWriter.Log(entity.Age, new TransactionDetails { stockDeposit = entity.Deposits.Stocks, stockInterests = entity.Interests.Stocks, stockTaxes = entity.Taxes.Stocks });
                protocolWriter.Log(entity.Age, new TransactionDetails { metalDeposit = entity.Deposits.Metals, metalInterests = entity.Interests.Metals, metalTaxes = entity.Taxes.Metals });
            }
        }
    }
}
