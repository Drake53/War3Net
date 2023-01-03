// ------------------------------------------------------------------------------
// <copyright file="MapTriggers.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Serialization.Json;

namespace War3Net.Build.Script
{
    [JsonConverter(typeof(JsonMapTriggersConverter))]
    public sealed partial class MapTriggers
    {
        internal MapTriggers(JsonElement jsonElement, TriggerData triggerData)
        {
            GetFrom(jsonElement, triggerData);
        }

        internal MapTriggers(ref Utf8JsonReader reader, TriggerData triggerData)
        {
            ReadFrom(ref reader, triggerData);
        }

        internal void GetFrom(JsonElement jsonElement, TriggerData triggerData)
        {
            throw new NotImplementedException();
        }

        internal void ReadFrom(ref Utf8JsonReader reader, TriggerData triggerData)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, triggerData);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}