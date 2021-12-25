// ------------------------------------------------------------------------------
// <copyright file="PreviewIcon.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;
using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed class PreviewIcon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewIcon"/> class.
        /// </summary>
        public PreviewIcon()
        {
        }

        internal PreviewIcon(BinaryReader reader, MapPreviewIconsFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        public PreviewIconType IconType { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Color Color { get; set; }

        public override string ToString() => IconType.ToString();

        internal void ReadFrom(BinaryReader reader, MapPreviewIconsFormatVersion formatVersion)
        {
            IconType = reader.ReadInt32<PreviewIconType>();
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            Color = Color.FromArgb(reader.ReadInt32());
        }

        internal void WriteTo(BinaryWriter writer, MapPreviewIconsFormatVersion formatVersion)
        {
            writer.Write((int)IconType);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Color.ToArgb());
        }
    }
}