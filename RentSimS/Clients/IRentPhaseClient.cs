using Domain;
using Finance.Results;
using Protocol;
using RentSimS.DTOs;

namespace RentSimS.Clients
{
    public interface IRentPhaseClient
    {
        public Task<decimal> ApproxStateRent(int ageCurrent, decimal netRentAgeCurrent, int ageRentStart, decimal netRentAgeRentStart, int ageInQuestion);

        public Task<RentPhaseResult> GetAndLogRentPhase(
            int ageStart, int ageEnd,
            double growRateCash, double growRateStocks, double growRateMetals,
            SavingPhaseResult savingPhaseResult,
            LaterNeedsResult laterNeedsResult,
            StateRentResult stateRentResult,
            IProtocolWriter protocolWriter);
    }
}
