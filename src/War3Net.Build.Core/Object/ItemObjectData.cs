// ------------------------------------------------------------------------------
// <copyright file="ItemObjectData.cs" company="Drake53">
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
    public abstract class ItemObjectData
    {
        internal ItemObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal ItemObjectData(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public List<SimpleObjectModification> BaseItems { get; init; } = new();

        public List<SimpleObjectModification> NewItems { get; init; } = new();

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<ObjectDataFormatVersion>();

            nint baseItemsCount = reader.ReadInt32();
            for (nint i = 0; i < baseItemsCount; i++)
            {
                BaseItems.Add(reader.ReadSimpleObjectModification(FormatVersion));
            }

            nint newItemsCount = reader.ReadInt32();
            for (nint i = 0; i < newItemsCount; i++)
            {
                NewItems.Add(reader.ReadSimpleObjectModification(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.Write(BaseItems.Count);
            foreach (var item in BaseItems)
            {
                writer.Write(item, FormatVersion);
            }

            writer.Write(NewItems.Count);
            foreach (var item in NewItems)
            {
                writer.Write(item, FormatVersion);
            }
        }
    }
}