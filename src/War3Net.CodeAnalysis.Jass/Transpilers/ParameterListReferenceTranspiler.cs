// ------------------------------------------------------------------------------
// <copyright file="ParameterListReferenceTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static IEnumerable<ParameterSyntax> Transpile(this Syntax.ParameterListReferenceSyntax parameterListReferenceNode, params TokenTranspileFlags[] flags)
        {
            _ = parameterListReferenceNode ?? throw new ArgumentNullException(nameof(parameterListReferenceNode));

            return parameterListReferenceNode.Select((node, index)
                => node.Transpile(
                    index + 1 > flags.Length
                    ? (TokenTranspileFlags)0
                    : flags[index]));
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.ParameterListReferenceSyntax parameterListReferenceNode, ref StringBuilder sb)
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
    }
}