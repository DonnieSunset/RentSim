namespace Finance.Results
{
    public class StopWorkPhaseResult
    {
        public int ageStopWork;
        public decimal neededCash;
        public decimal neededStocks;

        public decimal NeededTotal => neededCash + neededStocks;

        public void Print()
        {
            string result = $"===============--- {nameof(StopWorkPhaseResult)} ---====================" + Environment.NewLine +
                            $"{nameof(ageStopWork)}:             {ageStopWork}" + Environment.NewLine +
                            $"{nameof(neededCash)}:              {neededCash:F2}" + Environment.NewLine +
                            $"{nameof(neededStocks)}:            {neededStocks:F2}" + Environment.NewLine +
                            $"{nameof(NeededTotal)}:             {NeededTotal:F2}" + Environment.NewLine +
                            $"===============================================================" + Environment.NewLine;

            Console.WriteLine(result);
        }
    }
}
