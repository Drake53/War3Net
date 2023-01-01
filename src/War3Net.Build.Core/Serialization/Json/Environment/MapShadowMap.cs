// ------------------------------------------------------------------------------
// <copyright file="MapShadowMap.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed partial class MapShadowMap
    {
        internal MapShadowMap(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapShadowMap(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            foreach (var element in jsonElement.EnumerateArray(nameof(Cells)))
            {
                Cells.Add(element.GetByte());
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteObject(nameof(Cells), Cells, options);

            writer.WriteEndObject();
        }
    }
}