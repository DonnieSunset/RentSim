namespace PhaseIntegratorService.DTOs
{
    public class ResultDTO
    {
        public enum ResultType
        {
            Unspecified = 0,
            Success,
            Failure,
        }

        public ResultType Type { get; set; } = ResultType.Unspecified;
        public string Message { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}
