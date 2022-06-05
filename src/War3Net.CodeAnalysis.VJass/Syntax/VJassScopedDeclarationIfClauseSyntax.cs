// ------------------------------------------------------------------------------
// <copyright file="VJassScopedDeclarationIfClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassScopedDeclarationIfClauseSyntax : IEquatable<VJassScopedDeclarationIfClauseSyntax>
    {
        public VJassScopedDeclarationIfClauseSyntax(IExpressionSyntax condition, VJassScopedDeclarationListSyntax body)
        {
            Condition = condition;
            Body = body;
        }

        public IExpressionSyntax Condition { get; }

        public VJassScopedDeclarationListSyntax Body { get; }

        public bool Equals(VJassScopedDeclarationIfClauseSyntax? other)
        {
            return other is not null
                && Condition.Equals(other.Condition)
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{VJassKeyword.If} {Condition} {VJassKeyword.Then} [{Body.Declarations.Length}]";
    }
}