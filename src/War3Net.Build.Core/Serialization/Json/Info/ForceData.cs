// ------------------------------------------------------------------------------
// <copyright file="ForceData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class ForceData
    {
        internal ForceData(JsonElement jsonElement, MapInfoFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal ForceData(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, MapInfoFormatVersion formatVersion)
        {
            Flags = jsonElement.GetInt32<ForceFlags>(nameof(Flags));
            Players = jsonElement.GetBitmask32(nameof(Players));
            Name = jsonElement.GetString(nameof(Name));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapInfoFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteObject(nameof(Flags), Flags, options);
            writer.Write(nameof(Players), Players, options);
            writer.WriteString(nameof(Name), Name);

            writer.WriteEndObject();
        }
    }
}