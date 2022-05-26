// ------------------------------------------------------------------------------
// <copyright file="StatementLineParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, IStatementLineSyntax> GetStatementLineParser(
            Parser<char, JassEmptySyntax> emptyLineParser,
            Parser<char, JassCommentSyntax> commentParser,
            Parser<char, JassLocalVariableDeclarationStatementSyntax> localVariableDeclarationStatementParser,
            Parser<char, JassSetStatementSyntax> setCustomScriptActionParser,
            Parser<char, JassCallStatementSyntax> callCustomScriptActionParser,
            Parser<char, JassExitStatementSyntax> exitStatementParser,
            Parser<char, JassReturnStatementSyntax> returnStatementParser,
            Parser<char, IExpressionSyntax> expressionParser,
            Parser<char, Unit> whitespaceParser)
        {
            var setParser = setCustomScriptActionParser.Cast<IStatementLineSyntax>();
            var callParser = callCustomScriptActionParser.Cast<IStatementLineSyntax>();
            var ifCustomScriptActionParser = GetIfCustomScriptActionParser(expressionParser, whitespaceParser);
            var loopCustomScriptActionParser = GetLoopCustomScriptActionParser(whitespaceParser);

            return OneOf(
                emptyLineParser.Cast<IStatementLineSyntax>(),
                commentParser.Cast<IStatementLineSyntax>(),
                localVariableDeclarationStatementParser.Cast<IStatementLineSyntax>(),
                setParser,
                callParser,
                ifCustomScriptActionParser,
                GetElseIfCustomScriptActionParser(expressionParser, whitespaceParser),
                GetElseCustomScriptActionParser(whitespaceParser),
                GetEndIfCustomScriptActionParser(whitespaceParser),
                loopCustomScriptActionParser,
                GetEndLoopCustomScriptActionParser(whitespaceParser),
                exitStatementParser.Cast<IStatementLineSyntax>(),
                returnStatementParser.Cast<IStatementLineSyntax>(),
                GetEndFunctionCustomScriptActionParser(whitespaceParser),
                GetDebugCustomScriptActionParser(setParser, callParser, ifCustomScriptActionParser, loopCustomScriptActionParser, whitespaceParser));
        }
    }
}