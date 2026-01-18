// ------------------------------------------------------------------------------
// <copyright file="MapItemTables.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using War3Net.Build.Info;
using War3Net.CodeAnalysis;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateMapItemTables(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var randomItemTables = map.Info?.RandomItemTables;
            if (randomItemTables is null)
            {
                throw new ArgumentException($"DropItems functions cannot be generated without {nameof(MapInfo.RandomItemTables)}.");
            }

            foreach (var table in randomItemTables)
            {
                GenerateItemTableDropItems(map, table, writer);
            }

            writer.WriteLine();
        }

        protected internal virtual bool ShouldGenerateMapItemTables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info?.RandomItemTables is not null
                && map.Info.RandomItemTables.Count > 0;
        }
    }
}