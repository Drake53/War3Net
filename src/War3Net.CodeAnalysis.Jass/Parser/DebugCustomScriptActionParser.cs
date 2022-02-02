// ------------------------------------------------------------------------------
// <copyright file="DebugCustomScriptActionParser.cs" company="Drake53">
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
        internal static Parser<char, IStatementLineSyntax> GetDebugCustomScriptActionParser(
            Parser<char, IStatementLineSyntax> setCustomScriptActionParser,
            Parser<char, IStatementLineSyntax> callCustomScriptActionParser,
            Parser<char, IStatementLineSyntax> ifCustomScriptActionParser,
            Parser<char, IStatementLineSyntax> loopCustomScriptActionParser)
        {
            return Keyword.Debug.Then(
                OneOf(
                    setCustomScriptActionParser,
                    callCustomScriptActionParser,
                    ifCustomScriptActionParser)
                .Select<IStatementLineSyntax>(action => new JassDebugCustomScriptAction(action))
                .Or(loopCustomScriptActionParser.ThenReturn<IStatementLineSyntax>(JassDebugCustomScriptAction.DebugLoop)));
        }
    }
}