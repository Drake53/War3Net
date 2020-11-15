// ------------------------------------------------------------------------------
// <copyright file="UpgradeObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public abstract class UpgradeObjectData
    {
        internal UpgradeObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal UpgradeObjectData(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public List<LevelObjectModification> BaseUpgrades { get; init; } = new();

        public List<LevelObjectModification> NewUpgrades { get; init; } = new();

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<ObjectDataFormatVersion>();

            nint baseUpgradesCount = reader.ReadInt32();
            for (nint i = 0; i < baseUpgradesCount; i++)
            {
                BaseUpgrades.Add(reader.ReadLevelObjectModification(FormatVersion));
            }

            nint newUpgradesCount = reader.ReadInt32();
            for (nint i = 0; i < newUpgradesCount; i++)
            {
                NewUpgrades.Add(reader.ReadLevelObjectModification(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.Write(BaseUpgrades.Count);
            foreach (var upgrade in BaseUpgrades)
            {
                writer.Write(upgrade, FormatVersion);
            }

            writer.Write(NewUpgrades.Count);
            foreach (var upgrade in NewUpgrades)
            {
                writer.Write(upgrade, FormatVersion);
            }
        }
    }
}