// ------------------------------------------------------------------------------
// <copyright file="BuffObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public abstract partial class BuffObjectData
    {
        internal BuffObjectData(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal BuffObjectData(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<ObjectDataFormatVersion>(nameof(FormatVersion));

            foreach (var element in jsonElement.EnumerateArray(nameof(BaseBuffs)))
            {
                BaseBuffs.Add(element.GetSimpleObjectModification(FormatVersion));
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(NewBuffs)))
            {
                NewBuffs.Add(element.GetSimpleObjectModification(FormatVersion));
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

            writer.WriteStartArray(nameof(BaseBuffs));
            foreach (var buff in BaseBuffs)
            {
                writer.Write(buff, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteStartArray(nameof(NewBuffs));
            foreach (var buff in NewBuffs)
            {
                writer.Write(buff, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}