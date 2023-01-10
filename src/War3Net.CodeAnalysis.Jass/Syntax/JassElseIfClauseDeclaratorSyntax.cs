// ------------------------------------------------------------------------------
// <copyright file="JassElseIfClauseDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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