// ------------------------------------------------------------------------------
// <copyright file="ExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;
using Pidgin.Expression;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, IExpressionSyntax> GetExpressionParser(
            Parser<char, Unit> whitespaceParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser)
        {
            return Pidgin.Expression.ExpressionParser.Build<char, IExpressionSyntax>(
                expressionParser =>
                (
                    OneOf(
                        GetCharacterLiteralExpressionParser().Before(whitespaceParser),
                        GetFourCCLiteralExpressionParser().Before(whitespaceParser),
                        GetHexadecimalLiteralExpressionParser().Before(whitespaceParser),
                        GetRealLiteralExpressionParser().Before(whitespaceParser),
                        GetOctalLiteralExpressionParser().Before(whitespaceParser),
                        GetDecimalLiteralExpressionParser().Before(whitespaceParser),
                        GetBooleanLiteralExpressionParser(),
                        GetStringLiteralExpressionParser().Before(whitespaceParser),
                        GetNullLiteralExpressionParser(),
                        GetFunctionReferenceExpressionParser(identifierNameParser),
                        GetInvocationExpressionParser(whitespaceParser, expressionParser, identifierNameParser),
                        GetArrayReferenceExpressionParser(whitespaceParser, expressionParser, identifierNameParser),
                        GetVariableReferenceExpressionParser(identifierNameParser),
                        GetParenthesizedExpressionParser(whitespaceParser, expressionParser)),
                    new[]
                    {
                        // https://www.hiveworkshop.com/threads/precedence-in-jass.43500/#post-378439
                        Operator.PrefixChainable(GetUnaryNotOperatorParser().Prefix()),
                        Operator.PrefixChainable(GetUnaryPlusOperatorParser(whitespaceParser).Prefix(), GetUnaryMinusOperatorParser(whitespaceParser).Prefix()),
                        Operator.InfixL(GetBinaryMultiplicationOperatorParser(whitespaceParser).Infix())
                            .And(Operator.InfixL(GetBinaryDivisionOperatorParser(whitespaceParser).Infix())),
                        Operator.InfixL(GetBinaryAddOperatorParser(whitespaceParser).Infix())
                            .And(Operator.InfixL(GetBinarySubtractOperatorParser(whitespaceParser).Infix())),
                        Operator.InfixL(GetBinaryGreaterOrEqualOperatorParser(whitespaceParser).Infix())
                            .And(Operator.InfixL(GetBinaryLessOrEqualOperatorParser(whitespaceParser).Infix()))
                            .And(Operator.InfixL(GetBinaryEqualsOperatorParser(whitespaceParser).Infix()))
                            .And(Operator.InfixL(GetBinaryNotEqualsOperatorParser(whitespaceParser).Infix()))
                            .And(Operator.InfixL(GetBinaryGreaterThanOperatorParser(whitespaceParser).Infix()))
                            .And(Operator.InfixL(GetBinaryLessThanOperatorParser(whitespaceParser).Infix())),
                        Operator.InfixL(GetBinaryAndOperatorParser().Infix())
                            .And(Operator.InfixL(GetBinaryOrOperatorParser().Infix())),
                    }));
        }
    }
}