// ------------------------------------------------------------------------------
// <copyright file="CreateAllUnitsHelperFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Providers;

namespace War3Net.Build.Script.Main
{
    internal static partial class MainFunctionGenerator<TBuilder, TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private const string LocalUnitVariableName = "u";
        private const string LocalUnitIdVariableName = "unitID";

        private static TFunctionSyntax GenerateCreateAllUnitsHelperFunction(TBuilder builder)
        {
            var locals = new List<(string, string)>()
            {
                // player p
                (nameof(War3Api.Common.unit), LocalUnitVariableName),
                (builder.GetTypeName(BuiltinType.Int32), LocalUnitIdVariableName),
                (nameof(War3Api.Common.trigger), LocalTriggerVariableName),
                // real life
            };

            return builder.Build("CreateAllUnits", locals, GetCreateAllUnitsHelperFunctionStatements(builder));
        }

        private static IEnumerable<TStatementSyntax> GetCreateAllUnitsHelperFunctionStatements(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;

            foreach (var unit in builder.Data.MapUnits.Where(mapUnit => mapUnit.IsUnit))
            {
                if (unit.IsRandomUnit)
                {
                    var randomData = unit.RandomData;
                    switch (randomData.Mode)
                    {
                        case 0:
                            if (unit.IsRandomBuilding)
                            {
                                yield return builder.GenerateAssignmentStatement(
                                    LocalUnitVariableName,
                                    builder.GenerateInvocationExpression(
                                        nameof(War3Api.Common.ChooseRandomNPBuilding)));
                            }
                            else
                            {
                                yield return builder.GenerateAssignmentStatement(
                                    LocalUnitIdVariableName,
                                    builder.GenerateInvocationExpression(
                                        nameof(War3Api.Common.ChooseRandomCreep),
                                        builder.GenerateIntegerLiteralExpression(randomData.Level)));
                            }

                            break;

                        case 1:
                            yield return builder.GenerateAssignmentStatement(
                                LocalUnitIdVariableName,
                                builder.GenerateArrayReferenceExpression(
                                    $"gg_rg_{randomData.UnitGroupTableIndex.ToString("D3")}",
                                    builder.GenerateIntegerLiteralExpression(randomData.UnitGroupTableColumn)));
                            break;

                        case 2:
                            yield return builder.GenerateInvocationStatement(nameof(War3Api.Blizzard.RandomDistReset));
                            var summedChance = 0;
                            foreach (var randomUnitOption in randomData)
                            {
                                summedChance += randomUnitOption.chance;

                                var unitTypeExpression = RandomUnitProvider.IsRandomUnit(randomUnitOption.id, out var level)
                                    ? builder.GenerateInvocationExpression(
                                        nameof(War3Api.Common.ChooseRandomCreep),
                                        builder.GenerateIntegerLiteralExpression(level))
                                    : builder.GenerateFourCCExpression(new string(randomUnitOption.id));

                                yield return builder.GenerateInvocationStatement(
                                    nameof(War3Api.Blizzard.RandomDistAddItem),
                                    unitTypeExpression,
                                    builder.GenerateIntegerLiteralExpression(randomUnitOption.chance));
                            }

                            if (summedChance < 100)
                            {
                                yield return builder.GenerateInvocationStatement(
                                    nameof(War3Api.Blizzard.RandomDistAddItem),
                                    builder.GenerateIntegerLiteralExpression(-1),
                                    builder.GenerateIntegerLiteralExpression(100 - summedChance));
                            }

                            yield return builder.GenerateAssignmentStatement(
                                LocalUnitIdVariableName,
                                builder.GenerateInvocationExpression(nameof(War3Api.Blizzard.RandomDistChoose)));
                            break;
                    }

                    yield return builder.GenerateIfStatement(
                        builder.GenerateBinaryExpression(
                            BinaryOperator.NotEquals,
                            builder.GenerateVariableExpression(LocalUnitIdVariableName),
                            builder.GenerateIntegerLiteralExpression(-1)),
                        builder.GenerateAssignmentStatement(
                            LocalUnitVariableName,
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.CreateUnit),
                                builder.GenerateInvocationExpression(
                                    nameof(War3Api.Common.Player),
                                    builder.GenerateIntegerLiteralExpression(unit.Owner)),
                                builder.GenerateVariableExpression(LocalUnitIdVariableName),
                                builder.GenerateFloatLiteralExpression(unit.PositionX),
                                builder.GenerateFloatLiteralExpression(unit.PositionY),
                                builder.GenerateFloatLiteralExpression(unit.Facing))));
                }
                else
                {
                    yield return builder.GenerateAssignmentStatement(
                        LocalUnitVariableName,
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.CreateUnit),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.Player),
                                builder.GenerateIntegerLiteralExpression(unit.Owner)),
                            builder.GenerateFourCCExpression(unit.TypeId),
                            builder.GenerateFloatLiteralExpression(unit.PositionX),
                            builder.GenerateFloatLiteralExpression(unit.PositionY),
                            builder.GenerateFloatLiteralExpression(unit.Facing)));
                }

                // TODO: test which statements cannot be generated for random units, and put them inside the else block above (hero level/stats?)

                if (unit.HeroLevel > 1)
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetHeroLevel),
                        builder.GenerateVariableExpression(LocalUnitVariableName),
                        builder.GenerateIntegerLiteralExpression(unit.HeroLevel),
                        builder.GenerateBooleanLiteralExpression(false));
                }

                if (unit.HeroStrength > 0)
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetHeroStr),
                        builder.GenerateVariableExpression(LocalUnitVariableName),
                        builder.GenerateIntegerLiteralExpression(unit.HeroStrength),
                        builder.GenerateBooleanLiteralExpression(true));
                }

                if (unit.HeroAgility > 0)
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetHeroAgi),
                        builder.GenerateVariableExpression(LocalUnitVariableName),
                        builder.GenerateIntegerLiteralExpression(unit.HeroAgility),
                        builder.GenerateBooleanLiteralExpression(true));
                }

                if (unit.HeroIntelligence > 0)
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetHeroInt),
                        builder.GenerateVariableExpression(LocalUnitVariableName),
                        builder.GenerateIntegerLiteralExpression(unit.HeroIntelligence),
                        builder.GenerateBooleanLiteralExpression(true));
                }

                if (unit.Hp != -1)
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetUnitState),
                        builder.GenerateVariableExpression(LocalUnitVariableName),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.UNIT_STATE_LIFE)),
                        builder.GenerateBinaryExpression(
                            BinaryOperator.Multiplication,
                            builder.GenerateFloatLiteralExpression(unit.Hp * 0.01f),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.GetUnitState),
                                builder.GenerateVariableExpression(LocalUnitVariableName),
                                builder.GenerateVariableExpression(nameof(War3Api.Common.UNIT_STATE_LIFE)))));
                }

                if (unit.Mp != -1)
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetUnitState),
                        builder.GenerateVariableExpression(LocalUnitVariableName),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.UNIT_STATE_MANA)),
                        builder.GenerateFloatLiteralExpression(unit.Mp));
                }

                if (unit.TypeId == "ngol")
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetResourceAmount),
                        builder.GenerateVariableExpression(LocalUnitVariableName),
                        builder.GenerateIntegerLiteralExpression(unit.GoldAmount));
                }

                if (unit.TargetAcquisition != -1)
                {
                    const float CampAcquisitionRange = 200f;
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetUnitAcquireRange),
                        builder.GenerateVariableExpression(LocalUnitVariableName),
                        builder.GenerateFloatLiteralExpression(unit.TargetAcquisition == -2 ? CampAcquisitionRange : unit.TargetAcquisition));
                }

                // TODO: CustomPlayerColor
                // TODO: WaygateDestination (requires parsing war3map.w3r)
                // TODO: CreationNumber? (only used to declare global var if unit is referenced in triggers?, ie useless)

                foreach (var ability in unit.AbilityData)
                {
                    for (var i = 0; i < ability.Level; i++)
                    {
                        // TODO: make sure Level is 0 for non-hero abilities (can confirm this by checking second char is uppercase?)
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SelectHeroSkill),
                            builder.GenerateVariableExpression(LocalUnitVariableName),
                            builder.GenerateFourCCExpression(ability.Id));
                    }

                    if (ability.IsActive)
                    {
#if false
                            yield return builder.GenerateInvocationStatement(
                                nameof(War3Api.Common.IssueImmediateOrder),
                                builder.GenerateVariableExpression(LocalUnitVariableName),
                                builder.GenerateStringLiteralExpression(ability.OrderString));
#else
                        // TODO: test if this works
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.IssueImmediateOrderById),
                            builder.GenerateVariableExpression(LocalUnitVariableName),
                            builder.GenerateFourCCExpression(ability.Id));
