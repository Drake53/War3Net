// ------------------------------------------------------------------------------
// <copyright file="Bitmask32.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

namespace War3Net.Build.Common
{
    public sealed partial class Bitmask32
    {
        internal Bitmask32(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal Bitmask32(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            _mask = jsonElement.GetInt32();
        }

        internal void ReadFrom(ref Utf8JsonReader reader)
        {
            _mask = reader.GetInt32();
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(_mask);
        }
    }
}