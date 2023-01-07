// ------------------------------------------------------------------------------
// <copyright file="TriggerFunction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerFunction
    {
        internal TriggerFunction(JsonElement jsonElement, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion, bool isChildFunction)
        {
            GetFrom(jsonElement, formatVersion, subVersion, isChildFunction);
        }

        internal TriggerFunction(ref Utf8JsonReader reader, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion, bool isChildFunction)
        {
            ReadFrom(ref reader, formatVersion, subVersion, isChildFunction);
        }

        internal void GetFrom(JsonElement jsonElement, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion, bool isChildFunction)
        {
            Type = jsonElement.GetInt32<TriggerFunctionType>(nameof(Type));
            if (isChildFunction)
            {
                Branch = jsonElement.GetInt32(nameof(Branch));
            }

            Name = jsonElement.GetString(nameof(Name));
            IsEnabled = jsonElement.GetBoolean(nameof(IsEnabled));

            foreach (var element in jsonElement.EnumerateArray(nameof(Parameters)))
            {
                Parameters.Add(element.GetTriggerFunctionParameter(formatVersion, subVersion));
            }

            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                foreach (var element in jsonElement.EnumerateArray(nameof(ChildFunctions)))
                {
                    ChildFunctions.Add(element.GetTriggerFunction(formatVersion, subVersion, true));
                }
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion, bool isChildFunction)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion, subVersion, isChildFunction);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.WriteStartObject();

            writer.WriteObject(nameof(Type), Type, options);
            if (Branch.HasValue)
            {
                writer.WriteNumber(nameof(Branch), Branch.Value);
            }

            writer.WriteString(nameof(Name), Name);
            writer.WriteBoolean(nameof(IsEnabled), IsEnabled);

            writer.WriteStartArray(nameof(Parameters));
            foreach (var parameter in Parameters)
            {
                writer.Write(parameter, options, formatVersion, subVersion);
            }

            writer.WriteEndArray();

            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                writer.WriteStartArray(nameof(ChildFunctions));
                foreach (var childFunction in ChildFunctions)
                {
                    writer.Write(childFunction, options,  formatVersion, subVersion);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }
    }
}