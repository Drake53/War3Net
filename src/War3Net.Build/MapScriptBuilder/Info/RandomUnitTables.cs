// ------------------------------------------------------------------------------
// <copyright file="RandomUnitTables.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.Build.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        public virtual IEnumerable<MemberDeclarationSyntax> RandomUnitTablesApi(Map map, JassToCSharpTranspiler transpiler)
        {
            if (transpiler is null)
            {
                throw new ArgumentNullException(nameof(transpiler));
            }

            return RandomUnitTables(map).Select(randomUnitTable => transpiler.Transpile(randomUnitTable));
        }

        protected internal virtual IEnumerable<JassGlobalDeclarationSyntax> RandomUnitTables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var randomUnitTables = map.Info?.RandomUnitTables;
            if (randomUnitTables is null)
            {
                yield break;
            }

            var id = 0;
            foreach (var randomUnitTable in randomUnitTables)
            {
                yield return SyntaxFactory.GlobalArrayDeclaration(
                    JassPredefinedTypeSyntax.Integer,
                    randomUnitTable.GetVariableName(id));

                id++;
            }
        }
    }
}