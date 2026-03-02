namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateCreateNeutralUnits(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteFunction(GeneratedFunctionName.CreateNeutralUnits);

            if (ShouldGenerateCreateNeutralHostile(map))
            {
                writer.WriteCall(GeneratedFunctionName.CreateNeutralHostile);
            }

            if (ShouldGenerateCreateNeutralPassive(map))
            {
                writer.WriteCall(GeneratedFunctionName.CreateNeutralPassive);
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateCreateNeutralUnits(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.FormatVersion < MapInfoFormatVersion.v15;
        }
    }
}