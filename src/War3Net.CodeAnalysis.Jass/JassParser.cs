// ------------------------------------------------------------------------------
// <copyright file="JassParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        private static readonly JassParser _parser = new JassParser();

        private readonly Parser<char, JassArgumentListSyntax> _argumentListParser;
        private readonly Parser<char, JassCompilationUnitSyntax> _compilationUnitParser;
        private readonly Parser<char, ICustomScriptAction> _customScriptActionParser;
        private readonly Parser<char, IDeclarationSyntax> _declarationParser;
        private readonly Parser<char, IExpressionSyntax> _expressionParser;
        private readonly Parser<char, JassFunctionDeclarationSyntax> _functionDeclarationParser;
        private readonly Parser<char, JassIdentifierNameSyntax> _identifierNameParser;
        private readonly Parser<char, JassParameterListSyntax> _parameterListParser;
        private readonly Parser<char, IStatementSyntax> _statementParser;
        private readonly Parser<char, JassTypeSyntax> _typeParser;

        private JassParser()
        {
            var identifierNameParser = GetIdentifierNameParser();
            var typeParser = GetTypeParser(identifierNameParser);
            var expressionParser = GetExpressionParser(identifierNameParser);
            var equalsValueClauseParser = GetEqualsValueClauseParser(expressionParser);

            var argumentListParser = GetArgumentListParser(expressionParser);
            var parameterListParser = GetParameterListParser(GetParameterParser(identifierNameParser, typeParser));
            var functionDeclaratorParser = GetFunctionDeclaratorParser(identifierNameParser, parameterListParser, typeParser);
            var variableDeclaratorParser = GetVariableDeclaratorParser(equalsValueClauseParser, identifierNameParser, typeParser);

            var commentParser = GetCommentParser();
            var newLineParser = GetNewLineParser();
            var endOfLineParser = GetEndOfLineParser(commentParser, newLineParser);
            var emptyDeclarationParser = GetEmptyDeclarationParser();
            var commentDeclarationParser = GetCommentDeclarationParser(commentParser);

            var setStatementParser = GetSetStatementParser(expressionParser, equalsValueClauseParser, identifierNameParser).Cast<IStatementSyntax>();
            var callStatementParser = GetCallStatementParser(argumentListParser, identifierNameParser).Cast<IStatementSyntax>();
            var statementParser = GetStatementParser(
                expressionParser,
                setStatementParser,
                callStatementParser,
                variableDeclaratorParser,
                commentParser,
                endOfLineParser);

            var customScriptActionParser = GetCustomScriptActionParser(
                GetSetStatementParser(expressionParser, equalsValueClauseParser, identifierNameParser).Cast<ICustomScriptAction>(),
                GetCallStatementParser(argumentListParser, identifierNameParser).Cast<ICustomScriptAction>(),
                GetIfCustomScriptActionParser(expressionParser),
                GetLoopCustomScriptActionParser(),
                expressionParser,
                variableDeclaratorParser,
                functionDeclaratorParser,
                commentParser);

            var globalDeclarationParser = GetGlobalDeclarationParser(
                emptyDeclarationParser,
                commentDeclarationParser,
                equalsValueClauseParser,
                identifierNameParser,
                typeParser,
                endOfLineParser);

            var functionDeclarationParser = GetStandaloneFunctionDeclarationParser(
                functionDeclaratorParser,
                GetStatementListParser(statementParser, endOfLineParser),
                endOfLineParser);

            var declarationParser = GetDeclarationParser(
                emptyDeclarationParser,
                commentDeclarationParser,
                globalDeclarationParser,
                functionDeclaratorParser,
                identifierNameParser,
                GetStatementListParser(statementParser, endOfLineParser),
                typeParser,
                endOfLineParser);

            var compilationUnitParser = GetCompilationUnitParser(
                declarationParser,
                commentParser,
                newLineParser);

            var whitespaceParser = Return(Unit.Value).SkipWhitespaces();

            Parser<char, T> Create<T>(Parser<char, T> parser) => whitespaceParser.Then(parser).Before(End);

            _argumentListParser = Create(argumentListParser);
            _compilationUnitParser = Create(compilationUnitParser);
            _customScriptActionParser = Create(customScriptActionParser.Before(commentParser.Optional()));
            _declarationParser = Create(declarationParser);
            _expressionParser = Create(expressionParser);
            _functionDeclarationParser = Create(functionDeclarationParser);
            _identifierNameParser = Create(identifierNameParser);
            _parameterListParser = Create(parameterListParser);
            _statementParser = Create(statementParser);
            _typeParser = Create(typeParser);
        }

        internal static JassParser Instance => _parser;

        internal Parser<char, JassArgumentListSyntax> ArgumentListParser => _argumentListParser;

        internal Parser<char, JassCompilationUnitSyntax> CompilationUnitParser => _compilationUnitParser;

        internal Parser<char, ICustomScriptAction> CustomScriptActionParser => _customScriptActionParser;

        internal Parser<char, IDeclarationSyntax> DeclarationParser => _declarationParser;

        internal Parser<char, IExpressionSyntax> ExpressionParser => _expressionParser;

        internal Parser<char, JassFunctionDeclarationSyntax> FunctionDeclarationParser => _functionDeclarationParser;

        internal Parser<char, JassIdentifierNameSyntax> IdentifierNameParser => _identifierNameParser;

        internal Parser<char, JassParameterListSyntax> ParameterListParser => _parameterListParser;

        internal Parser<char, IStatementSyntax> StatementParser => _statementParser;

        internal Parser<char, JassTypeSyntax> TypeParser => _typeParser;

        private static class Keyword
        {
            internal static readonly Parser<char, string> Alias = GetKeywordParser(JassKeyword.Alias);
            internal static readonly Parser<char, string> And = GetKeywordParser(JassKeyword.And);
            internal static readonly Parser<char, string> Array = GetKeywordParser(JassKeyword.Array);
            internal static readonly Parser<char, string> Boolean = GetKeywordParser(JassKeyword.Boolean);
            internal static readonly Parser<char, string> Call = GetKeywordParser(JassKeyword.Call);
            internal static readonly Parser<char, string> Code = GetKeywordParser(JassKeyword.Code);
            internal static readonly Parser<char, string> Constant = GetKeywordParser(JassKeyword.Constant);
            internal static readonly Parser<char, string> Debug = GetKeywordParser(JassKeyword.Debug);
            internal static readonly Parser<char, string> Else = GetKeywordParser(JassKeyword.Else);
            internal static readonly Parser<char, string> ElseIf = GetKeywordParser(JassKeyword.ElseIf);
            internal static readonly Parser<char, string> EndFunction = GetKeywordParser(JassKeyword.EndFunction);
            internal static readonly Parser<char, string> EndGlobals = GetKeywordParser(JassKeyword.EndGlobals);
            internal static readonly Parser<char, string> EndIf = GetKeywordParser(JassKeyword.EndIf);
            internal static readonly Parser<char, string> EndLoop = GetKeywordParser(JassKeyword.EndLoop);
            internal static readonly Parser<char, string> ExitWhen = GetKeywordParser(JassKeyword.ExitWhen);
            internal static readonly Parser<char, string> Extends = GetKeywordParser(JassKeyword.Extends);
            internal static readonly Parser<char, string> False = GetKeywordParser(JassKeyword.False);
            internal static readonly Parser<char, string> Function = GetKeywordParser(JassKeyword.Function);
            internal static readonly Parser<char, string> Globals = GetKeywordParser(JassKeyword.Globals);
            internal static readonly Parser<char, string> Handle = GetKeywordParser(JassKeyword.Handle);
            internal static readonly Parser<char, string> If = GetKeywordParser(JassKeyword.If);
            internal static readonly Parser<char, string> Integer = GetKeywordParser(JassKeyword.Integer);
            internal static readonly Parser<char, string> Local = GetKeywordParser(JassKeyword.Local);
            internal static readonly Parser<char, string> Loop = GetKeywordParser(JassKeyword.Loop);
            internal static readonly Parser<char, string> Native = GetKeywordParser(JassKeyword.Native);
            internal static readonly Parser<char, string> Not = GetKeywordParser(JassKeyword.Not);
            internal static readonly Parser<char, string> Nothing = GetKeywordParser(JassKeyword.Nothing);
            internal static readonly Parser<char, string> Null = GetKeywordParser(JassKeyword.Null);
            internal static readonly Parser<char, string> Or = GetKeywordParser(JassKeyword.Or);
            internal static readonly Parser<char, string> Real = GetKeywordParser(JassKeyword.Real);
            internal static readonly Parser<char, string> Return = GetKeywordParser(JassKeyword.Return);
            internal static readonly Parser<char, string> Returns = GetKeywordParser(JassKeyword.Returns);
            internal static readonly Parser<char, string> Set = GetKeywordParser(JassKeyword.Set);
            internal static readonly Parser<char, string> String = GetKeywordParser(JassKeyword.String);
            internal static readonly Parser<char, string> Takes = GetKeywordParser(JassKeyword.Takes);
            internal static readonly Parser<char, string> Then = GetKeywordParser(JassKeyword.Then);
            internal static readonly Parser<char, string> True = GetKeywordParser(JassKeyword.True);
            internal static readonly Parser<char, string> Type = GetKeywordParser(JassKeyword.Type);

            private static Parser<char, string> GetKeywordParser(string keyword)
            {
                return Try(String(keyword).AssertNotFollowedByLetterOrDigitOrUnderscore())
                    .SkipWhitespaces()
                    .Labelled($"'{keyword}' keyword");
            }
        }

        private static class Symbol
        {
            internal static readonly Parser<char, char> LineFeed = Char(JassSymbol.LineFeed);
            internal static readonly Parser<char, char> CarriageReturn = Char(JassSymbol.CarriageReturn);
            internal static readonly Parser<char, char> QuotationMark = Char(JassSymbol.QuotationMark);
            internal static readonly Parser<char, char> DollarSign = Char(JassSymbol.DollarSign);
            internal static readonly Parser<char, char> Apostrophe = Char(JassSymbol.Apostrophe);
            internal static readonly Parser<char, char> LeftParenthesis = Char(JassSymbol.LeftParenthesis).SkipWhitespaces();
            internal static readonly Parser<char, char> RightParenthesis = Char(JassSymbol.RightParenthesis).SkipWhitespaces();
            internal static readonly Parser<char, char> Asterisk = Char(JassSymbol.Asterisk).SkipWhitespaces();
            internal static readonly Parser<char, char> PlusSign = Char(JassSymbol.PlusSign).SkipWhitespaces();
            internal static readonly Parser<char, char> Comma = Char(JassSymbol.Comma).SkipWhitespaces();
            internal static readonly Parser<char, char> MinusSign = Char(JassSymbol.MinusSign).SkipWhitespaces();
            internal static readonly Parser<char, char> FullStop = Char(JassSymbol.FullStop);
            internal static readonly Parser<char, char> Slash = Try(Char(JassSymbol.Slash).Before(Not(Lookahead(Char(JassSymbol.Slash))))).SkipWhitespaces();
            internal static readonly Parser<char, char> Zero = Char(JassSymbol.Zero);
            internal static readonly Parser<char, char> LessThanSign = Char(JassSymbol.LessThanSign).SkipWhitespaces();
            internal static readonly Parser<char, char> EqualsSign = Char(JassSymbol.EqualsSign).SkipWhitespaces();
            internal static readonly Parser<char, char> GreaterThanSign = Char(JassSymbol.GreaterThanSign).SkipWhitespaces();
            internal static readonly Parser<char, char> LeftSquareBracket = Char(JassSymbol.LeftSquareBracket).SkipWhitespaces();
            internal static readonly Parser<char, char> RightSquareBracket = Char(JassSymbol.RightSquareBracket).SkipWhitespaces();
            internal static readonly Parser<char, char> X = CIChar(JassSymbol.X);

            internal static readonly Parser<char, string> EqualsEquals = String($"{JassSymbol.EqualsSign}{JassSymbol.EqualsSign}").SkipWhitespaces();
            internal static readonly Parser<char, string> LessOrEquals = Try(String($"{JassSymbol.LessThanSign}{JassSymbol.EqualsSign}")).SkipWhitespaces();
            internal static readonly Parser<char, string> GreaterOrEquals = Try(String($"{JassSymbol.GreaterThanSign}{JassSymbol.EqualsSign}")).SkipWhitespaces();
            internal static readonly Parser<char, string> NotEquals = String($"{JassSymbol.ExclamationMark}{JassSymbol.EqualsSign}").SkipWhitespaces();
        }
    }
}