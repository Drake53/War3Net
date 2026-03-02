namespace War3Net.Build.Extensions
{
    public static class RegionExtensions
    {
        public static string GetVariableName(this Region region)
        {
            return $"gg_rct_{region.Name.Replace(' ', '_')}";
        }
    }
}