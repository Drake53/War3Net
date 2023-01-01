// ------------------------------------------------------------------------------
// <copyright file="DeletedTriggerItem.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class DeletedTriggerItem : TriggerItem
    {
        internal DeletedTriggerItem(JsonElement jsonElement, TriggerItemType triggerItemType, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
            : base(triggerItemType)
        {
            GetFrom(jsonElement, triggerData, formatVersion, subVersion);
        }

        internal DeletedTriggerItem(ref Utf8JsonReader reader, TriggerItemType triggerItemType, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
            : base(triggerItemType)
        {
            ReadFrom(ref reader, triggerData, formatVersion, subVersion);
        }

        internal void GetFrom(JsonElement jsonElement, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            Id = jsonElement.GetInt32(nameof(Id));

            Name = "<DELETED>";
            ParentId = -1;
        }

        internal void ReadFrom(ref Utf8JsonReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, triggerData, formatVersion, subVersion);
        }

        internal override void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.WriteStartObject();

            if (subVersion is not null)
            {
                writer.WriteNumber(nameof(Id), Id);
            }

            writer.WriteEndObject();
        }
    }
}