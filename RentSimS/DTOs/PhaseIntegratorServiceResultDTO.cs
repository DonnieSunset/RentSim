using Domain;
using Protocol;

namespace RentSimS.DTOs
{
    public class PhaseIntegratorServiceResultDTO
    {
        public SavingPhaseServiceResultDTO SavingPhaseServiceResult { get; set; }
        public StateRentResultDTO StateRentResult { get; set; }
        public LaterNeedsResultDTO LaterNeedsResult { get; set; }
        public List<ResultRow> Protocol { get; set; }
    }
}
