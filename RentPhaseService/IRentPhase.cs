using RentPhaseService.Clients;
using RentPhaseService.DTOs;

namespace RentPhaseService
{
    public interface IRentPhase
    {
        public decimal ApproxStateRent(int ageCurrent, decimal netRentAgeCurrent, int ageRentStart, decimal netRentAgeRentStart, int ageInQuestion);

        public Task<RentPhaseServiceResultDTO> Simulate(RentPhaseServiceInputDTO input, IFinanceMathClient financeMathClient);
    }
}
