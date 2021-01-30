// ------------------------------------------------------------------------------
// <copyright file="CustomScriptActionParser.cs" company="Drake53">
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
        internal static Parser<char, ICustomScriptAction> GetCustomScriptActionParser(
            Parser<char, ICustomScriptAction> setCustomScriptActionParser,
            Parser<char, ICustomScriptAction> callCustomScriptActionParser,
            Parser<char, ICustomScriptAction> ifCustomScriptActionParser,
            Parser<char, ICustomScriptAction> loopCustomScriptActionParser,
            Parser<char, IExpressionSyntax> expressionParser,
            Parser<char, IVariableDeclaratorSyntax> variableDeclaratorParser,
            Parser<char, JassFunctionDeclaratorSyntax> functionDeclaratorParser,
            Parser<char, string> commentParser)
        {
            return OneOf(
                GetEmptyStatementParser().Cast<ICustomScriptAction>(),
                GetCommentStatementParser(commentParser).Cast<ICustomScriptAction>(),
                GetLocalVariableDeclarationStatementParser(variableDeclaratorParser).Cast<ICustomScriptAction>(),
                setCustomScriptActionParser,
                callCustomScriptActionParser,
                ifCustomScriptActionParser,
                GetElseIfCustomScriptActionParser(expressionParser),
                GetElseCustomScriptActionParser(),
                GetEndIfCustomScriptActionParser(),
                loopCustomScriptActionParser,
                GetEndLoopCustomScriptActionParser(),
                GetExitStatementParser(expressionParser).Cast<ICustomScriptAction>(),
                GetReturnStatementParser(expressionParser).Cast<ICustomScriptAction>(),
                GetFunctionCustomScriptActionParser(functionDeclaratorParser),
                GetEndFunctionCustomScriptActionParser(),
                GetDebugCustomScriptActionParser(setCustomScriptActionParser, callCustomScriptActionParser, ifCustomScriptActionParser, loopCustomScriptActionParser));
        }
    }
}