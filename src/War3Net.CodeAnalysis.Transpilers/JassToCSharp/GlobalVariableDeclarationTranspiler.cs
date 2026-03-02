using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers.Extensions;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public MemberDeclarationSyntax Transpile(
            JassGlobalVariableDeclarationSyntax globalVariableDeclaration)
        {
            return Transpile(
                globalVariableDeclaration.GetLeadingTrivia(),
                globalVariableDeclaration,
                globalVariableDeclaration.GetTrailingTrivia());
        }

        public MemberDeclarationSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassGlobalVariableDeclarationSyntax globalVariableDeclaration)
        {
            return Transpile(
                leadingTrivia,
                globalVariableDeclaration,
                globalVariableDeclaration.GetTrailingTrivia());
        }

        public MemberDeclarationSyntax Transpile(
            JassGlobalVariableDeclarationSyntax globalVariableDeclaration,
            JassSyntaxTriviaList trailingTrivia)
        {
            return Transpile(
                globalVariableDeclaration.GetLeadingTrivia(),
                globalVariableDeclaration,
                trailingTrivia);
        }

        public MemberDeclarationSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassGlobalVariableDeclarationSyntax globalVariableDeclaration,
            JassSyntaxTriviaList trailingTrivia)
        {
            var declaration = SyntaxFactory.FieldDeclaration(
                default,
                new SyntaxTokenList(
                    TokenWithSpace(leadingTrivia, SyntaxKind.PublicKeyword),
                    TokenWithSpace(SyntaxKind.StaticKeyword)),
                Transpile(globalVariableDeclaration.Declarator, isGlobalDeclaration: true).WithoutTrivia(),
                Transpile(SyntaxKind.SemicolonToken, trailingTrivia));

            if (ApplyCSharpLuaTemplateAttribute)
            {
                var jassToLuaTranspiler = JassToLuaTranspiler ?? new JassToLuaTranspiler();

                declaration = declaration.WithCSharpLuaTemplateAttribute(
                    jassToLuaTranspiler.Transpile(globalVariableDeclaration.Declarator.GetIdentifierName().Token));
            }

            return declaration;
        }
    }
}