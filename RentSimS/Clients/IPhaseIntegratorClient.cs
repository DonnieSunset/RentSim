using Domain;
using RentSimS.DTOs;

namespace RentSimS.Clients
{
    public interface IPhaseIntegratorClient
    {
        public Task<PhaseIntegratorServiceResultDTO> GetPhaseIntegratorGoodCaseAsync(LifeAssumptions input);
    }
}
