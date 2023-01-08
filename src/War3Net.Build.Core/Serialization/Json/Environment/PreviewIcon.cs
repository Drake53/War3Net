// ------------------------------------------------------------------------------
// <copyright file="PreviewIcon.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed partial class PreviewIcon
    {
        internal PreviewIcon(JsonElement jsonElement, MapPreviewIconsFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal PreviewIcon(ref Utf8JsonReader reader, MapPreviewIconsFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, MapPreviewIconsFormatVersion formatVersion)
        {
            IconType = jsonElement.GetInt32<PreviewIconType>(nameof(IconType));
            X = jsonElement.GetInt32(nameof(X));
            Y = jsonElement.GetInt32(nameof(Y));
            Color = jsonElement.GetColor(nameof(Color));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapPreviewIconsFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapPreviewIconsFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteObject(nameof(IconType), IconType, options);
            writer.WriteNumber(nameof(X), X);
            writer.WriteNumber(nameof(Y), Y);
            writer.Write(nameof(Color), Color);

            writer.WriteEndObject();
        }
    }
}