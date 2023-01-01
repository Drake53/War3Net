// ------------------------------------------------------------------------------
// <copyright file="LevelObjectModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed partial class LevelObjectModification
    {
        internal LevelObjectModification(JsonElement jsonElement, ObjectDataFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal LevelObjectModification(ref Utf8JsonReader reader, ObjectDataFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, ObjectDataFormatVersion formatVersion)
        {
            OldId = jsonElement.GetInt32(nameof(OldId));
            NewId = jsonElement.GetInt32(nameof(NewId));

            if (formatVersion >= ObjectDataFormatVersion.v3)
            {
                foreach (var element in jsonElement.EnumerateArray(nameof(Unk)))
                {
                    Unk.Add(element.GetInt32());
                }
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(Modifications)))
            {
                Modifications.Add(element.GetLevelObjectDataModification(formatVersion));
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader, ObjectDataFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, ObjectDataFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(OldId), OldId);
            writer.WriteNumber(nameof(NewId), NewId);

            if (formatVersion >= ObjectDataFormatVersion.v3)
            {
                writer.WriteObject(nameof(Unk), Unk, options);
            }

            writer.WriteStartArray(nameof(Modifications));
            foreach (var modification in Modifications)
            {
                writer.Write(modification, options, formatVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}