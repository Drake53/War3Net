// ------------------------------------------------------------------------------
// <copyright file="OriginFrameTypeApi.cs" company="Drake53">
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
    public static class OriginFrameTypeApi
    {
        public static readonly OriginFrameType ORIGIN_FRAME_GAME_UI = ConvertOriginFrameType((int)OriginFrameType.Type.GameUI);
        public static readonly OriginFrameType ORIGIN_FRAME_COMMAND_BUTTON = ConvertOriginFrameType((int)OriginFrameType.Type.CommandButton);
        public static readonly OriginFrameType ORIGIN_FRAME_HERO_BAR = ConvertOriginFrameType((int)OriginFrameType.Type.HeroBar);
        public static readonly OriginFrameType ORIGIN_FRAME_HERO_BUTTON = ConvertOriginFrameType((int)OriginFrameType.Type.HeroButton);
        public static readonly OriginFrameType ORIGIN_FRAME_HERO_HP_BAR = ConvertOriginFrameType((int)OriginFrameType.Type.HeroHPBar);
        public static readonly OriginFrameType ORIGIN_FRAME_HERO_MANA_BAR = ConvertOriginFrameType((int)OriginFrameType.Type.HeroManaBar);
        public static readonly OriginFrameType ORIGIN_FRAME_HERO_BUTTON_INDICATOR = ConvertOriginFrameType((int)OriginFrameType.Type.HeroButtonIndicator);
        public static readonly OriginFrameType ORIGIN_FRAME_ITEM_BUTTON = ConvertOriginFrameType((int)OriginFrameType.Type.ItemButton);
        public static readonly OriginFrameType ORIGIN_FRAME_MINIMAP = ConvertOriginFrameType((int)OriginFrameType.Type.Minimap);
        public static readonly OriginFrameType ORIGIN_FRAME_MINIMAP_BUTTON = ConvertOriginFrameType((int)OriginFrameType.Type.MinimapButton);
        public static readonly OriginFrameType ORIGIN_FRAME_SYSTEM_BUTTON = ConvertOriginFrameType((int)OriginFrameType.Type.SystemButton);
        public static readonly OriginFrameType ORIGIN_FRAME_TOOLTIP = ConvertOriginFrameType((int)OriginFrameType.Type.Tooltip);
        public static readonly OriginFrameType ORIGIN_FRAME_UBERTOOLTIP = ConvertOriginFrameType((int)OriginFrameType.Type.Ubertooltip);
        public static readonly OriginFrameType ORIGIN_FRAME_CHAT_MSG = ConvertOriginFrameType((int)OriginFrameType.Type.ChatMessage);
        public static readonly OriginFrameType ORIGIN_FRAME_UNIT_MSG = ConvertOriginFrameType((int)OriginFrameType.Type.UnitMessage);
        public static readonly OriginFrameType ORIGIN_FRAME_TOP_MSG = ConvertOriginFrameType((int)OriginFrameType.Type.TopMessage);
        public static readonly OriginFrameType ORIGIN_FRAME_PORTRAIT = ConvertOriginFrameType((int)OriginFrameType.Type.Portrait);
        public static readonly OriginFrameType ORIGIN_FRAME_WORLD_FRAME = ConvertOriginFrameType((int)OriginFrameType.Type.WorldFrame);

        public static OriginFrameType ConvertOriginFrameType(int i)
        {
            return OriginFrameType.GetOriginFrameType(i);
        }
    }
}