// ------------------------------------------------------------------------------
// <copyright file="RectangleMargins.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Common.Extensions;

namespace War3Net.Build.Common
{
    public sealed partial class RectangleMargins
    {
        internal RectangleMargins(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal RectangleMargins(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            Left = jsonElement.GetInt32(nameof(Left));
            Right = jsonElement.GetInt32(nameof(Right));
            Bottom = jsonElement.GetInt32(nameof(Bottom));
            Top = jsonElement.GetInt32(nameof(Top));
        }

        internal void ReadFrom(ref Utf8JsonReader reader)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Left), Left);
            writer.WriteNumber(nameof(Right), Right);
            writer.WriteNumber(nameof(Bottom), Bottom);
            writer.WriteNumber(nameof(Top), Top);

            writer.WriteEndObject();
        }
    }
}