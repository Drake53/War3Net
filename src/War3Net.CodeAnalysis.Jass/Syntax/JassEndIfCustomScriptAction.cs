// ------------------------------------------------------------------------------
// <copyright file="JassEndIfCustomScriptAction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassEndIfCustomScriptAction : ICustomScriptAction
    {
        public static readonly JassEndIfCustomScriptAction Value = new JassEndIfCustomScriptAction();

        private JassEndIfCustomScriptAction()
        {
        }

        public bool Equals(ICustomScriptAction? other) => other is JassEndIfCustomScriptAction;

        public override string ToString() => JassKeyword.EndIf;
    }
}