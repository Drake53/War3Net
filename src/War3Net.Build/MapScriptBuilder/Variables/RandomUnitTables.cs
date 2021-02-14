// ------------------------------------------------------------------------------
// <copyright file="RandomUnitTables.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.Build.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual IEnumerable<JassGlobalDeclarationSyntax> RandomUnitTables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var randomUnitTables = map.Info.RandomUnitTables;
            if (randomUnitTables is null)
            {
                yield break;
            }

            var id = 0;
            foreach (var randomUnitTable in randomUnitTables)
            {
                yield return SyntaxFactory.GlobalArrayDeclaration(
                    JassTypeSyntax.Integer,
                    randomUnitTable.GetVariableName(id));

                id++;
            }
        }
    }
}