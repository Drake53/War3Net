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

        public byte X { get; set; }

        public byte Y { get; set; }

        public Color Color { get; set; }

        internal void ReadFrom(BinaryReader reader, MapPreviewIconsFormatVersion formatVersion)
        {
            IconType = reader.ReadInt32<PreviewIconType>();
            X = (byte)reader.ReadInt32();
            Y = (byte)reader.ReadInt32();
            Color = Color.FromArgb(reader.ReadInt32());
        }

        internal void WriteTo(BinaryWriter writer, MapPreviewIconsFormatVersion formatVersion)
        {
            writer.Write((int)IconType);
            writer.Write((int)X);
            writer.Write((int)Y);
            writer.Write(Color.ToArgb());
        }
    }
}