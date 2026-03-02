namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateInitRandomGroups(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var randomUnitTables = map.Info?.RandomUnitTables;
            if (randomUnitTables is null)
            {
                throw new ArgumentException($"Function '{GeneratedFunctionName.InitRandomGroups}' cannot be generated without {nameof(MapInfo.RandomUnitTables)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.InitRandomGroups);

            writer.WriteLocal(JassKeyword.Integer, VariableName.CurrentSet);
            writer.WriteLine();

            foreach (var unitTable in randomUnitTables)
            {
                writer.WriteComment($"Group {unitTable.Index} - {unitTable.Name}");
                writer.WriteCall(FunctionName.RandomDistReset);

                for (var i = 0; i < unitTable.UnitSets.Count; i++)
                {
                    writer.WriteCall(
                        FunctionName.RandomDistAddItem,
                        JassLiteral.Int(i),
                        JassLiteral.Int(unitTable.UnitSets[i].Chance));
                }

                writer.WriteSet(VariableName.CurrentSet, JassExpression.Invoke(FunctionName.RandomDistChoose));
                writer.WriteLine();

                var groupVarName = unitTable.GetVariableName();
                for (var setIndex = 0; setIndex < unitTable.UnitSets.Count; setIndex++)
                {
                    var set = unitTable.UnitSets[setIndex];

                    var condition = JassExpression.ParenthesizedCompact(JassExpression.EqualCompact(
                        VariableName.CurrentSet,
                        JassLiteral.Int(setIndex)));

                    if (setIndex == 0)
                    {
                        writer.WriteIf(condition);
                    }
                    else
                    {
                        writer.WriteElseIf(condition);
                    }

                    for (var position = 0; position < unitTable.Types.Count; position++)
                    {
                        var id = set?.UnitIds[position] ?? 0;
                        var unitTypeExpression = RandomUnitProvider.IsRandomUnit(id, out var level)
                            ? JassExpression.Invoke(NativeName.ChooseRandomCreep, JassLiteral.Int(level))
                            : id == 0 ? "-1" : JassLiteral.FourCC(id);

                        writer.WriteSet(
                            JassExpression.ElementAccess(groupVarName, position),
                            unitTypeExpression);
                    }
                }

                writer.WriteElse();

                for (var position = 0; position < unitTable.Types.Count; position++)
                {
                    writer.WriteSet(
                        JassExpression.ElementAccess(groupVarName, position),
                        "-1");
                }

                writer.WriteEndIf();
                writer.WriteLine();
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateInitRandomGroups(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info?.RandomUnitTables is not null
                && map.Info.RandomUnitTables.Count > 0;
        }
    }
}