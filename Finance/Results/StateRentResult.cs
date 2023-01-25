namespace Finance.Results
{
    public class StateRentResult
    {
        public decimal assumedStateRent_Gross_PerMonth;
        public decimal assumedStateRent_Net_PerMonth;

        public void Print()
        {
            string result = $"===============--- {nameof(StateRentResult)} ---===================="        + Environment.NewLine +
                            $"{nameof(assumedStateRent_Gross_PerMonth)}:       {assumedStateRent_Gross_PerMonth:F2}" + Environment.NewLine +
                            $"{nameof(assumedStateRent_Net_PerMonth)}:       {assumedStateRent_Net_PerMonth:F2}" + Environment.NewLine +
                            $"==========================================================";

            Console.WriteLine(result);
        }
    }
}
