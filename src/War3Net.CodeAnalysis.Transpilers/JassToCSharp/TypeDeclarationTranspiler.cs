// ------------------------------------------------------------------------------
// <copyright file="TypeDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public MemberDeclarationSyntax Transpile(JassTypeDeclarationSyntax typeDeclaration)
        {
            var identifier = Transpile(typeDeclaration.IdentifierName);
            var baseList = SyntaxFactory.BaseList(SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(SyntaxFactory.SimpleBaseType(Transpile(typeDeclaration.BaseType))));

            return SyntaxFactory.ClassDeclaration(
                default,
                new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)),
                identifier,
                null,
                baseList,
                default,
                SyntaxFactory.SingletonList(
                    (MemberDeclarationSyntax)SyntaxFactory.ConstructorDeclaration(
                        default,
                        new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.InternalKeyword)),
                        identifier,
                        SyntaxFactory.ParameterList(),
                        null,
                        SyntaxFactory.Block())));
        }
    }
}