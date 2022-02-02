// ------------------------------------------------------------------------------
// <copyright file="JassEndGlobalsCustomScriptAction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassEndGlobalsCustomScriptAction : IGlobalLineSyntax
    {
        public static readonly JassEndGlobalsCustomScriptAction Value = new JassEndGlobalsCustomScriptAction();

        private JassEndGlobalsCustomScriptAction()
        {
        }

        public bool Equals(IGlobalLineSyntax? other) => other is JassEndGlobalsCustomScriptAction;

        public override string ToString() => JassKeyword.EndGlobals;
    }
}