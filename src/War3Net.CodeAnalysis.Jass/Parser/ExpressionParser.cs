// ------------------------------------------------------------------------------
// <copyright file="ExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

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
                        Pidgin.Expression.Operator.Prefix(GetUnaryNotOperatorParser()),
                        Pidgin.Expression.Operator.Prefix(GetUnaryPlusOperatorParser())
                            .And(Pidgin.Expression.Operator.Prefix(GetUnaryMinusOperatorParser())),
                        Pidgin.Expression.Operator.InfixL(GetBinaryMultiplicationOperatorParser())
                            .And(Pidgin.Expression.Operator.InfixL(GetBinaryDivisionOperatorParser())),
                        Pidgin.Expression.Operator.InfixL(GetBinaryAddOperatorParser())
                            .And(Pidgin.Expression.Operator.InfixL(GetBinarySubtractOperatorParser())),
                        Pidgin.Expression.Operator.InfixL(GetBinaryGreaterOrEqualOperatorParser())
                            .And(Pidgin.Expression.Operator.InfixL(GetBinaryLessOrEqualOperatorParser()))
                            .And(Pidgin.Expression.Operator.InfixL(GetBinaryEqualsOperatorParser()))
                            .And(Pidgin.Expression.Operator.InfixL(GetBinaryNotEqualsOperatorParser()))
                            .And(Pidgin.Expression.Operator.InfixL(GetBinaryGreaterThanOperatorParser()))
                            .And(Pidgin.Expression.Operator.InfixL(GetBinaryLessThanOperatorParser())),
                        Pidgin.Expression.Operator.InfixL(GetBinaryAndOperatorParser())
                            .And(Pidgin.Expression.Operator.InfixL(GetBinaryOrOperatorParser())),
                    }));
        }
    }
}