// ------------------------------------------------------------------------------
// <copyright file="TypeIdentifierTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static TypeSyntax Transpile(this Jass.Syntax.TypeIdentifierSyntax typeIdentifierNode)
        {
            _ = typeIdentifierNode ?? throw new ArgumentNullException(nameof(typeIdentifierNode));

            return typeIdentifierNode.NothingKeywordToken is null
                ? typeIdentifierNode.TypeNameNode.Transpile()
                : SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword));
        }
    }
}