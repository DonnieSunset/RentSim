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

        public void Print()
        {
            string result = $"===============--- {nameof(LaterNeedsResult)} ---====================" + Environment.NewLine +
                            $"{nameof(needsMinimum_AgeRentStart_WithInflation_PerMonth)}:   {needsMinimum_AgeRentStart_WithInflation_PerMonth:F2}"  + Environment.NewLine +
                            $"{nameof(needsComfort_AgeRentStart_WithInflation_PerMonth)}:   {needsComfort_AgeRentStart_WithInflation_PerMonth:F2}"  + Environment.NewLine +
                            $"{nameof(NeedsMinimum_AgeRentStart_WithInflation_PerYear)}:    {NeedsMinimum_AgeRentStart_WithInflation_PerYear:F2}"   + Environment.NewLine +
                            $"{nameof(NeedsComfort_AgeRentStart_WithInflation_PerYear)}:    {NeedsComfort_AgeRentStart_WithInflation_PerYear:F2}"   + Environment.NewLine +
                            $"{nameof(needsMinimum_AgeStopWork_WithInflation_PerMonth)}:    {needsMinimum_AgeStopWork_WithInflation_PerMonth:F2}"   + Environment.NewLine +
                            $"{nameof(needsComfort_AgeStopWork_WithInflation_PerMonth)}:    {needsComfort_AgeStopWork_WithInflation_PerMonth:F2}"   + Environment.NewLine +
                            $"{nameof(NeedsMinimum_AgeStopWork_WithInflation_PerYear)}:     {NeedsMinimum_AgeStopWork_WithInflation_PerYear:F2}"    + Environment.NewLine +
                            $"{nameof(NeedsComfort_AgeStopWork_WithInflation_PerYear)}:     {NeedsComfort_AgeStopWork_WithInflation_PerYear:F2}"    + Environment.NewLine +
                            $"=================================================================";

            Console.WriteLine(result);
        }
    }
}
