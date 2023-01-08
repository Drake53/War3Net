// ------------------------------------------------------------------------------
// <copyright file="ModifiedAbilityData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class ModifiedAbilityData
    {
        internal ModifiedAbilityData(JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            GetFrom(jsonElement, formatVersion, subVersion, useNewFormat);
        }

        internal ModifiedAbilityData(ref Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(ref reader, formatVersion, subVersion, useNewFormat);
        }

        internal void GetFrom(JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            AbilityId = jsonElement.GetInt32(nameof(AbilityId));
            IsAutocastActive = jsonElement.GetBoolean(nameof(IsAutocastActive));
            HeroAbilityLevel = jsonElement.GetInt32(nameof(HeroAbilityLevel));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion, subVersion, useNewFormat);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(AbilityId), AbilityId);
            writer.WriteBoolean(nameof(IsAutocastActive), IsAutocastActive);
            writer.WriteNumber(nameof(HeroAbilityLevel), HeroAbilityLevel);

            writer.WriteEndObject();
        }
    }
}