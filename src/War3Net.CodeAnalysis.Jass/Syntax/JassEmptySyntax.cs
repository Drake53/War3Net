// ------------------------------------------------------------------------------
// <copyright file="JassEmptySyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassEmptySyntax : IDeclarationSyntax, IStatementSyntax, ICustomScriptAction
    {
        public static readonly JassEmptySyntax Value = new JassEmptySyntax();

        private JassEmptySyntax()
        {
        }

        public bool Equals(IDeclarationSyntax? other) => other is JassEmptySyntax;

        public bool Equals(IStatementSyntax? other) => other is JassEmptySyntax;

        public bool Equals(ICustomScriptAction? other) => other is JassEmptySyntax;

        public override string ToString() => string.Empty;
    }
}