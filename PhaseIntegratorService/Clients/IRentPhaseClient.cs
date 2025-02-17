﻿using Protocol;
using PhaseIntegratorService.DTOs;

namespace PhaseIntegratorService.Clients
{
    public interface IRentPhaseClient
    {
        public Task<decimal> ApproxStateRent(int ageCurrent, decimal netRentAgeCurrent, int ageRentStart, decimal netRentAgeRentStart, int ageInQuestion);

        public Task<RentPhaseServiceResultDTO> GetRentPhaseSimulationAsync(RentPhaseServiceInputDTO input);

        public void LogRentPhaseResult(RentPhaseServiceResultDTO result, IProtocolWriter protocol);
    }
}
