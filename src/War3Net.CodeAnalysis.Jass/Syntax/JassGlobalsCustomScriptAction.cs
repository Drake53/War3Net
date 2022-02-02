// ------------------------------------------------------------------------------
// <copyright file="JassGlobalsCustomScriptAction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassGlobalsCustomScriptAction : IDeclarationLineSyntax
    {
        public static readonly JassGlobalsCustomScriptAction Value = new JassGlobalsCustomScriptAction();

        private JassGlobalsCustomScriptAction()
        {
        }

        public bool Equals(IDeclarationLineSyntax? other) => other is JassGlobalsCustomScriptAction;

        public override string ToString() => JassKeyword.Globals;
    }
}