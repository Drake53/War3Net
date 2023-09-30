﻿// ------------------------------------------------------------------------------
// <copyright file="ExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;
using Pidgin.Expression;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Parsers;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassExpressionSyntax> GetExpressionParser(
            Parser<char, string> identifierParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Pidgin.Expression.ExpressionParser.Build<char, JassExpressionSyntax>(
                expressionParser =>
                {
                    var argumentListParser = GetArgumentListParser(triviaParser, expressionParser);
                    var elementAccessClauseParser = GetElementAccessClauseParser(triviaParser, expressionParser);

                    return (OneOf(
                        GetCharacterLiteralExpressionParser(triviaParser),
                        GetFourCCLiteralExpressionParser(triviaParser),
                        GetHexadecimalLiteralExpressionParser(triviaParser),
                        GetRealLiteralExpressionParser(triviaParser),
                        GetOctalLiteralExpressionParser(triviaParser),
                        GetDecimalLiteralExpressionParser(triviaParser),
                        GetStringLiteralExpressionParser(triviaParser),
                        new IdentifierExpressionParser(
                            identifierParser,
                            identifierNameParser,
                            argumentListParser,
                            elementAccessClauseParser,
                            triviaParser),
                        GetParenthesizedExpressionParser(triviaParser, expressionParser)),
                    new[]
                    {
                        // https://www.hiveworkshop.com/threads/precedence-in-jass.43500/#post-378439
                        Operator.PrefixChainable(GetUnaryNotOperatorParser(triviaParser).Prefix()),
                        Operator.PrefixChainable(GetUnaryPlusOperatorParser(triviaParser).Prefix(), GetUnaryMinusOperatorParser(triviaParser).Prefix()),
                        Operator.InfixL(GetBinaryMultiplicationOperatorParser(triviaParser).Infix())
                            .And(Operator.InfixL(GetBinaryDivisionOperatorParser(triviaParser).Infix())),
                        Operator.InfixL(GetBinaryAddOperatorParser(triviaParser).Infix())
                            .And(Operator.InfixL(GetBinarySubtractOperatorParser(triviaParser).Infix())),
                        Operator.InfixL(GetBinaryGreaterOrEqualOperatorParser(triviaParser).Infix())
                            .And(Operator.InfixL(GetBinaryLessOrEqualOperatorParser(triviaParser).Infix()))
                            .And(Operator.InfixL(GetBinaryEqualsOperatorParser(triviaParser).Infix()))
                            .And(Operator.InfixL(GetBinaryNotEqualsOperatorParser(triviaParser).Infix()))
                            .And(Operator.InfixL(GetBinaryGreaterThanOperatorParser(triviaParser).Infix()))
                            .And(Operator.InfixL(GetBinaryLessThanOperatorParser(triviaParser).Infix())),
                        Operator.InfixL(GetBinaryAndOperatorParser(triviaParser).Infix())
                            .And(Operator.InfixL(GetBinaryOrOperatorParser(triviaParser).Infix())),
                    });
                });
        }
    }
}