namespace Finance.Results
{
    public class StopWorkPhaseResult
    {
        public int ageStopWork;
        public decimal neededCash;
        public decimal neededStocks;

        public override string ToString()
        {
            string result = $"===============--- StopWorkPhase Result ---====================" + Environment.NewLine +
                            $"{nameof(ageStopWork)}:               {ageStopWork:F2}" + Environment.NewLine +
                            $"{nameof(neededCash)}:              {neededCash:F2}" + Environment.NewLine +
                            $"{nameof(neededStocks)}:            {neededStocks:F2}" + Environment.NewLine +
                            $"===============================================================" + Environment.NewLine;

            return result;
        }
    }
}
