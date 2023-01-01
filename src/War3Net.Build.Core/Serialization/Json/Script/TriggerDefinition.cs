// ------------------------------------------------------------------------------
// <copyright file="TriggerDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerDefinition : TriggerItem
    {
        internal TriggerDefinition(JsonElement jsonElement, TriggerItemType triggerItemType, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
            : base(triggerItemType)
        {
            GetFrom(jsonElement, triggerData, formatVersion, subVersion);
        }

        internal TriggerDefinition(ref Utf8JsonReader reader, TriggerItemType triggerItemType, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
            : base(triggerItemType)
        {
            ReadFrom(ref reader, triggerData, formatVersion, subVersion);
        }

        internal void GetFrom(JsonElement jsonElement, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            Name = jsonElement.GetString(nameof(Name));
            Description = jsonElement.GetString(nameof(Description));
            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                IsComment = jsonElement.GetBoolean(nameof(IsComment));
            }

            if (subVersion is not null)
            {
                Id = jsonElement.GetInt32(nameof(Id));
            }

            IsEnabled = jsonElement.GetBoolean(nameof(IsEnabled));
            IsCustomTextTrigger = jsonElement.GetBoolean(nameof(IsCustomTextTrigger));
            IsInitiallyOn = jsonElement.GetBoolean(nameof(IsInitiallyOn));
            RunOnMapInit = jsonElement.GetBoolean(nameof(RunOnMapInit));
            ParentId = jsonElement.GetInt32(nameof(ParentId));

            if (!IsCustomTextTrigger)
            {
                foreach (var element in jsonElement.EnumerateArray(nameof(Functions)))
                {
                    Functions.Add(element.GetTriggerFunction(triggerData, formatVersion, subVersion, false));
                }
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, triggerData, formatVersion, subVersion);
        }

        internal override void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(Name), Name);
            writer.WriteString(nameof(Description), Description);
            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                writer.WriteBoolean(nameof(IsComment), IsComment);
            }

            if (subVersion is not null)
            {
                writer.WriteNumber(nameof(Id), Id);
            }

            writer.WriteBoolean(nameof(IsEnabled), IsEnabled);
            writer.WriteBoolean(nameof(IsCustomTextTrigger), IsCustomTextTrigger);
            writer.WriteBoolean(nameof(IsInitiallyOn), IsInitiallyOn);
            writer.WriteBoolean(nameof(RunOnMapInit), RunOnMapInit);
            writer.WriteNumber(nameof(ParentId), ParentId);

            writer.WriteStartArray(nameof(Functions));
            foreach (var function in Functions)
            {
                writer.Write(function, options, formatVersion, subVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}