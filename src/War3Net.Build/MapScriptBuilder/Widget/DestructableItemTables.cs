// ------------------------------------------------------------------------------
// <copyright file="DestructableItemTables.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;
using War3Net.Build.Extensions;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateDestructableItemTables(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapDoodads = map.Doodads;
            if (mapDoodads is null)
            {
                throw new ArgumentException($"DropItems functions cannot be generated without {nameof(MapDoodads)}.", nameof(map));
            }

            for (var i = 0; i < mapDoodads.Doodads.Count; i++)
            {
                var doodad = mapDoodads.Doodads[i];
                if (ShouldGenerateDestructableItemTablesForDoodad(map, doodad))
                {
                    GenerateItemTableDropItems(map, doodad, i, writer);
                }
            }

            writer.WriteLine();
        }

        protected internal virtual bool ShouldGenerateDestructableItemTables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Doodads is not null
                && map.Doodads.Doodads.Any(doodad => ShouldGenerateDestructableItemTablesForDoodad(map, doodad));
        }

        protected internal virtual bool ShouldGenerateDestructableItemTablesForDoodad(Map map, DoodadData doodadData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (doodadData is null)
            {
                throw new ArgumentNullException(nameof(doodadData));
            }

            return doodadData.HasItemTableSets();
        }
    }
}