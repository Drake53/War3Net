namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateUnitVariables(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapUnits = map.Units;
            if (mapUnits is null)
            {
                return;
            }

            foreach (var unit in mapUnits.Units.Where(ShouldGenerateUnitVariable))
            {
                writer.WriteAlignedGlobal(
                    TypeName.Unit,
                    unit.GetVariableName(),
                    JassKeyword.Null);
            }
        }

        protected internal virtual bool ShouldGenerateUnitVariables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Units is not null
                && map.Units.Units.Any(ShouldGenerateUnitVariable);
        }

        protected internal virtual bool ShouldGenerateUnitVariable(UnitData unitData)
        {
            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            if (!unitData.IsUnit() || unitData.IsPlayerStartLocation())
            {
                return false;
            }

            var unitVariableName = unitData.GetVariableName();

            return ForceGenerateGlobalUnitVariable
                || TriggerVariableReferences.ContainsKey(unitVariableName);
        }
    }
}