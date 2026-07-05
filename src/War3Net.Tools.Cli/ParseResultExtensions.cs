namespace War3Net.Tools.Cli
{
    public static class ParseResultExtensions
    {
        public static MpqLocale GetMpqLocale(
            this ParseResult parseResult,
            Option<string> localeOption)
        {
            var localeString = parseResult.GetValue(localeOption);
            if (localeString is null)
            {
                return MpqLocale.Neutral;
            }

            return Enum.Parse<MpqLocale>(localeString, ignoreCase: true);
        }

        public static OutputDetail GetOutputDetail(
            this ParseResult parseResult,
            Option<string> detailOption)
        {
            var detailString = parseResult.GetValue(detailOption);

            return detailString?.ToLowerInvariant() switch
            {
                "names" => OutputDetail.Names,
                "full" => OutputDetail.Full,
                _ => OutputDetail.Summary,
            };
        }

        public static OutputFormat GetOutputFormat(
            this ParseResult parseResult,
            Option<string> formatOption)
        {
            var formatString = parseResult.GetValue(formatOption);

            return formatString?.ToLowerInvariant() switch
            {
                "json" => OutputFormat.Json,
                "jsonl" => OutputFormat.Jsonl,
                "base64" => OutputFormat.Base64,
                "binary" => OutputFormat.Binary,
                _ => OutputFormat.Text,
            };
        }
    }
}