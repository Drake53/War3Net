// ------------------------------------------------------------------------------
// <copyright file="MapCustomTextTriggers.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Extensions;
using War3Net.Build.Serialization.Json;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    [JsonConverter(typeof(JsonMapCustomTextTriggersConverter))]
    public sealed partial class MapCustomTextTriggers
    {
        internal MapCustomTextTriggers(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapCustomTextTriggers(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<MapCustomTextTriggersFormatVersion>(nameof(FormatVersion));
            var subVersionElement = jsonElement.GetProperty(nameof(SubVersion));
            if (subVersionElement.ValueKind != JsonValueKind.Null)
            {
                SubVersion = subVersionElement.GetInt32<MapCustomTextTriggersSubVersion>();
            }

            if (FormatVersion >= MapCustomTextTriggersFormatVersion.v1)
            {
                GlobalCustomScriptComment = jsonElement.GetString(nameof(GlobalCustomScriptComment));
                GlobalCustomScriptCode = jsonElement.GetProperty(nameof(GlobalCustomScriptCode)).GetCustomTextTrigger(FormatVersion, SubVersion);
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(CustomTextTriggers)))
            {
                CustomTextTriggers.Add(element.GetCustomTextTrigger(FormatVersion, SubVersion));
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
            if (SubVersion is not null)
            {
                writer.WriteObject(nameof(SubVersion), SubVersion, options);
            }

            if (FormatVersion >= MapCustomTextTriggersFormatVersion.v1)
            {
                writer.WriteString(nameof(GlobalCustomScriptComment), GlobalCustomScriptComment);

                writer.WritePropertyName(nameof(GlobalCustomScriptCode));
                writer.Write(GlobalCustomScriptCode, options, FormatVersion, SubVersion);
            }

            writer.WriteStartArray(nameof(CustomTextTriggers));
            foreach (var customTextTrigger in CustomTextTriggers)
            {
                writer.Write(customTextTrigger, options, FormatVersion, SubVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}