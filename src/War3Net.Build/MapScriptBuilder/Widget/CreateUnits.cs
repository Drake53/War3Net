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
using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual IEnumerable<JassStatementSyntax> CreateUnits(Map map, IEnumerable<(UnitData Unit, int Id)> units, JassExpressionSyntax playerNumber)
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

            var statements = new List<JassStatementSyntax>();
            statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(SyntaxFactory.ParseTypeName(TypeName.Player), VariableName.Player, SyntaxFactory.InvocationExpression(NativeName.Player, playerNumber)));
            if (!ForceGenerateGlobalUnitVariable)
            {
                statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(SyntaxFactory.ParseTypeName(TypeName.Unit), VariableName.Unit));
            }

            statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(JassPredefinedTypeSyntax.Integer, VariableName.UnitId));
            statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(SyntaxFactory.ParseTypeName(TypeName.Trigger), VariableName.Trigger));
            if (UseLifeVariable)
            {
                statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(JassPredefinedTypeSyntax.Real, VariableName.Life));
            }

            //statements.Add(JassEmptySyntax.Value);

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
                                statements.Add(SyntaxFactory.SetStatement(
                                    VariableName.UnitId,
                                    SyntaxFactory.InvocationExpression(NativeName.ChooseRandomNPBuilding)));
                            }
                            else
                            {
                                statements.Add(SyntaxFactory.SetStatement(
                                    VariableName.UnitId,
                                    SyntaxFactory.InvocationExpression(NativeName.ChooseRandomCreep, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(randomUnitAny.Level)))));
                            }

                            break;

                        case RandomUnitGlobalTable randomUnitGlobalTable:
                            statements.Add(SyntaxFactory.SetStatement(
                                VariableName.UnitId,
                                SyntaxFactory.ElementAccessExpression(randomUnitGlobalTable.GetVariableName(), SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(randomUnitGlobalTable.Column)))));

                            break;

                        case RandomUnitCustomTable randomUnitCustomTable:
                            statements.Add(SyntaxFactory.CallStatement(FunctionName.RandomDistReset));

                            var summedChance = 0;
                            foreach (var randomUnit in randomUnitCustomTable.RandomUnits)
                            {
                                statements.Add(SyntaxFactory.CallStatement(
                                    FunctionName.RandomDistAddItem,
                                    RandomUnitProvider.IsRandomUnit(randomUnit.UnitId, out var level)
                                        ? SyntaxFactory.InvocationExpression(NativeName.ChooseRandomCreep, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(level)))
                                        : SyntaxFactory.LiteralExpression(SyntaxFactory.FourCCLiteral(randomUnit.UnitId)),
                                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(randomUnit.Chance))));

                                summedChance += randomUnit.Chance;
                            }

                            if (summedChance < 100)
                            {
                                statements.Add(SyntaxFactory.CallStatement(
                                    FunctionName.RandomDistAddItem,
                                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(-1)),
                                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(100 - summedChance))));
                            }

                            statements.Add(SyntaxFactory.SetStatement(
                                VariableName.UnitId,
                                SyntaxFactory.InvocationExpression(FunctionName.RandomDistChoose)));

                            break;
                    }

                    var ifBodyStatements = new List<JassStatementSyntax>();
                    ifBodyStatements.Add(SyntaxFactory.SetStatement(
                        unitVariableName,
                        SyntaxFactory.InvocationExpression(
                            NativeName.CreateUnit,
                            SyntaxFactory.ParseIdentifierName(VariableName.Player),
                            SyntaxFactory.ParseIdentifierName(VariableName.UnitId),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.Position.X, precision: 1)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.Position.Y, precision: 1)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.Rotation * (180f / MathF.PI), precision: 3)))));

                    ifBodyStatements.AddRange(GetCreateUnitStatements(map, unit, id));

                    statements.Add(SyntaxFactory.IfStatement(
                        SyntaxFactory.ParenthesizedExpression(SyntaxFactory.BinaryNotEqualsExpression(SyntaxFactory.ParseIdentifierName(VariableName.UnitId), SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(-1)))),
                        ifBodyStatements.ToArray()));
                }
                else
                {
                    var args = new List<JassExpressionSyntax>()
                    {
                        SyntaxFactory.ParseIdentifierName(VariableName.Player),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.FourCCLiteral(unit.TypeId)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.Position.X, precision: 1)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.Position.Y, precision: 1)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.Rotation * (180f / MathF.PI), precision: 3)),
                    };

                    var skinId = unit.SkinId == 0 ? unit.TypeId : unit.SkinId;

                    var hasSkin = ForceGenerateUnitWithSkin || skinId != unit.TypeId;
                    if (hasSkin)
                    {
                        args.Add(SyntaxFactory.LiteralExpression(SyntaxFactory.FourCCLiteral(skinId)));
                    }

                    statements.Add(SyntaxFactory.SetStatement(
                        unitVariableName,
                        SyntaxFactory.InvocationExpression(hasSkin ? NativeName.BlzCreateUnitWithSkin : NativeName.CreateUnit, args.ToArray())));

                    if (unit.HeroLevel > 1)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetHeroLevel,
                            SyntaxFactory.ParseIdentifierName(unitVariableName),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.HeroLevel)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(false))));
                    }

                    if (unit.HeroStrength > 0)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetHeroStr,
                            SyntaxFactory.ParseIdentifierName(unitVariableName),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.HeroStrength)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(true))));
                    }

                    if (unit.HeroAgility > 0)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetHeroAgi,
                            SyntaxFactory.ParseIdentifierName(unitVariableName),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.HeroAgility)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(true))));
                    }

                    if (unit.HeroIntelligence > 0)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetHeroInt,
                            SyntaxFactory.ParseIdentifierName(unitVariableName),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.HeroIntelligence)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(true))));
                    }

                    statements.AddRange(GetCreateUnitStatements(map, unit, id));
                }
            }

            return statements;
        }

        protected internal virtual IEnumerable<JassStatementSyntax> GetCreateUnitStatements(Map map, UnitData unit, int id)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unit is null)
            {
                throw new ArgumentNullException(nameof(unit));
            }

            var randomItemTables = map.Info?.RandomItemTables;

            var statements = new List<JassStatementSyntax>();

            var unitVariableName = unit.GetVariableName();
            if (!ForceGenerateGlobalUnitVariable && (!TriggerVariableReferences.TryGetValue(unitVariableName, out var value) || !value))
            {
                unitVariableName = VariableName.Unit;
            }

            if (unit.HP != -1)
            {
                if (UseLifeVariable)
                {
                    statements.Add(SyntaxFactory.SetStatement(
                        VariableName.Life,
                        SyntaxFactory.InvocationExpression(
                            NativeName.GetUnitState,
                            SyntaxFactory.ParseIdentifierName(unitVariableName),
                            SyntaxFactory.ParseIdentifierName(UnitStateName.Life))));

                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.SetUnitState,
                        SyntaxFactory.ParseIdentifierName(unitVariableName),
                        SyntaxFactory.ParseIdentifierName(UnitStateName.Life),
                        SyntaxFactory.BinaryMultiplicationExpression(
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.HP * 0.01f, precision: 2)),
                            SyntaxFactory.ParseIdentifierName(VariableName.Life))));
                }
                else
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.SetUnitState,
                        SyntaxFactory.ParseIdentifierName(unitVariableName),
                        SyntaxFactory.ParseIdentifierName(UnitStateName.Life),
                        SyntaxFactory.BinaryMultiplicationExpression(
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.HP * 0.01f, precision: 2)),
                            SyntaxFactory.InvocationExpression(
                                NativeName.GetUnitState,
                                SyntaxFactory.ParseIdentifierName(unitVariableName),
                                SyntaxFactory.ParseIdentifierName(UnitStateName.Life)))));
                }
            }

            if (unit.MP != -1)
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetUnitState,
                    SyntaxFactory.ParseIdentifierName(unitVariableName),
                    SyntaxFactory.ParseIdentifierName(UnitStateName.Mana),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.MP))));
            }

            if (unit.IsGoldMine())
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetResourceAmount,
                    SyntaxFactory.ParseIdentifierName(unitVariableName),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.GoldAmount))));
            }

            var playerColorId = unit.CustomPlayerColorId;
            if (playerColorId == -1 && unit.TryGetDefaultPlayerColorId(out var defaultPlayerColorId))
            {
                playerColorId = defaultPlayerColorId;
            }

            if (playerColorId != -1)
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetUnitColor,
                    SyntaxFactory.ParseIdentifierName(unitVariableName),
                    SyntaxFactory.InvocationExpression(NativeName.ConvertPlayerColor, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(playerColorId)))));
            }

            if (unit.TargetAcquisition != -1f)
            {
                const float CampAcquisitionRange = 200f;
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetUnitAcquireRange,
                    SyntaxFactory.ParseIdentifierName(unitVariableName),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unit.TargetAcquisition == -2f ? CampAcquisitionRange : unit.TargetAcquisition, precision: 1))));
            }

            if (unit.WaygateDestinationRegionId != -1)
            {
                var destinationRect = map.Regions?.Regions.Where(region => region.CreationNumber == unit.WaygateDestinationRegionId).SingleOrDefault();
                if (destinationRect is not null)
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.WaygateSetDestination,
                        SyntaxFactory.ParseIdentifierName(unitVariableName),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(destinationRect.CenterX)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(destinationRect.CenterY))));

                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.WaygateActivate,
                        SyntaxFactory.ParseIdentifierName(unitVariableName),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(true))));
                }
            }

            foreach (var ability in unit.AbilityData)
            {
                for (var i = 0; i < ability.HeroAbilityLevel; i++)
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.SelectHeroSkill,
                        SyntaxFactory.ParseIdentifierName(unitVariableName),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.FourCCLiteral(ability.AbilityId))));
                }

                if (ability.IsAutocastActive)
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.IssueImmediateOrderById,
                        SyntaxFactory.ParseIdentifierName(unitVariableName),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.FourCCLiteral(ability.AbilityId))));
                }

                if (ability.TryGetOrderOffString(out var orderOffString))
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.IssueImmediateOrder,
                        SyntaxFactory.ParseIdentifierName(unitVariableName),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(orderOffString))));
                }
            }

            foreach (var item in unit.InventoryData)
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.UnitAddItemToSlotById,
                    SyntaxFactory.ParseIdentifierName(unitVariableName),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.FourCCLiteral(item.ItemId)),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(item.Slot))));
            }

            if (unit.HasItemTable())
            {
                statements.Add(SyntaxFactory.SetStatement(VariableName.Trigger, SyntaxFactory.InvocationExpression(NativeName.CreateTrigger)));

                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.TriggerRegisterUnitEvent,
                    SyntaxFactory.ParseIdentifierName(VariableName.Trigger),
                    SyntaxFactory.ParseIdentifierName(unitVariableName),
                    SyntaxFactory.ParseIdentifierName(UnitEventName.Death)));

                if (map.Info is null || map.Info.FormatVersion >= MapInfoFormatVersion.v24)
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.TriggerRegisterUnitEvent,
                        SyntaxFactory.ParseIdentifierName(VariableName.Trigger),
                        SyntaxFactory.ParseIdentifierName(unitVariableName),
                        SyntaxFactory.ParseIdentifierName(UnitEventName.ChangeOwner)));
                }

                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.TriggerAddAction,
                    SyntaxFactory.ParseIdentifierName(VariableName.Trigger),
                    SyntaxFactory.FunctionReferenceExpression(unit.GetDropItemsFunctionName(id))));
            }

            return statements;
        }
    }
}