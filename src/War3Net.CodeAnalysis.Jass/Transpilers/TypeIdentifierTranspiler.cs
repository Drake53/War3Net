// ------------------------------------------------------------------------------
// <copyright file="TypeIdentifierTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Text;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static TypeSyntax Transpile(this Syntax.TypeIdentifierSyntax typeIdentifierNode)
        {
            _ = typeIdentifierNode ?? throw new ArgumentNullException(nameof(typeIdentifierNode));

            return typeIdentifierNode.NothingKeywordToken is null
                ? typeIdentifierNode.TypeNameNode.Transpile()
                : SyntaxFactory.PredefinedType(SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.VoidKeyword));
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.TypeIdentifierSyntax typeIdentifierNode, ref StringBuilder sb)
        {
            // _ = typeIdentifierNode ?? throw new ArgumentNullException(nameof(typeIdentifierNode));

            throw new NotSupportedException();
        }
    }
}