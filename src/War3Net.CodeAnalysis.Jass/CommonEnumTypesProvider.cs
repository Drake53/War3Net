// ------------------------------------------------------------------------------
// <copyright file="CommonEnumTypesProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass
{
    internal static class CommonEnumTypesProvider
    {
        /// <summary>
        /// Gets all pairs of type definition and function identifier in common.j, that can be transpiled as <see cref="System.Enum"/>.
        /// </summary>
        public static IEnumerable<(string, string)> GetEnumTypes()
        {
            yield return GetPairFromConvertFunction("ConvertRace");
            yield return GetPairFromConvertFunction("ConvertAllianceType");
            yield return ("racepreference", "ConvertRacePref");
            yield return GetPairFromConvertFunction("ConvertIGameState");
            yield return GetPairFromConvertFunction("ConvertFGameState");
            yield return GetPairFromConvertFunction("ConvertPlayerState");
            yield return GetPairFromConvertFunction("ConvertPlayerScore");
            yield return GetPairFromConvertFunction("ConvertPlayerGameResult");
            yield return GetPairFromConvertFunction("ConvertUnitState");
            yield return GetPairFromConvertFunction("ConvertAIDifficulty");
            yield return GetPairFromConvertFunction("ConvertGameEvent");
            yield return GetPairFromConvertFunction("ConvertPlayerEvent");
            yield return GetPairFromConvertFunction("ConvertPlayerUnitEvent");
            yield return GetPairFromConvertFunction("ConvertWidgetEvent");
            yield return GetPairFromConvertFunction("ConvertDialogEvent");
            yield return GetPairFromConvertFunction("ConvertUnitEvent");
            yield return GetPairFromConvertFunction("ConvertLimitOp");
            yield return GetPairFromConvertFunction("ConvertUnitType");
            yield return GetPairFromConvertFunction("ConvertGameSpeed");
            yield return GetPairFromConvertFunction("ConvertPlacement");
            yield return GetPairFromConvertFunction("ConvertStartLocPrio");
            yield return GetPairFromConvertFunction("ConvertGameDifficulty");
            yield return GetPairFromConvertFunction("ConvertGameType");
            yield return GetPairFromConvertFunction("ConvertMapFlag");
            yield return GetPairFromConvertFunction("ConvertMapVisibility");
            yield return GetPairFromConvertFunction("ConvertMapSetting");
            yield return GetPairFromConvertFunction("ConvertMapDensity");
            yield return GetPairFromConvertFunction("ConvertMapControl");
            yield return GetPairFromConvertFunction("ConvertPlayerColor");
            yield return GetPairFromConvertFunction("ConvertPlayerSlotState");
            yield return GetPairFromConvertFunction("ConvertVolumeGroup");
            yield return GetPairFromConvertFunction("ConvertCameraField");
            yield return GetPairFromConvertFunction("ConvertBlendMode");
            yield return GetPairFromConvertFunction("ConvertRarityControl");
            yield return GetPairFromConvertFunction("ConvertTexMapFlags");
            yield return GetPairFromConvertFunction("ConvertFogState");
            yield return GetPairFromConvertFunction("ConvertEffectType");
            yield return GetPairFromConvertFunction("ConvertVersion");
            yield return GetPairFromConvertFunction("ConvertItemType");
            yield return GetPairFromConvertFunction("ConvertAttackType");
            yield return GetPairFromConvertFunction("ConvertDamageType");
            yield return GetPairFromConvertFunction("ConvertWeaponType");
            yield return GetPairFromConvertFunction("ConvertSoundType");
            yield return GetPairFromConvertFunction("ConvertPathingType");
            yield return GetPairFromConvertFunction("ConvertMouseButtonType");
            yield return GetPairFromConvertFunction("ConvertAnimType");
            yield return GetPairFromConvertFunction("ConvertSubAnimType");
            yield return GetPairFromConvertFunction("ConvertOriginFrameType");
            yield return GetPairFromConvertFunction("ConvertFramePointType");
            yield return GetPairFromConvertFunction("ConvertTextAlignType");
            yield return GetPairFromConvertFunction("ConvertFrameEventType");
            yield return GetPairFromConvertFunction("ConvertOsKeyType");
            yield return GetPairFromConvertFunction("ConvertAbilityIntegerField");
            yield return GetPairFromConvertFunction("ConvertAbilityRealField");
            yield return GetPairFromConvertFunction("ConvertAbilityBooleanField");
            yield return GetPairFromConvertFunction("ConvertAbilityStringField");
            yield return GetPairFromConvertFunction("ConvertAbilityIntegerLevelField");
            yield return GetPairFromConvertFunction("ConvertAbilityRealLevelField");
            yield return GetPairFromConvertFunction("ConvertAbilityBooleanLevelField");
            yield return GetPairFromConvertFunction("ConvertAbilityStringLevelField");
            yield return GetPairFromConvertFunction("ConvertAbilityIntegerLevelArrayField");
            yield return GetPairFromConvertFunction("ConvertAbilityRealLevelArrayField");
            yield return GetPairFromConvertFunction("ConvertAbilityBooleanLevelArrayField");
            yield return GetPairFromConvertFunction("ConvertAbilityStringLevelArrayField");
            yield return GetPairFromConvertFunction("ConvertUnitIntegerField");
            yield return GetPairFromConvertFunction("ConvertUnitRealField");
            yield return GetPairFromConvertFunction("ConvertUnitBooleanField");
            yield return GetPairFromConvertFunction("ConvertUnitStringField");
            yield return GetPairFromConvertFunction("ConvertUnitWeaponIntegerField");
            yield return GetPairFromConvertFunction("ConvertUnitWeaponRealField");
            yield return GetPairFromConvertFunction("ConvertUnitWeaponBooleanField");
            yield return GetPairFromConvertFunction("ConvertUnitWeaponStringField");
            yield return GetPairFromConvertFunction("ConvertItemIntegerField");
            yield return GetPairFromConvertFunction("ConvertItemRealField");
            yield return GetPairFromConvertFunction("ConvertItemBooleanField");
            yield return GetPairFromConvertFunction("ConvertItemStringField");
            yield return GetPairFromConvertFunction("ConvertMoveType");
            yield return GetPairFromConvertFunction("ConvertTargetFlag");
            yield return GetPairFromConvertFunction("ConvertArmorType");
            yield return GetPairFromConvertFunction("ConvertHeroAttribute");
            yield return GetPairFromConvertFunction("ConvertDefenseType");
            yield return GetPairFromConvertFunction("ConvertRegenType");
            yield return GetPairFromConvertFunction("ConvertUnitCategory");
            yield return GetPairFromConvertFunction("ConvertPathingFlag");
        }

        private static (string, string) GetPairFromConvertFunction(string functionName)
        {
            return (functionName.Substring(7).ToLower(), functionName);
        }
    }
}