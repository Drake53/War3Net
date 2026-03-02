namespace War3Net.TestTools.UnitTesting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class FlakyTestMethodAttribute
#if ENABLE_FLAKY_TESTS
        : TestMethodAttribute
#else
        : Attribute
#endif
    {
        public FlakyTestMethodAttribute(string? reason = null)
        {
            Reason = reason;
        }

        public string? Reason { get; }
    }
}