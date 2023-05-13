// ------------------------------------------------------------------------------
// <copyright file="Regions.cs" company="Drake53">
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
        public virtual IEnumerable<MemberDeclarationSyntax> RegionsApi(Map map, JassToCSharpTranspiler transpiler)
        {
            if (transpiler is null)
            {
                throw new ArgumentNullException(nameof(transpiler));
            }

            return Regions(map).Select(region => transpiler.Transpile(region));
        }

        protected internal virtual IEnumerable<JassGlobalDeclarationSyntax> Regions(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapRegions = map.Regions;
            if (mapRegions is null)
            {
                yield break;
            }

            foreach (var region in mapRegions.Regions)
            {
                yield return SyntaxFactory.GlobalVariableDeclaration(
                    SyntaxFactory.ParseTypeName(TypeName.Rect),
                    region.GetVariableName(),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(null)));
            }
        }
    }
}