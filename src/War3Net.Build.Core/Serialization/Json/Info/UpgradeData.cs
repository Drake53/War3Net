// ------------------------------------------------------------------------------
// <copyright file="UpgradeData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class UpgradeData
    {
        internal UpgradeData(JsonElement jsonElement, MapInfoFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal UpgradeData(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, MapInfoFormatVersion formatVersion)
        {
            Players = jsonElement.GetBitmask32(nameof(Players));
            Id = jsonElement.GetInt32(nameof(Id));
            Level = jsonElement.GetInt32(nameof(Level));
            Availability = jsonElement.GetInt32<UpgradeAvailability>(nameof(Availability));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapInfoFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.Write(nameof(Players), Players, options);
            writer.WriteNumber(nameof(Id), Id);
            writer.WriteNumber(nameof(Level), Level);
            writer.WriteObject(nameof(Availability), Availability, options);

            writer.WriteEndObject();
        }
    }
}