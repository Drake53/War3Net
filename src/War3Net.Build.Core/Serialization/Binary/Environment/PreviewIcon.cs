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
    public sealed partial class PreviewIcon
    {
        internal PreviewIcon(BinaryReader reader, MapPreviewIconsFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal void ReadFrom(BinaryReader reader, MapPreviewIconsFormatVersion formatVersion)
        {
            IconType = reader.ReadInt32<PreviewIconType>();
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            Color = reader.ReadColorBgra();
        }

        internal void WriteTo(BinaryWriter writer, MapPreviewIconsFormatVersion formatVersion)
        {
            writer.Write((int)IconType);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Color.ToBgra());
        }
    }
}