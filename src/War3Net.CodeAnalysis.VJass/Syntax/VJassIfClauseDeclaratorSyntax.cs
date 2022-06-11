// ------------------------------------------------------------------------------
// <copyright file="VJassIfClauseDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassIfClauseDeclaratorSyntax : VJassSyntaxNode
    {
        internal VJassIfClauseDeclaratorSyntax(
            VJassSyntaxToken ifToken,
            VJassExpressionSyntax condition,
            VJassSyntaxToken thenToken)
        {
            IfToken = ifToken;
            Condition = condition;
            ThenToken = thenToken;
        }

        public VJassSyntaxToken IfToken { get; }

        public VJassExpressionSyntax Condition { get; }

        public VJassSyntaxToken ThenToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassIfClauseDeclaratorSyntax ifClauseDeclarator
                && Condition.IsEquivalentTo(ifClauseDeclarator.Condition);
        }

        public override void WriteTo(TextWriter writer)
        {
            IfToken.WriteTo(writer);
            Condition.WriteTo(writer);
            ThenToken.WriteTo(writer);
        }

        public override string ToString() => $"{IfToken} {Condition} {ThenToken}";

        public override VJassSyntaxToken GetFirstToken() => IfToken;

        public override VJassSyntaxToken GetLastToken() => ThenToken;

        protected internal override VJassIfClauseDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassIfClauseDeclaratorSyntax(
                newToken,
                Condition,
                ThenToken);
        }

        protected internal override VJassIfClauseDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassIfClauseDeclaratorSyntax(
                IfToken,
                Condition,
                newToken);
        }
    }
}