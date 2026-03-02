using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public TypeSyntax Transpile(
            JassTypeSyntax type)
        {
            return type switch
            {
                JassIdentifierNameSyntax identifierName => Transpile(identifierName),
                JassPredefinedTypeSyntax predefinedType => Transpile(predefinedType),
            };
        }

        public TypeSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassTypeSyntax type)
        {
            return type switch
            {
                JassIdentifierNameSyntax identifierName => Transpile(leadingTrivia, identifierName),
                JassPredefinedTypeSyntax predefinedType => Transpile(leadingTrivia, predefinedType),
            };
        }

        public TypeSyntax Transpile(
            JassTypeSyntax type,
            JassSyntaxTriviaList trailingTrivia)
        {
            return type switch
            {
                JassIdentifierNameSyntax identifierName => Transpile(identifierName, trailingTrivia),
                JassPredefinedTypeSyntax predefinedType => Transpile(predefinedType, trailingTrivia),
            };
        }

        public TypeSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassTypeSyntax type,
            JassSyntaxTriviaList trailingTrivia)
        {
            return type switch
            {
                JassIdentifierNameSyntax identifierName => Transpile(leadingTrivia, identifierName, trailingTrivia),
                JassPredefinedTypeSyntax predefinedType => Transpile(leadingTrivia, predefinedType, trailingTrivia),
            };
        }

        public TypeSyntax TranspileAligned(
            JassTypeSyntax type,
            bool isArray)
        {
            return type switch
            {
                JassIdentifierNameSyntax identifierName => TranspileAligned(identifierName, isArray),
                JassPredefinedTypeSyntax predefinedType => TranspileAligned(predefinedType, isArray),
            };
        }

        public TypeSyntax TranspileAligned(
            JassSyntaxTriviaList leadingTrivia,
            JassTypeSyntax type,
            bool isArray)
        {
            return type switch
            {
                JassIdentifierNameSyntax identifierName => TranspileAligned(leadingTrivia, identifierName, isArray),
                JassPredefinedTypeSyntax predefinedType => TranspileAligned(leadingTrivia, predefinedType, isArray),
            };
        }

        public TypeSyntax TranspileAligned(
            JassTypeSyntax type,
            JassSyntaxTriviaList trailingTrivia,
            bool isArray)
        {
            return type switch
            {
                JassIdentifierNameSyntax identifierName => TranspileAligned(identifierName, trailingTrivia, isArray),
                JassPredefinedTypeSyntax predefinedType => TranspileAligned(predefinedType, trailingTrivia, isArray),
            };
        }

        public TypeSyntax TranspileAligned(
            JassSyntaxTriviaList leadingTrivia,
            JassTypeSyntax type,
            JassSyntaxTriviaList trailingTrivia,
            bool isArray)
        {
            return type switch
            {
                JassIdentifierNameSyntax identifierName => TranspileAligned(leadingTrivia, identifierName, trailingTrivia, isArray),
                JassPredefinedTypeSyntax predefinedType => TranspileAligned(leadingTrivia, predefinedType, trailingTrivia, isArray),
            };
        }
    }
}