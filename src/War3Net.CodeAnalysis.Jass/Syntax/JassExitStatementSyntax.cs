// ------------------------------------------------------------------------------
// <copyright file="JassExitStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using OneOf;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassExitStatementSyntax : JassStatementSyntax
    {
        internal JassExitStatementSyntax(
            JassSyntaxToken exitWhenToken,
            JassExpressionSyntax condition)
        {
            ExitWhenToken = exitWhenToken;
            Condition = condition;
        }

        public JassSyntaxToken ExitWhenToken { get; }

        public JassExpressionSyntax Condition { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassExitStatementSyntax exitStatement
                && Condition.IsEquivalentTo(exitStatement.Condition);
        }

        public override void WriteTo(TextWriter writer)
        {
            ExitWhenToken.WriteTo(writer);
            Condition.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Condition;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return ExitWhenToken;
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetChildNodesAndTokens()
        {
            yield return ExitWhenToken;
            yield return Condition;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return Condition;
            foreach (var descendant in Condition.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return ExitWhenToken;

            foreach (var descendant in Condition.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetDescendantNodesAndTokens()
        {
            yield return ExitWhenToken;

            yield return Condition;
            foreach (var descendant in Condition.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{ExitWhenToken} {Condition}";

        public override JassSyntaxToken GetFirstToken() => ExitWhenToken;

        public override JassSyntaxToken GetLastToken() => Condition.GetLastToken();

        protected internal override JassExitStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassExitStatementSyntax(
                newToken,
                Condition);
        }

        protected internal override JassExitStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassExitStatementSyntax(
                ExitWhenToken,
                Condition.ReplaceLastToken(newToken));
        }
    }
}