using SavingPhaseService.DTOs;

namespace SavingPhaseService
{
    internal class SavingPhase : ISavingPhase
    {
        public const decimal CAPITAL_YIELDS_TAX_FACTOR = 0.26375m;
        public const decimal TAX_FREE_AMOUNT_PER_YEAR = 1000;

        public SavingPhaseServiceResultDTO Simulate(SavingPhaseServiceInputDTO input)
        {
            SavingPhaseServiceResultDTO result = new();
            CAmount currentCapitalCash = new CAmount(input.StartCapitalCash);
            CAmount currentCapitalStocks = new CAmount(input.StartCapitalStocks);
            CAmount currentCapitalMetals = new CAmount(input.StartCapitalMetals);

            result.FirstYearBeginValues = new SavingPhaseServiceResultDTO.AssetsDTO
            {
                Cash = currentCapitalCash.Total,
                Stocks = currentCapitalStocks.Total,
                Metals = currentCapitalMetals.Total
            };

            for (int age = input.AgeFrom; age < input.AgeTo; age++)
            {
                // add savings
                decimal savingsCash = input.SaveAmountPerMonthCash * 12;
                decimal savingsStocks = input.SaveAmountPerMonthStocks * 12;
                decimal savingsMetals = input.SaveAmountPerMonthMetals * 12;
                currentCapitalCash.FromDeposits += savingsCash;
                currentCapitalStocks.FromDeposits += savingsStocks;
                currentCapitalMetals.FromDeposits += savingsMetals;

                // get interests
                decimal interestsCash = currentCapitalCash.Total * input.GrowthRateCash / 100m;
                decimal interestsStocks = currentCapitalStocks.Total * input.GrowthRateStocks / 100m;
                decimal interestsMetals = currentCapitalMetals.Total * input.GrowthRateMetals / 100m;
                currentCapitalCash.FromInterests += interestsCash;
                currentCapitalStocks.FromInterests += interestsStocks;
                currentCapitalMetals.FromInterests += interestsMetals;

                // todo: duplicate code
                // pay taxes for cash
                decimal taxes = 0;
                decimal taxRelevantInterests = interestsCash - TAX_FREE_AMOUNT_PER_YEAR;
                if (taxRelevantInterests > 0)
                { 
                    taxes = taxRelevantInterests * CAPITAL_YIELDS_TAX_FACTOR;
                    taxes = -taxes;
                }
                currentCapitalCash.DistributeEqually(taxes);
                //currentCapitalCash.FromDeposits += taxes;

                result.Entities.Add(new SavingPhaseServiceResultDTO.Entity
                { 
                    Age = age,

                    Deposits = new SavingPhaseServiceResultDTO.AssetsDTO
                    { 
                        Cash = savingsCash,
                        Stocks = savingsStocks,
                        Metals = savingsMetals,
                    },

                    Interests = new SavingPhaseServiceResultDTO.AssetsDTO
                    {
                        Cash = interestsCash,
                        Stocks = interestsStocks,
                        Metals = interestsMetals,
                    },

                    Taxes = new SavingPhaseServiceResultDTO.AssetsDTO
                    {
                        Cash = taxes,
                        Stocks = 0,
                        Metals = 0,
                    },
                });
            }
            result.FinalSavingsCash = currentCapitalCash;
            result.FinalSavingsStocks = currentCapitalStocks;
            result.FinalSavingsMetals = currentCapitalMetals;

            return result;
        }
    }
}
