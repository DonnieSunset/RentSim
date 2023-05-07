namespace RentSimS.DTOs
{
    public class LaterNeedsResultDTO
    {
        public decimal NeedsMinimum_AgeRentStart_WithInflation_PerMonth { get; set; }
        public decimal NeedsComfort_AgeRentStart_WithInflation_PerMonth { get; set; }

        public decimal NeedsMinimum_AgeStopWork_WithInflation_PerMonth { get; set; }
        public decimal NeedsComfort_AgeStopWork_WithInflation_PerMonth { get; set; }

        public decimal NeedsMinimum_AgeRentStart_WithInflation_PerYear => NeedsMinimum_AgeRentStart_WithInflation_PerMonth * 12;
        public decimal NeedsComfort_AgeRentStart_WithInflation_PerYear => NeedsComfort_AgeRentStart_WithInflation_PerMonth * 12;

        public decimal NeedsMinimum_AgeStopWork_WithInflation_PerYear => NeedsMinimum_AgeStopWork_WithInflation_PerMonth * 12;
        public decimal NeedsComfort_AgeStopWork_WithInflation_PerYear => NeedsComfort_AgeStopWork_WithInflation_PerMonth * 12;

        public void Print()
        {
            string result = $"===============--- {nameof(LaterNeedsResultDTO)} ---====================" + Environment.NewLine +
                            $"{nameof(NeedsMinimum_AgeRentStart_WithInflation_PerMonth)}:   {NeedsMinimum_AgeRentStart_WithInflation_PerMonth:F2}"  + Environment.NewLine +
                            $"{nameof(NeedsComfort_AgeRentStart_WithInflation_PerMonth)}:   {NeedsComfort_AgeRentStart_WithInflation_PerMonth:F2}"  + Environment.NewLine +
                            $"{nameof(NeedsMinimum_AgeRentStart_WithInflation_PerYear)}:    {NeedsMinimum_AgeRentStart_WithInflation_PerYear:F2}"   + Environment.NewLine +
                            $"{nameof(NeedsComfort_AgeRentStart_WithInflation_PerYear)}:    {NeedsComfort_AgeRentStart_WithInflation_PerYear:F2}"   + Environment.NewLine +
                            $"{nameof(NeedsMinimum_AgeStopWork_WithInflation_PerMonth)}:    {NeedsMinimum_AgeStopWork_WithInflation_PerMonth:F2}"   + Environment.NewLine +
                            $"{nameof(NeedsComfort_AgeStopWork_WithInflation_PerMonth)}:    {NeedsComfort_AgeStopWork_WithInflation_PerMonth:F2}"   + Environment.NewLine +
                            $"{nameof(NeedsMinimum_AgeStopWork_WithInflation_PerYear)}:     {NeedsMinimum_AgeStopWork_WithInflation_PerYear:F2}"    + Environment.NewLine +
                            $"{nameof(NeedsComfort_AgeStopWork_WithInflation_PerYear)}:     {NeedsComfort_AgeStopWork_WithInflation_PerYear:F2}"    + Environment.NewLine +
                            $"=================================================================";

            Console.WriteLine(result);
        }
    }
}
