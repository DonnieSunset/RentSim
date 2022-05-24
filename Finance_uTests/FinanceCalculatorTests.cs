using Finance;
using NUnit.Framework;

namespace Finance_uTests
{
    [TestFixture]
    public class FinanceCalculatorTests
    {
        //[TestCase(1000, 1000, 70, 35, 35)]
        //[TestCase(1000, 0, 70, 70, 0)]
        //[TestCase(0, 1000, 70, 0, 70)]
        //[TestCase(2000, 1000, 120, 80, 40)]
        //public void WithdrawUniformFromTwoAmounts_TwoValidSets_ReturnsCorrectResults(decimal amount1, decimal amount2, decimal withdrawalAmount, decimal expectedPartAmount1, decimal expectedPartAmount2)
        //{
        //    (decimal result1, decimal result2) = FinanceCalculator.WithdrawUniformFromTwoAmounts(amount1, amount2, withdrawalAmount);

        //    Assert.That(result1, Is.EqualTo(expectedPartAmount1).Within(0.000000000000001));
        //    Assert.That(result2, Is.EqualTo(expectedPartAmount2).Within(0.000000000000001));
        //}

        [Test]
        public void Blubb()
        {
            double stocks_interestRate_goodCase = 1.08;
            //double cash_interestRate_goodCase = 1.01;
            double stocks_interestRate_badCase = 1.1;
            //double cash_interestRate_badCase = 1.001;
            double cash_interestRate = 1.001;
            int years = 13;

            double stocks_crashFactor_badCase = 0.5;

            double comfort_total_needed_Year = 3000 * 12;
            double minimum_total_needed_Year = 2000 * 12;

            double minimum_total_needed = minimum_total_needed_Year * years;
            double comfort_total_needed = comfort_total_needed_Year * years;



            //double rate

            double rent_MONTH = 1690;
            double rent_YEAR = rent_MONTH * 12;
            double rent_TOTAL = rent_YEAR * years;

            //double cash_needed;
            //double stocks_needed;

            //double stocks_interestFactor_goodCase = Math.Pow(stocks_interestRate_goodCase, 13);
            //double cash_interestFactor_goodCase = Math.Pow(cash_interestRate_goodCase, 13);
            //double stocks_interestFactor_badCase = Math.Pow(stocks_interestRate_badCase, 13);
            //double cash_interestFactor_badCase = Math.Pow(cash_interestRate_badCase, 13);

            //(1) 3000 = rente + (festgeld * 1.00^13) + (aktien * 1.00^13 * 0.5)
            //(2) 5000 = rente + (festgeld * 1.01^13) + (aktien * 1.08^13)

            //(1) minimum_total_needed = rent + (cash_needed * cash_interestFactor_badCase) + (stocks_needed * stocks_interestFactor_badCase * stocks_crashFactor_badCase)
            //(2) comfort_total_needed = rent + (cash_needed * cash_interestFactor_goodCase) + (stocks_needed * stocks_interestFactor_goodCase)

            // py https://live.sympy.org/
            // minimum_total_needed, rent, cash_needed, cash_interestFactor_badCase, stocks_needed, stocks_interestFactor_badCase, stocks_crashFactor_badCase, comfort_total_needed, cash_interestFactor_goodCase, stocks_interestFactor_goodCase = symbols('minimum_total_needed rent cash_needed cash_interestFactor_badCase stocks_needed stocks_interestFactor_badCase stocks_crashFactor_badCase comfort_total_needed cash_interestFactor_goodCase stocks_interestFactor_goodCase')
            // eq1 = Eq(rent + (cash_needed * cash_interestFactor_badCase) + (stocks_needed * stocks_interestFactor_badCase * stocks_crashFactor_badCase), minimum_total_needed)
            // eq2 = Eq(rent + (cash_needed * cash_interestFactor_goodCase) + (stocks_needed * stocks_interestFactor_goodCase), comfort_total_needed)
            // sol = solve((eq1, eq2),(cash_needed, stocks_needed))
            //
            // 

            //cash_needed = (-stocks_crashFactor_badCase * stocks_interestFactor_badCase * (comfort_total_needed - rent_TOTAL) + stocks_interestFactor_goodCase * (minimum_total_needed - rent_TOTAL)) / (cash_interestFactor_badCase * stocks_interestFactor_goodCase - cash_interestFactor_goodCase * stocks_crashFactor_badCase * stocks_interestFactor_badCase);
            //stocks_needed = (cash_interestFactor_badCase*(comfort_total_needed - rent_TOTAL) - cash_interestFactor_goodCase*(minimum_total_needed - rent_TOTAL)) / (cash_interestFactor_badCase*stocks_interestFactor_goodCase - cash_interestFactor_goodCase*stocks_crashFactor_badCase*stocks_interestFactor_badCase);

            


            //Console.WriteLine($"cash_needed: {cash_needed}");
            //Console.WriteLine($"stocks_needed: {stocks_needed}");
            Console.WriteLine($"rent per year: {rent_YEAR}");





            // Sparkassenformel: Kn = K0 * q^n - R*(q^n-1)/(q-1)
            // Sparkassenformel: Kn = BEKANNT, 0, am ende soll das kapital aufgebraucht sein
            //                   K0 = ??? cash-needed
            //                   q = BEKANNT zinssatz, entweder good case oder bad case
            //                   n = BEKANNT 13
            //                   R = K0/n
            //                  ==> Wir müssen das Ding nach K0 auflösen, das könnte man auch manuell hinkriegen
            //                  Kn, K0, q, n = symbols('Kn K0 q n')
            //                  eq1 = Eq(K0 * q**n - (K0/n)*(q**n-1)/(q-1), Kn)
            //                  sol = solve(eq1,K0)



            //wenn sich z_* an n annähert bedeutet das dass der zinssatz nahe 1 ist, dann sind es bei 13 jahren genau 13 raten.
            //wenn z kleiner wird heisst es größere raten weil es gute und große zinsen gibt

            //var z_cash_max      = -(Math.Pow(cash_interestRate_goodCase, years) - 1)    / ((cash_interestRate_goodCase - 1)     * (Math.Pow(cash_interestRate_goodCase, years)));
            var z_stocks_max    = -(Math.Pow(stocks_interestRate_goodCase, years) - 1)  / ((stocks_interestRate_goodCase - 1)   * (Math.Pow(stocks_interestRate_goodCase, years)));
            //var z_cash_min      = -(Math.Pow(cash_interestRate_badCase, years) - 1)     / ((cash_interestRate_badCase - 1)      * (Math.Pow(cash_interestRate_badCase ,years)));
            var z_stocks_min    = -(Math.Pow(stocks_interestRate_badCase, years) - 1)   / ((stocks_interestRate_badCase - 1)    * (Math.Pow(stocks_interestRate_badCase,years)));
            var z_cash = -(Math.Pow(cash_interestRate, years) - 1) / ((cash_interestRate - 1) * (Math.Pow(cash_interestRate, years)));

            Console.WriteLine($"SSHH: z_stocks_min {z_stocks_min:F2} z_stocks_max {z_stocks_max:F2}");
            Console.WriteLine($"SSHH: z_cash {z_cash:F2}");

            var Rate_Cash = (comfort_total_needed_Year * stocks_crashFactor_badCase * z_stocks_max - minimum_total_needed_Year * z_stocks_min) / (stocks_crashFactor_badCase * z_stocks_max - z_stocks_min);
            var Rate_Stocks_Max = z_stocks_min * (-comfort_total_needed_Year + minimum_total_needed_Year) / (stocks_crashFactor_badCase * z_stocks_max - z_stocks_min);
            var Rate_Stocks_Min = stocks_crashFactor_badCase * z_stocks_max * (-comfort_total_needed_Year + minimum_total_needed_Year) / (stocks_crashFactor_badCase * z_stocks_max - z_stocks_min);
            var Total_Cash = z_cash * (comfort_total_needed_Year * stocks_crashFactor_badCase * z_stocks_max - minimum_total_needed_Year * z_stocks_min) / (stocks_crashFactor_badCase * z_stocks_max - z_stocks_min);
            var Total_Stocks = z_stocks_max * z_stocks_min * (-comfort_total_needed_Year + minimum_total_needed_Year) / (stocks_crashFactor_badCase * z_stocks_max - z_stocks_min);

            Console.WriteLine($"Rate_Cash: {Rate_Cash:F2}");
            Console.WriteLine($"Rate_Stocks_Max: {Rate_Stocks_Max:F2}");
            Console.WriteLine($"Rate_Stocks_Min: {Rate_Stocks_Min:F2}");
            Console.WriteLine($"Total_Cash: {Total_Cash:F2}");
            Console.WriteLine($"Total_Stocks: {Total_Stocks:F2}");

 
            //warum ist das negativ?
            Total_Cash = -Total_Cash;
            Total_Stocks = -Total_Stocks;

            for (int i = 0; i < years; i++)
            {
                Console.WriteLine($"State Begin Year {i}: Cash: {Total_Cash:F2} Stocks: {Total_Stocks:F2} Total: {Total_Stocks+Total_Cash:F2}");


                var interests_Cash = Total_Cash * (cash_interestRate - 1);
                var interests_Stocks = Total_Stocks * (stocks_interestRate_goodCase - 1);
                Total_Cash += interests_Cash;
                Total_Stocks += interests_Stocks;
                Console.WriteLine($"\tInterests: Cash: {interests_Cash:F2} Stocks: {interests_Stocks:F2} Total: {interests_Cash + interests_Stocks:F2}");

                Total_Cash -= Rate_Cash;
                Total_Stocks -= Rate_Stocks_Max;
                Console.WriteLine($"\tWithdrawal: Cash: {Rate_Cash:F2} Stocks: {Rate_Stocks_Max:F2} Total: {Rate_Cash + Rate_Stocks_Max:F2}");


                Console.WriteLine($"\tEnd Year {i}: Cash: {Total_Cash:F2} Stocks: {Total_Stocks:F2} Total: {Total_Stocks + Total_Cash:F2}");
            }

            Assert.That(Total_Cash, Is.EqualTo(0).Within(1));
            Assert.That(Total_Stocks, Is.EqualTo(0).Within(1));
        }
    }
}