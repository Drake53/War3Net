﻿// ------------------------------------------------------------------------------
// <copyright file="RandomItemSetItem.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Info;
using War3Net.Build.Widget;

namespace War3Net.Build.Common
{
    public sealed partial class RandomItemSetItem
    {
        internal void ReadFrom(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            Chance = reader.ReadInt32();
            ItemId = reader.ReadInt32();
        }

        internal void ReadFrom(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ItemId = reader.ReadInt32();
            Chance = reader.ReadInt32();
        }

        internal void WriteTo(BinaryWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.Write(Chance);
            writer.Write(ItemId);
        }

        internal void WriteTo(BinaryWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.Write(ItemId);
            writer.Write(Chance);
        }
    }
}