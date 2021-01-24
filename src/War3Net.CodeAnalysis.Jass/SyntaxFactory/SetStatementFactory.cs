// ------------------------------------------------------------------------------
// <copyright file="SetStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassSetStatementSyntax SetStatement(string name, IExpressionSyntax value)
        {
            return new JassSetStatementSyntax(
                ParseIdentifierName(name),
                null,
                new JassEqualsValueClauseSyntax(value));
        }

        public static JassSetStatementSyntax SetStatement(string name, JassEqualsValueClauseSyntax value)
        {
            return new JassSetStatementSyntax(
                ParseIdentifierName(name),
                null,
                value);
        }

        public static JassSetStatementSyntax SetStatement(string name, IExpressionSyntax indexer, IExpressionSyntax value)
        {
            return new JassSetStatementSyntax(
                ParseIdentifierName(name),
                indexer,
                new JassEqualsValueClauseSyntax(value));
        }

        public static JassSetStatementSyntax SetStatement(string name, IExpressionSyntax indexer, JassEqualsValueClauseSyntax value)
        {
            return new JassSetStatementSyntax(
                ParseIdentifierName(name),
                indexer,
                value);
        }
    }
}