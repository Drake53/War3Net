// ------------------------------------------------------------------------------
// <copyright file="JassElseIfCustomScriptAction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassElseIfCustomScriptAction : ICustomScriptAction
    {
        public JassElseIfCustomScriptAction(IExpressionSyntax condition)
        {
            Condition = condition;
        }

        public IExpressionSyntax Condition { get; init; }

        public bool Equals(ICustomScriptAction? other)
        {
            return other is JassElseIfCustomScriptAction elseIf
                && Condition.Equals(elseIf.Condition);
        }

        public override string ToString() => $"{JassKeyword.ElseIf} {Condition} {JassKeyword.Then}";
    }
}