// ------------------------------------------------------------------------------
// <copyright file="CustomTextTrigger.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text;
using System.Text.Json;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class CustomTextTrigger
    {
        internal CustomTextTrigger(JsonElement jsonElement, MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion)
        {
            GetFrom(jsonElement, formatVersion, subVersion);
        }

        internal CustomTextTrigger(ref Utf8JsonReader reader, MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion)
        {
            ReadFrom(ref reader, formatVersion, subVersion);
        }

        internal void GetFrom(JsonElement jsonElement, MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion)
        {
            Code = jsonElement.GetString(nameof(Code));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion, subVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(Code), Code);

            writer.WriteEndObject();
        }
    }
}