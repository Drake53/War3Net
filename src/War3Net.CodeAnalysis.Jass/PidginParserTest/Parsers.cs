// ------------------------------------------------------------------------------
// <copyright file="Parsers.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassPidginParser
    {
        private static readonly Parser<char, string> EscapeChar = OneOf(
            Char('"').ThenReturn("\\\""),
            Char('r').ThenReturn("\\r"),
            Char('n').ThenReturn("\\n"),
            Char('t').ThenReturn("\\t"),
            Char('\\').ThenReturn("\\\\"))
            .Labelled("escape character");

        private static Parser<char, T> Tok<T>(Parser<char, T> token) => Try(token).SkipWhitespaces();

        private static Parser<char, string> Tok(string token) => Tok(String(token));

        private static Parser<char, Unit> Keyword(string keyword) => Keyword(keyword, Unit.Value);
        private static Parser<char, T> Keyword<T>(string keyword, T result) => Try(String(keyword).AssertNotFollowedByLetterOrDigitOrUnderscore()).SkipWhitespaces().ThenReturn(result).Labelled($"'{keyword}' keyword");

        private static readonly Parser<char, char> LetterOrDigitOrUnderscore = Token(c => char.IsLetterOrDigit(c) || c == '_').Labelled("letter or digit or underscore");
        private static readonly Parser<char, char> Newline = Char('\n').SkipWhitespaces().Labelled("newline");

        private static readonly Parser<char, JassIdentifierNameSyntax> JassIdentifierNameParser = GetJassIdentifierNameParser();

        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> BinaryAddExpressionParser = GetBinaryExpressionParser(Tok("+").ThenReturn(BinaryOperatorType.Add));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> BinarySubtractExpressionParser = GetBinaryExpressionParser(Tok("-").ThenReturn(BinaryOperatorType.Subtract));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> BinaryMultiplicationExpressionParser = GetBinaryExpressionParser(Tok("*").ThenReturn(BinaryOperatorType.Multiplication));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> BinaryDivisionExpressionParser = GetBinaryExpressionParser(Tok("/").ThenReturn(BinaryOperatorType.Division));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> BinaryGreaterThanExpressionParser = GetBinaryExpressionParser(Tok(">").ThenReturn(BinaryOperatorType.GreaterThan));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> BinaryLessThanExpressionParser = GetBinaryExpressionParser(Tok("<").ThenReturn(BinaryOperatorType.LessThan));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> BinaryEqualsExpressionParser = GetBinaryExpressionParser(Tok("==").ThenReturn(BinaryOperatorType.Equals));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> BinaryNotEqualsExpressionParser = GetBinaryExpressionParser(Tok("!=").ThenReturn(BinaryOperatorType.NotEquals));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> BinaryGreaterOrEqualExpressionParser = GetBinaryExpressionParser(Tok(">=").ThenReturn(BinaryOperatorType.GreaterOrEqual));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> BinaryLessOrEqualExpressionParser = GetBinaryExpressionParser(Tok("<=").ThenReturn(BinaryOperatorType.LessOrEqual));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> BinaryAndExpressionParser = GetBinaryExpressionParser(Keyword("and", BinaryOperatorType.And));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> BinaryOrExpressionParser = GetBinaryExpressionParser(Keyword("or", BinaryOperatorType.Or));

        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax>> UnaryPlusExpressionParser = GetUnaryExpressionParser(Tok("+").ThenReturn(UnaryOperatorType.Plus));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax>> UnaryMinusExpressionParser = GetUnaryExpressionParser(Tok("-").ThenReturn(UnaryOperatorType.Minus));
        private static readonly Parser<char, Func<IExpressionSyntax, IExpressionSyntax>> UnaryNotExpressionParser = GetUnaryExpressionParser(Keyword("not").ThenReturn(UnaryOperatorType.Not));

        private static readonly Parser<char, IExpressionSyntax> FunctionReferenceExpressionParser = GetFunctionReferenceExpressionParser();
        private static readonly Parser<char, IExpressionSyntax> DecimalLiteralExpressionParser = GetDecimalLiteralExpressionParser();
        private static readonly Parser<char, IExpressionSyntax> OctalLiteralExpressionParser = GetOctalLiteralExpressionParser();
        private static readonly Parser<char, IExpressionSyntax> HexadecimalLiteralExpressionParser = GetHexadecimalLiteralExpressionParser();
        private static readonly Parser<char, IExpressionSyntax> FourCCLiteralExpressionParser = GetFourCCLiteralExpressionParser();
        private static readonly Parser<char, IExpressionSyntax> RealLiteralExpressionParser = GetRealLiteralExpressionParser();
        private static readonly Parser<char, IExpressionSyntax> BooleanLiteralExpressionParser = GetBooleanLiteralExpressionParser();
        private static readonly Parser<char, IExpressionSyntax> StringLiteralExpressionParser = GetStringLiteralExpressionParser();
        private static readonly Parser<char, IExpressionSyntax> NullLiteralExpressionParser = GetNullLiteralExpressionParser();
        private static readonly Parser<char, IExpressionSyntax> VariableReferenceExpressionParser = GetVariableReferenceExpressionParser();

        private static readonly Parser<char, IExpressionSyntax> _expressionParser = GetExpressionParser();
        private static readonly Parser<char, IExpressionSyntax> _expressionEndParser = _expressionParser.Before(End);
    }
}