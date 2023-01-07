// ------------------------------------------------------------------------------
// <copyright file="ObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed partial class ObjectData
    {
        internal ObjectData(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<ObjectDataFormatVersion>();

            UnitData = reader.ReadBool() ? reader.ReadUnitObjectData() : null;
            ItemData = reader.ReadBool() ? reader.ReadItemObjectData() : null;
            DestructableData = reader.ReadBool() ? reader.ReadDestructableObjectData() : null;
            DoodadData = reader.ReadBool() ? reader.ReadDoodadObjectData() : null;
            AbilityData = reader.ReadBool() ? reader.ReadAbilityObjectData() : null;
            BuffData = reader.ReadBool() ? reader.ReadBuffObjectData() : null;
            UpgradeData = reader.ReadBool() ? reader.ReadUpgradeObjectData() : null;
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.WriteBool(UnitData is not null);
            UnitData?.WriteTo(writer);

            writer.WriteBool(ItemData is not null);
            ItemData?.WriteTo(writer);

            writer.WriteBool(DestructableData is not null);
            DestructableData?.WriteTo(writer);

            writer.WriteBool(DoodadData is not null);
            DoodadData?.WriteTo(writer);

            writer.WriteBool(AbilityData is not null);
            AbilityData?.WriteTo(writer);

            writer.WriteBool(BuffData is not null);
            BuffData?.WriteTo(writer);

            writer.WriteBool(UpgradeData is not null);
            UpgradeData?.WriteTo(writer);
        }
    }
}