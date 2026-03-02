using System.IO;
using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed partial class ItemObjectData
    {
        internal ItemObjectData(BinaryReader reader)
        {
            ReadFrom(reader);
        }

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