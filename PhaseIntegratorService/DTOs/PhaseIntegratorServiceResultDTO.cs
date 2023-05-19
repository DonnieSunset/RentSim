using Domain;

namespace PhaseIntegratorService.DTOs
{
    public class PhaseIntegratorServiceResultDTO
    {
        public SavingPhaseServiceResultDTO SavingPhaseServiceResult { get; set; }
        public StopWorkPhaseServiceResultDTO StopWorkPhaseServiceResult { get; set; }
        public RentPhaseServiceResultDTO RentPhaseServiceResult { get; set; }
        public StateRentResultDTO StateRentResult { get; set; }
        public LaterNeedsResultDTO LaterNeedsResult { get; set; }
        public List<ResultRow> Protocol { get; set; }
        public ResultDTO Result { get; set; } = new ResultDTO();
        public int AgeStopWork { get; set; }
    }
}