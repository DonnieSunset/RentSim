using Processing.Assets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Processing.Withdrawal
{
    //public class UniformWithdrawalStrategy : IWithdrawalStrategy
    //{
    //    private Portfolio portfolio;
    //    private WithdrawalRateInfo result;

    //    public double adder;

    //    public UniformWithdrawalStrategy(Portfolio basePortfolio)
    //    {
    //        portfolio = basePortfolio;
    //    }

    //    public void Adder(double amount)
    //    {
    //        adder = amount;
    //    }

    //    public void Calculate()
    //    {
    //        result = new WithdrawalRateInfo();

    //        int index = portfolio.Input.ageStopWork - portfolio.Input.ageCurrent;
    //        if (portfolio.Cash.Protocol.Count - 1 != index) //substract - 1 because we always create a new (empty) protocol entry at the end of the list
    //        {
    //            throw new Exception($"{nameof(Calculate)} failed. Number of portfolio entries <{portfolio.Cash.Protocol.Count}> seems to be not matching for age stop work <{portfolio.Input.ageStopWork}> with index <{index}>.");
    //        }

    //        (result.Cash.RateRentStartGross, result.Cash.RateStopWorkGross) = GetWithdrawalAmount(typeof(Cash));
    //        result.Cash.RateRentStartNet = result.Cash.RateRentStartGross;
    //        result.Cash.RateStopWorkNet = result.Cash.RateStopWorkGross;

    //        (result.Stocks.RateRentStartGross, result.Stocks.RateStopWorkGross) = GetWithdrawalAmount(typeof(Stocks));
    //        result.Stocks.RateRentStartNet = result.Stocks.RateRentStartGross - portfolio.Stocks.GetTaxesAfterWithdrawal(result.Stocks.RateRentStartGross);
    //        result.Stocks.RateStopWorkNet = result.Stocks.RateStopWorkGross - portfolio.Stocks.GetTaxesAfterWithdrawal(result.Stocks.RateStopWorkGross); ;

    //        (result.Metals.RateRentStartGross, result.Metals.RateStopWorkGross) = GetWithdrawalAmount(typeof(Metals));
    //        result.Metals.RateRentStartNet = result.Metals.RateRentStartGross;
    //        result.Metals.RateStopWorkNet = result.Metals.RateStopWorkGross;

    //        result.Total.RateRentStartGross =
    //            result.Cash.RateRentStartGross +
    //            result.Stocks.RateRentStartGross +
    //            result.Metals.RateRentStartGross;

    //        result.Total.RateRentStartNet =
    //            result.Cash.RateRentStartNet +
    //            result.Stocks.RateRentStartNet +
    //            result.Metals.RateRentStartNet;

    //        result.Total.RateStopWorkGross =
    //            result.Cash.RateStopWorkGross +
    //            result.Stocks.RateStopWorkGross +
    //            result.Metals.RateStopWorkGross;

    //        result.Total.RateStopWorkNet =
    //            result.Cash.RateStopWorkNet +
    //            result.Stocks.RateStopWorkNet +
    //            result.Metals.RateStopWorkNet;
    //    }

    //    public WithdrawalRateInfo GetResults()
    //    {
    //        if (result == null)
    //        {
    //            throw new Exception($"Withdrawal result not yet calculated. Please call {nameof(Calculate)} first.");
    //        }

    //        return result;
    //    }

    //    /// <summary>
    //    /// Returns the amount of taxes that must be paid after the withdrawal of a 
    //    /// given amount from the total portfolio.
    //    /// </summary>
    //    /// <remarks>
    //    /// TODO: At the moment the total capital is calculated from the last protocol
    //    /// entry. it would be more safe to explicitely select the protocol entry at year stopwork
    //    /// or at least throw an exception if last entry != stopWorkage.
    //    /// </remarks>
    //    /// <param name="amount">The amount to be withdrawn from the total portfolio.</param>
    //    /// <returns>The amount of taxes to be paid.</returns>
    //    internal double SimulateTaxesAtWithdrawal(double amount)
    //    {
    //        List<Asset> assets = portfolio.GetAssets();
    //        double completeTaxesToPay = 0;
    //        double completeAssetFractions = 0;
    //        AgePhase agePhase = AgePhaseBy.Age(portfolio.Input.ageStopWork, portfolio.Input);

    //        assets.ForEach((a) =>
    //        {
    //            double assetFraction = portfolio.GetAssetFraction(agePhase, a.GetType());
    //            completeAssetFractions += assetFraction;
                
    //            if (assetFraction < 0 || assetFraction > 1)
    //            {
    //                throw new Exception($"Asset fraction is <{assetFraction}> but should be between 0 and 1.");
    //            }

    //            if (a is IMustPayTaxesAfterWithdrawal)
    //            {
    //                double taxes = (a as IMustPayTaxesAfterWithdrawal).GetTaxesAfterWithdrawal(amount * assetFraction);
    //                completeTaxesToPay += taxes;
    //            }
    //        });

    //        if (1 - completeAssetFractions > 0.0001)
    //        {
    //            throw new Exception($"Asset fractions sum up to <{completeAssetFractions}> but should sum up to 1.");
    //        }

    //        return completeTaxesToPay;
    //    }

    //    //todo: this is called now 3 times,but should be called only 1 time.
    //    internal (double ratePhaseRent, double ratePhaseStopWork) GetWithdrawalAmount()
    //    {
    //        double averageGrowthRate = portfolio.GetAverageGrowthRate(AgePhaseBy.Age(portfolio.Input.ageStopWork, portfolio.Input));

            
    //        ////////////////// only for debugging //////
    //        averageGrowthRate = 0;
    //        ////////////////// only for debugging //////
            

    //        int index = portfolio.Input.ageStopWork - portfolio.Input.ageCurrent;
    //        double totalSavingStopWorkAge = portfolio.Total.Protocol[index].yearEnd;

    //        double approxStopWorkAgeNetRent = RentSimMath.RentStopWorkAgeApproximation(
    //            portfolio.Input.ageCurrent,
    //            portfolio.Input.ageStopWork,
    //            portfolio.Input.ageRentStart,
    //            portfolio.Input.netStateRentFromCurrentAge,
    //            portfolio.Input.netStateRentFromRentStartAge);

    //        Func<double, double> localSimulateTaxesAtWithdrawal = (double amount) => SimulateTaxesAtWithdrawal(amount);

    //        (double ratePhaseRent, double ratePhaseStopWork) = SparkassenFormel.CalculateGrossPayoutRateWithRent(
    //            startCapital: totalSavingStopWorkAge,
    //            yearsStopWorkPhase: portfolio.Input.ageRentStart - portfolio.Input.ageStopWork,
    //            yearsRentPhase: portfolio.Input.ageEnd - portfolio.Input.ageRentStart,
    //            interestRate: averageGrowthRate,
    //            endCapital: 0,
    //            rent: approxStopWorkAgeNetRent,
    //            calcTaxes: localSimulateTaxesAtWithdrawal
    //        );

    //        ////////////////// only for debugging //////
    //        ratePhaseRent -= this.adder;
    //        ratePhaseStopWork -= this.adder;
    //        ///////////////////////////////////////

    //        return (-ratePhaseRent, -ratePhaseStopWork);
    //    }

    //    internal (double ratePhaseRent, double ratePhaseStopWork) GetWithdrawalAmount(Type assetType)
    //    {
    //        // The withdrawal amount must be calculated based on an age which does not change anymore,
    //        // otherwise there will be inconsistencies becaue the values of one asset could be already altered
    //        // which the values of another asset is yet to be processed!
    //        // so we choose input.ageStopWork-1 here
    //        // for uniform withdrawal strategy, the age of withdrawal does not matter. 
    //        // we choose here always the stopwork age - 1 and calculate it from yearEnd

    //        double fraction = portfolio.GetAssetFraction(AgePhase.StopWork, assetType);
    //        (double ratePhaseRent, double ratePhaseStopWork) = GetWithdrawalAmount();

    //        return (fraction * ratePhaseRent, fraction * ratePhaseStopWork);
    //    }
    //}
}
