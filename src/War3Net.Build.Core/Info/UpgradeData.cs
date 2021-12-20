// ------------------------------------------------------------------------------
// <copyright file="UpgradeData.cs" company="Drake53">
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
    public sealed class UpgradeData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeData"/> class.
        /// </summary>
        public UpgradeData()
        {
        }

        internal UpgradeData(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        public Bitmask32 Players { get; set; }

        public int Id { get; set; }

        // 0-indexed
        public int Level { get; set; }

        public UpgradeAvailability Availability { get; set; }

        public override string ToString() => Id.ToRawcode();

        internal void ReadFrom(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            Players = reader.ReadBitmask32();
            Id = reader.ReadInt32();
            Level = reader.ReadInt32();
            Availability = reader.ReadInt32<UpgradeAvailability>();
        }

        internal void WriteTo(BinaryWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.Write(Players);
            writer.Write(Id);
            writer.Write(Level);
            writer.Write((int)Availability);
        }
    }
}