// ------------------------------------------------------------------------------
// <copyright file="SpecialDoodadData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class SpecialDoodadData
    {
        internal SpecialDoodadData(JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, SpecialDoodadVersion specialDoodadVersion)
        {
            GetFrom(jsonElement, formatVersion, subVersion, specialDoodadVersion);
        }

        internal SpecialDoodadData(ref Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, SpecialDoodadVersion specialDoodadVersion)
        {
            ReadFrom(ref reader, formatVersion, subVersion, specialDoodadVersion);
        }

        internal void GetFrom(JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, SpecialDoodadVersion specialDoodadVersion)
        {
            TypeId = jsonElement.GetInt32(nameof(TypeId));
            Variation = jsonElement.GetInt32(nameof(Variation));
            Position = jsonElement.GetPoint(nameof(Position));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, SpecialDoodadVersion specialDoodadVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion, subVersion, specialDoodadVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, SpecialDoodadVersion specialDoodadVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(TypeId), TypeId);
            writer.WriteNumber(nameof(Variation), Variation);
            writer.Write(nameof(Position), Position);

            writer.WriteEndObject();
        }
    }
}