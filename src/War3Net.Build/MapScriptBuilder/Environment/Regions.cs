namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateRegionVariables(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapRegions = map.Regions;
            if (mapRegions is null)
            {
                return;
            }

            foreach (var region in mapRegions.Regions)
            {
                writer.WriteAlignedGlobal(
                    TypeName.Rect,
                    region.GetVariableName(),
                    JassKeyword.Null);
            }
        }

        protected internal virtual bool ShouldGenerateRegionVariables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Regions is not null
                && map.Regions.Regions.Count > 0;
        }
    }
}