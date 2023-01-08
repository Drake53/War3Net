// ------------------------------------------------------------------------------
// <copyright file="CampaignInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Extensions;
using War3Net.Build.Serialization.Json;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    [JsonConverter(typeof(JsonCampaignInfoConverter))]
    public sealed partial class CampaignInfo
    {
        internal CampaignInfo(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal CampaignInfo(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<CampaignInfoFormatVersion>(nameof(FormatVersion));
            CampaignVersion = jsonElement.GetInt32(nameof(CampaignVersion));
            EditorVersion = jsonElement.GetInt32(nameof(EditorVersion));
            CampaignName = jsonElement.GetString(nameof(CampaignName));
            CampaignDifficulty = jsonElement.GetString(nameof(CampaignDifficulty));
            CampaignAuthor = jsonElement.GetString(nameof(CampaignAuthor));
            CampaignDescription = jsonElement.GetString(nameof(CampaignDescription));
            CampaignFlags = jsonElement.GetInt32<CampaignFlags>(nameof(CampaignFlags));
            CampaignBackgroundNumber = jsonElement.GetInt32(nameof(CampaignBackgroundNumber));
            BackgroundScreenPath = jsonElement.GetString(nameof(BackgroundScreenPath));
            MinimapPath = jsonElement.GetString(nameof(MinimapPath));
            AmbientSoundNumber = jsonElement.GetInt32(nameof(AmbientSoundNumber));
            AmbientSoundPath = jsonElement.GetString(nameof(AmbientSoundPath));
            FogStyle = jsonElement.GetInt32<FogStyle>(nameof(FogStyle));
            FogStartZ = jsonElement.GetSingle(nameof(FogStartZ));
            FogEndZ = jsonElement.GetSingle(nameof(FogEndZ));
            FogDensity = jsonElement.GetSingle(nameof(FogDensity));
            FogColor = jsonElement.GetColor(nameof(FogColor));
            Race = jsonElement.GetInt32<CampaignRace>(nameof(Race));

            foreach (var element in jsonElement.EnumerateArray(nameof(MapButtons)))
            {
                MapButtons.Add(element.GetCampaignMapButton(FormatVersion));
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(Maps)))
            {
                Maps.Add(element.GetCampaignMap(FormatVersion));
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
            writer.WriteNumber(nameof(CampaignVersion), CampaignVersion);
            writer.WriteNumber(nameof(EditorVersion), EditorVersion);
            writer.WriteString(nameof(CampaignName), CampaignName);
            writer.WriteString(nameof(CampaignDifficulty), CampaignDifficulty);
            writer.WriteString(nameof(CampaignAuthor), CampaignAuthor);
            writer.WriteString(nameof(CampaignDescription), CampaignDescription);
            writer.WriteObject(nameof(CampaignFlags), CampaignFlags, options);
            writer.WriteNumber(nameof(CampaignBackgroundNumber), CampaignBackgroundNumber);
            writer.WriteString(nameof(BackgroundScreenPath), BackgroundScreenPath);
            writer.WriteString(nameof(MinimapPath), MinimapPath);
            writer.WriteNumber(nameof(AmbientSoundNumber), AmbientSoundNumber);
            writer.WriteString(nameof(AmbientSoundPath), AmbientSoundPath);
            writer.WriteObject(nameof(FogStyle), FogStyle, options);
            writer.WriteNumber(nameof(FogStartZ), FogStartZ);
            writer.WriteNumber(nameof(FogEndZ), FogEndZ);
            writer.WriteNumber(nameof(FogDensity), FogDensity);
            writer.Write(nameof(FogColor), FogColor);
            writer.WriteObject(nameof(Race), Race, options);

            writer.WriteStartArray(nameof(MapButtons));
            foreach (var mapButton in MapButtons)
            {
                writer.Write(mapButton, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteStartArray(nameof(Maps));
            foreach (var map in Maps)
            {
                writer.Write(map, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}