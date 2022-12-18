// ------------------------------------------------------------------------------
// <copyright file="RandomItemSet.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Widget;

namespace War3Net.Build.Common
{
    public sealed partial class RandomItemSet
    {
        internal RandomItemSet(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal RandomItemSet(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(reader, formatVersion, subVersion, useNewFormat);
        }

        internal void ReadFrom(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            nint itemCount = reader.ReadInt32();
            for (nint i = 0; i < itemCount; i++)
            {
                Items.Add(reader.ReadRandomItemSetItem(formatVersion));
            }
        }

        internal void ReadFrom(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            nint itemCount = reader.ReadInt32();
            for (nint i = 0; i < itemCount; i++)
            {
                Items.Add(reader.ReadRandomItemSetItem(formatVersion, subVersion, useNewFormat));
            }
        }

        internal void WriteTo(BinaryWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.Write(Items.Count);
            foreach (var item in Items)
            {
                writer.Write(item, formatVersion);
            }
        }

        internal void WriteTo(BinaryWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.Write(Items.Count);
            foreach (var item in Items)
            {
                writer.Write(item, formatVersion, subVersion, useNewFormat);
            }
        }
    }
}