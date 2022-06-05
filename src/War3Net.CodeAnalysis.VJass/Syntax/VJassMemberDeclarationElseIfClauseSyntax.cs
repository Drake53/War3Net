// ------------------------------------------------------------------------------
// <copyright file="VJassMemberDeclarationElseIfClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassMemberDeclarationElseIfClauseSyntax : IEquatable<VJassMemberDeclarationElseIfClauseSyntax>
    {
        public VJassMemberDeclarationElseIfClauseSyntax(IExpressionSyntax condition, VJassMemberDeclarationListSyntax body)
        {
            Condition = condition;
            Body = body;
        }

        public IExpressionSyntax Condition { get; }

        public VJassMemberDeclarationListSyntax Body { get; }

        public bool Equals(VJassMemberDeclarationElseIfClauseSyntax? other)
        {
            return other is not null
                && Condition.Equals(other.Condition)
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{VJassKeyword.ElseIf} {Condition} {VJassKeyword.Then} [{Body.Declarations.Length}]";
    }
}