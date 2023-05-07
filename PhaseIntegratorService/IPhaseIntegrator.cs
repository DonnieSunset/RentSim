using Domain;
using PhaseIntegratorService.Clients;
using PhaseIntegratorService.DTOs;

namespace PhaseIntegratorService
{
    public interface IPhaseIntegrator
    {
        public Task<PhaseIntegratorServiceResultDTO> SimulateGoodCase(
            LifeAssumptions lifeAssumptions,
            IFinanceMathClient financeMathClient,
            ISavingPhaseClient savingPhaseClient,
            IStopWorkPhaseClient stopWorkPhaseClient,
            IRentPhaseClient rentPhaseClient
            );
    }
}
