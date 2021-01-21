// ------------------------------------------------------------------------------
// <copyright file="JassElseCustomScriptAction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassElseCustomScriptAction : ICustomScriptAction
    {
        public static readonly JassElseCustomScriptAction Value = new JassElseCustomScriptAction();

        private JassElseCustomScriptAction()
        {
        }

        public bool Equals(ICustomScriptAction? other) => other is JassElseCustomScriptAction;

        public override string ToString() => JassKeyword.Else;
    }
}