using System;
using System.Collections.Generic;
using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateItemTableDropItems(Map map, RandomItemTable table, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteFunction(table.GetDropItemsFunctionName());

            WriteItemTableDropItemsStatements(map, table.ItemSets, false, writer);

            writer.EndFunction();
            writer.WriteLine();
        }

        protected internal virtual void GenerateItemTableDropItems(Map map, WidgetData widgetData, int id, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (widgetData is null)
            {
                throw new ArgumentNullException(nameof(widgetData));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var funcName = widgetData switch
            {
                DoodadData doodadData => doodadData.GetDropItemsFunctionName(id),
                UnitData unitData => unitData.GetDropItemsFunctionName(id),
            };

            writer.WriteFunction(funcName);

            WriteItemTableDropItemsStatements(map, widgetData.ItemTableSets, true, writer);

            writer.EndFunction();
            writer.WriteLine();
        }

        protected internal virtual void WriteItemTableDropItemsStatements(Map map, IEnumerable<RandomItemSet> itemSets, bool chooseItemClass, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (itemSets is null)
            {
                throw new ArgumentNullException(nameof(itemSets));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteAlignedLocal(TypeName.Widget, VariableName.TrigWidget, JassKeyword.Null);
            writer.WriteAlignedLocal(TypeName.Unit, VariableName.TrigUnit, JassKeyword.Null);
            writer.WriteAlignedLocal(JassKeyword.Integer, VariableName.ItemId, "0");
            writer.WriteAlignedLocal(JassKeyword.Boolean, VariableName.CanDrop, JassKeyword.True);
            writer.WriteLine();

            writer.WriteSet(
                VariableName.TrigWidget,
                VariableName.BJLastDyingWidget);

            writer.WriteIf(JassExpression.ParenthesizedCompact(JassExpression.Equal(
                VariableName.TrigWidget,
                JassKeyword.Null)));

            writer.WriteSet(VariableName.TrigUnit, JassExpression.Invoke(NativeName.GetTriggerUnit));
            writer.WriteEndIf();

            writer.WriteLine();

            var trigUnitNotNullExpression = JassExpression.ParenthesizedCompact(JassExpression.NotEqual(
                VariableName.TrigUnit,
                JassKeyword.Null));

            var changingUnitNotNullExpression = JassExpression.NotEqual(
                JassExpression.Invoke(NativeName.GetChangingUnit),
                JassKeyword.Null);

            writer.WriteIf(trigUnitNotNullExpression);

            writer.WriteSet(
                VariableName.CanDrop,
                JassExpression.Not(JassExpression.Invoke(
                    NativeName.IsUnitHidden,
                    VariableName.TrigUnit)));

            writer.WriteIf(JassExpression.ParenthesizedCompact(JassExpression.And(
                VariableName.CanDrop,
                changingUnitNotNullExpression)));

            writer.WriteSet(
                VariableName.CanDrop,
                JassExpression.ParenthesizedCompact(JassExpression.Equal(
                    JassExpression.Invoke(NativeName.GetChangingUnitPrevOwner),
                    JassExpression.Invoke(NativeName.Player, GlobalVariableName.PlayerNeutralHostile))));

            writer.WriteEndIf();
            writer.WriteEndIf();
            writer.WriteLine();

            writer.WriteIf(JassExpression.ParenthesizedCompact(VariableName.CanDrop));

            var i = 0;
            foreach (var itemSet in itemSets)
            {
                writer.WriteComment($"Item set {i}");
                writer.WriteCall(FunctionName.RandomDistReset);

                var summedChance = 0;
                foreach (var item in itemSet.Items)
                {
                    string itemIdExpression;
                    if (RandomItemProvider.IsRandomItem(item.ItemId, out var itemClass, out var level))
                    {
                        if (chooseItemClass)
                        {
                            itemIdExpression = JassExpression.InvokeSpaced(
                                NativeName.ChooseRandomItemEx,
                                itemClass.GetVariableName(),
                                JassLiteral.Int(level));
                        }
                        else
                        {
                            itemIdExpression = JassExpression.InvokeSpaced(
                                NativeName.ChooseRandomItem,
                                JassLiteral.Int(level));
                        }
                    }
                    else
                    {
                        itemIdExpression = JassLiteral.FourCC(item.ItemId);
                    }

                    writer.WriteCall(
                        FunctionName.RandomDistAddItem,
                        itemIdExpression,
                        JassLiteral.Int(item.Chance));

                    summedChance += item.Chance;
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

                writer.WriteIf(trigUnitNotNullExpression);
                writer.WriteCall(
                    FunctionName.UnitDropItem,
                    VariableName.TrigUnit,
                    VariableName.ItemId);

                writer.WriteElse();
                writer.WriteCall(
                    FunctionName.WidgetDropItem,
                    VariableName.TrigWidget,
                    VariableName.ItemId);

                writer.WriteEndIf();
                writer.WriteLine();

                i++;
            }

            writer.WriteEndIf();
            writer.WriteLine();

            writer.WriteSet(VariableName.BJLastDyingWidget, JassKeyword.Null);
            writer.WriteCallCompact(NativeName.DestroyTrigger, JassExpression.Invoke(NativeName.GetTriggeringTrigger));
        }
    }
}