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
            Parser<char, IExpressionSyntax> expressionParser)
        {
            var setParser = setCustomScriptActionParser.Cast<IStatementLineSyntax>();
            var callParser = callCustomScriptActionParser.Cast<IStatementLineSyntax>();
            var ifCustomScriptActionParser = GetIfCustomScriptActionParser(expressionParser);
            var loopCustomScriptActionParser = GetLoopCustomScriptActionParser();

            return OneOf(
                emptyLineParser.Cast<IStatementLineSyntax>(),
                commentParser.Cast<IStatementLineSyntax>(),
                localVariableDeclarationStatementParser.Cast<IStatementLineSyntax>(),
                setParser,
                callParser,
                ifCustomScriptActionParser,
                GetElseIfCustomScriptActionParser(expressionParser),
                GetElseCustomScriptActionParser(),
                GetEndIfCustomScriptActionParser(),
                loopCustomScriptActionParser,
                GetEndLoopCustomScriptActionParser(),
                exitStatementParser.Cast<IStatementLineSyntax>(),
                returnStatementParser.Cast<IStatementLineSyntax>(),
                GetEndFunctionCustomScriptActionParser(),
                GetDebugCustomScriptActionParser(setParser, callParser, ifCustomScriptActionParser, loopCustomScriptActionParser));
        }
    }
}