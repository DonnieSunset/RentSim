namespace RentSimS.Clients
{
    public interface IRentPhaseClient
    {
        public Task<decimal> ApproxStateRent(int ageCurrent, decimal netRentAgeCurrent, int ageRentStart, decimal netRentAgeRentStart, int ageInQuestion);
    }
}
