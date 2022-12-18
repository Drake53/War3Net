// ------------------------------------------------------------------------------
// <copyright file="RandomItemTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

using War3Net.Build.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class RandomItemTable
    {
        internal void ReadFrom(Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            throw new NotImplementedException();
        }

        internal void WriteTo(Utf8JsonWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.WriteNumber(nameof(Index), Index);
            writer.WriteString(nameof(Name), Name);

            writer.WriteStartArray(nameof(ItemSets));
            foreach (var itemSet in ItemSets)
            {
                writer.Write(itemSet, formatVersion);
            }

            writer.WriteEndArray();
        }
    }
}