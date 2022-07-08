namespace Finance.Results
{
    public class StateRentResult
    {
        public decimal assumedStateRent_FromStopWorkAge_PerMonth;

        public override string ToString()
        {
            string result = $"===============--- {nameof(StateRentResult)} ---===================="                                         + Environment.NewLine +
                            $"{nameof(assumedStateRent_FromStopWorkAge_PerMonth)}:       {assumedStateRent_FromStopWorkAge_PerMonth:F2}"    + Environment.NewLine +
                            $"==========================================================";

            return result;
        }
    }
}
