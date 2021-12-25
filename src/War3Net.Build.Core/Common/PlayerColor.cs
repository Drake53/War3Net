// ------------------------------------------------------------------------------
// <copyright file="PlayerColor.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.ComponentModel;
using System.Drawing;

namespace War3Net.Build.Common
{
    public static class PlayerColor
    {
        public static Color Red => Color.FromArgb(unchecked((int)0xFFFF0303));

        public static Color Blue => Color.FromArgb(unchecked((int)0xFF0042FF));

        public static Color Teal => Color.FromArgb(unchecked((int)0xFF1CE6B9));

        public static Color Purple => Color.FromArgb(unchecked((int)0xFF540081));

        public static Color Yellow => Color.FromArgb(unchecked((int)0xFFFFFC00));

        // The value of yellow's blue channel can be either 0 or 1.
        public static Color YellowAlt => Color.FromArgb(unchecked((int)0xFFFFFC01));

        public static Color Orange => Color.FromArgb(unchecked((int)0xFFFE8A0E));

        public static Color Green => Color.FromArgb(unchecked((int)0xFF20C000));

        public static Color Pink => Color.FromArgb(unchecked((int)0xFFE55BB0));

        public static Color Gray => Color.FromArgb(unchecked((int)0xFF959697));

        public static Color LightBlue => Color.FromArgb(unchecked((int)0xFF7EBFF1));

        public static Color DarkGreen => Color.FromArgb(unchecked((int)0xFF106246));

        public static Color Brown => Color.FromArgb(unchecked((int)0xFF4E2A04));

        public static Color Maroon => Color.FromArgb(unchecked((int)0xFF9B0000));

        public static Color Navy => Color.FromArgb(unchecked((int)0xFF0000C3));

        public static Color Turquoise => Color.FromArgb(unchecked((int)0xFF00EAFF));

        public static Color Violet => Color.FromArgb(unchecked((int)0xFFBE00FE));

        public static Color Wheat => Color.FromArgb(unchecked((int)0xFFEBCD87));

        public static Color Peach => Color.FromArgb(unchecked((int)0xFFF8A48B));

        public static Color Mint => Color.FromArgb(unchecked((int)0xFFBFFF80));

        public static Color Lavender => Color.FromArgb(unchecked((int)0xFFDCB9EB));

        public static Color Coal => Color.FromArgb(unchecked((int)0xFF282828));

        public static Color Snow => Color.FromArgb(unchecked((int)0xFFEBF0FF));

        public static Color Emerald => Color.FromArgb(unchecked((int)0xFF00781E));

        public static Color Peanut => Color.FromArgb(unchecked((int)0xFFA46F33));

        public static Color Black => Color.FromArgb(unchecked((int)0xFF000000));

        public static Color FromKnownColor(KnownPlayerColor playerColor)
        {
            return playerColor switch
            {
                KnownPlayerColor.Red => Red,
                KnownPlayerColor.Blue => Blue,
                KnownPlayerColor.Teal => Teal,
                KnownPlayerColor.Purple => Purple,
                KnownPlayerColor.Yellow => Yellow,
                KnownPlayerColor.Orange => Orange,
                KnownPlayerColor.Green => Green,
                KnownPlayerColor.Pink => Pink,
                KnownPlayerColor.Gray => Gray,
                KnownPlayerColor.LightBlue => LightBlue,
                KnownPlayerColor.DarkGreen => DarkGreen,
                KnownPlayerColor.Brown => Brown,
                KnownPlayerColor.Maroon => Maroon,
                KnownPlayerColor.Navy => Navy,
                KnownPlayerColor.Turquoise => Turquoise,
                KnownPlayerColor.Violet => Violet,
                KnownPlayerColor.Wheat => Wheat,
                KnownPlayerColor.Peach => Peach,
                KnownPlayerColor.Mint => Mint,
                KnownPlayerColor.Lavender => Lavender,
                KnownPlayerColor.Coal => Coal,
                KnownPlayerColor.Snow => Snow,
                KnownPlayerColor.Emerald => Emerald,
                KnownPlayerColor.Peanut => Peanut,
                KnownPlayerColor.Black => Black,

                _ => throw new InvalidEnumArgumentException(nameof(playerColor), (int)playerColor, typeof(KnownPlayerColor)),
            };
        }
    }
}