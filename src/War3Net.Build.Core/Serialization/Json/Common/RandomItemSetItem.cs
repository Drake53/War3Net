// ------------------------------------------------------------------------------
// <copyright file="RandomItemSetItem.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

using War3Net.Build.Info;
using War3Net.Build.Widget;

namespace War3Net.Build.Common
{
    public sealed partial class RandomItemSetItem
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
            writer.WriteNumber(nameof(Chance), Chance);
            writer.WriteNumber(nameof(ItemId), ItemId);
        }

        internal void WriteTo(Utf8JsonWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.WriteNumber(nameof(ItemId), ItemId);
            writer.WriteNumber(nameof(Chance), Chance);
        }
    }
}