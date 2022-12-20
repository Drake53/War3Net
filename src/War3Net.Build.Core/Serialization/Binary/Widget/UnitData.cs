// ------------------------------------------------------------------------------
// <copyright file="UnitData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Numerics;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class UnitData : WidgetData
    {
        internal UnitData(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, out bool useNewFormat)
        {
            ReadFrom(reader, formatVersion, subVersion, out useNewFormat);
        }

        internal void ReadFrom(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, out bool useNewFormat)
        {
            TypeId = reader.ReadInt32();
            Variation = reader.ReadInt32();
            Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Rotation = reader.ReadSingle();
            Scale = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            // Check if next byte is 'printable' (this also assumes that _flags byte is a low number).
            useNewFormat = reader.PeekChar() >= 0x20;
            SkinId = useNewFormat ? reader.ReadInt32() : TypeId;

            Flags = reader.ReadByte();
            OwnerId = reader.ReadInt32();
            Unk1 = reader.ReadByte();
            Unk2 = reader.ReadByte();
            HP = reader.ReadInt32();
            MP = reader.ReadInt32();

            if (formatVersion == MapWidgetsFormatVersion.v8 && subVersion == MapWidgetsSubVersion.v11)
            {
                MapItemTableId = reader.ReadInt32();
            }

            nint itemSetCount = reader.ReadInt32();
            for (nint i = 0; i < itemSetCount; i++)
            {
                ItemTableSets.Add(reader.ReadRandomItemSet(formatVersion, subVersion, useNewFormat));
            }

            GoldAmount = reader.ReadInt32();
            TargetAcquisition = reader.ReadSingle();

            HeroLevel = reader.ReadInt32();
            if ((formatVersion == MapWidgetsFormatVersion.v8 && subVersion == MapWidgetsSubVersion.v11) || subVersion == MapWidgetsSubVersion.v10)
            {
                HeroStrength = reader.ReadInt32();
                HeroAgility = reader.ReadInt32();
                HeroIntelligence = reader.ReadInt32();
            }

            nint itemCount = reader.ReadInt32();
            for (nint i = 0; i < itemCount; i++)
            {
                InventoryData.Add(reader.ReadInventoryItemData(formatVersion, subVersion, useNewFormat));
            }

            nint abilityCount = reader.ReadInt32();
            for (nint i = 0; i < abilityCount; i++)
            {
                AbilityData.Add(reader.ReadModifiedAbilityData(formatVersion, subVersion, useNewFormat));
            }

            var randomDataMode = reader.ReadInt32<RandomUnitDataMode>();
            RandomData = randomDataMode switch
            {
                RandomUnitDataMode.Any => reader.ReadRandomUnitNeutral(formatVersion, subVersion, useNewFormat),
                RandomUnitDataMode.GlobalTable => reader.ReadRandomUnitGlobalTable(formatVersion, subVersion, useNewFormat),
                RandomUnitDataMode.CustomTable => reader.ReadRandomUnitCustomTable(formatVersion, subVersion, useNewFormat),
                _ => null,
            };

            if (formatVersion > MapWidgetsFormatVersion.v6 && subVersion > MapWidgetsSubVersion.v7)
            {
                CustomPlayerColorId = reader.ReadInt32();
                WaygateDestinationRegionId = reader.ReadInt32();
                CreationNumber = reader.ReadInt32();
            }
        }

        internal void WriteTo(BinaryWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.Write(TypeId);
            writer.Write(Variation);
            writer.Write(Position.X);
            writer.Write(Position.Y);
            writer.Write(Position.Z);
            writer.Write(Rotation);
            writer.Write(Scale.X);
            writer.Write(Scale.Y);
            writer.Write(Scale.Z);

            if (useNewFormat)
            {
                writer.Write(SkinId);
            }

            writer.Write(Flags);
            writer.Write(OwnerId);
            writer.Write(Unk1);
            writer.Write(Unk2);
            writer.Write(HP);
            writer.Write(MP);

            if (formatVersion == MapWidgetsFormatVersion.v8 && subVersion == MapWidgetsSubVersion.v11)
            {
                writer.Write(MapItemTableId);
            }

            writer.Write(ItemTableSets.Count);
            foreach (var itemSet in ItemTableSets)
            {
                writer.Write(itemSet, formatVersion, subVersion, useNewFormat);
            }

            writer.Write(GoldAmount);
            writer.Write(TargetAcquisition);

            writer.Write(HeroLevel);
            if ((formatVersion == MapWidgetsFormatVersion.v8 && subVersion == MapWidgetsSubVersion.v11) || subVersion == MapWidgetsSubVersion.v10)
            {
                writer.Write(HeroStrength);
                writer.Write(HeroAgility);
                writer.Write(HeroIntelligence);
            }

            writer.Write(InventoryData.Count);
            foreach (var item in InventoryData)
            {
                writer.Write(item, formatVersion, subVersion, useNewFormat);
            }

            writer.Write(AbilityData.Count);
            foreach (var ability in AbilityData)
            {
                writer.Write(ability, formatVersion, subVersion, useNewFormat);
            }

            var mode = RandomData switch
            {
                RandomUnitAny => RandomUnitDataMode.Any,
                RandomUnitGlobalTable => RandomUnitDataMode.GlobalTable,
                RandomUnitCustomTable => RandomUnitDataMode.CustomTable,
                _ => RandomUnitDataMode.None,
            };

            writer.Write((int)mode);
            if (RandomData is not null)
            {
                writer.Write(RandomData, formatVersion, subVersion, useNewFormat);
            }

            if (formatVersion > MapWidgetsFormatVersion.v6 && subVersion > MapWidgetsSubVersion.v7)
            {
                writer.Write(CustomPlayerColorId);
                writer.Write(WaygateDestinationRegionId);
                writer.Write(CreationNumber);
            }
        }
    }
}