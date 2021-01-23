// ------------------------------------------------------------------------------
// <copyright file="JassTypeSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassTypeSyntax : IEquatable<JassTypeSyntax>
    {
        public static readonly JassTypeSyntax Boolean = new JassTypeSyntax(new JassIdentifierNameSyntax(JassKeyword.Boolean));
        public static readonly JassTypeSyntax Code = new JassTypeSyntax(new JassIdentifierNameSyntax(JassKeyword.Code));
        public static readonly JassTypeSyntax Handle = new JassTypeSyntax(new JassIdentifierNameSyntax(JassKeyword.Handle));
        public static readonly JassTypeSyntax Integer = new JassTypeSyntax(new JassIdentifierNameSyntax(JassKeyword.Integer));
        public static readonly JassTypeSyntax Nothing = new JassTypeSyntax(new JassIdentifierNameSyntax(JassKeyword.Nothing));
        public static readonly JassTypeSyntax Real = new JassTypeSyntax(new JassIdentifierNameSyntax(JassKeyword.Real));
        public static readonly JassTypeSyntax String = new JassTypeSyntax(new JassIdentifierNameSyntax(JassKeyword.String));

        internal JassTypeSyntax(JassIdentifierNameSyntax typeName)
        {
            TypeName = typeName;
        }

        public JassIdentifierNameSyntax TypeName { get; init; }

        public bool Equals(JassTypeSyntax? other)
        {
            return other is not null
                && TypeName.Equals(other.TypeName);
        }

        public override string ToString() => TypeName.ToString();
    }
}