#endif
                    }
                }

                foreach (var item in unit.Inventory)
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.UnitAddItemToSlotById),
                        builder.GenerateVariableExpression(LocalUnitVariableName),
                        builder.GenerateFourCCExpression(item.Id),
                        builder.GenerateIntegerLiteralExpression(item.Slot));
                }

                if (unit.MapItemTablePointer != -1 || unit.DroppedItemData.FirstOrDefault() != null)
                {
                    yield return builder.GenerateAssignmentStatement(
                        LocalTriggerVariableName,
                        builder.GenerateInvocationExpression(nameof(War3Api.Common.CreateTrigger)));

                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.TriggerRegisterUnitEvent),
                        builder.GenerateVariableExpression(LocalTriggerVariableName),
                        builder.GenerateVariableExpression(LocalUnitVariableName),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.EVENT_UNIT_DEATH)));

                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.TriggerRegisterUnitEvent),
                        builder.GenerateVariableExpression(LocalTriggerVariableName),
                        builder.GenerateVariableExpression(LocalUnitVariableName),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.EVENT_UNIT_CHANGE_OWNER)));

                    if (unit.MapItemTablePointer != -1)
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.TriggerAddAction),
                            builder.GenerateVariableExpression(LocalTriggerVariableName),
                            builder.GenerateFunctionReferenceExpression($"ItemTable{mapInfo.GetItemTable(unit.MapItemTablePointer).Index.ToString("D6")}_DropItems"));
                    }
                    else
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.TriggerAddAction),
                            builder.GenerateVariableExpression(LocalTriggerVariableName),
                            builder.GenerateFunctionReferenceExpression($"Unit{unit.CreationNumber.ToString("D6")}_DropItems"));
                    }
                }
            }
        }
    }
}