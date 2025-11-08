// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationTranspiler.cs" company="Drake53">
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
        public MemberDeclarationSyntax Transpile(JassGlobalDeclarationSyntax globalDeclaration)
        {
            return globalDeclaration switch
            {
                JassGlobalConstantDeclarationSyntax globalConstantDeclaration => Transpile(globalConstantDeclaration),
                JassGlobalVariableDeclarationSyntax globalVariableDeclaration => Transpile(globalVariableDeclaration),
            };
        }

        public MemberDeclarationSyntax Transpile(JassGlobalConstantDeclarationSyntax globalConstantDeclaration)
        {
            var variableDeclaration = SyntaxFactory.VariableDeclaration(
                Transpile(globalConstantDeclaration.Type),
                SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(
                    Transpile(globalConstantDeclaration.IdentifierName.Token),
                    null,
                    Transpile(globalConstantDeclaration.Value))));

            var declaration = SyntaxFactory.FieldDeclaration(
                default,
                new SyntaxTokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.ConstKeyword)),
                variableDeclaration);

            if (ApplyCSharpLuaTemplateAttribute)
            {
                var jassToLuaTranspiler = JassToLuaTranspiler ?? new JassToLuaTranspiler();

                declaration = declaration.WithCSharpLuaTemplateAttribute(jassToLuaTranspiler.Transpile(globalConstantDeclaration.IdentifierName.Token));
            }

            return declaration;
        }

        public MemberDeclarationSyntax Transpile(JassGlobalVariableDeclarationSyntax globalVariableDeclaration)
        {
            var declaration = SyntaxFactory.FieldDeclaration(
                default,
                new SyntaxTokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.StaticKeyword)),
                Transpile(globalVariableDeclaration.Declarator));

            if (ApplyCSharpLuaTemplateAttribute)
            {
                var jassToLuaTranspiler = JassToLuaTranspiler ?? new JassToLuaTranspiler();
                var token = globalVariableDeclaration.Declarator switch
                {
                    JassArrayDeclaratorSyntax arrayDeclarator => arrayDeclarator.IdentifierName.Token,
                    JassVariableDeclaratorSyntax variableDeclarator => variableDeclarator.IdentifierName.Token,
                };

                declaration = declaration.WithCSharpLuaTemplateAttribute(jassToLuaTranspiler.Transpile(token));
            }

            return declaration;
        }
    }
}