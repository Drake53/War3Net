// ------------------------------------------------------------------------------
// <copyright file="PlayerColorApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums;

namespace War3Net.Runtime.Common.Api.Enums
{
    public static class PlayerColorApi
    {
        public static readonly PlayerColor PLAYER_COLOR_RED = ConvertPlayerColor((int)PlayerColor.Type.Red);
        public static readonly PlayerColor PLAYER_COLOR_BLUE = ConvertPlayerColor((int)PlayerColor.Type.Blue);
        public static readonly PlayerColor PLAYER_COLOR_CYAN = ConvertPlayerColor((int)PlayerColor.Type.Cyan);
        public static readonly PlayerColor PLAYER_COLOR_PURPLE = ConvertPlayerColor((int)PlayerColor.Type.Purple);
        public static readonly PlayerColor PLAYER_COLOR_YELLOW = ConvertPlayerColor((int)PlayerColor.Type.Yellow);
        public static readonly PlayerColor PLAYER_COLOR_ORANGE = ConvertPlayerColor((int)PlayerColor.Type.Orange);
        public static readonly PlayerColor PLAYER_COLOR_GREEN = ConvertPlayerColor((int)PlayerColor.Type.Green);
        public static readonly PlayerColor PLAYER_COLOR_PINK = ConvertPlayerColor((int)PlayerColor.Type.Pink);
        public static readonly PlayerColor PLAYER_COLOR_LIGHT_GRAY = ConvertPlayerColor((int)PlayerColor.Type.LightGray);
        public static readonly PlayerColor PLAYER_COLOR_LIGHT_BLUE = ConvertPlayerColor((int)PlayerColor.Type.LightBlue);
        public static readonly PlayerColor PLAYER_COLOR_AQUA = ConvertPlayerColor((int)PlayerColor.Type.DarkGreen);
        public static readonly PlayerColor PLAYER_COLOR_BROWN = ConvertPlayerColor((int)PlayerColor.Type.Brown);
        public static readonly PlayerColor PLAYER_COLOR_MAROON = ConvertPlayerColor((int)PlayerColor.Type.Maroon);
        public static readonly PlayerColor PLAYER_COLOR_NAVY = ConvertPlayerColor((int)PlayerColor.Type.Navy);
        public static readonly PlayerColor PLAYER_COLOR_TURQUOISE = ConvertPlayerColor((int)PlayerColor.Type.Turquoise);
        public static readonly PlayerColor PLAYER_COLOR_VIOLET = ConvertPlayerColor((int)PlayerColor.Type.Violet);
        public static readonly PlayerColor PLAYER_COLOR_WHEAT = ConvertPlayerColor((int)PlayerColor.Type.Wheat);
        public static readonly PlayerColor PLAYER_COLOR_PEACH = ConvertPlayerColor((int)PlayerColor.Type.Peach);
        public static readonly PlayerColor PLAYER_COLOR_MINT = ConvertPlayerColor((int)PlayerColor.Type.Mint);
        public static readonly PlayerColor PLAYER_COLOR_LAVENDER = ConvertPlayerColor((int)PlayerColor.Type.Lavender);
        public static readonly PlayerColor PLAYER_COLOR_COAL = ConvertPlayerColor((int)PlayerColor.Type.Coal);
        public static readonly PlayerColor PLAYER_COLOR_SNOW = ConvertPlayerColor((int)PlayerColor.Type.Snow);
        public static readonly PlayerColor PLAYER_COLOR_EMERALD = ConvertPlayerColor((int)PlayerColor.Type.Emerald);
        public static readonly PlayerColor PLAYER_COLOR_PEANUT = ConvertPlayerColor((int)PlayerColor.Type.Peanut);

        public static PlayerColor ConvertPlayerColor(int i)
        {
            return PlayerColor.GetPlayerColor(i);
        }
    }
}