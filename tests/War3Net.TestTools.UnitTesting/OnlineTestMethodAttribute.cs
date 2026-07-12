namespace War3Net.TestTools.UnitTesting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class OnlineTestMethodAttribute
#if ENABLE_ONLINE_TESTS
        : TestMethodAttribute
#else
        : Attribute
#endif
    {
        public OnlineTestMethodAttribute(string? reason = null)
        {
            Reason = reason;
        }

        public string? Reason { get; }
    }
}