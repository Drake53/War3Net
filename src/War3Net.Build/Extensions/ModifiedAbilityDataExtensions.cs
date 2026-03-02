namespace War3Net.Build.Extensions
{
    public static class ModifiedAbilityDataExtensions
    {
        private static readonly Lazy<Dictionary<int, string>> _abilityOrderOnStrings = new(GetAbilityOrderOnStrings);
        private static readonly Lazy<Dictionary<int, string>> _abilityOrderOffStrings = new(GetAbilityOrderOffStrings);

        public static bool TryGetOrderOnString(this ModifiedAbilityData abilityData, [NotNullWhen(true)] out string? orderOnString)
        {
            return _abilityOrderOnStrings.Value.TryGetValue(abilityData.AbilityId, out orderOnString);
        }

        public static bool TryGetOrderOffString(this ModifiedAbilityData abilityData, [NotNullWhen(true)] out string? orderOffString)
        {
            return _abilityOrderOffStrings.Value.TryGetValue(abilityData.AbilityId, out orderOffString);
        }

        private static Dictionary<int, string> GetAbilityOrderOnStrings()
        {
            return new Dictionary<int, string>
            {
                { "Ahea".FromRawcode(), "healon" },
                { "ACsa".FromRawcode(), "flamingarrows" },
                { "ANth".FromRawcode(), "Thornyshield" },
                { "AEim".FromRawcode(), "immolation" },
                { "ANba".FromRawcode(), "blackarrowon" },
                { "AHds".FromRawcode(), "divineshield" },
            };
        }

        private static Dictionary<int, string> GetAbilityOrderOffStrings()
        {
            return new Dictionary<int, string>
            {
                { "ANia".FromRawcode(), "incineratearrowon" },
                { "ACpa".FromRawcode(), "parasiteoff" },
                { "ANms".FromRawcode(), "manashieldoff" },
                { "ANba".FromRawcode(), "blackarrowoff" },
                { "Anhe".FromRawcode(), "healoff" },
                { "ACbb".FromRawcode(), "bloodlustoff" },
                { "Afzy".FromRawcode(), "frenzyoff" },
                { "ACbl".FromRawcode(), "bloodlustoff" },
                { "ACcs".FromRawcode(), "curseoff" },
                { "ACff".FromRawcode(), "faeriefireoff" },
                { "ACf2".FromRawcode(), "frostarmoroff" },
                { "Anh1".FromRawcode(), "healoff" },
                { "Anh2".FromRawcode(), "healoff" },
                { "ACif".FromRawcode(), "innerfireoff" },
                { "ACrd".FromRawcode(), "raisedeadoff" },
                { "Ahrp".FromRawcode(), "repairoff" },
                { "ACwb".FromRawcode(), "weboff" },
                { "ACdm".FromRawcode(), "autodispeloff" },
                { "ACd2".FromRawcode(), "autodispeloff" },
            };
        }
    }
}