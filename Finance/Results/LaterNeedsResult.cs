namespace Finance.Results
{
    public class LaterNeedsResult
    {
        public decimal needsMinimum_AgeRentStart_WithInflation_PerMonth;
        public decimal needsComfort_AgeRentStart_WithInflation_PerMonth;

        public decimal needsMinimum_AgeStopWork_WithInflation_PerMonth;
        public decimal needsComfort_AgeStopWork_WithInflation_PerMonth;

        public decimal NeedsMinimum_AgeRentStart_WithInflation_PerYear => needsMinimum_AgeRentStart_WithInflation_PerMonth * 12;
        public decimal NeedsComfort_AgeRentStart_WithInflation_PerYear => needsComfort_AgeRentStart_WithInflation_PerMonth * 12;

        public decimal NeedsMinimum_AgeStopWork_WithInflation_PerYear => needsMinimum_AgeStopWork_WithInflation_PerMonth * 12;
        public decimal NeedsComfort_AgeStopWork_WithInflation_PerYear => needsComfort_AgeStopWork_WithInflation_PerMonth * 12;

        public override string ToString()
        {
            string result = $"===============--- StateRentResult ---====================" + Environment.NewLine +
                            $"Needs minimum from Rent Start Age (per month):    {needsMinimum_AgeRentStart_WithInflation_PerMonth:F2}" + Environment.NewLine +
                            $"Needs comfort from Rent Start Age (per month):    {needsComfort_AgeRentStart_WithInflation_PerMonth:F2}" + Environment.NewLine +
                            $"Needs minimum from Rent Start Age (per year):     {NeedsMinimum_AgeRentStart_WithInflation_PerYear:F2}" + Environment.NewLine +
                            $"Needs comfort from Rent Start Age (per year):     {NeedsComfort_AgeRentStart_WithInflation_PerYear:F2}" + Environment.NewLine +
                            $"Needs minimum from Stop Work  Age (per month):    {needsMinimum_AgeStopWork_WithInflation_PerMonth:F2}" + Environment.NewLine +
                            $"Needs comfort from Stop Work  Age (per month):    {needsComfort_AgeStopWork_WithInflation_PerMonth:F2}" + Environment.NewLine +
                            $"Needs minimum from Stop Work  Age (per year):     {NeedsMinimum_AgeStopWork_WithInflation_PerYear:F2}" + Environment.NewLine +
                            $"Needs comfort from Stop Work  Age (per year):     {NeedsComfort_AgeStopWork_WithInflation_PerYear:F2}" + Environment.NewLine +
                            $"==========================================================" + Environment.NewLine;

            return result;
        }
    }
}
