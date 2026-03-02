namespace War3Net.Build.Extensions
{
    public static class RandomItemTableExtensions
    {
        public static string GetDropItemsFunctionName(this RandomItemTable randomItemTable)
        {
            return $"ItemTable{randomItemTable.Index:D6}_DropItems";
        }
    }
}