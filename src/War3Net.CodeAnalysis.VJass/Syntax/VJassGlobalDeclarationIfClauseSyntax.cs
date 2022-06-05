// ------------------------------------------------------------------------------
// <copyright file="VJassGlobalDeclarationIfClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassGlobalDeclarationIfClauseSyntax : IEquatable<VJassGlobalDeclarationIfClauseSyntax>
    {
        public VJassGlobalDeclarationIfClauseSyntax(
            IExpressionSyntax condition,
            VJassGlobalDeclarationListSyntax body)
        {
            Condition = condition;
            Body = body;
        }

        public IExpressionSyntax Condition { get; }

        public VJassGlobalDeclarationListSyntax Body { get; }

        public bool Equals(VJassGlobalDeclarationIfClauseSyntax? other)
        {
            return other is not null
                && Condition.Equals(other.Condition)
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{VJassKeyword.If} {Condition} {VJassKeyword.Then} [{Body.Globals.Length}]";
    }
}