// ------------------------------------------------------------------------------
// <copyright file="DestructableObjectData.cs" company="Drake53">
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
    public abstract class DestructableObjectData
    {
        internal DestructableObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal DestructableObjectData(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public List<SimpleObjectModification> BaseDestructables { get; init; } = new();

        public List<SimpleObjectModification> NewDestructables { get; init; } = new();

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<ObjectDataFormatVersion>();

            nint baseDestructablesCount = reader.ReadInt32();
            for (nint i = 0; i < baseDestructablesCount; i++)
            {
                BaseDestructables.Add(reader.ReadSimpleObjectModification(FormatVersion));
            }

            nint newDestructablesCount = reader.ReadInt32();
            for (nint i = 0; i < newDestructablesCount; i++)
            {
                NewDestructables.Add(reader.ReadSimpleObjectModification(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.Write(BaseDestructables.Count);
            foreach (var destructable in BaseDestructables)
            {
                writer.Write(destructable, FormatVersion);
            }

            writer.Write(NewDestructables.Count);
            foreach (var destructable in NewDestructables)
            {
                writer.Write(destructable, FormatVersion);
            }
        }
    }
}