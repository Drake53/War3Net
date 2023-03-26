// ------------------------------------------------------------------------------
// <copyright file="Destructables.cs" company="Drake53">
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
        public virtual IEnumerable<MemberDeclarationSyntax> DestructablesApi(Map map, JassToCSharpTranspiler transpiler)
        {
            if (transpiler is null)
            {
                throw new ArgumentNullException(nameof(transpiler));
            }

            return Destructables(map).Select(destructable => transpiler.Transpile(destructable));
        }

        protected internal virtual IEnumerable<JassGlobalDeclarationSyntax> Destructables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapDoodads = map.Doodads;
            if (mapDoodads is null)
            {
                yield break;
            }

            foreach (var destructable in mapDoodads.Doodads.Where(destructable => CreateAllDestructablesConditionSingleDoodad(map, destructable)))
            {
                var destructableVariableName = destructable.GetVariableName();
                if (ForceGenerateGlobalDestructableVariable || TriggerVariableReferences.ContainsKey(destructableVariableName))
                {
                    yield return SyntaxFactory.GlobalVariableDeclaration(
                        SyntaxFactory.ParseTypeName(TypeName.Destructable),
                        destructableVariableName,
                        SyntaxFactory.LiteralExpression(null));
                }
            }
        }
    }
}