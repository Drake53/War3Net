// ------------------------------------------------------------------------------
// <copyright file="MainFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Info;
using War3Net.Build.Providers;

namespace War3Net.Build.Script.Main
{
    internal static partial class MainFunctionGenerator<TBuilder, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TBuilder : FunctionBuilder<TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TExpressionSyntax : class
    {
        private const string MusicName = "Music";
        private const bool MusicRandom = true;
        private const int MusicIndex = 0;

        private const string LocalDestructableVariableName = "d";
        private const string LocalUnitVariableName = "u";
        private const string LocalUnitIdVariableName = "unitID";
        private const string LocalItemIdVariableName = "itemID";
        private const string LocalTriggerVariableName = "t";

        public static IEnumerable<TFunctionSyntax> GetFunctions(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;
            for (var i = 0; i < mapInfo.RandomItemTableCount; i++)
            {
                yield return GenerateItemTableHelperFunction(builder, i);
            }

            var locals = new List<(string, string)>()
            {
                // Note: typenames are for JASS, but should be abstracted away (doesn't matter for now though since don't use typename when declaring local in lua)
                (nameof(War3Api.Common.destructable), LocalDestructableVariableName),
                (nameof(War3Api.Common.unit), LocalUnitVariableName),
                // var integerKeyword = CodeAnalysis.Jass.SyntaxToken.GetDefaultTokenValue(CodeAnalysis.Jass.SyntaxTokenType.IntegerKeyword);
                ("integer", LocalUnitIdVariableName),
                ("integer", LocalItemIdVariableName),
                (nameof(War3Api.Common.trigger), LocalTriggerVariableName),
            };

            yield return builder.Build("main", locals, GetStatements(builder));
        }

