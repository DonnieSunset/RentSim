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
            decimal currentCapitalCash = input.StartCapitalCash.Total;
            decimal currentCapitalStocks = input.StartCapitalStocks.Total;
            decimal currentCapitalMetals = input.StartCapitalMetals.Total;

            result.FirstYearBeginValues = new SavingPhaseServiceResultDTO.AssetsDTO
            {
                Cash= currentCapitalCash,
                Stocks= currentCapitalStocks,
                Metals= currentCapitalMetals
            };

            for (int age = input.AgeFrom; age < input.AgeTo; age++)
            {
                // add savings
                decimal savingsCash = input.SaveAmountPerMonthCash * 12;
                decimal savingsStocks = input.SaveAmountPerMonthStocks * 12;
                decimal savingsMetals = input.SaveAmountPerMonthMetals * 12;
                currentCapitalCash += savingsCash;
                currentCapitalStocks += savingsStocks;
                currentCapitalMetals += savingsMetals;

                // get interests
                decimal interestsCash = currentCapitalCash * input.GrowthRateCash / 100m;
                decimal interestsStocks = currentCapitalStocks * input.GrowthRateStocks / 100m;
                decimal interestsMetals = currentCapitalMetals * input.GrowthRateMetals / 100m;
                currentCapitalCash += interestsCash;
                currentCapitalStocks += interestsStocks;
                currentCapitalMetals += interestsMetals;

                // pay taxes for cash
                decimal taxes = 0;
                decimal taxRelevantInterests = interestsCash - TAX_FREE_AMOUNT_PER_YEAR;
                if (taxRelevantInterests > 0)
                { 
                    taxes = taxRelevantInterests * CAPITAL_YIELDS_TAX_FACTOR;
                    taxes = -taxes;
                }
                currentCapitalCash += taxes;

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
            result.FinalSavingsCash.FromDeposits = currentCapitalCash;
            result.FinalSavingsStocks.FromDeposits = currentCapitalStocks;
            result.FinalSavingsMetals.FromDeposits = currentCapitalMetals;

            return result;
        }
    }
}
