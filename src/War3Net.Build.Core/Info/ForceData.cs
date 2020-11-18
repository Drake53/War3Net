// ------------------------------------------------------------------------------
// <copyright file="ForceData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed class ForceData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForceData"/> class.
        /// </summary>
        public ForceData()
        {
        }

        internal ForceData(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        public ForceFlags Flags { get; set; }

        public Bitmask32 Players { get; set; }

        public string Name { get; set; }

        internal void ReadFrom(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            Flags = reader.ReadInt32<ForceFlags>();
            Players = reader.ReadBitmask32();
            Name = reader.ReadChars();
        }

        internal void WriteTo(BinaryWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.Write((int)Flags);
            writer.Write(Players);
            writer.WriteString(Name);
        }
    }
}