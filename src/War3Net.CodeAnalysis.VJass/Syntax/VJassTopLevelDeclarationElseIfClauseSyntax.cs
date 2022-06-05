// ------------------------------------------------------------------------------
// <copyright file="VJassTopLevelDeclarationElseIfClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassTopLevelDeclarationElseIfClauseSyntax : IEquatable<VJassTopLevelDeclarationElseIfClauseSyntax>
    {
        public VJassTopLevelDeclarationElseIfClauseSyntax(
            IExpressionSyntax condition,
            VJassTopLevelDeclarationListSyntax body)
        {
            Condition = condition;
            Body = body;
        }

        public IExpressionSyntax Condition { get; }

        public VJassTopLevelDeclarationListSyntax Body { get; }

        public bool Equals(VJassTopLevelDeclarationElseIfClauseSyntax? other)
        {
            return other is not null
                && Condition.Equals(other.Condition)
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{VJassKeyword.ElseIf} {Condition} {VJassKeyword.Then} [{Body.Declarations.Length}]";
    }
}