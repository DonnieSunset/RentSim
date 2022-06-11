namespace Portfolio
{
    public class Age
    {
        private static int myCurrent = int.MinValue;

        private int myAbsoluteAge = 0;

        public int Absolut => myAbsoluteAge;
        public int Index => myAbsoluteAge - myCurrent;

        private Age(int absolute)
        {
            myAbsoluteAge = absolute;
        }

        public static void SetCurrent(int current)
        {
            myCurrent = current;
        }

        public static Age NewByAbsoluteAge(int absoluteAge)
        {
            ThrowIfNotInitialized();

            return new Age(absoluteAge);
        }

        public static Age NewByIndexAge(int indexedAge)
        {
            ThrowIfNotInitialized();

            return new Age(indexedAge + myCurrent);
        }

        public static void ThrowIfNotInitialized()
        {
            if (myCurrent == int.MinValue)
            {
                throw new Exception("Not initialized");
            }
        }
    }
}
