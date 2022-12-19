// ------------------------------------------------------------------------------
// <copyright file="Utf8JsonWriterExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;
using System.Numerics;
using System.Text.Json;

using War3Net.Build.Common;
using War3Net.Build.Info;
using War3Net.Build.Widget;

namespace War3Net.Build.Extensions
{
    internal static class Utf8JsonWriterExtensions
    {
        public static void WriteObject<TValue>(this Utf8JsonWriter writer, string propertyName, TValue value, JsonSerializerOptions? options = null)
        {
            writer.WritePropertyName(propertyName);
            JsonSerializer.Serialize(writer, value, options);
        }

        public static void Write(this Utf8JsonWriter writer, Color color)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Color.R), color.R);
            writer.WriteNumber(nameof(Color.G), color.G);
            writer.WriteNumber(nameof(Color.B), color.B);
            writer.WriteNumber(nameof(Color.A), color.A);

            writer.WriteEndObject();
        }

        public static void Write(this Utf8JsonWriter writer, string propertyName, Color color)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(color);
        }

        public static void Write(this Utf8JsonWriter writer, Vector2 vector)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Vector2.X), vector.X);
            writer.WriteNumber(nameof(Vector2.Y), vector.Y);

            writer.WriteEndObject();
        }

        public static void Write(this Utf8JsonWriter writer, string propertyName, Vector2 vector)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(vector);
        }

        public static void Write(this Utf8JsonWriter writer, Bitmask32 bitmask, JsonSerializerOptions options) => bitmask.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, string propertyName, Bitmask32 bitmask, JsonSerializerOptions options)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(bitmask, options);
        }

        public static void Write(this Utf8JsonWriter writer, Quadrilateral quadrilateral, JsonSerializerOptions options) => quadrilateral.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, string propertyName, Quadrilateral quadrilateral, JsonSerializerOptions options)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(quadrilateral, options);
        }

        public static void Write(this Utf8JsonWriter writer, RandomItemSet randomItemSet, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => randomItemSet.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomItemSet randomItemSet, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => randomItemSet.WriteTo(writer, options, formatVersion, subVersion, useNewFormat);

        public static void Write(this Utf8JsonWriter writer, RandomItemSetItem randomItemSetItem, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => randomItemSetItem.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomItemSetItem randomItemSetItem, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => randomItemSetItem.WriteTo(writer, options, formatVersion, subVersion, useNewFormat);

        public static void Write(this Utf8JsonWriter writer, RectangleMargins rectangleMargins, JsonSerializerOptions options) => rectangleMargins.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, string propertyName, RectangleMargins? rectangleMargins, JsonSerializerOptions options)
        {
            writer.WritePropertyName(propertyName);

            if (rectangleMargins is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.Write(rectangleMargins, options);
            }
        }

        public static void Write(this Utf8JsonWriter writer, MapInfo mapInfo, JsonSerializerOptions options) => mapInfo.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, PlayerData playerData, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => playerData.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, ForceData forceData, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => forceData.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, UpgradeData upgradeData, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => upgradeData.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, TechData techData, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => techData.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomUnitTable randomUnitTable, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => randomUnitTable.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomItemTable randomItemTable, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => randomItemTable.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomUnitSet randomUnitSet, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => randomUnitSet.WriteTo(writer, options, formatVersion);
    }
}