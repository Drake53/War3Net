// ------------------------------------------------------------------------------
// <copyright file="JassElseIfClauseDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassElseIfClauseDeclaratorSyntax : JassSyntaxNode
    {
        internal JassElseIfClauseDeclaratorSyntax(
            JassSyntaxToken elseIfToken,
            JassExpressionSyntax condition,
            JassSyntaxToken thenToken)
        {
            ElseIfToken = elseIfToken;
            Condition = condition;
            ThenToken = thenToken;
        }

        public JassSyntaxToken ElseIfToken { get; }

        public JassExpressionSyntax Condition { get; }

        public JassSyntaxToken ThenToken { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.ElseIfClauseDeclarator;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator
                && Condition.IsEquivalentTo(elseIfClauseDeclarator.Condition);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseIfToken.WriteTo(writer);
            Condition.WriteTo(writer);
            ThenToken.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Condition;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return ElseIfToken;
            yield return ThenToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return ElseIfToken;
            yield return Condition;
            yield return ThenToken;
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
            yield return ElseIfToken;

            foreach (var descendant in Condition.GetDescendantTokens())
            {
                yield return descendant;
            }

            yield return ThenToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return ElseIfToken;

            yield return Condition;
            foreach (var descendant in Condition.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return ThenToken;
        }

        public override string ToString() => $"{ElseIfToken} {Condition} {ThenToken}";

        public override JassSyntaxToken GetFirstToken() => ElseIfToken;

        public override JassSyntaxToken GetLastToken() => ThenToken;

        protected internal override JassElseIfClauseDeclaratorSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassElseIfClauseDeclaratorSyntax(
                newToken,
                Condition,
                ThenToken);
        }

        protected internal override JassElseIfClauseDeclaratorSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassElseIfClauseDeclaratorSyntax(
                ElseIfToken,
                Condition,
                newToken);
        }
    }
}