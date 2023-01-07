// ------------------------------------------------------------------------------
// <copyright file="ObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Extensions;
using War3Net.Build.Serialization.Json;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    [JsonConverter(typeof(JsonObjectDataConverter))]
    public sealed partial class ObjectData
    {
        internal ObjectData(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal ObjectData(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<ObjectDataFormatVersion>(nameof(FormatVersion));

            var unitDataElement = jsonElement.GetProperty(nameof(UnitData));
            if (unitDataElement.ValueKind != JsonValueKind.Null)
            {
                UnitData = unitDataElement.GetUnitObjectData();
            }

            var itemDataElement = jsonElement.GetProperty(nameof(ItemData));
            if (itemDataElement.ValueKind != JsonValueKind.Null)
            {
                ItemData = itemDataElement.GetItemObjectData();
            }

            var destructableDataElement = jsonElement.GetProperty(nameof(DestructableData));
            if (destructableDataElement.ValueKind != JsonValueKind.Null)
            {
                DestructableData = destructableDataElement.GetDestructableObjectData();
            }

            var doodadDataElement = jsonElement.GetProperty(nameof(DoodadData));
            if (doodadDataElement.ValueKind != JsonValueKind.Null)
            {
                DoodadData = doodadDataElement.GetDoodadObjectData();
            }

            var abilityDataElement = jsonElement.GetProperty(nameof(AbilityData));
            if (abilityDataElement.ValueKind != JsonValueKind.Null)
            {
                AbilityData = abilityDataElement.GetAbilityObjectData();
            }

            var buffDataElement = jsonElement.GetProperty(nameof(BuffData));
            if (buffDataElement.ValueKind != JsonValueKind.Null)
            {
                BuffData = buffDataElement.GetBuffObjectData();
            }

            var upgradeDataElement = jsonElement.GetProperty(nameof(UpgradeData));
            if (upgradeDataElement.ValueKind != JsonValueKind.Null)
            {
                UpgradeData = upgradeDataElement.GetUpgradeObjectData();
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteObject(nameof(FormatVersion), FormatVersion, options);

            if (UnitData is null)
            {
                writer.WriteNull(nameof(UnitData));
            }
            else
            {
                writer.WritePropertyName(nameof(UnitData));
                writer.Write(UnitData, options);
            }

            if (ItemData is null)
            {
                writer.WriteNull(nameof(ItemData));
            }
            else
            {
                writer.WritePropertyName(nameof(ItemData));
                writer.Write(ItemData, options);
            }

            if (DestructableData is null)
            {
                writer.WriteNull(nameof(DestructableData));
            }
            else
            {
                writer.WritePropertyName(nameof(DestructableData));
                writer.Write(DestructableData, options);
            }

            if (DoodadData is null)
            {
                writer.WriteNull(nameof(DoodadData));
            }
            else
            {
                writer.WritePropertyName(nameof(DoodadData));
                writer.Write(DoodadData, options);
            }

            if (AbilityData is null)
            {
                writer.WriteNull(nameof(AbilityData));
            }
            else
            {
                writer.WritePropertyName(nameof(AbilityData));
                writer.Write(AbilityData, options);
            }

            if (BuffData is null)
            {
                writer.WriteNull(nameof(BuffData));
            }
            else
            {
                writer.WritePropertyName(nameof(BuffData));
                writer.Write(BuffData, options);
            }

            if (UpgradeData is null)
            {
                writer.WriteNull(nameof(UpgradeData));
            }
            else
            {
                writer.WritePropertyName(nameof(UpgradeData));
                writer.Write(UpgradeData, options);
            }
        }
    }
}