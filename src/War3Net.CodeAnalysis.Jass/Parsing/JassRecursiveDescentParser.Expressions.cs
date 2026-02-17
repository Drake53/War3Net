// ------------------------------------------------------------------------------
// <copyright file="JassRecursiveDescentParser.Expressions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Diagnostics;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Parsing
{
    internal sealed partial class JassRecursiveDescentParser
    {
        private JassExpressionSyntax ParseExpression()
        {
            return ParseBinaryExpression(maxPrecedence: 4);
        }

        private JassExpressionSyntax ParseBinaryExpression(int maxPrecedence)
        {
            var left = ParseUnaryExpression();

            while (true)
            {
                var expressionKind = JassSyntaxFacts.GetBinaryExpressionKind(Current.SyntaxKind);
                if (expressionKind == JassSyntaxKind.None)
                {
                    break;
                }

                var precedence = JassSyntaxFacts.GetBinaryOperatorPrecedence(expressionKind);
                if (precedence > maxPrecedence)
                {
                    break;
                }

                var operatorToken = EatToken();
                var right = ParseBinaryExpression(precedence - 1);
                left = new JassBinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private JassExpressionSyntax ParseUnaryExpression()
        {
            if (JassSyntaxFacts.IsUnaryExpressionToken(Current.SyntaxKind))
            {
                var operatorToken = EatToken();
                var expression = ParseUnaryExpression();
                return new JassUnaryExpressionSyntax(operatorToken, expression);
            }

            return ParsePrimaryExpression();
        }

        private JassExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.SyntaxKind)
            {
                case JassSyntaxKind.OpenParenToken:
                    return ParseParenthesizedExpression();

                case JassSyntaxKind.FunctionKeyword:
                    return ParseFunctionReferenceExpression();

                case JassSyntaxKind.TrueKeyword:
                case JassSyntaxKind.FalseKeyword:
                case JassSyntaxKind.NullKeyword:
                case JassSyntaxKind.DecimalLiteralToken:
                case JassSyntaxKind.HexadecimalLiteralToken:
                case JassSyntaxKind.OctalLiteralToken:
                case JassSyntaxKind.RealLiteralToken:
                case JassSyntaxKind.StringLiteralToken:
                case JassSyntaxKind.CharacterLiteralToken:
                case JassSyntaxKind.FourCCLiteralToken:
                    return new JassLiteralExpressionSyntax(EatToken());

                case JassSyntaxKind.IdentifierToken:
                    return ParseIdentifierExpression();

                default:
                    _diagnostics.Report(
                        JassSyntaxDiagnostics.MissingExpression,
                        GetCurrentLocation());
                    var missingToken = CreateMissingTokenSilent(JassSyntaxKind.IdentifierToken);
                    return new JassIdentifierNameSyntax(missingToken);
            }
        }

        private JassParenthesizedExpressionSyntax ParseParenthesizedExpression()
        {
            var openParenToken = EatToken(JassSyntaxKind.OpenParenToken);
            var expression = ParseExpression();
            var closeParenToken = EatToken(JassSyntaxKind.CloseParenToken);

            return new JassParenthesizedExpressionSyntax(openParenToken, expression, closeParenToken);
        }

        private JassFunctionReferenceExpressionSyntax ParseFunctionReferenceExpression()
        {
            var functionToken = EatToken(JassSyntaxKind.FunctionKeyword);
            var identifierName = ParseIdentifierName();

            return new JassFunctionReferenceExpressionSyntax(functionToken, identifierName);
        }

        private JassExpressionSyntax ParseIdentifierExpression()
        {
            var identifierName = ParseIdentifierName();

            if (At(JassSyntaxKind.OpenParenToken))
            {
                var argumentList = ParseArgumentList();
                return new JassInvocationExpressionSyntax(identifierName, argumentList);
            }

            if (At(JassSyntaxKind.OpenBracketToken))
            {
                var elementAccessClause = ParseElementAccessClause();
                return new JassElementAccessExpressionSyntax(identifierName, elementAccessClause);
            }

            return identifierName;
        }

        private JassArgumentListSyntax ParseArgumentList()
        {
            var openParenToken = EatToken(JassSyntaxKind.OpenParenToken);

            if (At(JassSyntaxKind.CloseParenToken))
            {
                var closeParenToken = EatToken();
                return new JassArgumentListSyntax(
                    openParenToken,
                    SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.Empty,
                    closeParenToken);
            }

            var firstArgument = ParseExpression();

            if (!At(JassSyntaxKind.CommaToken))
            {
                var closeToken = EatToken(JassSyntaxKind.CloseParenToken);
                return new JassArgumentListSyntax(
                    openParenToken,
                    SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.Create(firstArgument),
                    closeToken);
            }

            var builder = SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.CreateBuilder(firstArgument);

            while (At(JassSyntaxKind.CommaToken))
            {
                var commaToken = EatToken();
                var argument = ParseExpression();
                builder.Add(commaToken, argument);
            }

            var closeToken2 = EatToken(JassSyntaxKind.CloseParenToken);
            return new JassArgumentListSyntax(openParenToken, builder.ToSeparatedSyntaxList(), closeToken2);
        }
    }
}