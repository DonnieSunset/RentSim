namespace Finance.Results
{
    public class PhaseIntegratorResult
    {
        public int ageStopWork;
        public decimal overAmount;

        public SavingPhaseResult savingPhaseResult;
        public StateRentResult stateRentResult;
        public LaterNeedsResult laterNeedsResult;
        public RentPhaseResult rentPhaseResult;
        public StopWorkPhaseResult stopWorkPhaseResult;

        public void Print()
        {
            string result = $"===============--- {nameof(PhaseIntegratorResult)} ---===================="   + Environment.NewLine +
                            $"{nameof(ageStopWork)}:   {ageStopWork}"                                       + Environment.NewLine +
                            $"{nameof(overAmount)}:    {overAmount:F2}"                                     + Environment.NewLine +
                            $"==========================================================";

            Console.WriteLine(result);
        }
    }
}
