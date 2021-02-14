// ------------------------------------------------------------------------------
// <copyright file="Units.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual IEnumerable<JassGlobalDeclarationSyntax> Units(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapUnits = map.Units;
            if (mapUnits is null)
            {
                yield break;
            }

            foreach (var unit in mapUnits.Units.Where(unit => CreateAllUnitsConditionSingleUnit(map, unit)))
            {
                yield return SyntaxFactory.GlobalDeclaration(
                    SyntaxFactory.ParseTypeName(nameof(unit)),
                    unit.GetVariableName());
            }
        }
    }
}