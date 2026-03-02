using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement)
        {
            var leadingTrivia = MergeTrivia(
                localVariableDeclarationStatement.LocalToken,
                localVariableDeclarationStatement.Declarator.GetLeadingTrivia());

            var declaration = Transpile(
                leadingTrivia,
                localVariableDeclarationStatement.Declarator,
                isGlobalDeclaration: false);

            var trailingTrivia = declaration.GetTrailingTrivia();

            return SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.TokenList(),
                declaration.WithoutTrailingTrivia(),
                SyntaxFactory.Token(
                    SyntaxTriviaList.Empty,
                    SyntaxKind.SemicolonToken,
                    trailingTrivia));
        }
    }
}