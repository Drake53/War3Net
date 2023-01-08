// ------------------------------------------------------------------------------
// <copyright file="RandomUnitSet.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class RandomUnitSet
    {
        internal RandomUnitSet(JsonElement jsonElement, MapInfoFormatVersion formatVersion, int setSize)
        {
            GetFrom(jsonElement, formatVersion);

            if (UnitIds.Length != setSize)
            {
                throw new ArgumentException("Size does not match JSON array length.", nameof(setSize));
            }
        }

        internal RandomUnitSet(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion, int setSize)
        {
            ReadFrom(ref reader, formatVersion);

            if (UnitIds.Length != setSize)
            {
                throw new ArgumentException("Size does not match JSON array length.", nameof(setSize));
            }
        }

        internal void GetFrom(JsonElement jsonElement, MapInfoFormatVersion formatVersion)
        {
            Chance = jsonElement.GetInt32(nameof(Chance));

            var arrayElement = jsonElement.GetProperty(nameof(UnitIds));
            UnitIds = new int[arrayElement.GetArrayLength()];

            var i = 0;
            foreach (var element in arrayElement.EnumerateArray())
            {
                UnitIds[i++] = element.GetInt32();
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapInfoFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Chance), Chance);

            writer.WriteObject(nameof(UnitIds), UnitIds, options);

            writer.WriteEndObject();
        }
    }
}