// ------------------------------------------------------------------------------
// <copyright file="MapTriggers.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Extensions;
using War3Net.Build.Serialization.Json;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    [JsonConverter(typeof(JsonMapTriggersConverter))]
    public sealed partial class MapTriggers
    {
        internal MapTriggers(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapTriggers(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<MapTriggersFormatVersion>(nameof(FormatVersion));
            if (jsonElement.TryGetProperty(nameof(SubVersion), out var subVersionElement))
            {
                SubVersion = subVersionElement.GetInt32<MapTriggersSubVersion>();
            }

            GameVersion = jsonElement.GetInt32(nameof(GameVersion));

            foreach (var element in jsonElement.EnumerateArray(nameof(Variables)))
            {
                Variables.Add(element.GetVariableDefinition(FormatVersion, SubVersion));
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(TriggerItems)))
            {
                var triggerItemType = element.GetInt32<TriggerItemType>(nameof(TriggerItem.Type));

                if (element.TryGetProperty(nameof(TriggerItem.Name), out _))
                {
                    switch (triggerItemType)
                    {
                        case TriggerItemType.RootCategory:
                        case TriggerItemType.Category:
                            TriggerItems.Add(element.GetTriggerCategoryDefinition(triggerItemType, FormatVersion, SubVersion));
                            break;

                        case TriggerItemType.Gui:
                        case TriggerItemType.Comment:
                        case TriggerItemType.Script:
                            TriggerItems.Add(element.GetTriggerDefinition(triggerItemType, FormatVersion, SubVersion));
                            break;

                        case TriggerItemType.Variable:
                            TriggerItems.Add(element.GetTriggerVariableDefinition(triggerItemType, FormatVersion, SubVersion));
                            break;

                        case TriggerItemType.UNK1:
                        case TriggerItemType.UNK7:
                            throw new NotSupportedException();

                        default:
                            throw new InvalidEnumArgumentException(nameof(triggerItemType), (int)triggerItemType, typeof(TriggerItemType));
                    }
                }
                else
                {
                    TriggerItems.Add(element.GetDeletedTriggerItem(triggerItemType, FormatVersion, SubVersion));
                }
            }

            if (SubVersion is not null)
            {
                foreach (var element in jsonElement.EnumerateObject(nameof(TriggerItemCounts)))
                {
                    TriggerItemCounts.Add(Enum.Parse<TriggerItemType>(element.Name), element.Value.GetInt32());
                }
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
            if (SubVersion is not null)
            {
                writer.WriteObject(nameof(SubVersion), SubVersion, options);
            }

            writer.WriteNumber(nameof(GameVersion), GameVersion);

            writer.WriteStartArray(nameof(Variables));
            foreach (var variable in Variables)
            {
                writer.Write(variable, options, FormatVersion, SubVersion);
            }

            writer.WriteEndArray();

            writer.WriteStartArray(nameof(TriggerItems));
            foreach (var triggerItem in TriggerItems)
            {
                if (SubVersion is null && triggerItem is not TriggerDefinition && (triggerItem is not TriggerCategoryDefinition || triggerItem.Type == TriggerItemType.RootCategory))
                {
                    continue;
                }

                writer.Write(triggerItem, options, FormatVersion, SubVersion);
            }

            writer.WriteEndArray();

            if (SubVersion is not null)
            {
                writer.WriteObject(nameof(TriggerItemCounts), TriggerItemCounts, options);
            }

            writer.WriteEndObject();
        }
    }
}