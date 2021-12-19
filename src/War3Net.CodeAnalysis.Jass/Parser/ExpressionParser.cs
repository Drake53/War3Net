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
        internal static Parser<char, IExpressionSyntax> GetExpressionParser(Parser<char, JassIdentifierNameSyntax> identifierNameParser)
        {
            return Pidgin.Expression.ExpressionParser.Build<char, IExpressionSyntax>(
                expressionParser =>
                (
                    OneOf(
                        GetCharacterLiteralExpressionParser().SkipWhitespaces(),
                        GetFourCCLiteralExpressionParser().SkipWhitespaces(),
                        GetHexadecimalLiteralExpressionParser().SkipWhitespaces(),
                        GetRealLiteralExpressionParser().SkipWhitespaces(),
                        GetOctalLiteralExpressionParser().SkipWhitespaces(),
                        GetDecimalLiteralExpressionParser().SkipWhitespaces(),
                        GetBooleanLiteralExpressionParser(),
                        GetStringLiteralExpressionParser().SkipWhitespaces(),
                        GetNullLiteralExpressionParser(),
                        GetFunctionReferenceExpressionParser(identifierNameParser),
                        GetInvocationExpressionParser(expressionParser, identifierNameParser),
                        GetArrayReferenceExpressionParser(expressionParser, identifierNameParser),
                        GetVariableReferenceExpressionParser(identifierNameParser),
                        GetParenthesizedExpressionParser(expressionParser)),
                    new[]
                    {
                        // https://www.hiveworkshop.com/threads/precedence-in-jass.43500/#post-378439
                        Operator.PrefixChainable(GetUnaryNotOperatorParser().Prefix()),
                        Operator.PrefixChainable(GetUnaryPlusOperatorParser().Prefix(), GetUnaryMinusOperatorParser().Prefix()),
                        Operator.InfixL(GetBinaryMultiplicationOperatorParser().Infix())
                            .And(Operator.InfixL(GetBinaryDivisionOperatorParser().Infix())),
                        Operator.InfixL(GetBinaryAddOperatorParser().Infix())
                            .And(Operator.InfixL(GetBinarySubtractOperatorParser().Infix())),
                        Operator.InfixL(GetBinaryGreaterOrEqualOperatorParser().Infix())
                            .And(Operator.InfixL(GetBinaryLessOrEqualOperatorParser().Infix()))
                            .And(Operator.InfixL(GetBinaryEqualsOperatorParser().Infix()))
                            .And(Operator.InfixL(GetBinaryNotEqualsOperatorParser().Infix()))
                            .And(Operator.InfixL(GetBinaryGreaterThanOperatorParser().Infix()))
                            .And(Operator.InfixL(GetBinaryLessThanOperatorParser().Infix())),
                        Operator.InfixL(GetBinaryAndOperatorParser().Infix())
                            .And(Operator.InfixL(GetBinaryOrOperatorParser().Infix())),
                    }));
        }
    }
}