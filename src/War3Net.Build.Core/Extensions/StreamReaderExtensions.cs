namespace War3Net.Build.Extensions
{
    public static class StreamReaderExtensions
    {
        public static TriggerStrings ReadTriggerStrings(this StreamReader reader) => new TriggerStrings(reader);

        public static TriggerString ReadTriggerString(this StreamReader reader) => new TriggerString(reader);
    }
}