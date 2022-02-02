// ------------------------------------------------------------------------------
// <copyright file="EndGlobalsCustomScriptActionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, IGlobalLineSyntax> GetEndGlobalsCustomScriptActionParser()
        {
            return Keyword.EndGlobals.ThenReturn<IGlobalLineSyntax>(JassEndGlobalsCustomScriptAction.Value);
        }
    }
}