// ------------------------------------------------------------------------------
// <copyright file="ParameterListReferenceTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this ParameterListReferenceSyntax parameterListReferenceNode, ref StringBuilder sb)
        {
            _ = parameterListReferenceNode ?? throw new ArgumentNullException(nameof(parameterListReferenceNode));

            var firstParam = true;
            foreach (var parameterNode in parameterListReferenceNode)
            {
                if (firstParam)
                {
                    firstParam = false;
                }
                else
                {
                    sb.Append(", ");
                }

                parameterNode.Transpile(ref sb);
            }
        }

        public static IEnumerable<LuaIdentifierNameSyntax> TranspileToLua(this ParameterListReferenceSyntax parameterListReferenceNode)
        {
            _ = parameterListReferenceNode ?? throw new ArgumentNullException(nameof(parameterListReferenceNode));

            return parameterListReferenceNode.Select(parameterNode => parameterNode.TranspileToLua());
        }
    }
}