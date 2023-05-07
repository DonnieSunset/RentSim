using Domain;
using Protocol;
using System.Collections.ObjectModel;

namespace PhaseIntegratorService.DTOs
{
    public class PhaseIntegratorServiceResultDTO
    {
        public SavingPhaseServiceResultDTO SavingPhaseServiceResult { get; set; }
        public StateRentResultDTO StateRentResult { get; set; }
        public LaterNeedsResultDTO LaterNeedsResult { get; set; }

        //public MemoryProtocolWriter ProtocolWriter { get; set; }
        public List<ResultRow> Protocol { get; set; }
    }
}