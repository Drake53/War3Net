using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers.Extensions;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        private const string ActionTypeName = "System.Action";

        public TypeSyntax Transpile(
            JassPredefinedTypeSyntax type)
        {
            return type.Token.SyntaxKind == JassSyntaxKind.CodeKeyword
                ? SyntaxFactory.IdentifierName(Transpile(ActionTypeName, type.Token))
                : SyntaxFactory.PredefinedType(Transpile(TranspileTypeKeyword(type.Token.SyntaxKind), type.Token));
        }

        public TypeSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassPredefinedTypeSyntax type)
        {
            return type.Token.SyntaxKind == JassSyntaxKind.CodeKeyword
                ? SyntaxFactory.IdentifierName(Transpile(leadingTrivia, ActionTypeName, type.Token.TrailingTrivia))
                : SyntaxFactory.PredefinedType(Transpile(leadingTrivia, TranspileTypeKeyword(type.Token.SyntaxKind), type.Token.TrailingTrivia));
        }

        public TypeSyntax Transpile(
            JassPredefinedTypeSyntax type,
            JassSyntaxTriviaList trailingTrivia)
        {
            return type.Token.SyntaxKind == JassSyntaxKind.CodeKeyword
                ? SyntaxFactory.IdentifierName(Transpile(type.Token.LeadingTrivia, ActionTypeName, trailingTrivia))
                : SyntaxFactory.PredefinedType(Transpile(type.Token.LeadingTrivia, TranspileTypeKeyword(type.Token.SyntaxKind), trailingTrivia));
        }

        public TypeSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassPredefinedTypeSyntax type,
            JassSyntaxTriviaList trailingTrivia)
        {
            return type.Token.SyntaxKind == JassSyntaxKind.CodeKeyword
                ? SyntaxFactory.IdentifierName(Transpile(leadingTrivia, ActionTypeName, trailingTrivia))
                : SyntaxFactory.PredefinedType(Transpile(leadingTrivia, TranspileTypeKeyword(type.Token.SyntaxKind), trailingTrivia));
        }

        public TypeSyntax TranspileAligned(
            JassPredefinedTypeSyntax type,
            bool isArray)
        {
            return Transpile(type)
                .WithAlignedWhitespace(GetWhitespaceDiff(type.Token.SyntaxKind, isArray));
        }

        public TypeSyntax TranspileAligned(
            JassSyntaxTriviaList leadingTrivia,
            JassPredefinedTypeSyntax type,
            bool isArray)
        {
            return Transpile(leadingTrivia, type)
                .WithAlignedWhitespace(GetWhitespaceDiff(type.Token.SyntaxKind, isArray));
        }

        public TypeSyntax TranspileAligned(
            JassPredefinedTypeSyntax type,
            JassSyntaxTriviaList trailingTrivia,
            bool isArray)
        {
            return Transpile(type, trailingTrivia)
                .WithAlignedWhitespace(GetWhitespaceDiff(type.Token.SyntaxKind, isArray));
        }

        public TypeSyntax TranspileAligned(
            JassSyntaxTriviaList leadingTrivia,
            JassPredefinedTypeSyntax type,
            JassSyntaxTriviaList trailingTrivia,
            bool isArray)
        {
            return Transpile(leadingTrivia, type, trailingTrivia)
                .WithAlignedWhitespace(GetWhitespaceDiff(type.Token.SyntaxKind, isArray));
        }

        private int GetWhitespaceDiff(JassSyntaxKind keyword, bool isArray)
        {
            return (isArray ? ArrayWhitespaceDiff : 0) + keyword switch
            {
                JassSyntaxKind.BooleanKeyword => 3,
                JassSyntaxKind.CodeKeyword => -9,
                JassSyntaxKind.HandleKeyword => 0,
                JassSyntaxKind.IntegerKeyword => 4,
                JassSyntaxKind.NothingKeyword => 3,
                JassSyntaxKind.RealKeyword => -1,
                JassSyntaxKind.StringKeyword => 0,
            };
        }
    }
}