// ------------------------------------------------------------------------------
// <copyright file="VariationObjectDataModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed partial class VariationObjectDataModification : ObjectDataModification
    {
        internal VariationObjectDataModification(JsonElement jsonElement, ObjectDataFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal VariationObjectDataModification(ref Utf8JsonReader reader, ObjectDataFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, ObjectDataFormatVersion formatVersion)
        {
            Id = jsonElement.GetInt32(nameof(Id));
            Type = jsonElement.GetInt32<ObjectDataType>(nameof(Type));
            Variation = jsonElement.GetInt32(nameof(Variation));
            Pointer = jsonElement.GetInt32(nameof(Pointer));
            Value = GetValue(jsonElement, nameof(Value), formatVersion);
            SanityCheck = jsonElement.GetInt32(nameof(SanityCheck));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, ObjectDataFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, ObjectDataFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Id), Id);
            writer.WriteObject(nameof(Type), Type, options);
            writer.WriteNumber(nameof(Variation), Variation);
            writer.WriteNumber(nameof(Pointer), Pointer);
            WriteValue(writer, nameof(Value), formatVersion);
            writer.WriteNumber(nameof(SanityCheck), SanityCheck);

            writer.WriteEndObject();
        }
    }
}