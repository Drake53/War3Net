using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgaLib
{
    /// <summary>
    /// Color map type.
    /// </summary>
    public static class ColorMapTypes
    {
        /// <summary>No color-map is included with the image.</summary>
        public const byte DoesNotIncludePalette = 0;

        /// <summary>color-map is included with the image.</summary>
        public const byte IncludePalette = 1;

        /// <summary>
        /// Formatted text.
        /// </summary>
        private static readonly string[] FormattedText = { "no palette", "palette" };

        /// <summary>
        /// Returns a formatted text.
        /// </summary>
        /// <param name="value">A color map type value.</param>
        /// <returns>Returns a formatted text.</returns>
        public static string ToFormattedText(byte value)
        {
            if (value >= FormattedText.Length)
            {
                return "???";
            }
            return FormattedText[value];
        }
    }

    /// <summary>
    /// Image type.
    /// </summary>
    public static class ImageTypes
    {
        /// <summary>No image data included.</summary>
        public const byte NoImage = 0;

        /// <summary>Uncompressed, color-mapped image.</summary>
        public const byte ColorMapped = 1;

        /// <summary>Uncompressed, true-color image.</summary>
        public const byte TrueColor = 2;

        /// <summary>Uncompressed, black and white image.</summary>
        public const byte Monochrome = 3;

        /// <summary>Run-length encoded, color-mapped image.</summary>
        public const byte CompressedColorMapped = 9;

        /// <summary>Run-length encoded, true-color image.</summary>
        public const byte CompressedTrueColor = 10;

        /// <summary>Run-length encoded, black and white image.</summary>
        public const byte CompressedMonochrome = 11;

        /// <summary>
        /// Formatted text.
        /// </summary>
        private static readonly Dictionary<byte, string> FormattedText = new Dictionary<byte, string>()
        {
            { NoImage, "no image" },
            { ColorMapped, "color-mapped(uncompressed)" },
            { TrueColor, "true-color(uncompressed)" },
            { Monochrome, "monochrome(uncompressed)" },
            { CompressedColorMapped, "color-mapped(RLE)" },
            { CompressedTrueColor, "true-color(RLE)" },
            { CompressedMonochrome, "monochrome(RLE)" },
        };

        /// <summary>
        /// Returns a formatted text.
        /// </summary>
        /// <param name="value">An image type value.</param>
        /// <returns>Returns a formatted text.</returns>
        public static string ToFormattedText(byte value)
        {
            if (!FormattedText.ContainsKey(value))
            {
                return "???";
            }
            return FormattedText[value];
        }
    }

    /// <summary>
    /// Image origin.
    /// </summary>
    public static class ImageOriginTypes
    {
        /// <summary>Bottom left.</summary>
        public const byte BottomLeft = 0;

        /// <summary>Bottom right.</summary>
        public const byte BottomRight = 1;

        /// <summary>Top left.</summary>
        public const byte TopLeft = 2;

        /// <summary>Top right.</summary>
        public const byte TopRight = 3;

        /// <summary>
        /// Formatted text.
        /// </summary>
        private static readonly string[] FormattedText =
        {
            "bottom left", "bottom right", "top left", "top right"
        };

        /// <summary>
        /// Returns a formatted text.
        /// </summary>
        /// <param name="value">An image origin value.</param>
        /// <returns>Returns a formatted text.</returns>
        public static string ToFormattedText(byte value)
        {
            if (value >= FormattedText.Length)
            {
                return "???";
            }
            return FormattedText[value];
        }
    }

    /// <summary>
    /// Attributes type in the extension are.
    /// </summary>
    public static class AttributeTypes
    {
        /// <summary>No alpha data included.</summary>
        public const byte NoAlpha = 0;

        /// <summary>Undefined data in the alpha field, can be ignored.</summary>
        public const byte UndefinedAlphaCanBeIgnored = 1;

        /// <summary>Undefined data in the alpha field, but should be retained.</summary>
        public const byte UndefinedAlphaShouldBeRetained = 2;

        /// <summary>Useful alpha channel datais present.</summary>
        public const byte HasAlpha = 3;

        /// <summary>Pre-multiplied alpha.</summary>
        public const byte HasPreMultipliedAlpha = 4;
    }

    /// <summary>
    /// Color depth constants.
    /// </summary>
    internal static class ColorDepth
    {
        /// <summary>8 bits per pixel.</summary>
        public const int Bpp8 = 8;

        /// <summary>15 bits per pixel.</summary>
        public const int Bpp15 = 15;

        /// <summary>15 bits per pixel.</summary>
        public const int Bpp16 = 16;

        /// <summary>24 bits per pixel.</summary>
        public const int Bpp24 = 24;

        /// <summary>32 bits per pixel.</summary>
        public const int Bpp32 = 32;
    }

    /// <summary>
    /// ARGB offset.
    /// </summary>
    internal static class ArgbOffset
    {
        /// <summary>Blue。</summary>
        public const int Blue = 0;

        /// <summary>Green。</summary>
        public const int Green = 1;

        /// <summary>Red。</summary>
        public const int Red = 2;

        /// <summary>Alpha。</summary>
        public const int Alpha = 3;
    }
}
