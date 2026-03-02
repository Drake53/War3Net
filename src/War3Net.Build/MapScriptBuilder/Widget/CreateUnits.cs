using System;
using System.Collections.Generic;
using System.Linq;
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
        protected internal virtual void GenerateCreateUnits(Map map, IEnumerable<(UnitData Unit, int Id)> units, string playerNumberExpression, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (units is null)
            {
                throw new ArgumentNullException(nameof(units));
            }

            if (playerNumberExpression is null)
            {
                throw new ArgumentNullException(nameof(playerNumberExpression));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteLocal(
                TypeName.Player,
                VariableName.Player,
                JassExpression.Invoke(NativeName.Player, playerNumberExpression));

            if (!ForceGenerateGlobalUnitVariable)
            {
                writer.WriteLocal(TypeName.Unit, VariableName.Unit);
            }

            writer.WriteLocal(JassKeyword.Integer, VariableName.UnitId);
            writer.WriteLocal(TypeName.Trigger, VariableName.Trigger);
            if (UseLifeVariable)
            {
                writer.WriteLocal(JassKeyword.Real, VariableName.Life);
            }

            writer.WriteLine();

            foreach (var (unit, id) in units.OrderBy(pair => pair.Unit.CreationNumber))
            {
                var unitVariableName = unit.GetVariableName();
                if (!ForceGenerateGlobalUnitVariable && (!TriggerVariableReferences.TryGetValue(unitVariableName, out var value) || !value))
                {
                    unitVariableName = VariableName.Unit;
                }

                if (unit.IsRandomUnit() || unit.IsRandomBuilding())
                {
                    var randomData = unit.RandomData;
                    switch (randomData)
                    {
                        case RandomUnitAny randomUnitAny:
                            if (unit.IsRandomBuilding())
                            {
                                writer.WriteSet(
                                    VariableName.UnitId,
                                    JassExpression.InvokeSpaced(NativeName.ChooseRandomNPBuilding));
                            }
                            else
                            {
                                writer.WriteSet(
                                    VariableName.UnitId,
                                    JassExpression.InvokeSpaced(NativeName.ChooseRandomCreep, JassLiteral.Int(randomUnitAny.Level)));
                            }

                            break;

                        case RandomUnitGlobalTable randomUnitGlobalTable:
                            writer.WriteSet(
                                VariableName.UnitId,
                                JassExpression.ElementAccess(randomUnitGlobalTable.GetVariableName(), randomUnitGlobalTable.Column));

                            break;

                        case RandomUnitCustomTable randomUnitCustomTable:
                            writer.WriteCall(FunctionName.RandomDistReset);

                            var summedChance = 0;
                            foreach (var randomUnit in randomUnitCustomTable.RandomUnits)
                            {
                                writer.WriteCall(
                                    FunctionName.RandomDistAddItem,
                                    RandomUnitProvider.IsRandomUnit(randomUnit.UnitId, out var level)
                                        ? JassExpression.Invoke(NativeName.ChooseRandomCreep, JassLiteral.Int(level))
                                        : JassLiteral.FourCC(randomUnit.UnitId),
                                    JassLiteral.Int(randomUnit.Chance));

                                summedChance += randomUnit.Chance;
                            }

                            if (summedChance < 100)
                            {
                                writer.WriteCall(
                                    FunctionName.RandomDistAddItem,
                                    "-1",
                                    JassLiteral.Int(100 - summedChance));
                            }

                            writer.WriteSet(
                                VariableName.UnitId,
                                JassExpression.InvokeSpaced(FunctionName.RandomDistChoose));

                            break;
                    }

                    writer.WriteIf(JassExpression.Parenthesized(JassExpression.NotEqual(
                        VariableName.UnitId,
                        "-1")));

                    writer.WriteSet(
                        unitVariableName,
                        JassExpression.InvokeSpaced(
                            NativeName.CreateUnit,
                            VariableName.Player,
                            VariableName.UnitId,
                            JassLiteral.Real(unit.Position.X),
                            JassLiteral.Real(unit.Position.Y),
                            JassLiteral.Real(unit.Rotation * W3MathF.Rad2Deg, 3)));

                    WriteCreateUnitStatements(map, unit, id, writer);

                    writer.WriteEndIf();
                }
                else
                {
                    var args = new List<string>
                    {
                        VariableName.Player,
                        JassLiteral.FourCC(unit.TypeId),
                        JassLiteral.Real(unit.Position.X),
                        JassLiteral.Real(unit.Position.Y),
                        JassLiteral.Real(unit.Rotation * W3MathF.Rad2Deg, 3),
                    };

                    var skinId = unit.SkinId == 0 ? unit.TypeId : unit.SkinId;

                    var hasSkin = ForceGenerateUnitWithSkin || skinId != unit.TypeId;
                    if (hasSkin)
                    {
                        args.Add(JassLiteral.FourCC(skinId));
                    }

                    writer.WriteSet(
                        unitVariableName,
                        JassExpression.InvokeSpaced(
                            hasSkin ? NativeName.BlzCreateUnitWithSkin : NativeName.CreateUnit,
                            args.ToArray()));

                    if (unit.HeroLevel > 1)
                    {
                        writer.WriteCall(
                            NativeName.SetHeroLevel,
                            unitVariableName,
                            JassLiteral.Int(unit.HeroLevel),
                            JassKeyword.False);
                    }

                    if (unit.HeroStrength > 0)
                    {
                        writer.WriteCall(
                            NativeName.SetHeroStr,
                            unitVariableName,
                            JassLiteral.Int(unit.HeroStrength),
                            JassKeyword.True);
                    }

                    if (unit.HeroAgility > 0)
                    {
                        writer.WriteCall(
                            NativeName.SetHeroAgi,
                            unitVariableName,
                            JassLiteral.Int(unit.HeroAgility),
                            JassKeyword.True);
                    }

                    if (unit.HeroIntelligence > 0)
                    {
                        writer.WriteCall(
                            NativeName.SetHeroInt,
                            unitVariableName,
                            JassLiteral.Int(unit.HeroIntelligence),
                            JassKeyword.True);
                    }

                    WriteCreateUnitStatements(map, unit, id, writer);
                }
            }
        }

        protected internal virtual void WriteCreateUnitStatements(Map map, UnitData unit, int id, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unit is null)
            {
                throw new ArgumentNullException(nameof(unit));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var unitVariableName = unit.GetVariableName();
            if (!ForceGenerateGlobalUnitVariable && (!TriggerVariableReferences.TryGetValue(unitVariableName, out var value) || !value))
            {
                unitVariableName = VariableName.Unit;
            }

            if (unit.HP != -1)
            {
                var lifePercentLiteral = JassLiteral.Real(unit.HP * 0.01f, 2);

                if (UseLifeVariable)
                {
                    writer.WriteSet(
                        VariableName.Life,
                        JassExpression.InvokeSpaced(
                            NativeName.GetUnitState,
                            unitVariableName,
                            UnitStateName.Life));

                    writer.WriteCall(
                        NativeName.SetUnitState,
                        unitVariableName,
                        UnitStateName.Life,
                        JassExpression.Multiply(
                            lifePercentLiteral,
                            VariableName.Life));
                }
                else
                {
                    writer.WriteCall(
                        NativeName.SetUnitState,
                        unitVariableName,
                        UnitStateName.Life,
                        JassExpression.Multiply(
                            lifePercentLiteral,
                            JassExpression.Invoke(
                                NativeName.GetUnitState,
                                unitVariableName,
                                UnitStateName.Life)));
                }
            }

            if (unit.MP != -1)
            {
                writer.WriteCall(
                    NativeName.SetUnitState,
                    unitVariableName,
                    UnitStateName.Mana,
                    JassLiteral.Int(unit.MP));
            }

            if (unit.IsGoldMine())
            {
                writer.WriteCall(
                    NativeName.SetResourceAmount,
                    unitVariableName,
                    JassLiteral.Int(unit.GoldAmount));
            }

            var playerColorId = unit.CustomPlayerColorId;
            if (playerColorId == -1 && unit.TryGetDefaultPlayerColorId(out var defaultPlayerColorId))
            {
                playerColorId = defaultPlayerColorId;
            }

            if (playerColorId != -1)
            {
                writer.WriteCall(
                    NativeName.SetUnitColor,
                    unitVariableName,
                    JassExpression.Invoke(NativeName.ConvertPlayerColor, JassLiteral.Int(playerColorId)));
            }

            if (unit.TargetAcquisition != -1f)
            {
                const float CampAcquisitionRange = 200f;
                var acquisitionRange = unit.TargetAcquisition == -2f ? CampAcquisitionRange : unit.TargetAcquisition;
                writer.WriteCall(
                    NativeName.SetUnitAcquireRange,
                    unitVariableName,
                    JassLiteral.Real(acquisitionRange));
            }

            if (unit.WaygateDestinationRegionId != -1)
            {
                var destinationRect = map.Regions?.Regions.Where(region => region.CreationNumber == unit.WaygateDestinationRegionId).SingleOrDefault();
                if (destinationRect is not null)
                {
                    writer.WriteCall(
                        NativeName.WaygateSetDestination,
                        unitVariableName,
                        JassLiteral.Real(destinationRect.CenterX, 0),
                        JassLiteral.Real(destinationRect.CenterY, 0));

                    writer.WriteCall(
                        NativeName.WaygateActivate,
                        unitVariableName,
                        JassKeyword.True);
                }
            }

            foreach (var ability in unit.AbilityData)
            {
                for (var i = 0; i < ability.HeroAbilityLevel; i++)
                {
                    writer.WriteCall(
                        NativeName.SelectHeroSkill,
                        unitVariableName,
                        JassLiteral.FourCC(ability.AbilityId));
                }

                if (ability.IsAutocastActive)
                {
                    writer.WriteCall(
                        NativeName.IssueImmediateOrderById,
                        unitVariableName,
                        JassLiteral.FourCC(ability.AbilityId));
                }

                if (ability.TryGetOrderOffString(out var orderOffString))
                {
                    writer.WriteCall(
                        NativeName.IssueImmediateOrder,
                        unitVariableName,
                        JassLiteral.String(orderOffString));
                }
            }

            foreach (var item in unit.InventoryData)
            {
                writer.WriteCall(
                    NativeName.UnitAddItemToSlotById,
                    unitVariableName,
                    JassLiteral.FourCC(item.ItemId),
                    JassLiteral.Int(item.Slot));
            }

            if (unit.HasItemTable())
            {
                writer.WriteSet(
                    VariableName.Trigger,
                    JassExpression.InvokeSpaced(NativeName.CreateTrigger));

                writer.WriteCall(
                    NativeName.TriggerRegisterUnitEvent,
                    VariableName.Trigger,
                    unitVariableName,
                    UnitEventName.Death);

                if (map.Info is null || map.Info.FormatVersion >= MapInfoFormatVersion.v24)
                {
                    writer.WriteCall(
                        NativeName.TriggerRegisterUnitEvent,
                        VariableName.Trigger,
                        unitVariableName,
                        UnitEventName.ChangeOwner);
                }

                writer.WriteCall(
                    NativeName.TriggerAddAction,
                    VariableName.Trigger,
                    JassExpression.FunctionRef(unit.GetDropItemsFunctionName(id)));
            }
        }
    }
}