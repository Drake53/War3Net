// ------------------------------------------------------------------------------
// <copyright file="AllianceTypeApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums;

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class AllianceTypeApi
    {
        public static readonly AllianceType ALLIANCE_PASSIVE = ConvertAllianceType((int)AllianceType.Type.Passive);
        public static readonly AllianceType ALLIANCE_HELP_REQUEST = ConvertAllianceType((int)AllianceType.Type.HelpRequest);
        public static readonly AllianceType ALLIANCE_HELP_RESPONSE = ConvertAllianceType((int)AllianceType.Type.HelpResponse);
        public static readonly AllianceType ALLIANCE_SHARED_XP = ConvertAllianceType((int)AllianceType.Type.SharedXP);
        public static readonly AllianceType ALLIANCE_SHARED_SPELLS = ConvertAllianceType((int)AllianceType.Type.SharedSpells);
        public static readonly AllianceType ALLIANCE_SHARED_VISION = ConvertAllianceType((int)AllianceType.Type.SharedVision);
        public static readonly AllianceType ALLIANCE_SHARED_CONTROL = ConvertAllianceType((int)AllianceType.Type.SharedControl);
        public static readonly AllianceType ALLIANCE_SHARED_ADVANCED_CONTROL = ConvertAllianceType((int)AllianceType.Type.SharedAdvancedControl);
        public static readonly AllianceType ALLIANCE_RESCUABLE = ConvertAllianceType((int)AllianceType.Type.Rescuable);
        public static readonly AllianceType ALLIANCE_SHARED_VISION_FORCED = ConvertAllianceType((int)AllianceType.Type.SharedVisionForced);

        public static AllianceType ConvertAllianceType(int i)
        {
            return AllianceType.GetAllianceType(i);
        }
    }
}