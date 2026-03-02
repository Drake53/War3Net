namespace War3Net.Build.Extensions
{
    public static class RandomUnitGlobalTableExtensions
    {
        public static string GetVariableName(this RandomUnitGlobalTable randomUnitGlobalTable)
        {
            return $"gg_rg_{randomUnitGlobalTable.TableId:D3}";
        }
    }
}