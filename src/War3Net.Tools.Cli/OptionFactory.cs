namespace War3Net.Tools.Cli
{
    public static class OptionFactory
    {
        public static Option<bool> FlagOption(
            string name,
            string? alias,
            string description)
        {
            var option = alias is null
                ? new Option<bool>(name)
                : new Option<bool>(name, alias);

            option.Description = description;

            return option;
        }

        public static Option<int?> Int32Option(
            string name,
            string? alias,
            string description)
        {
            var option = alias is null
                ? new Option<int?>(name)
                : new Option<int?>(name, alias);

            option.Description = description;

            return option;
        }

        public static Option<string> StringOption(
            string name,
            string? alias,
            string description)
        {
            var option = alias is null
                ? new Option<string>(name)
                : new Option<string>(name, alias);

            option.Description = description;

            return option;
        }

        public static Option<string> StringOption(
            string name,
            string? alias,
            string[] allowedValues,
            string defaultValue,
            string description)
        {
            if (allowedValues.Length < 2)
            {
                throw new ArgumentException("Must specify at least two allowed values. If there is only one possible value, use a flag instead.", nameof(allowedValues));
            }

            var option = alias is null
                ? new Option<string>(name)
                : new Option<string>(name, alias);

            option.Description = description;
            option.DefaultValueFactory = _ => defaultValue;
            _ = option.AcceptOnlyFromAmong(allowedValues);

            return option;
        }
    }
}