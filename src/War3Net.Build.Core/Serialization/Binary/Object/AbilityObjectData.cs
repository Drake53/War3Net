// ------------------------------------------------------------------------------
// <copyright file="AbilityObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed partial class AbilityObjectData
    {
        internal AbilityObjectData(BinaryReader reader)
        {
            ReadFrom(reader);
        }

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