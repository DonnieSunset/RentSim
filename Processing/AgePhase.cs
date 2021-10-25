using System;

namespace Processing
{
    public enum AgePhase
    {
        StopWork,
        RentStart
    }

    public static class AgePhaseBy
    {
        public static AgePhase Age(int age, Input input)
        {
            if (age >= input.ageStopWork && age < input.ageRentStart)
            {
                return AgePhase.StopWork;
            }
            else if (age >= input.ageRentStart && age < input.ageEnd)
            {
                return AgePhase.RentStart;
            }
            else
            {
                throw new Exception($"Unable to calculate AgePhase from age <{age}>.");
            }
        }
    }
}
