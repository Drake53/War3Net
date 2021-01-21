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
        public static readonly JassTypeSyntax Boolean = new JassTypeSyntax(JassKeyword.Boolean);
        public static readonly JassTypeSyntax Code = new JassTypeSyntax(JassKeyword.Code);
        public static readonly JassTypeSyntax Handle = new JassTypeSyntax(JassKeyword.Handle);
        public static readonly JassTypeSyntax Integer = new JassTypeSyntax(JassKeyword.Integer);
        public static readonly JassTypeSyntax Nothing = new JassTypeSyntax(JassKeyword.Nothing);
        public static readonly JassTypeSyntax Real = new JassTypeSyntax(JassKeyword.Real);
        public static readonly JassTypeSyntax String = new JassTypeSyntax(JassKeyword.String);

        internal JassTypeSyntax(string type)
        {
            Type = type;
        }

        public string Type { get; init; }

        public bool Equals(JassTypeSyntax? other)
        {
            return other is not null
                && string.Equals(Type, other.Type, StringComparison.Ordinal);
        }

        public override string ToString() => Type;
    }
}