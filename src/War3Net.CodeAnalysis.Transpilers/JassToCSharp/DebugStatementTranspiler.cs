using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassDebugStatementSyntax debugStatement)
        {
            var leadingTrivia = Transpile(debugStatement.DebugToken.LeadingTrivia);
            var statement = Transpile(debugStatement.Statement);

            var ifDebugDirective = SyntaxFactory.Trivia(
                SyntaxFactory.IfDirectiveTrivia(
                    SyntaxFactory.Token(SyntaxKind.HashToken),
                    Transpile(SyntaxKind.IfKeyword, debugStatement.DebugToken.TrailingTrivia),
                    SyntaxFactory.IdentifierName("DEBUG"),
                    SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken),
                    isActive: true,
                    branchTaken: true,
                    conditionValue: true));

            if (leadingTrivia.Count == 0 || leadingTrivia[^1].IsKind(SyntaxKind.EndOfLineTrivia))
            {
                leadingTrivia = leadingTrivia.Add(ifDebugDirective);
                leadingTrivia = leadingTrivia.Add(SyntaxFactory.CarriageReturnLineFeed);
            }
            else
            {
                leadingTrivia = leadingTrivia.Insert(leadingTrivia.Count - 1, ifDebugDirective);
                leadingTrivia = leadingTrivia.Insert(leadingTrivia.Count - 1, SyntaxFactory.CarriageReturnLineFeed);
            }

            leadingTrivia = leadingTrivia.AddRange(statement.GetLeadingTrivia());

            var trailingTrivia = statement
                .GetTrailingTrivia()
                .Add(SyntaxFactory.Trivia(SyntaxFactory.EndIfDirectiveTrivia(isActive: true)))
                .Add(SyntaxFactory.CarriageReturnLineFeed);

            return statement
                .WithLeadingTrivia(leadingTrivia)
                .WithTrailingTrivia(trailingTrivia);
        }
    }
}