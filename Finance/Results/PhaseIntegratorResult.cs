namespace Finance.Results
{
    public class PhaseIntegratorResult
    {
        public int ageStopWork;
        public decimal overAmount_goodCase;
        public decimal overAmount_badCase;

        public StateRentResult stateRentResult;
        public LaterNeedsResult laterNeedsResult;
        public SavingPhaseResult savingPhaseResult;
        public StopWorkPhaseResult stopWorkPhaseResult_goodCase;
        public StopWorkPhaseResult stopWorkPhaseResult_badCase;
        public RentPhaseResult rentPhaseResult;

        public void Print()
        {
            string result = $"===============--- {nameof(PhaseIntegratorResult)} ---===================="   + Environment.NewLine +
                            $"{nameof(ageStopWork)}:            {ageStopWork}" + Environment.NewLine +
                            $"{nameof(overAmount_goodCase)}:    {overAmount_goodCase:F2}" + Environment.NewLine +
                            $"{nameof(overAmount_badCase)}:     {overAmount_badCase:F2}" + Environment.NewLine +
                            $"==========================================================";

            Console.WriteLine(result);
        }

        public void PrintFull()
        {
            Print();
            Console.WriteLine();
            stateRentResult.Print();
            Console.WriteLine();
            laterNeedsResult.Print();
            Console.WriteLine();
            savingPhaseResult.Print();
            Console.WriteLine();
            stopWorkPhaseResult_goodCase.Print();
            Console.WriteLine();
            stopWorkPhaseResult_badCase.Print();
            Console.WriteLine();
            rentPhaseResult.Print();
        }
    }
}
