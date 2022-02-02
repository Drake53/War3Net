// ------------------------------------------------------------------------------
// <copyright file="JassLoopCustomScriptAction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassLoopCustomScriptAction : IStatementLineSyntax
    {
        public static readonly JassLoopCustomScriptAction Value = new JassLoopCustomScriptAction();

        private JassLoopCustomScriptAction()
        {
        }

        public bool Equals(IStatementLineSyntax? other) => other is JassLoopCustomScriptAction;

        public override string ToString() => JassKeyword.Loop;
    }
}