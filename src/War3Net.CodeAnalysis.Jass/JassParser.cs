// ------------------------------------------------------------------------------
// <copyright file="JassParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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
        private readonly Parser<char, BinaryOperatorType> _binaryOperatorParser;
        private readonly Parser<char, JassCommentSyntax> _commentParser;
        private readonly Parser<char, JassCompilationUnitSyntax> _compilationUnitParser;
        private readonly Parser<char, ITopLevelDeclarationSyntax> _declarationParser;
        private readonly Parser<char, IDeclarationLineSyntax> _declarationLineParser;
        private readonly Parser<char, IExpressionSyntax> _expressionParser;
        private readonly Parser<char, JassFunctionDeclarationSyntax> _functionDeclarationParser;
        private readonly Parser<char, IGlobalDeclarationSyntax> _globalDeclarationParser;
        private readonly Parser<char, IGlobalLineSyntax> _globalLineParser;
        private readonly Parser<char, JassIdentifierNameSyntax> _identifierNameParser;
        private readonly Parser<char, JassParameterListSyntax> _parameterListParser;
        private readonly Parser<char, IStatementSyntax> _statementParser;
        private readonly Parser<char, IStatementLineSyntax> _statementLineParser;
        private readonly Parser<char, JassTypeSyntax> _typeParser;
        private readonly Parser<char, UnaryOperatorType> _unaryOperatorParser;

        private JassParser()
        {
            var whitespaceParser = GetWhitespaceParser();
            var identifierNameParser = GetIdentifierNameParser(whitespaceParser);
            var typeParser = GetTypeParser(identifierNameParser, whitespaceParser);
            var expressionParser = GetExpressionParser(whitespaceParser, identifierNameParser);
            var equalsValueClauseParser = GetEqualsValueClauseParser(whitespaceParser, expressionParser);

            var argumentListParser = GetArgumentListParser(whitespaceParser, expressionParser);
            var parameterListParser = GetParameterListParser(whitespaceParser, GetParameterParser(identifierNameParser, typeParser));
            var functionDeclaratorParser = GetFunctionDeclaratorParser(identifierNameParser, parameterListParser, typeParser, whitespaceParser);
            var variableDeclaratorParser = GetVariableDeclaratorParser(equalsValueClauseParser, identifierNameParser, typeParser, whitespaceParser);

            var commentStringParser = GetCommentStringParser();
            var newLineParser = GetNewLineParser(whitespaceParser);
            var endOfLineParser = GetEndOfLineParser(commentStringParser, newLineParser);
            var emptyParser = GetEmptyParser();
            var emptyLineParser = GetEmptyLineParser();
            var commentParser = GetCommentParser(commentStringParser);

            var setStatementParser = GetSetStatementParser(whitespaceParser, expressionParser, equalsValueClauseParser, identifierNameParser);
            var callStatementParser = GetCallStatementParser(whitespaceParser, argumentListParser, identifierNameParser);
            var exitStatementParser = GetExitStatementParser(expressionParser, whitespaceParser);
            var localVariableDeclarationStatementParser = GetLocalVariableDeclarationStatementParser(variableDeclaratorParser, whitespaceParser);
            var returnStatementParser = GetReturnStatementParser(expressionParser, whitespaceParser);

            var statementParser = GetStatementParser(
                emptyParser,
                commentParser,
                localVariableDeclarationStatementParser,
                exitStatementParser,
                returnStatementParser,
                setStatementParser,
                callStatementParser,
                expressionParser,
                whitespaceParser,
                endOfLineParser);

            var statementLineParser = GetStatementLineParser(
                emptyLineParser,
                commentParser,
                localVariableDeclarationStatementParser,
                setStatementParser,
                callStatementParser,
                exitStatementParser,
                returnStatementParser,
                expressionParser,
                whitespaceParser);

            var constantDeclarationParser = GetConstantDeclarationParser(
                equalsValueClauseParser,
                identifierNameParser,
                typeParser,
                whitespaceParser);

            var variableDeclarationParser = GetVariableDeclarationParser(
                equalsValueClauseParser,
                identifierNameParser,
                typeParser,
                whitespaceParser);

            var globalDeclarationParser = GetGlobalDeclarationParser(
                emptyParser,
                commentParser,
                constantDeclarationParser,
                variableDeclarationParser);

            var globalLineParser = GetGlobalLineParser(
                emptyLineParser,
                commentParser,
                constantDeclarationParser,
                variableDeclarationParser,
                whitespaceParser);

            var statementListParser = GetStatementListParser(
                statementParser,
                endOfLineParser);

            var functionDeclarationParser = GetFunctionDeclarationParser(
                functionDeclaratorParser,
                statementListParser,
                whitespaceParser,
                endOfLineParser);

            var typeDeclarationParser = GetTypeDeclarationParser(
                identifierNameParser,
                typeParser,
                whitespaceParser);

            var nativeFunctionDeclarationParser = GetNativeFunctionDeclarationParser(
                functionDeclaratorParser,
                whitespaceParser);

            var declarationParser = GetDeclarationParser(
                emptyParser,
                commentParser,
                typeDeclarationParser,
                nativeFunctionDeclarationParser,
                functionDeclarationParser,
                globalDeclarationParser.Before(endOfLineParser),
                whitespaceParser,
                endOfLineParser);

            var declarationLineParser = GetDeclarationLineParser(
                emptyLineParser,
                commentParser,
                typeDeclarationParser,
                nativeFunctionDeclarationParser,
                functionDeclaratorParser,
                whitespaceParser);

            var compilationUnitParser = GetCompilationUnitParser(
                declarationParser,
                commentStringParser,
                newLineParser);

            Parser<char, T> Create<T>(Parser<char, T> parser) => whitespaceParser.Then(parser).Before(End);

            _argumentListParser = Create(argumentListParser);
            _binaryOperatorParser = Create(GetBinaryOperatorParser(whitespaceParser));
            _commentParser = Create(commentParser);
            _compilationUnitParser = Create(compilationUnitParser);
            _declarationParser = Create(declarationParser);
            _declarationLineParser = Create(declarationLineParser);
            _expressionParser = Create(expressionParser);
            _functionDeclarationParser = Create(functionDeclarationParser);
            _globalDeclarationParser = Create(globalDeclarationParser);
            _globalLineParser = Create(globalLineParser);
            _identifierNameParser = Create(identifierNameParser);
            _parameterListParser = Create(parameterListParser);
            _statementParser = Create(statementParser);
            _statementLineParser = Create(statementLineParser.Before(commentStringParser.Optional()));
            _typeParser = Create(typeParser);
            _unaryOperatorParser = Create(GetUnaryOperatorParser(whitespaceParser));
        }

        internal static JassParser Instance => _parser;

        internal Parser<char, JassArgumentListSyntax> ArgumentListParser => _argumentListParser;

        internal Parser<char, BinaryOperatorType> BinaryOperatorParser => _binaryOperatorParser;

        internal Parser<char, JassCommentSyntax> CommentParser => _commentParser;

        internal Parser<char, JassCompilationUnitSyntax> CompilationUnitParser => _compilationUnitParser;

        internal Parser<char, ITopLevelDeclarationSyntax> DeclarationParser => _declarationParser;

        internal Parser<char, IDeclarationLineSyntax> DeclarationLineParser => _declarationLineParser;

        internal Parser<char, IExpressionSyntax> ExpressionParser => _expressionParser;

        internal Parser<char, JassFunctionDeclarationSyntax> FunctionDeclarationParser => _functionDeclarationParser;

        internal Parser<char, IGlobalDeclarationSyntax> GlobalDeclarationParser => _globalDeclarationParser;

        internal Parser<char, IGlobalLineSyntax> GlobalLineParser => _globalLineParser;

        internal Parser<char, JassIdentifierNameSyntax> IdentifierNameParser => _identifierNameParser;

        internal Parser<char, JassParameterListSyntax> ParameterListParser => _parameterListParser;

        internal Parser<char, IStatementSyntax> StatementParser => _statementParser;

        internal Parser<char, IStatementLineSyntax> StatementLineParser => _statementLineParser;

        internal Parser<char, JassTypeSyntax> TypeParser => _typeParser;

        internal Parser<char, UnaryOperatorType> UnaryOperatorParser => _unaryOperatorParser;

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
            internal static readonly Parser<char, char> LineFeed = Char(JassSymbol.LineFeed);
            internal static readonly Parser<char, char> CarriageReturn = Char(JassSymbol.CarriageReturn);
            internal static readonly Parser<char, char> QuotationMark = Char(JassSymbol.QuotationMark);
            internal static readonly Parser<char, char> DollarSign = Char(JassSymbol.DollarSign);
            internal static readonly Parser<char, char> Apostrophe = Char(JassSymbol.Apostrophe);
            internal static readonly Parser<char, char> LeftParenthesis = Char(JassSymbol.LeftParenthesis);
            internal static readonly Parser<char, char> RightParenthesis = Char(JassSymbol.RightParenthesis);
            internal static readonly Parser<char, char> Asterisk = Char(JassSymbol.Asterisk);
            internal static readonly Parser<char, char> PlusSign = Char(JassSymbol.PlusSign);
            internal static readonly Parser<char, char> Comma = Char(JassSymbol.Comma);
            internal static readonly Parser<char, char> MinusSign = Char(JassSymbol.MinusSign);
            internal static readonly Parser<char, char> FullStop = Char(JassSymbol.FullStop);
            internal static readonly Parser<char, char> Slash = Char(JassSymbol.Slash);
            internal static readonly Parser<char, char> Zero = Char(JassSymbol.Zero);
            internal static readonly Parser<char, char> LessThanSign = Char(JassSymbol.LessThanSign);
            internal static readonly Parser<char, char> EqualsSign = Char(JassSymbol.EqualsSign);
            internal static readonly Parser<char, char> GreaterThanSign = Char(JassSymbol.GreaterThanSign);
            internal static readonly Parser<char, char> LeftSquareBracket = Char(JassSymbol.LeftSquareBracket);
            internal static readonly Parser<char, char> RightSquareBracket = Char(JassSymbol.RightSquareBracket);
            internal static readonly Parser<char, char> X = CIChar(JassSymbol.X);

            internal static readonly Parser<char, string> EqualsEquals = String($"{JassSymbol.EqualsSign}{JassSymbol.EqualsSign}");
            internal static readonly Parser<char, string> LessOrEquals = String($"{JassSymbol.LessThanSign}{JassSymbol.EqualsSign}");
            internal static readonly Parser<char, string> GreaterOrEquals = String($"{JassSymbol.GreaterThanSign}{JassSymbol.EqualsSign}");
            internal static readonly Parser<char, string> NotEquals = String($"{JassSymbol.ExclamationMark}{JassSymbol.EqualsSign}");
        }
    }
}