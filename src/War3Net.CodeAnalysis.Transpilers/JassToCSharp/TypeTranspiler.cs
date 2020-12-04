// ------------------------------------------------------------------------------
// <copyright file="TypeTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static TypeSyntax Transpile(this Jass.Syntax.TypeSyntax typeNode, TokenTranspileFlags flags = (TokenTranspileFlags)0)
        {
            _ = typeNode ?? throw new ArgumentNullException(nameof(typeNode));

            return typeNode.TypeNameToken.TranspileType(flags);
        }
    }
}