﻿// ------------------------------------------------------------------------------
// <copyright file="TriggerVariableDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerVariableDefinition : TriggerItem
    {
        internal TriggerVariableDefinition(JsonElement jsonElement, TriggerItemType triggerItemType, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
            : base(triggerItemType)
        {
            GetFrom(jsonElement, triggerData, formatVersion, subVersion);
        }

        internal TriggerVariableDefinition(ref Utf8JsonReader reader, TriggerItemType triggerItemType, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
            : base(triggerItemType)
        {
            ReadFrom(ref reader, triggerData, formatVersion, subVersion);
        }

        internal void GetFrom(JsonElement jsonElement, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            Id = jsonElement.GetInt32(nameof(Id));
            Name = jsonElement.GetString(nameof(Name));
            ParentId = jsonElement.GetInt32(nameof(ParentId));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, triggerData, formatVersion, subVersion);
        }

        internal override void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Id), Id);
            writer.WriteString(nameof(Name), Name);
            writer.WriteNumber(nameof(ParentId), ParentId);

            writer.WriteEndObject();
        }
    }
}