// ------------------------------------------------------------------------------
// <copyright file="MainFunctionStatementsProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Info;
using War3Net.Build.Script;

namespace War3Net.Build.Providers
{
    internal static class MainFunctionProvider
    {
        public const string FunctionName = "main";
        public const string LocalUnitVariableName = "u";
        // IMPORTANT: handle these for JASS version
        public const string LocalUnitIdVariableName = "unitID";
        public const string LocalItemIdVariableName = "itemID";
    }

    internal static class MainFunctionStatementsProvider<TBuilder, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TBuilder : FunctionBuilder<TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private const string MusicName = "Music";
        private const bool MusicRandom = true;
        private const int MusicIndex = 0;

        public static IEnumerable<TStatementSyntax> GetStatements(TBuilder builder)
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
                // TODO
            }

            if (builder.Data.MapUnits != null)
            {
                foreach (var localDeclaration in builder.GenerateLocalDeclarationStatements(
                    MainFunctionProvider.LocalUnitVariableName,
                    MainFunctionProvider.LocalUnitIdVariableName,
                    MainFunctionProvider.LocalItemIdVariableName))
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
                                    MainFunctionProvider.LocalItemIdVariableName,
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
                                    MainFunctionProvider.LocalItemIdVariableName,
                                    builder.GenerateInvocationExpression(nameof(War3Api.Blizzard.RandomDistChoose)));
                                break;

                            default:
                                break;
                        }

                        yield return builder.GenerateIfStatement(
                            builder.GenerateBinaryExpression(
                                BinaryOperator.NotEquals,
                                builder.GenerateVariableExpression(MainFunctionProvider.LocalItemIdVariableName),
                                builder.GenerateIntegerLiteralExpression(-1)),
                            builder.GenerateInvocationStatement(
                                nameof(War3Api.Common.CreateItem),
                                builder.GenerateVariableExpression(MainFunctionProvider.LocalItemIdVariableName),
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
                                        MainFunctionProvider.LocalUnitVariableName,
                                        builder.GenerateInvocationExpression(
                                            nameof(War3Api.Common.ChooseRandomNPBuilding)));
                                }
                                else
                                {
                                    yield return builder.GenerateAssignmentStatement(
                                        MainFunctionProvider.LocalUnitIdVariableName,
                                        builder.GenerateInvocationExpression(
                                            nameof(War3Api.Common.ChooseRandomCreep),
                                            builder.GenerateIntegerLiteralExpression(randomData.Level)));
                                }

                                break;

                            case 1:
                                yield return builder.GenerateAssignmentStatement(
                                    MainFunctionProvider.LocalUnitIdVariableName,
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
                                    MainFunctionProvider.LocalItemIdVariableName,
                                    builder.GenerateInvocationExpression(nameof(War3Api.Blizzard.RandomDistChoose)));
                                break;
                        }

                        yield return builder.GenerateIfStatement(
                            builder.GenerateBinaryExpression(
                                BinaryOperator.NotEquals,
                                builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitIdVariableName),
                                builder.GenerateIntegerLiteralExpression(-1)),
                            builder.GenerateAssignmentStatement(
                                MainFunctionProvider.LocalUnitVariableName,
                                builder.GenerateInvocationExpression(
                                    nameof(War3Api.Common.CreateUnit),
                                    builder.GenerateInvocationExpression(
                                        nameof(War3Api.Common.Player),
                                        builder.GenerateIntegerLiteralExpression(unit.Owner)),
                                    builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitIdVariableName),
                                    builder.GenerateFloatLiteralExpression(unit.PositionX),
                                    builder.GenerateFloatLiteralExpression(unit.PositionY),
                                    builder.GenerateFloatLiteralExpression(unit.Facing))));
                    }
                    else
                    {
                        yield return builder.GenerateAssignmentStatement(
                            MainFunctionProvider.LocalUnitVariableName,
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

                    if (unit.HeroLevel > 1)
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetHeroLevel),
                            builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                            builder.GenerateIntegerLiteralExpression(unit.HeroLevel),
                            builder.GenerateBooleanLiteralExpression(false));
                    }

                    if (unit.HeroStrength > 0)
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetHeroStr),
                            builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                            builder.GenerateIntegerLiteralExpression(unit.HeroStrength),
                            builder.GenerateBooleanLiteralExpression(true));
                    }

                    if (unit.HeroAgility > 0)
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetHeroAgi),
                            builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                            builder.GenerateIntegerLiteralExpression(unit.HeroAgility),
                            builder.GenerateBooleanLiteralExpression(true));
                    }

                    if (unit.HeroIntelligence > 0)
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetHeroInt),
                            builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                            builder.GenerateIntegerLiteralExpression(unit.HeroIntelligence),
                            builder.GenerateBooleanLiteralExpression(true));
                    }

                    if (unit.Hp != -1)
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetUnitState),
                            builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                            builder.GenerateVariableExpression(nameof(War3Api.Common.UNIT_STATE_LIFE)),
                            builder.GenerateBinaryExpression(
                                BinaryOperator.Multiplication,
                                builder.GenerateFloatLiteralExpression(unit.Hp * 0.01f),
                                builder.GenerateInvocationExpression(
                                    nameof(War3Api.Common.GetUnitState),
                                    builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                                    builder.GenerateVariableExpression(nameof(War3Api.Common.UNIT_STATE_LIFE)))));
                    }

                    if (unit.Mp != -1)
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetUnitState),
                            builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                            builder.GenerateVariableExpression(nameof(War3Api.Common.UNIT_STATE_MANA)),
                            builder.GenerateFloatLiteralExpression(unit.Mp));
                    }

                    if (unit.TypeId == "ngol")
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetResourceAmount),
                            builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                            builder.GenerateIntegerLiteralExpression(unit.GoldAmount));
                    }

                    if (unit.TargetAcquisition != -1)
                    {
                        const float CampAcquisitionRange = 200f;
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetUnitAcquireRange),
                            builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
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
                                builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                                builder.GenerateFourCCExpression(ability.Id));
                        }

                        if (ability.IsActive)
                        {
#if false
                            yield return builder.GenerateInvocationStatement(
                                nameof(War3Api.Common.IssueImmediateOrder),
                                builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                                builder.GenerateStringLiteralExpression(ability.OrderString));
#else
                            // TODO: test if this works
                            yield return builder.GenerateInvocationStatement(
                                nameof(War3Api.Common.IssueImmediateOrderById),
                                builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                                builder.GenerateFourCCExpression(ability.Id));
#endif
                        }
                    }

                    foreach (var item in unit.Inventory)
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.UnitAddItemToSlotById),
                            builder.GenerateVariableExpression(MainFunctionProvider.LocalUnitVariableName),
                            builder.GenerateFourCCExpression(item.Id),
                            builder.GenerateIntegerLiteralExpression(item.Slot));
                    }

                    foreach (var droppedItem in unit.DroppedItemData)
                    {
                        // TODO: implement
                        // Create trigger ItemTable######_DropItems or Unit######_DropItems, add events EVENT_UNIT_DEATH and EVENT_UNIT_CHANGE_OWNER
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