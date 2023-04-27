// ------------------------------------------------------------------------------
// <copyright file="JassParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        private static readonly JassParser _parser = new JassParser();

        private readonly Parser<char, JassArgumentListSyntax> _argumentListParser;
        private readonly Parser<char, JassSyntaxToken> _binaryOperatorParser;
        private readonly Parser<char, JassCompilationUnitSyntax> _compilationUnitParser;
        private readonly Parser<char, JassExpressionSyntax> _expressionParser;
        private readonly Parser<char, JassGlobalDeclarationSyntax> _globalDeclarationParser;
        private readonly Parser<char, JassIdentifierNameSyntax> _identifierNameParser;
        private readonly Parser<char, JassSyntaxTriviaList> _leadingTriviaListParser;
        private readonly Parser<char, JassParameterListOrEmptyParameterListSyntax> _parameterListParser;
        private readonly Parser<char, JassStatementSyntax> _statementParser;
        private readonly Parser<char, JassTopLevelDeclarationSyntax> _topLevelDeclarationParser;
        private readonly Parser<char, JassSyntaxTriviaList> _trailingTriviaListParser;
        private readonly Parser<char, JassTypeSyntax> _typeParser;
        private readonly Parser<char, JassSyntaxToken> _unaryOperatorParser;

        private JassParser()
        {
            var whitespaceTriviaParser = GetWhitespaceTriviaParser();
            var newlineTriviaParser = GetNewlineTriviaParser();
            var singleNewlineTriviaParser = GetSingleNewlineTriviaParser();
            var singleLineCommentTriviaParser = GetSingleLineCommentTriviaParser();

            var simpleTriviaListParser = GetSimpleTriviaListParser(
                whitespaceTriviaParser);

            var leadingTriviaListParser = GetLeadingTriviaListParser(
                whitespaceTriviaParser,
                newlineTriviaParser,
                singleLineCommentTriviaParser);

            var trailingTriviaListParser = GetTrailingTriviaListParser(
                whitespaceTriviaParser,
                singleNewlineTriviaParser,
                singleLineCommentTriviaParser);

            var identifierParser = GetIdentifierParser();
            var identifierNameParser = GetIdentifierNameParser(identifierParser, simpleTriviaListParser);
            var typeParser = GetTypeParser(identifierParser, simpleTriviaListParser);

            var expressionParser = GetExpressionParser(identifierParser, identifierNameParser, simpleTriviaListParser);
            var equalsValueClauseParser = GetEqualsValueClauseParser(simpleTriviaListParser, expressionParser);

            var parameterParser = GetParameterParser(identifierNameParser, typeParser);
            var parameterListParser = GetParameterListParser(simpleTriviaListParser, parameterParser);
            var argumentListParser = GetArgumentListParser(simpleTriviaListParser, expressionParser);
            var returnClauseParser = GetReturnClauseParser(typeParser, simpleTriviaListParser);
            var functionDeclaratorParser = GetFunctionDeclaratorParser(identifierNameParser, parameterListParser, returnClauseParser, simpleTriviaListParser, trailingTriviaListParser);
            var variableOrArrayDeclaratorParser = GetVariableOrArrayDeclaratorParser(equalsValueClauseParser, identifierNameParser, typeParser, simpleTriviaListParser);
            var elementAccessClauseParser = GetElementAccessClauseParser(simpleTriviaListParser, expressionParser);

            var setStatementParser = GetSetStatementParser(identifierNameParser, elementAccessClauseParser, equalsValueClauseParser, simpleTriviaListParser, trailingTriviaListParser);
            var callStatementParser = GetCallStatementParser(identifierNameParser, argumentListParser, simpleTriviaListParser, trailingTriviaListParser);
            var exitStatementParser = GetExitStatementParser(expressionParser, simpleTriviaListParser, trailingTriviaListParser);
            var localVariableDeclarationStatementParser = GetLocalVariableDeclarationStatementParser(variableOrArrayDeclaratorParser, simpleTriviaListParser, trailingTriviaListParser);
            var returnStatementParser = GetReturnStatementParser(expressionParser, simpleTriviaListParser, trailingTriviaListParser);

            var ifClauseDeclaratorParser = GetIfClauseDeclaratorParser(expressionParser, simpleTriviaListParser, trailingTriviaListParser);
            var elseIfClauseDeclaratorParser = GetElseIfClauseDeclaratorParser(expressionParser, simpleTriviaListParser, trailingTriviaListParser);

            var statementParser = GetStatementParser(
                localVariableDeclarationStatementParser,
                exitStatementParser,
                returnStatementParser,
                setStatementParser,
                callStatementParser,
                ifClauseDeclaratorParser,
                elseIfClauseDeclaratorParser,
                simpleTriviaListParser,
                leadingTriviaListParser,
                trailingTriviaListParser);

            var globalConstantDeclarationParser = GetGlobalConstantDeclarationParser(
                equalsValueClauseParser,
                identifierNameParser,
                typeParser,
                simpleTriviaListParser,
                trailingTriviaListParser);

            var globalVariableDeclarationParser = GetGlobalVariableDeclarationParser(
                variableOrArrayDeclaratorParser,
                trailingTriviaListParser);

            var globalDeclarationParser = GetGlobalDeclarationParser(
                globalConstantDeclarationParser,
                globalVariableDeclarationParser);

            var functionDeclarationParser = GetFunctionDeclarationParser(
                functionDeclaratorParser,
                statementParser,
                leadingTriviaListParser,
                trailingTriviaListParser);

            var typeDeclarationParser = GetTypeDeclarationParser(
                identifierNameParser,
                typeParser,
                simpleTriviaListParser,
                trailingTriviaListParser);

            var nativeFunctionDeclarationParser = GetNativeFunctionDeclarationParser(
                identifierNameParser,
                parameterListParser,
                returnClauseParser,
                simpleTriviaListParser,
                trailingTriviaListParser);

            var topLevelDeclarationParser = GetTopLevelDeclarationParser(
                typeDeclarationParser,
                nativeFunctionDeclarationParser,
                functionDeclarationParser,
                globalDeclarationParser,
                simpleTriviaListParser,
                leadingTriviaListParser,
                trailingTriviaListParser);

            var compilationUnitParser = GetCompilationUnitParser(
                topLevelDeclarationParser,
                leadingTriviaListParser);

            Parser<char, T> Create<T>(Parser<char, T> parser) => simpleTriviaListParser.Then(parser).Before(End);

            _argumentListParser = Create(argumentListParser);
            _binaryOperatorParser = Create(GetBinaryOperatorParser(simpleTriviaListParser));
            _compilationUnitParser = Create(compilationUnitParser);
            _expressionParser = Create(expressionParser);
            _globalDeclarationParser = Create(globalDeclarationParser);
            _identifierNameParser = Create(identifierNameParser);
            _leadingTriviaListParser = leadingTriviaListParser.Before(End);
            _parameterListParser = Create(parameterListParser);
            _statementParser = Create(statementParser);
            _topLevelDeclarationParser = Create(topLevelDeclarationParser);
            _trailingTriviaListParser = trailingTriviaListParser.Before(End);
            _typeParser = Create(typeParser);
            _unaryOperatorParser = Create(GetUnaryOperatorParser(simpleTriviaListParser));
        }

        internal static JassParser Instance => _parser;

        internal Parser<char, JassArgumentListSyntax> ArgumentListParser => _argumentListParser;

        internal Parser<char, JassSyntaxToken> BinaryOperatorParser => _binaryOperatorParser;

        internal Parser<char, JassCompilationUnitSyntax> CompilationUnitParser => _compilationUnitParser;

        internal Parser<char, JassExpressionSyntax> ExpressionParser => _expressionParser;

        internal Parser<char, JassGlobalDeclarationSyntax> GlobalDeclarationParser => _globalDeclarationParser;

        internal Parser<char, JassIdentifierNameSyntax> IdentifierNameParser => _identifierNameParser;

        internal Parser<char, JassSyntaxTriviaList> LeadingTriviaListParser => _leadingTriviaListParser;

        internal Parser<char, JassParameterListOrEmptyParameterListSyntax> ParameterListParser => _parameterListParser;

        internal Parser<char, JassStatementSyntax> StatementParser => _statementParser;

        internal Parser<char, JassTopLevelDeclarationSyntax> TopLevelDeclarationParser => _topLevelDeclarationParser;

        internal Parser<char, JassSyntaxTriviaList> TrailingTriviaListParser => _trailingTriviaListParser;

        internal Parser<char, JassTypeSyntax> TypeParser => _typeParser;

        internal Parser<char, JassSyntaxToken> UnaryOperatorParser => _unaryOperatorParser;

        private static class Keyword
        {
            internal static readonly Parser<char, string> Alias;
            internal static readonly Parser<char, string> And;
            internal static readonly Parser<char, string> Array;
            internal static readonly Parser<char, string> Boolean;
            internal static readonly Parser<char, string> Call;
            internal static readonly Parser<char, string> Code;
            internal static readonly Parser<char, string> Constant;
            internal static readonly Parser<char, string> Debug;
            internal static readonly Parser<char, string> Else;
            internal static readonly Parser<char, string> ElseIf;
            internal static readonly Parser<char, string> EndFunction;
            internal static readonly Parser<char, string> EndGlobals;
            internal static readonly Parser<char, string> EndIf;
            internal static readonly Parser<char, string> EndLoop;
            internal static readonly Parser<char, string> ExitWhen;
            internal static readonly Parser<char, string> Extends;
            internal static readonly Parser<char, string> False;
            internal static readonly Parser<char, string> Function;
            internal static readonly Parser<char, string> Globals;
            internal static readonly Parser<char, string> Handle;
            internal static readonly Parser<char, string> If;
            internal static readonly Parser<char, string> Integer;
            internal static readonly Parser<char, string> Local;
            internal static readonly Parser<char, string> Loop;
            internal static readonly Parser<char, string> Native;
            internal static readonly Parser<char, string> Not;
            internal static readonly Parser<char, string> Nothing;
            internal static readonly Parser<char, string> Null;
            internal static readonly Parser<char, string> Or;
            internal static readonly Parser<char, string> Real;
            internal static readonly Parser<char, string> Return;
            internal static readonly Parser<char, string> Returns;
            internal static readonly Parser<char, string> Set;
            internal static readonly Parser<char, string> String;
            internal static readonly Parser<char, string> Takes;
            internal static readonly Parser<char, string> Then;
            internal static readonly Parser<char, string> True;
            internal static readonly Parser<char, string> Type;

            static Keyword()
            {
                var keywordEndParser = Not(Token(JassSyntaxFacts.IsIdentifierPartCharacter));

                Alias = GetKeywordParser(JassKeyword.Alias, keywordEndParser);
                And = GetKeywordParser(JassKeyword.And, keywordEndParser);
                Array = GetKeywordParser(JassKeyword.Array, keywordEndParser);
                Boolean = GetKeywordParser(JassKeyword.Boolean, keywordEndParser);
                Call = GetKeywordParser(JassKeyword.Call, keywordEndParser);
                Code = GetKeywordParser(JassKeyword.Code, keywordEndParser);
                Constant = GetKeywordParser(JassKeyword.Constant, keywordEndParser);
                Debug = GetKeywordParser(JassKeyword.Debug, keywordEndParser);
                Else = GetKeywordParser(JassKeyword.Else, keywordEndParser);
                ElseIf = GetKeywordParser(JassKeyword.ElseIf, keywordEndParser);
                EndFunction = GetKeywordParser(JassKeyword.EndFunction, keywordEndParser);
                EndGlobals = GetKeywordParser(JassKeyword.EndGlobals, keywordEndParser);
                EndIf = GetKeywordParser(JassKeyword.EndIf, keywordEndParser);
                EndLoop = GetKeywordParser(JassKeyword.EndLoop, keywordEndParser);
                ExitWhen = GetKeywordParser(JassKeyword.ExitWhen, keywordEndParser);
                Extends = GetKeywordParser(JassKeyword.Extends, keywordEndParser);
                False = GetKeywordParser(JassKeyword.False, keywordEndParser);
                Function = GetKeywordParser(JassKeyword.Function, keywordEndParser);
                Globals = GetKeywordParser(JassKeyword.Globals, keywordEndParser);
                Handle = GetKeywordParser(JassKeyword.Handle, keywordEndParser);
                If = GetKeywordParser(JassKeyword.If, keywordEndParser);
                Integer = GetKeywordParser(JassKeyword.Integer, keywordEndParser);
                Local = GetKeywordParser(JassKeyword.Local, keywordEndParser);
                Loop = GetKeywordParser(JassKeyword.Loop, keywordEndParser);
                Native = GetKeywordParser(JassKeyword.Native, keywordEndParser);
                Not = GetKeywordParser(JassKeyword.Not, keywordEndParser);
                Nothing = GetKeywordParser(JassKeyword.Nothing, keywordEndParser);
                Null = GetKeywordParser(JassKeyword.Null, keywordEndParser);
                Or = GetKeywordParser(JassKeyword.Or, keywordEndParser);
                Real = GetKeywordParser(JassKeyword.Real, keywordEndParser);
                Return = GetKeywordParser(JassKeyword.Return, keywordEndParser);
                Returns = GetKeywordParser(JassKeyword.Returns, keywordEndParser);
                Set = GetKeywordParser(JassKeyword.Set, keywordEndParser);
                String = GetKeywordParser(JassKeyword.String, keywordEndParser);
                Takes = GetKeywordParser(JassKeyword.Takes, keywordEndParser);
                Then = GetKeywordParser(JassKeyword.Then, keywordEndParser);
                True = GetKeywordParser(JassKeyword.True, keywordEndParser);
                Type = GetKeywordParser(JassKeyword.Type, keywordEndParser);
            }

            private static Parser<char, string> GetKeywordParser(string keyword, Parser<char, Unit> keywordEndParser)
            {
                return Try(String(keyword).Before(keywordEndParser)).Labelled($"'{keyword}' keyword");
            }
        }

        private static class Symbol
        {
            internal static readonly Parser<char, char> LineFeed = Char(JassSymbol.LineFeedChar);
            internal static readonly Parser<char, char> CarriageReturn = Char(JassSymbol.CarriageReturnChar);
            internal static readonly Parser<char, char> DoubleQuote = Char(JassSymbol.DoubleQuoteChar);
            internal static readonly Parser<char, char> Dollar = Char(JassSymbol.DollarChar);
            internal static readonly Parser<char, char> SingleQuote = Char(JassSymbol.SingleQuoteChar);
            internal static readonly Parser<char, char> OpenParen = Char(JassSymbol.OpenParenChar);
            internal static readonly Parser<char, char> CloseParen = Char(JassSymbol.CloseParenChar);
            internal static readonly Parser<char, char> Asterisk = Char(JassSymbol.AsteriskChar);
            internal static readonly Parser<char, char> Plus = Char(JassSymbol.PlusChar);
            internal static readonly Parser<char, char> Comma = Char(JassSymbol.CommaChar);
            internal static readonly Parser<char, char> Minus = Char(JassSymbol.MinusChar);
            internal static readonly Parser<char, char> Dot = Char(JassSymbol.DotChar);
            internal static readonly Parser<char, char> Slash = Char(JassSymbol.SlashChar);
            internal static readonly Parser<char, char> Zero = Char(JassSymbol.ZeroChar);
            internal static readonly Parser<char, char> LessThan = Char(JassSymbol.LessThanChar);
            internal static new readonly Parser<char, char> Equals = Char(JassSymbol.EqualsChar);
            internal static readonly Parser<char, char> GreaterThan = Char(JassSymbol.GreaterThanChar);
            internal static readonly Parser<char, char> OpenBracket = Char(JassSymbol.OpenBracketChar);
            internal static readonly Parser<char, char> CloseBracket = Char(JassSymbol.CloseBracketChar);
            internal static readonly Parser<char, char> X = CIChar(JassSymbol.XChar);

            internal static readonly Parser<char, string> CarriageReturnLineFeed = String(JassSymbol.CarriageReturnLineFeed);
            internal static readonly Parser<char, string> CarriageReturnString = String(JassSymbol.CarriageReturn);
            internal static readonly Parser<char, string> LineFeedString = String(JassSymbol.LineFeed);
            internal static readonly Parser<char, string> EqualsEquals = String(JassSymbol.EqualsEquals);
            internal static readonly Parser<char, string> LessThanEquals = String(JassSymbol.LessThanEquals);
            internal static readonly Parser<char, string> GreaterThanEquals = String(JassSymbol.GreaterThanEquals);
            internal static readonly Parser<char, string> ExclamationEquals = String(JassSymbol.ExclamationEquals);
            internal static readonly Parser<char, string> SlashSlash = String(JassSymbol.SlashSlash);
        }
    }
}