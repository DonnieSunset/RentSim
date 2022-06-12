namespace Finance.Results
{
    public class StateRentResult
    {
        public decimal assumedStateRent_FromStopWorkAge_PerMonth;

        public override string ToString()
        {
            string result = $"===============--- StateRentResult ---====================" + Environment.NewLine +
                            $"Net Rent from stop work age:       {assumedStateRent_FromStopWorkAge_PerMonth:F2}" + Environment.NewLine +
                            $"==========================================================";

            return result;
        }
    }
}
