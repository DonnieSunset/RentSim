namespace PhaseIntegratorService.DTOs
{
    public class StateRentResultDTO
    {
        public decimal AssumedStateRent_Gross_PerMonth { get; set; }
        public decimal AssumedStateRent_Net_PerMonth { get; set; }

        public void Print()
        {
            string result = $"===============--- {nameof(StateRentResultDTO)} ---===================="        + Environment.NewLine +
                            $"{nameof(AssumedStateRent_Gross_PerMonth)}:       {AssumedStateRent_Gross_PerMonth:F2}" + Environment.NewLine +
                            $"{nameof(AssumedStateRent_Net_PerMonth)}:       {AssumedStateRent_Net_PerMonth:F2}" + Environment.NewLine +
                            $"==========================================================";

            Console.WriteLine(result);
        }
    }
}
