using System.Threading.Tasks;

namespace RentSim.Shared
{
    public interface ISavingPhaseService
    {
        Task<decimal> Calculate(int ageCurrent,
            int ageStopWork,
            decimal startCapital,
            int growthRate,
            decimal saveAmountPerMonth);
    }
}
