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

        public static void Write(this Utf8JsonWriter writer, Bitmask32 bitmask) => bitmask.WriteTo(writer);

        public static void Write(this Utf8JsonWriter writer, string propertyName, Bitmask32 bitmask)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(bitmask);
        }

        public static void Write(this Utf8JsonWriter writer, Quadrilateral quadrilateral) => quadrilateral.WriteTo(writer);

        public static void Write(this Utf8JsonWriter writer, string propertyName, Quadrilateral quadrilateral)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(quadrilateral);
        }

        public static void Write(this Utf8JsonWriter writer, RandomItemSet randomItemSet, MapInfoFormatVersion formatVersion) => randomItemSet.WriteTo(writer, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomItemSet randomItemSet, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => randomItemSet.WriteTo(writer, formatVersion, subVersion, useNewFormat);

        public static void Write(this Utf8JsonWriter writer, RandomItemSetItem randomItemSetItem, MapInfoFormatVersion formatVersion) => randomItemSetItem.WriteTo(writer, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomItemSetItem randomItemSetItem, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => randomItemSetItem.WriteTo(writer, formatVersion, subVersion, useNewFormat);

        public static void Write(this Utf8JsonWriter writer, RectangleMargins rectangleMargins) => rectangleMargins.WriteTo(writer);

        public static void Write(this Utf8JsonWriter writer, string propertyName, RectangleMargins? rectangleMargins)
        {
            writer.WritePropertyName(propertyName);

            if (rectangleMargins is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.Write(rectangleMargins);
            }
        }

        public static void Write(this Utf8JsonWriter writer, MapInfo mapInfo, JsonSerializerOptions options) => mapInfo.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, PlayerData playerData, MapInfoFormatVersion formatVersion) => playerData.WriteTo(writer, formatVersion);

        public static void Write(this Utf8JsonWriter writer, ForceData forceData, MapInfoFormatVersion formatVersion) => forceData.WriteTo(writer, formatVersion);

        public static void Write(this Utf8JsonWriter writer, UpgradeData upgradeData, MapInfoFormatVersion formatVersion) => upgradeData.WriteTo(writer, formatVersion);

        public static void Write(this Utf8JsonWriter writer, TechData techData, MapInfoFormatVersion formatVersion) => techData.WriteTo(writer, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomUnitTable randomUnitTable, MapInfoFormatVersion formatVersion) => randomUnitTable.WriteTo(writer, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomItemTable randomItemTable, MapInfoFormatVersion formatVersion) => randomItemTable.WriteTo(writer, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomUnitSet randomUnitSet, MapInfoFormatVersion formatVersion) => randomUnitSet.WriteTo(writer, formatVersion);
    }
}