        // TODO: split into multiple methods, one per function
        private static IEnumerable<TStatementSyntax> GetStatements(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetCameraBounds),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Addition,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.BottomLeft.X),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_LEFT)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Addition,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.BottomLeft.Y),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_BOTTOM)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Subtraction,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.TopRight.X),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_RIGHT)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Subtraction,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.TopRight.Y),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_TOP)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Addition,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.TopLeft.X),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_LEFT)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Subtraction,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.TopLeft.Y),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_TOP)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Subtraction,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.BottomRight.X),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_RIGHT)))),
                builder.GenerateBinaryExpression(
                    BinaryOperator.Addition,
                    builder.GenerateFloatLiteralExpression(mapInfo.CameraBounds.BottomRight.Y),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.GetCameraMargin),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.CAMERA_MARGIN_BOTTOM)))));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetDayNightModels),
                builder.GenerateStringLiteralExpression(LightEnvironmentProvider.GetTerrainLightEnvironmentModel(mapInfo.LightEnvironment)),
                builder.GenerateStringLiteralExpression(LightEnvironmentProvider.GetUnitLightEnvironmentModel(mapInfo.LightEnvironment)));

            if (mapInfo.MapFlags.HasFlag(MapFlags.HasTerrainFog))
            {
                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.SetTerrainFogEx),
                    builder.GenerateIntegerLiteralExpression((int)mapInfo.FogStyle),
                    builder.GenerateFloatLiteralExpression(mapInfo.FogStartZ),
                    builder.GenerateFloatLiteralExpression(mapInfo.FogEndZ),
                    builder.GenerateFloatLiteralExpression(mapInfo.FogDensity),
                    builder.GenerateFloatLiteralExpression(mapInfo.FogColor.R / 255f),
                    builder.GenerateFloatLiteralExpression(mapInfo.FogColor.G / 255f),
                    builder.GenerateFloatLiteralExpression(mapInfo.FogColor.B / 255f));
            }

            if (mapInfo.GlobalWeather != WeatherType.None)
            {
                // TODO: use GetWorldBounds or get coords from w3i/w3e
                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.EnableWeatherEffect),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.AddWeatherEffect),
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.Rect),
                            builder.GenerateFloatLiteralExpression(-999999),
                            builder.GenerateFloatLiteralExpression(-999999),
                            builder.GenerateFloatLiteralExpression(999999),
                            builder.GenerateFloatLiteralExpression(999999)),
                        builder.GenerateIntegerLiteralExpression((int)mapInfo.GlobalWeather)),
                    builder.GenerateBooleanLiteralExpression(true));
            }

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.NewSoundEnvironment),
                builder.GenerateStringLiteralExpression(mapInfo.SoundEnvironment));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Blizzard.SetAmbientDaySound),
                builder.GenerateStringLiteralExpression(SoundEnvironmentProvider.GetAmbientDaySound(mapInfo.Tileset)));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Blizzard.SetAmbientNightSound),
                builder.GenerateStringLiteralExpression(SoundEnvironmentProvider.GetAmbientNightSound(mapInfo.Tileset)));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetMapMusic),
                builder.GenerateStringLiteralExpression(MusicName),
                builder.GenerateBooleanLiteralExpression(MusicRandom),
                builder.GenerateIntegerLiteralExpression(MusicIndex));

            if (builder.Data.MapDoodads != null)
            {
                var localDeclaration = builder.GenerateLocalDeclarationStatement(LocalDestructableVariableName);
                if (localDeclaration != null)
                {
                    yield return localDeclaration;
                }

                foreach (var destructable in builder.Data.MapDoodads.Where(mapDoodad => mapDoodad.DroppedItemData.FirstOrDefault() != null))
                {
                    yield return builder.GenerateAssignmentStatement(
                        LocalDestructableVariableName,
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.CreateDestructable),
                            builder.GenerateFourCCExpression(destructable.TypeId),
                            builder.GenerateFloatLiteralExpression(destructable.PositionX),
                            builder.GenerateFloatLiteralExpression(destructable.PositionY),
                            builder.GenerateFloatLiteralExpression(destructable.Facing),
                            builder.GenerateFloatLiteralExpression(destructable.ScaleX),
                            builder.GenerateIntegerLiteralExpression(destructable.Variation)));

                    if (destructable.Life != 100)
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetDestructableLife),
                            builder.GenerateVariableExpression(LocalDestructableVariableName),
                            builder.GenerateBinaryExpression(
                                BinaryOperator.Multiplication,
                                builder.GenerateFloatLiteralExpression(destructable.Life * 0.01f),
                                builder.GenerateInvocationExpression(
                                    nameof(War3Api.Common.GetDestructableLife),
                                    builder.GenerateVariableExpression(LocalDestructableVariableName))));
                    }

                    foreach (var droppedItem in destructable.DroppedItemData)
                    {
                        // TODO: implement
                        // Create trigger ItemTable######_DropItems or Doodad######_DropItems, add event TriggerRegisterDeathEvent and action SaveDyingWidget
                    }
                }
            }

            if (builder.Data.MapUnits != null)
            {
                foreach (var localDeclaration in builder.GenerateLocalDeclarationStatements(
                    LocalUnitVariableName,
                    LocalUnitIdVariableName,
                    LocalItemIdVariableName))
                {
                    if (localDeclaration != null)
                    {
                        yield return localDeclaration;
                    }
                }

                foreach (var item in builder.Data.MapUnits.Where(mapUnit => mapUnit.IsItem))
                {
                    if (item.TypeId == "sloc")
                    {
                        continue;
                    }

                    if (item.IsRandomItem)
                    {
                        var randomData = item.RandomData;
                        switch (randomData.Mode)
                        {
                            case 0:
                                yield return builder.GenerateAssignmentStatement(
                                    LocalItemIdVariableName,
                                    builder.GenerateInvocationExpression(
                                        nameof(War3Api.Common.ChooseRandomItemEx),
                                        builder.GenerateInvocationExpression(
                                            nameof(War3Api.Common.ConvertItemType),
                                            builder.GenerateIntegerLiteralExpression(randomData.ItemClass)),
                                        builder.GenerateIntegerLiteralExpression(randomData.ItemLevel)));
                                break;

                            case 2:
                                yield return builder.GenerateInvocationStatement(nameof(War3Api.Blizzard.RandomDistReset));
                                var summedChance = 0;
                                foreach (var randomItemOption in randomData)
                                {
                                    summedChance += randomItemOption.chance;

                                    var itemTypeExpression = RandomItemProvider.IsRandomItem(randomItemOption.id, out var level, out var @class)
                                        ? builder.GenerateInvocationExpression(
                                            nameof(War3Api.Common.ChooseRandomItemEx),
                                            builder.GenerateInvocationExpression(
                                                nameof(War3Api.Common.ConvertItemType),
                                                builder.GenerateIntegerLiteralExpression(@class)),
                                            builder.GenerateIntegerLiteralExpression(level))
                                        : builder.GenerateFourCCExpression(new string(randomItemOption.id));

                                    yield return builder.GenerateInvocationStatement(
                                        nameof(War3Api.Blizzard.RandomDistAddItem),
                                        itemTypeExpression,
                                        builder.GenerateIntegerLiteralExpression(randomItemOption.chance));
                                }

                                if (summedChance < 100)
                                {
                                    yield return builder.GenerateInvocationStatement(
                                        nameof(War3Api.Blizzard.RandomDistAddItem),
                                        builder.GenerateIntegerLiteralExpression(-1),
                                        builder.GenerateIntegerLiteralExpression(100 - summedChance));
                                }

                                yield return builder.GenerateAssignmentStatement(
                                    LocalItemIdVariableName,
                                    builder.GenerateInvocationExpression(nameof(War3Api.Blizzard.RandomDistChoose)));
                                break;

                            default:
                                break;
                        }

                        yield return builder.GenerateIfStatement(
                            builder.GenerateBinaryExpression(
                                BinaryOperator.NotEquals,
                                builder.GenerateVariableExpression(LocalItemIdVariableName),
                                builder.GenerateIntegerLiteralExpression(-1)),
                            builder.GenerateInvocationStatement(
                                nameof(War3Api.Common.CreateItem),
                                builder.GenerateVariableExpression(LocalItemIdVariableName),
                                builder.GenerateFloatLiteralExpression(item.PositionX),
                                builder.GenerateFloatLiteralExpression(item.PositionY)));
                    }
                    else
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.CreateItem),
                            builder.GenerateFourCCExpression(item.TypeId),
                            builder.GenerateFloatLiteralExpression(item.PositionX),
                            builder.GenerateFloatLiteralExpression(item.PositionY));
                    }
                }

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
                                    builder.GenerateIntegerLiteralExpression(-1));

                                // TODO: create integer array(s) for global random unit table(s)
                                /*yield return builder.GenerateAssignmentStatement(
                                    MainFunctionProvider.LocalUnitIdVariableName,
                                    builder.GenerateArrayIndexExpression(
                                        builder.GenerateVariableExpression("gg_rg_000"),
                                        builder.GenerateIntegerLiteralExpression(randomData.UnitGroupTableColumn)));*/
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
                                builder.GenerateFunctionExpression($"ItemTable{mapInfo.GetItemTable(unit.MapItemTablePointer).Index.ToString("D6")}_DropItems"));
                        }
                        else
                        {
                            // TODO: generate funtion Unit######_DropItems

                            yield return builder.GenerateInvocationStatement(
                                nameof(War3Api.Common.TriggerAddAction),
                                builder.GenerateVariableExpression(LocalTriggerVariableName),
                                builder.GenerateFunctionExpression($"Unit{unit.CreationNumber.ToString("D6")}_DropItems"));
                        }
                    }
                }
            }

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Blizzard.InitBlizzard));

            if (builder.Data.CSharp)
            {
                yield return builder.GenerateInvocationStatement(
                    CSharpLua.LuaSyntaxGenerator.kManifestFuncName);
            }
        }
    }
}