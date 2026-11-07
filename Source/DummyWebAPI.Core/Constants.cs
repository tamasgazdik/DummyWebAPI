namespace DummyWebAPI.Core
{
    public static class Constants
    {
        public static class ManifacturingYear
        {
            public const int MIN = 1885;

            public static readonly int MAX = DateTime.UtcNow.Year;
        }

        public static class Displacement
        {
            public const int MAX = 3000;
        }
    }
}
