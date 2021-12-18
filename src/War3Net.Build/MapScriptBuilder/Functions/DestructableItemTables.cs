// ------------------------------------------------------------------------------
// <copyright file="DestructableItemTables.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual IEnumerable<JassFunctionDeclarationSyntax> DestructableItemTables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapDoodads = map.Doodads;
            if (mapDoodads is null)
            {
                throw new ArgumentException($"Function '{nameof(DestructableItemTables)}' cannot be generated without {nameof(MapDoodads)}.", nameof(map));
            }

            for (var i = 0; i < mapDoodads.Doodads.Count; i++)
            {
                var doodad = mapDoodads.Doodads[i];
                if (DestructableItemTablesConditionSingleDoodad(map, doodad))
                {
                    yield return ItemTableDropItems(map, doodad, i);
                }
            }
        }

        protected internal virtual bool DestructableItemTablesCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Doodads is not null
                && map.Doodads.Doodads.Any(doodad => DestructableItemTablesConditionSingleDoodad(map, doodad));
        }

        protected internal virtual bool DestructableItemTablesConditionSingleDoodad(Map map, DoodadData doodadData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (doodadData is null)
            {
                throw new ArgumentNullException(nameof(doodadData));
            }

            return doodadData.ItemTableSets.Any();
        }
    }
}