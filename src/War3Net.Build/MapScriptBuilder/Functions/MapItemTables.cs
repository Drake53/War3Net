// ------------------------------------------------------------------------------
// <copyright file="MapItemTables.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Info;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual IEnumerable<JassFunctionDeclarationSyntax> MapItemTables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var randomItemTables = map.Info.RandomItemTables;
            if (randomItemTables is null)
            {
                throw new ArgumentException($"'ItemTable_DropItems' functions cannot be generated without {nameof(MapInfo.RandomItemTables)}.");
            }

            return randomItemTables.Select(table => ItemTableDropItems(map, table));
        }

        protected internal virtual bool MapItemTablesCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info.RandomItemTables is not null && map.Info.RandomItemTables.Any();
        }
    }
}