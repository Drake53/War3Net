// ------------------------------------------------------------------------------
// <copyright file="Region.cs" company="Drake53">
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
    public sealed class Region
    {
        private float _left;
        private float _bottom;
        private float _right;
        private float _top;

        private string _name;
        private int _creationNumber;

        private char[] _weatherId;
        private string _ambientSound;

        private Color _color;

        public static Region Parse(Stream stream, bool leaveOpen)
        {
            var region = new Region();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                region._left = reader.ReadSingle();
                region._bottom = reader.ReadSingle();
                region._right = reader.ReadSingle();
                region._top = reader.ReadSingle();

                region._name = reader.ReadChars();
                region._creationNumber = reader.ReadInt32();

                region._weatherId = reader.ReadChars(4);
                region._ambientSound = reader.ReadChars();

                region._color = Color.FromArgb(reader.ReadInt32());
            }

            return region;
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
            writer.Write(_left);
            writer.Write(_bottom);
            writer.Write(_right);
            writer.Write(_top);

            writer.WriteString(_name);
            writer.Write(_creationNumber);

            writer.Write(_weatherId);
            writer.WriteString(_ambientSound);

            writer.Write(_color.B);
            writer.Write(_color.G);
            writer.Write(_color.R);
            writer.Write(_color.A);
        }
    }
}