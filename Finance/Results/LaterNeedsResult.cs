namespace Finance.Results
{
    public class LaterNeedsResult
    {
        public decimal needsMinimum_AgeRentStart_WithInflation_PerMonth;
        public decimal needsComfort_AgeRentStart_WithInflation_PerMonth;

        public decimal needsMinimum_AgeRentStart_WithInflation_PerYear;
        public decimal needsComfort_AgeRentStart_WithInflation_PerYear;

        public override string ToString()
        {
            string result = $"===============--- StateRentResult ---====================" + Environment.NewLine +
                            $"Needs minimum from Rent Start Age (per month):       {needsMinimum_AgeRentStart_WithInflation_PerMonth:F2}" + Environment.NewLine +
                            $"Needs comfort from Rent Start Age (per month):       {needsComfort_AgeRentStart_WithInflation_PerMonth:F2}" + Environment.NewLine +
                            $"Needs minimum from Rent Start Age (per year):       {needsMinimum_AgeRentStart_WithInflation_PerYear:F2}" + Environment.NewLine +
                            $"Needs comfort from Rent Start Age (per year):       {needsComfort_AgeRentStart_WithInflation_PerYear:F2}" + Environment.NewLine +
                            $"==========================================================";

            return result;
        }
    }
}
