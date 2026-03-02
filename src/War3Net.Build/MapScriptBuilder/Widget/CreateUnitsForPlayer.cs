namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateCreateUnitsForPlayer(Map map, int playerId, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var functionName = GeneratedFunctionName.CreateUnitsForPlayer(playerId);

            var mapUnits = map.Units;
            if (mapUnits is null)
            {
                throw new ArgumentException($"Function '{functionName}' cannot be generated without {nameof(MapUnits)}.", nameof(map));
            }

            writer.WriteFunction(functionName);

            GenerateCreateUnits(
                map,
                mapUnits.Units.IncludeId().Where(pair => ShouldGenerateCreateUnitsForPlayerAndUnit(map, playerId, pair.Obj)),
                JassLiteral.Int(playerId),
                writer);

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateCreateUnitsForPlayer(Map map, int playerId)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Units is not null
                && map.Units.Units.Any(unit => ShouldGenerateCreateUnitsForPlayerAndUnit(map, playerId, unit));
        }

        protected internal virtual bool ShouldGenerateCreateUnitsForPlayerAndUnit(Map map, int playerId, UnitData unitData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            return unitData.OwnerId == playerId && unitData.IsUnit() && !unitData.IsPlayerStartLocation() && !unitData.IsBuilding(map.UnitObjectData);
        }
    }
}