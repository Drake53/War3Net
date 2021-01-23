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
        internal static Parser<char, ICustomScriptAction> GetDebugCustomScriptActionParser(
            Parser<char, ICustomScriptAction> setCustomScriptActionParser,
            Parser<char, ICustomScriptAction> callCustomScriptActionParser,
            Parser<char, ICustomScriptAction> ifCustomScriptActionParser,
            Parser<char, ICustomScriptAction> loopCustomScriptActionParser)
        {
            return Keyword.Debug.Then(
                OneOf(
                    setCustomScriptActionParser,
                    callCustomScriptActionParser,
                    ifCustomScriptActionParser)
                .Select<ICustomScriptAction>(action => new JassDebugCustomScriptAction(action))
                .Or(loopCustomScriptActionParser.ThenReturn<ICustomScriptAction>(JassDebugCustomScriptAction.DebugLoop)));
        }
    }
}