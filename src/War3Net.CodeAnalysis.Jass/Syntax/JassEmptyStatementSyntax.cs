// ------------------------------------------------------------------------------
// <copyright file="JassEmptyStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassEmptyStatementSyntax : IStatementSyntax, ICustomScriptAction
    {
        public static readonly JassEmptyStatementSyntax Value = new JassEmptyStatementSyntax();

        private JassEmptyStatementSyntax()
        {
        }

        public bool Equals(IStatementSyntax? other) => other is JassEmptyStatementSyntax;

        public bool Equals(ICustomScriptAction? other) => other is JassEmptyStatementSyntax;

        public override string ToString() => string.Empty;
    }
}