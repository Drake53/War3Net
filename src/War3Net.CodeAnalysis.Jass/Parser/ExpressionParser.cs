// ------------------------------------------------------------------------------
// <copyright file="ExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.Common.Extensions;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassPidginParser
    {
        private static Parser<char, JassIdentifierNameSyntax> GetJassIdentifierNameParser()
        {
#if false
            var identifierNameTailParser = LetterOrDigitOrUnderscore.Many().Then(LetterOrDigit, (chars, lastChar) => new string(chars.ToArray()) + lastChar).Optional();
            return Tok(Letter.Then(identifierNameTailParser, (head, tail) => tail.HasValue ? head + tail.Value : head.ToString()))
#else
            var identifierNameTailParser = LetterOrDigitOrUnderscore.ManyString().Assert(tail => tail.Length == 0 || tail[^1] != '_');
            return Tok(Letter.Then(identifierNameTailParser, (head, tail) => head + tail))
#endif
                .Select(name => new JassIdentifierNameSyntax(name))
                .Labelled("identifier name");
        }

        private static Parser<char, JassArgumentListSyntax> GetArgumentListParser(Parser<char, IExpressionSyntax> expressionParser)
        {
            return expressionParser.Separated(Char(',').SkipWhitespaces()).Select(arguments => new JassArgumentListSyntax(arguments.ToImmutableArray()));
        }

        private static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryExpressionParser(Parser<char, Syntax.BinaryOperatorType> operatorParser)
        {
            return operatorParser.Select<Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>>(@operator => (left, right) => new JassBinaryExpressionSyntax(@operator, left, right));
        }

        private static Parser<char, Func<IExpressionSyntax, IExpressionSyntax>> GetUnaryExpressionParser(Parser<char, Syntax.UnaryOperatorType> operatorParser)
        {
            return operatorParser.Select<Func<IExpressionSyntax, IExpressionSyntax>>(@operator => expression => new JassUnaryExpressionSyntax(@operator, expression));
        }

#if false
        private static Parser<char, IExpressionSyntax> GetInvocationExpressionParser(Parser<char, IExpressionSyntax> expressionParser)
        {
            return Try(JassIdentifierNameParser.Before(Char('(').SkipWhitespaces()))
                .Then(expressionParser.Separated(Char(',').SkipWhitespaces()).Before(Char(')')), (id, args) => (IExpressionSyntax)new JassInvocationExpressionSyntax(id, args.ToImmutableArray()))
                .Labelled("invocation expression");
        }
#else
        private static Parser<char, Func<IExpressionSyntax, IExpressionSyntax>> GetInvocationExpressionParser(Parser<char, IExpressionSyntax> expressionParser)
        {
            return Char('(').SkipWhitespaces().Then(GetArgumentListParser(expressionParser)).Before(Char(')'))
                .Select<Func<IExpressionSyntax, IExpressionSyntax>>(arguments => expression => expression is JassVariableReferenceExpressionSyntax e
                    ? new JassInvocationExpressionSyntax(e.IdentifierName, arguments)
                    : new JassInvocationExpressionSyntax(new JassIdentifierNameSyntax("INVALID"), arguments))
                .Labelled("invocation expression");
        }
#endif

        private static Parser<char, IExpressionSyntax> GetArrayReferenceExpressionParser(Parser<char, IExpressionSyntax> expressionParser)
        {
            return Try(JassIdentifierNameParser
                .Before(Char('[').SkipWhitespaces()))
                .Then(expressionParser, (id, indexer) => (IExpressionSyntax)new JassArrayReferenceExpressionSyntax(id, indexer))
                .Before(Char(']').SkipWhitespaces())
                .Labelled("array reference");
        }

        private static Parser<char, IExpressionSyntax> GetFunctionReferenceExpressionParser()
        {
            return Keyword("function").Then(JassIdentifierNameParser)
                .Select<IExpressionSyntax>(name => new JassFunctionReferenceExpressionSyntax(name))
                .Labelled("function reference");
        }

        private static Parser<char, IExpressionSyntax> GetDecimalLiteralExpressionParser()
        {
            return Tok(UnsignedInt(10)).AssertNotFollowedByLetterOrUnderscore()
                .Select<IExpressionSyntax>(value => new JassDecimalLiteralExpressionSyntax(value))
                .Labelled("decimal literal");
        }

        private static Parser<char, IExpressionSyntax> GetOctalLiteralExpressionParser()
        {
            return Char('0').Then(Tok(UnsignedInt(8)).Optional()).AssertNotFollowedByLetterOrUnderscore()
                .Select<IExpressionSyntax>(value => new JassOctalLiteralExpressionSyntax(value.GetValueOrDefault()))
                .Labelled("octal literal");
        }

        private static Parser<char, IExpressionSyntax> GetHexadecimalLiteralExpressionParser()
        {
            return Char('$').Or(Try(Char('0').Then(CIChar('x')))).Then(UnsignedInt(16)).AssertNotFollowedByLetterOrUnderscore()
                .Select<IExpressionSyntax>(value => new JassHexadecimalLiteralExpressionSyntax(value))
                .Labelled("hexadecimal literal");
        }

        private static Parser<char, IExpressionSyntax> GetFourCCLiteralExpressionParser()
        {
            return Tok(AnyCharExcept('\n')).Repeat(4).Between(Char('\''))
                .Select<IExpressionSyntax>(value => new JassFourCCLiteralExpressionSyntax(new string(value.ToArray()).FromRawcode()))
                .Labelled("fourCC literal");
        }

        private static Parser<char, IExpressionSyntax> GetRealLiteralExpressionParser()
        {
            return Try(Tok(UnsignedInt(10)).Before(Char('.'))).Then(Tok(UnsignedInt(10)).Optional(), (intPart, fracPart) => (IExpressionSyntax)new JassRealLiteralExpressionSyntax(float.Parse($"{intPart}.{fracPart.GetValueOrDefault()}")))
                .Or(Char('.').Then(Tok(UnsignedInt(10))).Select<IExpressionSyntax>((fracPart) => new JassRealLiteralExpressionSyntax(float.Parse($"0.{fracPart}"))))
                .AssertNotFollowedByLetterOrUnderscore()
                .Labelled("real literal");
        }

        private static Parser<char, IExpressionSyntax> GetBooleanLiteralExpressionParser()
        {
            return Keyword<IExpressionSyntax>("true", JassBooleanLiteralExpressionSyntax.True)
                .Or(Keyword<IExpressionSyntax>("false", JassBooleanLiteralExpressionSyntax.False))
                .Labelled("boolean literal");
        }

        private static Parser<char, IExpressionSyntax> GetStringLiteralExpressionParser()
        {
            return Char('\\').Then(EscapeChar).Or(Token(@char => @char != '\n' && @char != '"').Map(@char => @char.ToString())).ManyString().Between(Char('"'))
                .Select<IExpressionSyntax>(value => new JassStringLiteralExpressionSyntax(value))
                .Labelled("string literal");
        }

        private static Parser<char, IExpressionSyntax> GetNullLiteralExpressionParser()
        {
            return Keyword<IExpressionSyntax>("null", JassNullLiteralExpressionSyntax.Null)
                .Labelled("null literal");
        }

        private static Parser<char, IExpressionSyntax> GetParenthesizedExpressionParser(Parser<char, IExpressionSyntax> expressionParser)
        {
            return Char('(').SkipWhitespaces().Then(expressionParser.SkipWhitespaces()).Before(Char(')').SkipWhitespaces())
                .Select<IExpressionSyntax>(expression => new JassParenthesizedExpressionSyntax(expression))
                .Labelled("parenthesized expression");
        }

        private static Parser<char, IExpressionSyntax> GetVariableReferenceExpressionParser()
        {
            return JassIdentifierNameParser
                .Select<IExpressionSyntax>(name => new JassVariableReferenceExpressionSyntax(name))
                .Labelled("variable reference");
        }
    }
}