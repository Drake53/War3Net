// ------------------------------------------------------------------------------
// <copyright file="JassDebugCustomScriptAction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassDebugCustomScriptAction : ICustomScriptAction
    {
        public static readonly JassDebugCustomScriptAction DebugLoop = new JassDebugCustomScriptAction(JassLoopCustomScriptAction.Value);

        public JassDebugCustomScriptAction(ICustomScriptAction action)
        {
            Action = action;
        }

        public ICustomScriptAction Action { get; init; }

        public bool Equals(ICustomScriptAction? other)
        {
            return other is JassDebugCustomScriptAction debug
                && Action.Equals(debug.Action);
        }

        public override string ToString() => $"{JassKeyword.Debug} {Action}";
    }
}