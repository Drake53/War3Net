// ------------------------------------------------------------------------------
// <copyright file="VJassGlobalDeclarationElseIfClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassGlobalDeclarationElseIfClauseSyntax : IEquatable<VJassGlobalDeclarationElseIfClauseSyntax>
    {
        public VJassGlobalDeclarationElseIfClauseSyntax(
            IExpressionSyntax condition,
            VJassGlobalDeclarationListSyntax body)
        {
            Condition = condition;
            Body = body;
        }

        public IExpressionSyntax Condition { get; }

        public VJassGlobalDeclarationListSyntax Body { get; }

        public bool Equals(VJassGlobalDeclarationElseIfClauseSyntax? other)
        {
            return other is not null
                && Condition.Equals(other.Condition)
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{VJassKeyword.ElseIf} {Condition} {VJassKeyword.Then} [{Body.Globals.Length}]";
    }
}