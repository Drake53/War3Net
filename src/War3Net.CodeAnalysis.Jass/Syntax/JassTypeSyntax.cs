// ------------------------------------------------------------------------------
// <copyright file="JassTypeSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassTypeSyntax : JassSyntaxNode
    {
        public static readonly JassTypeSyntax Boolean = new(new(new JassSyntaxToken(JassSyntaxKind.BooleanKeyword, JassKeyword.Boolean, JassSyntaxTriviaList.SingleSpace)));
        public static readonly JassTypeSyntax Code = new(new(new JassSyntaxToken(JassSyntaxKind.CodeKeyword, JassKeyword.Code, JassSyntaxTriviaList.SingleSpace)));
        public static readonly JassTypeSyntax Handle = new(new(new JassSyntaxToken(JassSyntaxKind.HandleKeyword, JassKeyword.Handle, JassSyntaxTriviaList.SingleSpace)));
        public static readonly JassTypeSyntax Integer = new(new(new JassSyntaxToken(JassSyntaxKind.IntegerKeyword, JassKeyword.Integer, JassSyntaxTriviaList.SingleSpace)));
        public static readonly JassTypeSyntax Nothing = new(new(new JassSyntaxToken(JassSyntaxKind.NothingKeyword, JassKeyword.Nothing, JassSyntaxTriviaList.SingleSpace)));
        public static readonly JassTypeSyntax Real = new(new(new JassSyntaxToken(JassSyntaxKind.RealKeyword, JassKeyword.Real, JassSyntaxTriviaList.SingleSpace)));
        public static readonly JassTypeSyntax String = new(new(new JassSyntaxToken(JassSyntaxKind.StringKeyword, JassKeyword.String, JassSyntaxTriviaList.SingleSpace)));

        internal JassTypeSyntax(
            JassIdentifierNameSyntax typeName)
        {
            TypeName = typeName;
        }

        public JassIdentifierNameSyntax TypeName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassTypeSyntax type
                && TypeName.IsEquivalentTo(type.TypeName);
        }

        public override void WriteTo(TextWriter writer)
        {
            TypeName.WriteTo(writer);
        }

        public override string ToString() => TypeName.ToString();

        public override JassSyntaxToken GetFirstToken() => TypeName.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => TypeName.GetLastToken();

        protected internal override JassTypeSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassTypeSyntax(TypeName.ReplaceFirstToken(newToken));
        }

        protected internal override JassTypeSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassTypeSyntax(TypeName.ReplaceLastToken(newToken));
        }
    }
}