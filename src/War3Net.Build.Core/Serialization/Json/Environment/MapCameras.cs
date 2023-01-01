// ------------------------------------------------------------------------------
// <copyright file="MapCameras.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed partial class MapCameras
    {
        internal MapCameras(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapCameras(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<MapCamerasFormatVersion>(nameof(FormatVersion));
            UseNewFormat = jsonElement.GetBoolean(nameof(UseNewFormat));

            foreach (var element in jsonElement.EnumerateArray(nameof(Cameras)))
            {
                Cameras.Add(element.GetCamera(FormatVersion, UseNewFormat));
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
            writer.WriteBoolean(nameof(UseNewFormat), UseNewFormat);

            writer.WriteStartArray(nameof(Cameras));
            foreach (var camera in Cameras)
            {
                writer.Write(camera, options, FormatVersion, UseNewFormat);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}