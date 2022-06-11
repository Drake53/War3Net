// ------------------------------------------------------------------------------
// <copyright file="VJassElseIfClauseDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassElseIfClauseDeclaratorSyntax : VJassSyntaxNode
    {
        internal VJassElseIfClauseDeclaratorSyntax(
            VJassSyntaxToken elseIfToken,
            VJassExpressionSyntax condition,
            VJassSyntaxToken thenToken)
        {
            ElseIfToken = elseIfToken;
            Condition = condition;
            ThenToken = thenToken;
        }

        public VJassSyntaxToken ElseIfToken { get; }

        public VJassExpressionSyntax Condition { get; }

        public VJassSyntaxToken ThenToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator
                && Condition.IsEquivalentTo(elseIfClauseDeclarator.Condition);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseIfToken.WriteTo(writer);
            Condition.WriteTo(writer);
            ThenToken.WriteTo(writer);
        }

        public override string ToString() => $"{ElseIfToken} {Condition} {ThenToken}";

        public override VJassSyntaxToken GetFirstToken() => ElseIfToken;

        public override VJassSyntaxToken GetLastToken() => ThenToken;

        protected internal override VJassElseIfClauseDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassElseIfClauseDeclaratorSyntax(
                newToken,
                Condition,
                ThenToken);
        }

        protected internal override VJassElseIfClauseDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassElseIfClauseDeclaratorSyntax(
                ElseIfToken,
                Condition,
                newToken);
        }
    }
}