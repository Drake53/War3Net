// ------------------------------------------------------------------------------
// <copyright file="AbilityObjectData.cs" company="Drake53">
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
    public abstract class AbilityObjectData
    {
        internal AbilityObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal AbilityObjectData(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public List<LevelObjectModification> BaseAbilities { get; init; } = new();

        public List<LevelObjectModification> NewAbilities { get; init; } = new();

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<ObjectDataFormatVersion>();

            nint baseAbilitiesCount = reader.ReadInt32();
            for (nint i = 0; i < baseAbilitiesCount; i++)
            {
                BaseAbilities.Add(reader.ReadLevelObjectModification(FormatVersion));
            }

            nint newAbilitiesCount = reader.ReadInt32();
            for (nint i = 0; i < newAbilitiesCount; i++)
            {
                NewAbilities.Add(reader.ReadLevelObjectModification(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.Write(BaseAbilities.Count);
            foreach (var ability in BaseAbilities)
            {
                writer.Write(ability, FormatVersion);
            }

            writer.Write(NewAbilities.Count);
            foreach (var ability in NewAbilities)
            {
                writer.Write(ability, FormatVersion);
            }
        }
    }
}