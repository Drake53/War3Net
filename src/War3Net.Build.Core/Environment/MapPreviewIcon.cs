// ------------------------------------------------------------------------------
// <copyright file="MapPreviewIcon.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;
using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed class MapPreviewIcon
    {
        private MapPreviewIconType _iconType;
        private byte _x;
        private byte _y;
        private Color _color;

        public MapPreviewIcon()
        {
        }

        public MapPreviewIconType IconType
        {
            get => _iconType;
            set => _iconType = value;
        }

        public byte X
        {
            get => _x;
            set => _x = value;
        }

        public byte Y
        {
            get => _y;
            set => _y = value;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public static MapPreviewIcon Parse(Stream stream, bool leaveOpen = false)
        {
            var iconData = new MapPreviewIcon();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                iconData._iconType = reader.ReadInt32<MapPreviewIconType>();
                iconData._x = (byte)reader.ReadInt32();
                iconData._y = (byte)reader.ReadInt32();
                iconData._color = Color.FromArgb(reader.ReadInt32());
            }

            return iconData;
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                WriteTo(writer);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)_iconType);
            writer.Write((int)_x);
            writer.Write((int)_y);
            writer.Write(_color.ToArgb());
        }
    }
}