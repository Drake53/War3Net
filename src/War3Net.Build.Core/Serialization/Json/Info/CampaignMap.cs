// ------------------------------------------------------------------------------
// <copyright file="CampaignMap.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class CampaignMap
    {
        internal CampaignMap(JsonElement jsonElement, CampaignInfoFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal CampaignMap(ref Utf8JsonReader reader, CampaignInfoFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, CampaignInfoFormatVersion formatVersion)
        {
            Unk = jsonElement.GetString(nameof(Unk));
            MapFilePath = jsonElement.GetString(nameof(MapFilePath));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, CampaignInfoFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, CampaignInfoFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(Unk), Unk);
            writer.WriteString(nameof(MapFilePath), MapFilePath);

            writer.WriteEndObject();
        }
    }
}