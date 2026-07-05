namespace War3Net.Tools.Cli
{
    public static class ValidatorListExtensions
    {
        public static void AddOneOfValidation(
            this IList<Action<OptionResult>> validators,
            params string[] allowedValues)
        {
            var deconstructedAllowedValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var allowedValue in allowedValues)
            {
                var openBracketIndex = allowedValue.IndexOf('[', StringComparison.Ordinal);
                if (openBracketIndex == -1)
                {
                    if (!deconstructedAllowedValues.Add(allowedValue))
                    {
                        throw new ArgumentException($"Allowed value '{allowedValue}' is not unique.", nameof(allowedValues));
                    }

                    continue;
                }

                var closeBracketIndex = allowedValue.IndexOf(']', openBracketIndex + 1);
                if (closeBracketIndex == -1 ||
                    closeBracketIndex != allowedValue.Length - 1 ||
                    allowedValue.IndexOf('[', openBracketIndex + 1) != -1)
                {
                    throw new ArgumentException($"Allowed value '{allowedValue}' has mismatched or multiple '[]' groups.", nameof(allowedValues));
                }

                var shortForm = allowedValue[..openBracketIndex];
                var longForm = string.Concat(shortForm, allowedValue.AsSpan(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1));

                if (!deconstructedAllowedValues.Add(shortForm))
                {
                    throw new ArgumentException($"Allowed value '{shortForm}' is not unique.", nameof(allowedValues));
                }

                if (!deconstructedAllowedValues.Add(longForm))
                {
                    throw new ArgumentException($"Allowed value '{longForm}' is not unique.", nameof(allowedValues));
                }
            }

            validators.Add(result =>
            {
                if (result.Tokens.Count == 0)
                {
                    return;
                }

                var value = result.Tokens.Single().Value;
                if (!deconstructedAllowedValues.Contains(value))
                {
                    result.AddError($"Argument '{value}' not recognized. Must be one of: {string.Join(", ", allowedValues.Select(allowedValue => $"'{allowedValue}'"))}");
                }
            });
        }
    }
}