// ------------------------------------------------------------------------------
// <copyright file="UnitData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class UnitData : WidgetData
    {
        internal UnitData(JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            GetFrom(jsonElement, formatVersion, subVersion, useNewFormat);
        }

        internal UnitData(ref Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(ref reader, formatVersion, subVersion, useNewFormat);
        }

        internal void GetFrom(JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            TypeId = jsonElement.GetInt32(nameof(TypeId));
            Variation = jsonElement.GetInt32(nameof(Variation));
            Position = jsonElement.GetVector3(nameof(Position));
            Rotation = jsonElement.GetSingle(nameof(Rotation));
            Scale = jsonElement.GetVector3(nameof(Scale));
            SkinId = useNewFormat ? jsonElement.GetInt32(nameof(SkinId)) : TypeId;

            Flags = jsonElement.GetByte(nameof(Flags));
            OwnerId = jsonElement.GetInt32(nameof(OwnerId));
            Unk1 = jsonElement.GetByte(nameof(Unk1));
            Unk2 = jsonElement.GetByte(nameof(Unk2));
            HP = jsonElement.GetInt32(nameof(HP));
            MP = jsonElement.GetInt32(nameof(MP));

            if (formatVersion == MapWidgetsFormatVersion.v8 && subVersion == MapWidgetsSubVersion.v11)
            {
                MapItemTableId = jsonElement.GetInt32(nameof(MapItemTableId));
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(ItemTableSets)))
            {
                ItemTableSets.Add(element.GetRandomItemSet(formatVersion, subVersion, useNewFormat));
            }

            GoldAmount = jsonElement.GetInt32(nameof(GoldAmount));
            TargetAcquisition = jsonElement.GetSingle(nameof(TargetAcquisition));

            HeroLevel = jsonElement.GetInt32(nameof(HeroLevel));
            if ((formatVersion == MapWidgetsFormatVersion.v8 && subVersion == MapWidgetsSubVersion.v11) || subVersion == MapWidgetsSubVersion.v10)
            {
                HeroStrength = jsonElement.GetInt32(nameof(HeroStrength));
                HeroAgility = jsonElement.GetInt32(nameof(HeroAgility));
                HeroIntelligence = jsonElement.GetInt32(nameof(HeroIntelligence));
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(InventoryData)))
            {
                InventoryData.Add(element.GetInventoryItemData(formatVersion, subVersion, useNewFormat));
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(AbilityData)))
            {
                AbilityData.Add(element.GetModifiedAbilityData(formatVersion, subVersion, useNewFormat));
            }

            var randomDataMode = jsonElement.GetInt32<RandomUnitDataMode>(nameof(RandomDataMode));
            var randomDataElement = jsonElement.GetProperty(nameof(RandomData));
            RandomData = randomDataMode switch
            {
                RandomUnitDataMode.Any => randomDataElement.GetRandomUnitNeutral(formatVersion, subVersion, useNewFormat),
                RandomUnitDataMode.GlobalTable => randomDataElement.GetRandomUnitGlobalTable(formatVersion, subVersion, useNewFormat),
                RandomUnitDataMode.CustomTable => randomDataElement.GetRandomUnitCustomTable(formatVersion, subVersion, useNewFormat),
                _ => null,
            };

            if (formatVersion > MapWidgetsFormatVersion.v6 && subVersion > MapWidgetsSubVersion.v7)
            {
                CustomPlayerColorId = jsonElement.GetInt32(nameof(CustomPlayerColorId));
                WaygateDestinationRegionId = jsonElement.GetInt32(nameof(WaygateDestinationRegionId));
                CreationNumber = jsonElement.GetInt32(nameof(CreationNumber));
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion, subVersion, useNewFormat);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(TypeId), TypeId);
            writer.WriteNumber(nameof(Variation), Variation);
            writer.Write(nameof(Position), Position);
            writer.WriteNumber(nameof(Rotation), Rotation);
            writer.Write(nameof(Scale), Scale);

            if (useNewFormat)
            {
                writer.WriteNumber(nameof(SkinId), SkinId);
            }

            writer.WriteNumber(nameof(Flags), Flags);
            writer.WriteNumber(nameof(OwnerId), OwnerId);
            writer.WriteNumber(nameof(Unk1), Unk1);
            writer.WriteNumber(nameof(Unk2), Unk2);
            writer.WriteNumber(nameof(HP), HP);
            writer.WriteNumber(nameof(MP), MP);

            if (formatVersion == MapWidgetsFormatVersion.v8 && subVersion == MapWidgetsSubVersion.v11)
            {
                writer.WriteNumber(nameof(MapItemTableId), MapItemTableId);
            }

            writer.WriteStartArray(nameof(ItemTableSets));
            foreach (var itemSet in ItemTableSets)
            {
                writer.Write(itemSet, options, formatVersion, subVersion, useNewFormat);
            }

            writer.WriteEndArray();

            writer.WriteNumber(nameof(GoldAmount), GoldAmount);
            writer.WriteNumber(nameof(TargetAcquisition), TargetAcquisition);

            writer.WriteNumber(nameof(HeroLevel), HeroLevel);
            if ((formatVersion == MapWidgetsFormatVersion.v8 && subVersion == MapWidgetsSubVersion.v11) || subVersion == MapWidgetsSubVersion.v10)
            {
                writer.WriteNumber(nameof(HeroStrength), HeroStrength);
                writer.WriteNumber(nameof(HeroAgility), HeroAgility);
                writer.WriteNumber(nameof(HeroIntelligence), HeroIntelligence);
            }

            writer.WriteStartArray(nameof(InventoryData));
            foreach (var item in InventoryData)
            {
                writer.Write(item, options, formatVersion, subVersion, useNewFormat);
            }

            writer.WriteEndArray();

            writer.WriteStartArray(nameof(AbilityData));
            foreach (var ability in AbilityData)
            {
                writer.Write(ability, options, formatVersion, subVersion, useNewFormat);
            }

            writer.WriteEndArray();

            writer.WriteObject(nameof(RandomDataMode), RandomDataMode, options);
            if (RandomData is not null)
            {
                writer.Write(RandomData, options, formatVersion, subVersion, useNewFormat);
            }

            if (formatVersion > MapWidgetsFormatVersion.v6 && subVersion > MapWidgetsSubVersion.v7)
            {
                writer.WriteNumber(nameof(CustomPlayerColorId), CustomPlayerColorId);
                writer.WriteNumber(nameof(WaygateDestinationRegionId), WaygateDestinationRegionId);
                writer.WriteNumber(nameof(CreationNumber), CreationNumber);
            }

            writer.WriteEndObject();
        }
    }
}