// ------------------------------------------------------------------------------
// <copyright file="Region.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed partial class Region
    {
        internal Region(JsonElement jsonElement, MapRegionsFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal Region(ref Utf8JsonReader reader, MapRegionsFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, MapRegionsFormatVersion formatVersion)
        {
            Left = jsonElement.GetSingle(nameof(Left));
            Bottom = jsonElement.GetSingle(nameof(Bottom));
            Right = jsonElement.GetSingle(nameof(Right));
            Top = jsonElement.GetSingle(nameof(Top));
            Name = jsonElement.GetString(nameof(Name));
            CreationNumber = jsonElement.GetInt32(nameof(CreationNumber));
            WeatherType = jsonElement.GetInt32<WeatherType>(nameof(WeatherType));
            AmbientSound = jsonElement.GetString(nameof(AmbientSound));
            Color = jsonElement.GetColor(nameof(Color));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapRegionsFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapRegionsFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Left), Left);
            writer.WriteNumber(nameof(Bottom), Bottom);
            writer.WriteNumber(nameof(Right), Right);
            writer.WriteNumber(nameof(Top), Top);
            writer.WriteString(nameof(Name), Name);
            writer.WriteNumber(nameof(CreationNumber), CreationNumber);
            writer.WriteObject(nameof(WeatherType), WeatherType, options);
            writer.WriteString(nameof(AmbientSound), AmbientSound);
            writer.Write(nameof(Color), Color);

            writer.WriteEndObject();
        }
    }
}