// ------------------------------------------------------------------------------
// <copyright file="JassArrayReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassArrayReferenceExpressionSyntax : IExpressionSyntax
    {
        public JassArrayReferenceExpressionSyntax(JassIdentifierNameSyntax identifierName, IExpressionSyntax indexer)
        {
            IdentifierName = identifierName;
            Indexer = indexer;
        }

        public JassArrayReferenceExpressionSyntax(string name, IExpressionSyntax indexer)
        {
            IdentifierName = new JassIdentifierNameSyntax(name);
            Indexer = indexer;
        }

        public JassIdentifierNameSyntax IdentifierName { get; init; }

        public IExpressionSyntax Indexer { get; init; }

        public bool Equals(IExpressionSyntax? other) => other is JassArrayReferenceExpressionSyntax e && IdentifierName.Equals(e.IdentifierName) && Indexer.Equals(e.Indexer);

        public override string ToString() => $"{IdentifierName}[{Indexer}]";
    }
}