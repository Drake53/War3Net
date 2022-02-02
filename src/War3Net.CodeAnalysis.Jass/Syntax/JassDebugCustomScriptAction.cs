// ------------------------------------------------------------------------------
// <copyright file="JassDebugCustomScriptAction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassDebugCustomScriptAction : IStatementLineSyntax
    {
        public static readonly JassDebugCustomScriptAction DebugLoop = new JassDebugCustomScriptAction(JassLoopCustomScriptAction.Value);

        public JassDebugCustomScriptAction(IStatementLineSyntax action)
        {
            Action = action;
        }

        public IStatementLineSyntax Action { get; init; }

        public bool Equals(IStatementLineSyntax? other)
        {
            return other is JassDebugCustomScriptAction debug
                && Action.Equals(debug.Action);
        }

        public override string ToString() => $"{JassKeyword.Debug} {Action}";
    }
}