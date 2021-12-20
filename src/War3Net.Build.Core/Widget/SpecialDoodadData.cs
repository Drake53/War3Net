// ------------------------------------------------------------------------------
// <copyright file="SpecialDoodadData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;
using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed class SpecialDoodadData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialDoodadData"/> class.
        /// </summary>
        public SpecialDoodadData()
        {
        }

        internal SpecialDoodadData(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, SpecialDoodadVersion specialDoodadVersion)
        {
            ReadFrom(reader, formatVersion, subVersion, specialDoodadVersion);
        }

        public int TypeId { get; set; }

        public int Variation { get; set; }

        public Point Position { get; set; }

        public override string ToString() => TypeId.ToRawcode();

        internal void ReadFrom(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, SpecialDoodadVersion specialDoodadVersion)
        {
            TypeId = reader.ReadInt32();
            Variation = reader.ReadInt32();
            Position = new Point(reader.ReadInt32(), reader.ReadInt32());
        }

        internal void WriteTo(BinaryWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, SpecialDoodadVersion specialDoodadVersion)
        {
            writer.Write(TypeId);
            writer.Write(Variation);
            writer.Write(Position.X);
            writer.Write(Position.Y);
        }
    }
}