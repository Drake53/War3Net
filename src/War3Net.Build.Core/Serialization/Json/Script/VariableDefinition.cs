// ------------------------------------------------------------------------------
// <copyright file="VariableDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class VariableDefinition
    {
        internal VariableDefinition(JsonElement jsonElement, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            GetFrom(jsonElement, triggerData, formatVersion, subVersion);
        }

        internal VariableDefinition(ref Utf8JsonReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            ReadFrom(ref reader, triggerData, formatVersion, subVersion);
        }

        internal void GetFrom(JsonElement jsonElement, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            Name = jsonElement.GetString(nameof(Name));
            Type = jsonElement.GetString(nameof(Type));
            Unk = jsonElement.GetInt32(nameof(Unk));
            IsArray = jsonElement.GetBoolean(nameof(IsArray));
            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                ArraySize = jsonElement.GetInt32(nameof(ArraySize));
            }

            IsInitialized = jsonElement.GetBoolean(nameof(IsInitialized));
            InitialValue = jsonElement.GetString(nameof(InitialValue));

            if (subVersion is not null)
            {
                Id = jsonElement.GetInt32(nameof(Id));
                ParentId = jsonElement.GetInt32(nameof(ParentId));
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, triggerData, formatVersion, subVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(Name), Name);
            writer.WriteString(nameof(Type), Type);
            writer.WriteNumber(nameof(Unk), Unk);
            writer.WriteBoolean(nameof(IsArray), IsArray);
            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                writer.WriteNumber(nameof(ArraySize), ArraySize);
            }

            writer.WriteBoolean(nameof(IsInitialized), IsInitialized);
            writer.WriteString(nameof(InitialValue), InitialValue);

            if (subVersion is not null)
            {
                writer.WriteNumber(nameof(Id), Id);
                writer.WriteNumber(nameof(ParentId), ParentId);
            }

            writer.WriteEndObject();
        }
    }
}