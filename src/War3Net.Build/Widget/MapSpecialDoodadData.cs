// ------------------------------------------------------------------------------
// <copyright file="MapSpecialDoodadData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Build.Widget
{
    public sealed class MapSpecialDoodadData
    {
        private char[] _typeId;
        private int _variation;

        private int _positionX;
        private int _positionY;

        public string TypeId => new string(_typeId);

        public static MapSpecialDoodadData Parse(Stream stream, bool leaveOpen = false)
        {
            var doodadData = new MapSpecialDoodadData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                doodadData._typeId = reader.ReadChars(4);
                doodadData._variation = reader.ReadInt32();

                doodadData._positionX = reader.ReadInt32();
                doodadData._positionY = reader.ReadInt32();
            }

            return doodadData;
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
            writer.Write(_typeId);
            writer.Write(_variation);

            writer.Write(_positionX);
            writer.Write(_positionY);
        }
    }
}