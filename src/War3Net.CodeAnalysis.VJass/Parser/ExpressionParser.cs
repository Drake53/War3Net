// ------------------------------------------------------------------------------
// <copyright file="ExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;
using Pidgin.Expression;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassExpressionSyntax> GetExpressionParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, string> stringLiteralParser)
        {
            return Pidgin.Expression.ExpressionParser.Build<char, VJassExpressionSyntax>(
                expressionParser =>
                {
                    var argumentListParser = GetArgumentListParser(triviaParser, expressionParser);

                    return (OneOf(
                        GetCharacterLiteralExpressionParser(triviaParser),
                        GetFourCCLiteralExpressionParser(triviaParser),
                        GetHexadecimalLiteralExpressionParser(triviaParser),
                        GetRealLiteralExpressionParser(triviaParser),
                        GetOctalLiteralExpressionParser(triviaParser),
                        GetDecimalLiteralExpressionParser(triviaParser),
                        GetBooleanLiteralExpressionParser(triviaParser),
                        GetStringLiteralExpressionParser(stringLiteralParser, triviaParser),
                        GetNullLiteralExpressionParser(triviaParser),
                        GetThisLiteralExpressionParser(triviaParser),
                        GetImplicitAccessMemberLiteralExpressionParser(identifierNameParser),
                        GetFunctionReferenceExpressionParser(expressionParser, triviaParser),
                        GetInvocationExpressionParser(triviaParser, argumentListParser, identifierNameParser),
                        GetThisTypeLiteralExpressionParser(identifierNameParser, triviaParser),
                        identifierNameParser.Cast<VJassExpressionSyntax>(),
                        GetParenthesizedExpressionParser(triviaParser, expressionParser)),
                    new[]
                    {
                        Operator.PostfixChainable(GetMemberAccessPostfixParser(triviaParser, argumentListParser, identifierNameParser), GetElementAccessPostfixParser(triviaParser, expressionParser)),

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