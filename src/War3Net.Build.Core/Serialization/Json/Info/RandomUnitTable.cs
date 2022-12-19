// ------------------------------------------------------------------------------
// <copyright file="RandomUnitTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

using War3Net.Build.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class RandomUnitTable
    {
        internal void ReadFrom(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            throw new NotImplementedException();
        }

        internal void WriteTo(Utf8JsonWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Index), Index);
            writer.WriteString(nameof(Name), Name);

            writer.WriteStartArray(nameof(Types));
            foreach (var type in Types)
            {
                writer.WriteNumberValue((int)type);
            }

            writer.WriteEndArray();

            writer.WriteStartArray(nameof(UnitSets));
            foreach (var unitSet in UnitSets)
            {
                writer.Write(unitSet, formatVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}