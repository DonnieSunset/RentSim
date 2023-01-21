namespace Finance.Results
{
    public class StateRentResult
    {
        public decimal assumedStateRent_FromStopWorkAge_PerMonth;

        public void Print()
        {
            string result = $"===============--- {nameof(StateRentResult)} ---===================="                                         + Environment.NewLine +
                            $"{nameof(assumedStateRent_FromStopWorkAge_PerMonth)}:       {assumedStateRent_FromStopWorkAge_PerMonth:F2}"    + Environment.NewLine +
                            $"==========================================================";

            Console.WriteLine(result);
        }
    }
}
