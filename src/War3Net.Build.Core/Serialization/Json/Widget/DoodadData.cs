// ------------------------------------------------------------------------------
// <copyright file="DoodadData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class DoodadData : WidgetData
    {
        internal DoodadData(JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            GetFrom(jsonElement, formatVersion, subVersion, useNewFormat);
        }

        internal DoodadData(ref Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(ref reader, formatVersion, subVersion, useNewFormat);
        }

        internal void GetFrom(JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            TypeId = jsonElement.GetInt32(nameof(TypeId));
            Variation = jsonElement.GetInt32(nameof(Variation));
            Position = jsonElement.GetVector3(nameof(Position));
            Rotation = jsonElement.GetSingle(nameof(Rotation));
            Scale = jsonElement.GetVector3(nameof(Scale));
            SkinId = useNewFormat ? jsonElement.GetInt32(nameof(SkinId)) : TypeId;

            if (formatVersion > MapWidgetsFormatVersion.v6)
            {
                State = jsonElement.GetByte<DoodadState>(nameof(State));
            }

            Life = jsonElement.GetByte(nameof(Life));

            if (formatVersion == MapWidgetsFormatVersion.v8)
            {
                MapItemTableId = jsonElement.GetInt32(nameof(MapItemTableId));

                foreach (var element in jsonElement.EnumerateArray(nameof(ItemTableSets)))
                {
                    ItemTableSets.Add(element.GetRandomItemSet(formatVersion, subVersion, useNewFormat));
                }
            }

            CreationNumber = jsonElement.GetInt32(nameof(CreationNumber));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion, subVersion, useNewFormat);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(TypeId), TypeId);
            writer.WriteNumber(nameof(Variation), Variation);
            writer.Write(nameof(Position), Position);
            writer.WriteNumber(nameof(Rotation), Rotation);
            writer.Write(nameof(Scale), Scale);

            if (useNewFormat)
            {
                writer.WriteNumber(nameof(SkinId), SkinId);
            }

            if (formatVersion > MapWidgetsFormatVersion.v6)
            {
                writer.WriteObject(nameof(State), State, options);
            }

            writer.WriteNumber(nameof(Life), Life);

            if (formatVersion == MapWidgetsFormatVersion.v8)
            {
                writer.WriteNumber(nameof(MapItemTableId), MapItemTableId);

                writer.WriteStartArray(nameof(ItemTableSets));
                foreach (var itemSet in ItemTableSets)
                {
                    writer.Write(itemSet, options, formatVersion, subVersion, useNewFormat);
                }

                writer.WriteEndArray();
            }

            writer.WriteNumber(nameof(CreationNumber), CreationNumber);

            writer.WriteEndObject();
        }
    }
}