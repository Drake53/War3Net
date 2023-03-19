// ------------------------------------------------------------------------------
// <copyright file="JassLoopStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassLoopStatementSyntax : JassStatementSyntax
    {
        internal JassLoopStatementSyntax(
            JassSyntaxToken loopToken,
            ImmutableArray<JassStatementSyntax> statements,
            JassSyntaxToken endLoopToken)
        {
            LoopToken = loopToken;
            Statements = statements;
            EndLoopToken = endLoopToken;
        }

        public JassSyntaxToken LoopToken { get; }

        public ImmutableArray<JassStatementSyntax> Statements { get; }

        public JassSyntaxToken EndLoopToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassLoopStatementSyntax loopStatement
                && Statements.IsEquivalentTo(loopStatement.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            LoopToken.WriteTo(writer);
            Statements.WriteTo(writer);
            EndLoopToken.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            return Statements;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return LoopToken;
            yield return EndLoopToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return LoopToken;

            foreach (var child in Statements)
            {
                yield return child;
            }

            yield return EndLoopToken;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            return Statements.GetDescendantNodes();
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return LoopToken;

            foreach (var descendant in Statements.GetDescendantTokens())
            {
                yield return descendant;
            }

            yield return EndLoopToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return LoopToken;

            foreach (var descendant in Statements.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return EndLoopToken;
        }

        public override string ToString() => $"{LoopToken} [...]";

        public override JassSyntaxToken GetFirstToken() => LoopToken;

        public override JassSyntaxToken GetLastToken() => EndLoopToken;

        protected internal override JassLoopStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassLoopStatementSyntax(
                newToken,
                Statements,
                EndLoopToken);
        }

        protected internal override JassLoopStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassLoopStatementSyntax(
                LoopToken,
                Statements,
                newToken);
        }
    }
}