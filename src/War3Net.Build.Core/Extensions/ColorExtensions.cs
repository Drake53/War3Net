// ------------------------------------------------------------------------------
// <copyright file="ColorExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;

using War3Net.Build.Common;

namespace War3Net.Build.Extensions
{
    public static class ColorExtensions
    {
        public static bool TryGetKnownPlayerColor(this Color color, out KnownPlayerColor playerColor)
        {
            playerColor = color.ToKnownPlayerColorInternal();

            if (playerColor == KnownPlayerColor.Yellow)
            {
                return color == PlayerColor.Yellow
                    || color == PlayerColor.YellowAlt;
            }
            else
            {
                return color == PlayerColor.FromKnownColor(playerColor);
            }
        }

        internal static KnownPlayerColor ToKnownPlayerColorInternal(this Color color)
        {
            return color.G switch
            {
                0x03 => KnownPlayerColor.Red, // FF 03 03
                0x42 => KnownPlayerColor.Blue, // 00 42 FF
                0xE6 => KnownPlayerColor.Teal, // 1c E6 B9
                0xFC => KnownPlayerColor.Yellow, // FF FC 00 or FF FC 01
                0x8A => KnownPlayerColor.Orange, // FE 8A 0E
                0xC0 => KnownPlayerColor.Green, // 20 C0 00
                0x5B => KnownPlayerColor.Pink, // E5 5B B0
                0x96 => KnownPlayerColor.Gray, // 95 96 97
                0xBF => KnownPlayerColor.LightBlue, // 7E BF F1
                0x62 => KnownPlayerColor.DarkGreen, // 10 62 46
                0x2A => KnownPlayerColor.Brown, // 4E 2A 04
                0xEA => KnownPlayerColor.Turquoise, // 00 EA FF
                0xCD => KnownPlayerColor.Wheat, // EB CD 87
                0xA4 => KnownPlayerColor.Peach, // F8 A4 8B
                0xFF => KnownPlayerColor.Mint, // BF FF 80
                0xB9 => KnownPlayerColor.Lavender, // DC B9 EB
                0x28 => KnownPlayerColor.Coal, // 28 28 28
                0xF0 => KnownPlayerColor.Snow, // EB F0 FF
                0x78 => KnownPlayerColor.Emerald, // 00 78 1E
                0x6F => KnownPlayerColor.Peanut, // A4 6F 33
                _ => color.R switch
                {
                    0x54 => KnownPlayerColor.Purple, // 54 00 81
                    0x9B => KnownPlayerColor.Maroon, // 9B 00 00
                    0xBE => KnownPlayerColor.Violet, // BE 00 FE
                    _ => color.B switch
                    {
                        0xC3 => KnownPlayerColor.Navy, // 00 00 C3
                        _ => KnownPlayerColor.Black, // 00 00 00
                    },
                },
            };
        }
    }
}