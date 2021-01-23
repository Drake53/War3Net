// ------------------------------------------------------------------------------
// <copyright file="ElseIfCustomScriptActionParser.cs" company="Drake53">
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
        internal static Parser<char, ICustomScriptAction> GetElseIfCustomScriptActionParser(Parser<char, IExpressionSyntax> expressionParser)
        {
            return Keyword.ElseIf.Then(expressionParser).Before(Keyword.Then)
                .Select<ICustomScriptAction>(expression => new JassElseIfCustomScriptAction(expression));
        }
    }
}