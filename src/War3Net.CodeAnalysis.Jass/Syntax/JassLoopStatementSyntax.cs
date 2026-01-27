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
using System.Linq;

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

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.LoopStatement;

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

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitLoopStatement(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitLoopStatement(this);

        public JassLoopStatementSyntax Update(
            JassSyntaxToken loopToken,
            ImmutableArray<JassStatementSyntax> statements,
            JassSyntaxToken endLoopToken)
        {
            if (ReferenceEquals(LoopToken, loopToken) &&
                Statements.SequenceEqual(statements) &&
                ReferenceEquals(EndLoopToken, endLoopToken))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(loopToken, JassSyntaxKind.LoopKeyword);
            ThrowHelper.ThrowIfInvalidToken(endLoopToken, JassSyntaxKind.EndLoopKeyword);

            return new JassLoopStatementSyntax(loopToken, statements, endLoopToken);
        }

        public JassLoopStatementSyntax WithLoopToken(JassSyntaxToken loopToken) => Update(loopToken, Statements, EndLoopToken);

        public JassLoopStatementSyntax WithStatements(ImmutableArray<JassStatementSyntax> statements) => Update(LoopToken, statements, EndLoopToken);

        public JassLoopStatementSyntax WithEndLoopToken(JassSyntaxToken endLoopToken) => Update(LoopToken, Statements, endLoopToken);

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