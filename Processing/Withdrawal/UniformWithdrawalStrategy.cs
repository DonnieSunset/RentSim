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
        /// TODO: carve out calculation of asset fractions.
        /// TODO: At the moment the total capital is calculated from the last protocol
        /// entry. it would be more safe to explicitely select the protocol entry at year stopwork
        /// or at least throw an exception if last entry != stopWorkage.
        /// </remarks>
        /// <param name="amount">The amount to be withdrawn from the total portfolio.</param>
        /// <returns>The amount of taxes to be paid.</returns>
        public double SimulateTaxesAtWithdrawal(double amount)
        {
            List<Asset> assets = portfolio.GetAssets();
            double completeTaxesToPay = 0;
            double completeAssetFractions = 0;

            double totalCapital = assets.Sum(a => a.protocol.Last().yearEnd);

            assets.ForEach((a) =>
            {
                double assetFraction = a.protocol.Last().yearEnd / totalCapital;
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
            return 1000;
        }
    }
}
