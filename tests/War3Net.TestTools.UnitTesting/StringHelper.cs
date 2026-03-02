namespace War3Net.TestTools.UnitTesting
{
    internal static class StringHelper
    {
        [return: NotNullIfNotNull(nameof(text))]
        internal static string? ShowNewLineCharacters(string? text)
        {
            if (text is null)
            {
                return null;
            }

            return text
                .Replace("\r\n", "\\r\n", StringComparison.Ordinal)
                .Replace("\n", "\\n\n", StringComparison.Ordinal)
                .Replace("\r", "\\r\n", StringComparison.Ordinal);
        }
    }
}