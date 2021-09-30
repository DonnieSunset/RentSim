using Processing.Assets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Processing.Withdrawal
{
    public class UniformWithdrawalStrategy : IWithdrawalStrategy
    {
        private Portfolio portfolio;

        public UniformWithdrawalStrategy(Portfolio basePortfolio)
        {
            portfolio = basePortfolio;
        }

        /// <summary>
        /// Returns the amount of taxes that must be paid after the withdrawal of a 
        /// given amount from the total portfolio.
        /// </summary>
        /// <remarks>
        /// TODO: At the moment the total capital is calculated from the last protocol
        /// entry. it would be more safe to explicitely select the protocol entry at year stopwork
        /// or at least throw an exception if last entry != stopWorkage.
        /// </remarks>
        /// <param name="amount">The amount to be withdrawn from the total portfolio.</param>
        /// <returns>The amount of taxes to be paid.</returns>
        public double SimulateTaxesAtWithdrawal(int age, double amount)
        {
            List<Asset> assets = portfolio.GetAssets();
            double completeTaxesToPay = 0;
            double completeAssetFractions = 0;

            assets.ForEach((a) =>
            {
                double assetFraction = portfolio.GetAssetFraction(age, a.GetType());
                completeAssetFractions += assetFraction;
                
                if (assetFraction < 0 || assetFraction > 1)
                {
                    throw new Exception($"Asset fraction is <{assetFraction}> but should be between 0 and 1.");
                }

                if (a is IMustPayTaxesAfterWithdrawal)
                {
                    double taxes = (a as IMustPayTaxesAfterWithdrawal).GetTaxesAfterWithdrawal(amount * assetFraction);
                    completeTaxesToPay += taxes;
                }
            });

            if (1 - completeAssetFractions > 0.0001)
            {
                throw new Exception($"Asset fractions sum up to <{completeAssetFractions}> but should sum up to 1.");
            }

            return completeTaxesToPay;
        }

        public double GetWithdrawalAmount(int age)
        {
            double averageGrowthRate = portfolio.GetAverageGrowthRate(age);

            int index = age - portfolio.Input.ageCurrent;
            double totalSavingStopWorkAge = portfolio.Total.protocol[index].yearBegin;

            double approxStopWorkAgeNetRent = RentSimMath.RentStopWorkAgeApproximation(
                portfolio.Input.ageCurrent,
                portfolio.Input.ageStopWork,
                portfolio.Input.ageRentStart,
                portfolio.Input.netStateRentFromCurrentAge,
                portfolio.Input.netStateRentFromRentStartAge);

            Func<double, double> localSimulateTaxesAtWithdrawal = (double amount) => portfolio.WithdrawalStrategy.SimulateTaxesAtWithdrawal(age, amount); 

            (double ratePhaseRent, double ratePhaseStopWork) = SparkassenFormel.CalculatePayoutRateWithRent(
                startCapital: totalSavingStopWorkAge,
                yearsStopWorkPhase: portfolio.Input.ageRentStart - portfolio.Input.ageStopWork,
                yearsRentPhase: portfolio.Input.ageEnd - portfolio.Input.ageRentStart,
                interestRate: averageGrowthRate,
                endCapital: 0,
                rent: approxStopWorkAgeNetRent,
                calcTaxes: localSimulateTaxesAtWithdrawal
            );

            if (age < portfolio.Input.ageStopWork || age >= portfolio.Input.ageEnd)
            {
                throw new Exception($"Age <{age}> not valid. Must be between StopWorkAge (<{portfolio.Input.ageStopWork}>) and EndAge (<{portfolio.Input.ageEnd}>).");
            }
            else if (age < portfolio.Input.ageRentStart)
            {
                return ratePhaseStopWork * 12;
            }
            else
            {
                return ratePhaseRent * 12;
            }
        }

        public double GetWithdrawalAmount(int age, Type assetType)
        {
            double fraction = portfolio.GetAssetFraction(age, assetType);
            double amount = GetWithdrawalAmount(age);

            return fraction * amount;
        }
    }
}
