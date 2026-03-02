namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateCreateAllItems(Map map, IndentedTextWriter writer)
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
                throw new ArgumentException($"Function '{GeneratedFunctionName.CreateAllItems}' cannot be generated without {nameof(MapUnits)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.CreateAllItems);

            writer.WriteLocal(JassKeyword.Integer, VariableName.ItemId);

            foreach (var item in mapUnits.Units.Where(item => ShouldGenerateCreateAllItemsForItem(map, item)))
            {
                if (item.IsRandomItem())
                {
                    var randomData = item.RandomData;
                    switch (randomData)
                    {
                        case RandomUnitAny randomUnitAny:
                            writer.WriteSet(
                                VariableName.ItemId,
                                JassExpression.InvokeSpaced(
                                    NativeName.ChooseRandomItemEx,
                                    JassExpression.Invoke(NativeName.ConvertItemType, JassLiteral.Int((int)randomUnitAny.Class)),
                                    JassLiteral.Int(randomUnitAny.Level)));

                            break;

                        case RandomUnitGlobalTable randomUnitGlobalTable:
                            break;

                        case RandomUnitCustomTable randomUnitCustomTable:
                            writer.WriteCall(FunctionName.RandomDistReset);

                            var summedChance = 0;
                            foreach (var randomItem in randomUnitCustomTable.RandomUnits)
                            {
                                var itemIdExpression = RandomItemProvider.IsRandomItem(randomItem.UnitId, out var itemClass, out var level)
                                    ? JassExpression.Invoke(
                                        NativeName.ChooseRandomItemEx,
                                        JassExpression.Invoke(NativeName.ConvertItemType, JassLiteral.Int((int)itemClass)),
                                        JassLiteral.Int(level))
                                    : JassLiteral.FourCC(randomItem.UnitId);

                                writer.WriteCall(
                                    FunctionName.RandomDistAddItem,
                                    itemIdExpression,
                                    JassLiteral.Int(randomItem.Chance));

                                summedChance += randomItem.Chance;
                            }

                            if (summedChance < 100)
                            {
                                writer.WriteCall(
                                    FunctionName.RandomDistAddItem,
                                    "-1",
                                    JassLiteral.Int(100 - summedChance));
                            }

                            writer.WriteSet(
                                VariableName.ItemId,
                                JassExpression.InvokeSpaced(FunctionName.RandomDistChoose));

                            break;

                        default:
                            break;
                    }

                    writer.WriteIf(JassExpression.NotEqual(VariableName.ItemId, "-1"));

                    writer.WriteCall(
                        NativeName.CreateItem,
                        VariableName.ItemId,
                        JassLiteral.Real(item.Position.X),
                        JassLiteral.Real(item.Position.Y));

                    writer.WriteEndIf();
                }
                else
                {
                    var args = new List<string>
                    {
                        JassLiteral.FourCC(item.TypeId),
                        JassLiteral.Real(item.Position.X),
                        JassLiteral.Real(item.Position.Y),
                    };

                    var hasSkin = item.SkinId != 0 && item.SkinId != item.TypeId;
                    if (hasSkin)
                    {
                        args.Add(JassLiteral.FourCC(item.SkinId));
                    }

                    writer.WriteCall(
                        hasSkin ? NativeName.BlzCreateItemWithSkin : NativeName.CreateItem,
                        args.ToArray());
                }
            }

            writer.WriteLine();

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateCreateAllItems(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is not null && map.Info.FormatVersion == MapInfoFormatVersion.v8)
            {
                return true;
            }

            return map.Units is not null
                && map.Units.Units.Any(item => ShouldGenerateCreateAllItemsForItem(map, item));
        }

        protected internal virtual bool ShouldGenerateCreateAllItemsForItem(Map map, UnitData unitData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            return unitData.IsItem() && !unitData.IsPlayerStartLocation();
        }
    }
}