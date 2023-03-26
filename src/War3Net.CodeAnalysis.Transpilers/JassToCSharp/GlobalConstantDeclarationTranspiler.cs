// ------------------------------------------------------------------------------
// <copyright file="GlobalConstantDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers.Extensions;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public MemberDeclarationSyntax Transpile(JassGlobalConstantDeclarationSyntax globalDeclaration)
        {
            var type = Transpile(globalDeclaration.Type);
            var constantDeclaration = SyntaxFactory.VariableDeclaration(
                type,
                SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(
                    Transpile(globalDeclaration.IdentifierName),
                    null,
                    Transpile(globalDeclaration.Value))));

            var declaration = SyntaxFactory.FieldDeclaration(
                default,
                new SyntaxTokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.ConstKeyword)),
                constantDeclaration);

            if (ApplyCSharpLuaTemplateAttribute)
            {
                var jassToLuaTranspiler = JassToLuaTranspiler ?? new JassToLuaTranspiler();
                declaration = declaration.WithCSharpLuaTemplateAttribute(jassToLuaTranspiler.Transpile(globalDeclaration.IdentifierName));
            }

            return declaration;
        }
    }
}