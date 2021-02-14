// ------------------------------------------------------------------------------
// <copyright file="CreateUnits.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Extensions;
using War3Net.Build.Providers;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Syntax;

using static War3Api.Common;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual IEnumerable<IStatementSyntax> CreateUnits(Map map, IEnumerable<(UnitData Unit, int Id)> units, IExpressionSyntax playerNumber)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (units is null)
            {
                throw new ArgumentNullException(nameof(units));
            }

            if (playerNumber is null)
            {
                throw new ArgumentNullException(nameof(playerNumber));
            }

            var statements = new List<IStatementSyntax>();
            statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(SyntaxFactory.ParseTypeName(nameof(player)), VariableName.Player, SyntaxFactory.InvocationExpression(nameof(Player), playerNumber)));
            if (!ForceGenerateGlobalUnitVariable)
            {
                statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(SyntaxFactory.ParseTypeName(nameof(unit)), VariableName.Unit));
            }

            statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(JassTypeSyntax.Integer, VariableName.UnitId));
            statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(SyntaxFactory.ParseTypeName(nameof(trigger)), VariableName.Trigger));
            if (UseLifeVariable)
            {
                statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(JassTypeSyntax.Real, VariableName.Life));
            }

            statements.Add(JassEmptyStatementSyntax.Value);

            foreach (var (unit, id) in units.OrderBy(pair => pair.Unit.CreationNumber))
            {
                var unitVariableName = ForceGenerateGlobalUnitVariable ? unit.GetVariableName() : VariableName.Unit;

                if (unit.IsRandomUnit() || unit.IsRandomBuilding())
                {
                    var randomData = unit.RandomData;
                    switch (randomData)
                    {
                        case RandomUnitAny randomUnitAny:
                            if (unit.IsRandomBuilding())
                            {
                                statements.Add(SyntaxFactory.SetStatement(
                                    VariableName.UnitId,
                                    SyntaxFactory.InvocationExpression(nameof(ChooseRandomNPBuilding))));
                            }
                            else
                            {
                                statements.Add(SyntaxFactory.SetStatement(
                                    VariableName.UnitId,
                                    SyntaxFactory.InvocationExpression(nameof(ChooseRandomCreep), SyntaxFactory.LiteralExpression(randomUnitAny.Level))));
                            }

                            break;

                        case RandomUnitGlobalTable randomUnitGlobalTable:
                            statements.Add(SyntaxFactory.SetStatement(
                                VariableName.UnitId,
                                SyntaxFactory.ArrayReferenceExpression(randomUnitGlobalTable.GetVariableName(), SyntaxFactory.LiteralExpression(randomUnitGlobalTable.Column))));

                            break;

                        case RandomUnitCustomTable randomUnitCustomTable:
                            statements.Add(SyntaxFactory.CallStatement(nameof(War3Api.Blizzard.RandomDistReset)));

                            var summedChance = 0;
                            foreach (var randomUnit in randomUnitCustomTable.RandomUnits)
                            {
                                statements.Add(SyntaxFactory.CallStatement(
                                    nameof(War3Api.Blizzard.RandomDistAddItem),
                                    RandomUnitProvider.IsRandomUnit(randomUnit.UnitId, out var level)
                                        ? SyntaxFactory.InvocationExpression(nameof(ChooseRandomCreep), SyntaxFactory.LiteralExpression(level))
                                        : SyntaxFactory.FourCCLiteralExpression(randomUnit.UnitId),
                                    SyntaxFactory.LiteralExpression(randomUnit.Chance)));

                                summedChance += randomUnit.Chance;
                            }

                            if (summedChance < 100)
                            {
                                statements.Add(SyntaxFactory.CallStatement(
                                    nameof(War3Api.Blizzard.RandomDistAddItem),
                                    SyntaxFactory.LiteralExpression(-1),
                                    SyntaxFactory.LiteralExpression(100 - summedChance)));
                            }

                            statements.Add(SyntaxFactory.SetStatement(
                                VariableName.UnitId,
                                SyntaxFactory.InvocationExpression(nameof(War3Api.Blizzard.RandomDistChoose))));

                            break;
                    }

                    var ifBodyStatements = new List<IStatementSyntax>();
                    ifBodyStatements.Add(SyntaxFactory.SetStatement(
                        unitVariableName,
                        SyntaxFactory.InvocationExpression(
                            nameof(CreateUnit),
                            SyntaxFactory.VariableReferenceExpression(VariableName.Player),
                            SyntaxFactory.VariableReferenceExpression(VariableName.UnitId),
                            SyntaxFactory.LiteralExpression(unit.Position.X, precision: 1),
                            SyntaxFactory.LiteralExpression(unit.Position.Y, precision: 1),
                            SyntaxFactory.LiteralExpression(unit.Rotation * (180f / MathF.PI), precision: 3))));

                    ifBodyStatements.AddRange(GetCreateUnitStatements(map, unit, id));

                    statements.Add(SyntaxFactory.IfStatement(
                        new JassParenthesizedExpressionSyntax(SyntaxFactory.BinaryNotEqualsExpression(SyntaxFactory.VariableReferenceExpression(VariableName.UnitId), SyntaxFactory.LiteralExpression(-1))),
                        ifBodyStatements.ToArray()));
                }
                else
                {
                    var args = new List<IExpressionSyntax>()
                    {
                        SyntaxFactory.VariableReferenceExpression(VariableName.Player),
                        SyntaxFactory.FourCCLiteralExpression(unit.TypeId),
                        SyntaxFactory.LiteralExpression(unit.Position.X, precision: 1),
                        SyntaxFactory.LiteralExpression(unit.Position.Y, precision: 1),
                        SyntaxFactory.LiteralExpression(unit.Rotation * (180f / MathF.PI), precision: 3),
                    };

                    var hasSkin = unit.SkinId != 0 && unit.SkinId != unit.TypeId;
                    if (hasSkin)
                    {
                        args.Add(SyntaxFactory.FourCCLiteralExpression(unit.SkinId));
                    }

                    statements.Add(SyntaxFactory.SetStatement(
                        unitVariableName,
                        SyntaxFactory.InvocationExpression(hasSkin ? nameof(BlzCreateUnitWithSkin) : nameof(CreateUnit), args.ToArray())));

                    if (unit.HeroLevel > 1)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            nameof(SetHeroLevel),
                            SyntaxFactory.VariableReferenceExpression(unitVariableName),
                            SyntaxFactory.LiteralExpression(unit.HeroLevel),
                            SyntaxFactory.LiteralExpression(false)));
                    }

                    if (unit.HeroStrength > 0)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            nameof(SetHeroStr),
                            SyntaxFactory.VariableReferenceExpression(unitVariableName),
                            SyntaxFactory.LiteralExpression(unit.HeroStrength),
                            SyntaxFactory.LiteralExpression(true)));
                    }

                    if (unit.HeroAgility > 0)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            nameof(SetHeroAgi),
                            SyntaxFactory.VariableReferenceExpression(unitVariableName),
                            SyntaxFactory.LiteralExpression(unit.HeroAgility),
                            SyntaxFactory.LiteralExpression(true)));
                    }

                    if (unit.HeroIntelligence > 0)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            nameof(SetHeroInt),
                            SyntaxFactory.VariableReferenceExpression(unitVariableName),
                            SyntaxFactory.LiteralExpression(unit.HeroIntelligence),
                            SyntaxFactory.LiteralExpression(true)));
                    }

                    statements.AddRange(GetCreateUnitStatements(map, unit, id));
                }
            }

            return statements;
        }

        protected virtual IEnumerable<IStatementSyntax> GetCreateUnitStatements(Map map, UnitData unit, int id)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unit is null)
            {
                throw new ArgumentNullException(nameof(unit));
            }

            var randomItemTables = map.Info.RandomItemTables;

            var statements = new List<IStatementSyntax>();

            var unitVariableName = ForceGenerateGlobalUnitVariable ? unit.GetVariableName() : VariableName.Unit;

            if (unit.HP != -1)
            {
                if (UseLifeVariable)
                {
                    statements.Add(SyntaxFactory.SetStatement(
                        VariableName.Life,
                        SyntaxFactory.InvocationExpression(
                            nameof(GetUnitState),
                            SyntaxFactory.VariableReferenceExpression(unitVariableName),
                            SyntaxFactory.VariableReferenceExpression(nameof(UNIT_STATE_LIFE)))));

                    statements.Add(SyntaxFactory.CallStatement(
                        nameof(SetUnitState),
                        SyntaxFactory.VariableReferenceExpression(unitVariableName),
                        SyntaxFactory.VariableReferenceExpression(nameof(UNIT_STATE_LIFE)),
                        SyntaxFactory.BinaryMultiplicationExpression(
                            SyntaxFactory.LiteralExpression(unit.HP * 0.01f, precision: 2),
                            SyntaxFactory.VariableReferenceExpression(VariableName.Life))));
                }
                else
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        nameof(SetUnitState),
                        SyntaxFactory.VariableReferenceExpression(unitVariableName),
                        SyntaxFactory.VariableReferenceExpression(nameof(UNIT_STATE_LIFE)),
                        SyntaxFactory.BinaryMultiplicationExpression(
                            SyntaxFactory.LiteralExpression(unit.HP * 0.01f, precision: 2),
                            SyntaxFactory.InvocationExpression(
                                nameof(GetUnitState),
                                SyntaxFactory.VariableReferenceExpression(unitVariableName),
                                SyntaxFactory.VariableReferenceExpression(nameof(UNIT_STATE_LIFE))))));
                }
            }

            if (unit.MP != -1)
            {
                statements.Add(SyntaxFactory.CallStatement(
                    nameof(SetUnitState),
                    SyntaxFactory.VariableReferenceExpression(unitVariableName),
                    SyntaxFactory.VariableReferenceExpression(nameof(UNIT_STATE_MANA)),
                    SyntaxFactory.LiteralExpression(unit.MP)));
            }

            if (unit.IsGoldMine())
            {
                statements.Add(SyntaxFactory.CallStatement(
                    nameof(SetResourceAmount),
                    SyntaxFactory.VariableReferenceExpression(unitVariableName),
                    SyntaxFactory.LiteralExpression(unit.GoldAmount)));
            }

            if (unit.CustomPlayerColorId != -1)
            {
                statements.Add(SyntaxFactory.CallStatement(
                    nameof(SetUnitColor),
                    SyntaxFactory.VariableReferenceExpression(unitVariableName),
                    SyntaxFactory.InvocationExpression(nameof(ConvertPlayerColor), SyntaxFactory.LiteralExpression(unit.CustomPlayerColorId))));
            }

            if (unit.TargetAcquisition != -1f)
            {
                const float CampAcquisitionRange = 200f;
                statements.Add(SyntaxFactory.CallStatement(
                    nameof(SetUnitAcquireRange),
                    SyntaxFactory.VariableReferenceExpression(unitVariableName),
                    SyntaxFactory.LiteralExpression(unit.TargetAcquisition == -2f ? CampAcquisitionRange : unit.TargetAcquisition, precision: 1)));
            }

            if (unit.WaygateDestinationRegionId != -1)
            {
                var destinationRect = map.Regions?.Regions.Where(region => region.CreationNumber == unit.WaygateDestinationRegionId).SingleOrDefault();
                if (destinationRect is not null)
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        nameof(WaygateSetDestination),
                        SyntaxFactory.VariableReferenceExpression(unitVariableName),
                        SyntaxFactory.LiteralExpression(destinationRect.CenterX),
                        SyntaxFactory.LiteralExpression(destinationRect.CenterY)));

                    statements.Add(SyntaxFactory.CallStatement(
                        nameof(WaygateActivate),
                        SyntaxFactory.VariableReferenceExpression(unitVariableName),
                        SyntaxFactory.LiteralExpression(true)));
                }
            }

            foreach (var ability in unit.AbilityData)
            {
                for (var i = 0; i < ability.HeroAbilityLevel; i++)
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        nameof(SelectHeroSkill),
                        SyntaxFactory.VariableReferenceExpression(unitVariableName),
                        SyntaxFactory.FourCCLiteralExpression(ability.AbilityId)));
                }

                if (ability.IsAutocastActive)
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        nameof(IssueImmediateOrderById),
                        SyntaxFactory.VariableReferenceExpression(unitVariableName),
                        SyntaxFactory.FourCCLiteralExpression(ability.AbilityId)));
                }
            }

            foreach (var item in unit.InventoryData)
            {
                statements.Add(SyntaxFactory.CallStatement(
                    nameof(UnitAddItemToSlotById),
                    SyntaxFactory.VariableReferenceExpression(unitVariableName),
                    SyntaxFactory.FourCCLiteralExpression(item.ItemId),
                    SyntaxFactory.LiteralExpression(item.Slot)));
            }

            if (unit.ItemTableSets.Any() || (unit.MapItemTableId != -1 && randomItemTables is not null))
            {
                statements.Add(SyntaxFactory.SetStatement(VariableName.Trigger, SyntaxFactory.InvocationExpression(nameof(CreateTrigger))));

                statements.Add(SyntaxFactory.CallStatement(
                    nameof(TriggerRegisterUnitEvent),
                    SyntaxFactory.VariableReferenceExpression(VariableName.Trigger),
                    SyntaxFactory.VariableReferenceExpression(unitVariableName),
                    SyntaxFactory.VariableReferenceExpression(nameof(EVENT_UNIT_DEATH))));

                statements.Add(SyntaxFactory.CallStatement(
                    nameof(TriggerRegisterUnitEvent),
                    SyntaxFactory.VariableReferenceExpression(VariableName.Trigger),
                    SyntaxFactory.VariableReferenceExpression(unitVariableName),
                    SyntaxFactory.VariableReferenceExpression(nameof(EVENT_UNIT_CHANGE_OWNER))));

                statements.Add(SyntaxFactory.CallStatement(
                    nameof(TriggerAddAction),
                    SyntaxFactory.VariableReferenceExpression(VariableName.Trigger),
                    SyntaxFactory.FunctionReferenceExpression(unit.GetDropItemsFunctionName(id))));
            }

            return statements;
        }
    }
}