// ------------------------------------------------------------------------------
// <copyright file="JassReturnStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassReturnStatementSyntax : JassStatementSyntax
    {
        public static readonly JassReturnStatementSyntax Empty = new(
            new JassSyntaxToken(JassSyntaxKind.ReturnKeyword, JassKeyword.Return, JassSyntaxTriviaList.Empty),
            null);

        internal JassReturnStatementSyntax(
            JassSyntaxToken returnToken,
            JassExpressionSyntax? expression)
        {
            ReturnToken = returnToken;
            Expression = expression;
        }

        public JassSyntaxToken ReturnToken { get; }

        public JassExpressionSyntax? Expression { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.ReturnStatement;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassReturnStatementSyntax returnStatement
                && Expression.NullableEquivalentTo(returnStatement.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            ReturnToken.WriteTo(writer);
            Expression?.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            if (Expression is not null)
            {
                yield return Expression;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return ReturnToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return ReturnToken;

            if (Expression is not null)
            {
                yield return Expression;
            }
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            if (Expression is not null)
            {
                yield return Expression;
                foreach (var descendant in Expression.GetDescendantNodes())
                {
                    yield return descendant;
                }
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return ReturnToken;

            if (Expression is not null)
            {
                foreach (var descendant in Expression.GetDescendantTokens())
                {
                    yield return descendant;
                }
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return ReturnToken;

            if (Expression is not null)
            {
                yield return Expression;
                foreach (var descendant in Expression.GetDescendantNodesAndTokens())
                {
                    yield return descendant;
                }
            }
        }

        public override string ToString() => $"{ReturnToken}{Expression.OptionalPrefixed()}";

        public override JassSyntaxToken GetFirstToken() => ReturnToken;

        public override JassSyntaxToken GetLastToken() => Expression?.GetLastToken() ?? ReturnToken;

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitReturnStatement(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitReturnStatement(this);

        public JassReturnStatementSyntax Update(
            JassSyntaxToken returnToken,
            JassExpressionSyntax? expression)
        {
            if (ReferenceEquals(ReturnToken, returnToken) &&
                ReferenceEquals(Expression, expression))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(returnToken, JassSyntaxKind.ReturnKeyword);

            return new JassReturnStatementSyntax(returnToken, expression);
        }

        public JassReturnStatementSyntax WithReturnToken(JassSyntaxToken returnToken) => Update(returnToken, Expression);

        public JassReturnStatementSyntax WithExpression(JassExpressionSyntax? expression) => Update(ReturnToken, expression);

        protected internal override JassReturnStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassReturnStatementSyntax(
                newToken,
                Expression);
        }

        protected internal override JassReturnStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            if (Expression is not null)
            {
                return new JassReturnStatementSyntax(
                    ReturnToken,
                    Expression.ReplaceLastToken(newToken));
            }

            return new JassReturnStatementSyntax(
                newToken,
                null);
        }
    }
}