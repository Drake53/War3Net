// ------------------------------------------------------------------------------
// <copyright file="RandomUnitSet.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

namespace War3Net.Build.Info
{
    public sealed partial class RandomUnitSet
    {
        internal void ReadFrom(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            throw new NotImplementedException();
        }

        internal void WriteTo(Utf8JsonWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Chance), Chance);

            writer.WriteStartArray(nameof(UnitIds));
            for (nint i = 0; i < UnitIds.Length; i++)
            {
                writer.WriteNumberValue(UnitIds[i]);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}