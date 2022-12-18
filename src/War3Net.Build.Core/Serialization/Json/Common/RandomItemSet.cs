// ------------------------------------------------------------------------------
// <copyright file="RandomItemSet.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Widget;

namespace War3Net.Build.Common
{
    public sealed partial class RandomItemSet
    {
        internal void ReadFrom(Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            throw new NotImplementedException();
        }

        internal void ReadFrom(Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            throw new NotImplementedException();
        }

        internal void WriteTo(Utf8JsonWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.WriteStartArray(nameof(Items));
            foreach (var item in Items)
            {
                writer.Write(item, formatVersion);
            }

            writer.WriteEndArray();
        }

        internal void WriteTo(Utf8JsonWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.WriteStartArray(nameof(Items));
            foreach (var item in Items)
            {
                writer.Write(item, formatVersion, subVersion, useNewFormat);
            }

            writer.WriteEndArray();
        }
    }
}