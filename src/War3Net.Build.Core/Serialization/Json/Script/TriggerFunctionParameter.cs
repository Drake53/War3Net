// ------------------------------------------------------------------------------
// <copyright file="TriggerFunctionParameter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerFunctionParameter
    {
        internal TriggerFunctionParameter(JsonElement jsonElement, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            GetFrom(jsonElement, triggerData, formatVersion, subVersion);
        }

        internal TriggerFunctionParameter(ref Utf8JsonReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            ReadFrom(ref reader, triggerData, formatVersion, subVersion);
        }

        internal void GetFrom(JsonElement jsonElement, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            Type = jsonElement.GetInt32<TriggerFunctionParameterType>(nameof(Type));
            Value = jsonElement.GetString(nameof(Value));

            if (jsonElement.TryGetProperty(nameof(Function), out var functionElement))
            {
                if (Type != TriggerFunctionParameterType.Function)
                {
                    throw new InvalidDataException($"Parameter must be of type '{TriggerFunctionParameterType.Function}' to have a function.");
                }

                Function = functionElement.GetTriggerFunction(triggerData, formatVersion, subVersion, false);
            }

            if (jsonElement.TryGetProperty(nameof(ArrayIndexer), out var arrayIndexerElement))
            {
                if (Type != TriggerFunctionParameterType.Variable)
                {
                    throw new InvalidDataException($"Parameter must be of type '{TriggerFunctionParameterType.Variable}' to have an array indexer.");
                }

                ArrayIndexer = arrayIndexerElement.GetTriggerFunctionParameter(triggerData, formatVersion, subVersion);
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, triggerData, formatVersion, subVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.WriteStartObject();

            writer.WriteObject(nameof(Type), Type, options);
            writer.WriteString(nameof(Value), Value);

            if (Function is not null)
            {
                writer.WritePropertyName(nameof(Function));
                Function.WriteTo(writer, options, formatVersion, subVersion);
            }

            if (ArrayIndexer is not null)
            {
                writer.WritePropertyName(nameof(ArrayIndexer));
                ArrayIndexer.WriteTo(writer, options, formatVersion, subVersion);
            }

            writer.WriteEndObject();
        }
    }
}