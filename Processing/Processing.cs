using System;

namespace Processing
{
    public class Calc
    {
        public ResultSet DoCalculation(int years, int startCapital, float interestRate)
        {
            ResultSet result = new ResultSet();
            result.Total.Add(startCapital);

            for (int i = 0; i < years; i++)
            {
                result.Total.Add(result.Total[i] + result.Total[i] * interestRate);
            }

            return result;
        }
    }
}
