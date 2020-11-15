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
    public sealed class ObjectData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public ObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal ObjectData(BinaryReader reader, bool fromCampaign)
        {
            ReadFrom(reader, fromCampaign);
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public UnitObjectData? UnitData { get; set; }

        public ItemObjectData? ItemData { get; set; }

        public DestructableObjectData? DestructableData { get; set; }

        public DoodadObjectData? DoodadData { get; set; }

        public AbilityObjectData? AbilityData { get; set; }

        public BuffObjectData? BuffData { get; set; }

        public UpgradeObjectData? UpgradeData { get; set; }

        internal void ReadFrom(BinaryReader reader, bool fromCampaign)
        {
            FormatVersion = reader.ReadInt32<ObjectDataFormatVersion>();

            UnitData = reader.ReadBool() ? reader.ReadUnitObjectData(fromCampaign) : null;
            ItemData = reader.ReadBool() ? reader.ReadItemObjectData(fromCampaign) : null;
            DestructableData = reader.ReadBool() ? reader.ReadDestructableObjectData(fromCampaign) : null;
            DoodadData = reader.ReadBool() ? reader.ReadDoodadObjectData(fromCampaign) : null;
            AbilityData = reader.ReadBool() ? reader.ReadAbilityObjectData(fromCampaign) : null;
            BuffData = reader.ReadBool() ? reader.ReadBuffObjectData(fromCampaign) : null;
            UpgradeData = reader.ReadBool() ? reader.ReadUpgradeObjectData(fromCampaign) : null;
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