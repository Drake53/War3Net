// ------------------------------------------------------------------------------
// <copyright file="JassPidginParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;
using Pidgin.Expression;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassPidginParser
    {
        public static IExpressionSyntax ParseExpression(string expression) => _expressionEndParser.ParseOrThrow(expression);

        private static Parser<char, IExpressionSyntax> GetExpressionParser()
        {
            return ExpressionParser.Build<char, IExpressionSyntax>(
                expressionParser =>
                (
                    OneOf(
                        FourCCLiteralExpressionParser,
                        HexadecimalLiteralExpressionParser,
                        RealLiteralExpressionParser,
                        OctalLiteralExpressionParser,
                        DecimalLiteralExpressionParser,
                        BooleanLiteralExpressionParser,
                        StringLiteralExpressionParser,
                        NullLiteralExpressionParser,
                        FunctionReferenceExpressionParser,
                        GetArrayReferenceExpressionParser(expressionParser),
                        VariableReferenceExpressionParser,
                        GetParenthesizedExpressionParser(expressionParser)),
                    new[]
                    {
                        Operator.PostfixChainable(GetInvocationExpressionParser(expressionParser)),
                        Operator.Prefix(UnaryNotExpressionParser),
                        Operator.Prefix(UnaryPlusExpressionParser).And(Operator.Prefix(UnaryMinusExpressionParser)),
                        Operator.InfixL(BinaryMultiplicationExpressionParser)
                            .And(Operator.InfixL(BinaryDivisionExpressionParser)),
                        Operator.InfixL(BinaryAddExpressionParser)
                            .And(Operator.InfixL(BinarySubtractExpressionParser)),
                        Operator.InfixL(BinaryGreaterThanExpressionParser)
                            .And(Operator.InfixL(BinaryLessThanExpressionParser))
                            .And(Operator.InfixL(BinaryEqualsExpressionParser))
                            .And(Operator.InfixL(BinaryNotEqualsExpressionParser))
                            .And(Operator.InfixL(BinaryGreaterOrEqualExpressionParser))
                            .And(Operator.InfixL(BinaryLessOrEqualExpressionParser)),
                        Operator.InfixL(BinaryAndExpressionParser)
                            .And(Operator.InfixL(BinaryOrExpressionParser)),
                    }));
        }
    }
}