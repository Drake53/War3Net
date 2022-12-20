// ------------------------------------------------------------------------------
// <copyright file="SpecialDoodadData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;
using System.IO;

namespace War3Net.Build.Widget
{
    public sealed partial class SpecialDoodadData
    {
        internal SpecialDoodadData(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, SpecialDoodadVersion specialDoodadVersion)
        {
            ReadFrom(reader, formatVersion, subVersion, specialDoodadVersion);
        }

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