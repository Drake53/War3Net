// ------------------------------------------------------------------------------
// <copyright file="UpgradeObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public abstract partial class UpgradeObjectData
    {
        internal UpgradeObjectData(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal UpgradeObjectData(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<ObjectDataFormatVersion>(nameof(FormatVersion));

            foreach (var element in jsonElement.EnumerateArray(nameof(BaseUpgrades)))
            {
                BaseUpgrades.Add(element.GetLevelObjectModification(FormatVersion));
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(NewUpgrades)))
            {
                NewUpgrades.Add(element.GetLevelObjectModification(FormatVersion));
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

            writer.WriteStartArray(nameof(BaseUpgrades));
            foreach (var upgrade in BaseUpgrades)
            {
                writer.Write(upgrade, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteStartArray(nameof(NewUpgrades));
            foreach (var upgrade in NewUpgrades)
            {
                writer.Write(upgrade, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}