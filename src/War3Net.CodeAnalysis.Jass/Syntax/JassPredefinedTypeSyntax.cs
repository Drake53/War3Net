// ------------------------------------------------------------------------------
// <copyright file="JassPredefinedTypeSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassPredefinedTypeSyntax : JassTypeSyntax
    {
        public static readonly JassPredefinedTypeSyntax Boolean = new(new JassSyntaxToken(JassSyntaxKind.BooleanKeyword, JassKeyword.Boolean, JassSyntaxTriviaList.SingleSpace));
        public static readonly JassPredefinedTypeSyntax Code = new(new JassSyntaxToken(JassSyntaxKind.CodeKeyword, JassKeyword.Code, JassSyntaxTriviaList.SingleSpace));
        public static readonly JassPredefinedTypeSyntax Handle = new(new JassSyntaxToken(JassSyntaxKind.HandleKeyword, JassKeyword.Handle, JassSyntaxTriviaList.SingleSpace));
        public static readonly JassPredefinedTypeSyntax Integer = new(new JassSyntaxToken(JassSyntaxKind.IntegerKeyword, JassKeyword.Integer, JassSyntaxTriviaList.SingleSpace));
        public static readonly JassPredefinedTypeSyntax Nothing = new(new JassSyntaxToken(JassSyntaxKind.NothingKeyword, JassKeyword.Nothing, JassSyntaxTriviaList.SingleSpace));
        public static readonly JassPredefinedTypeSyntax Real = new(new JassSyntaxToken(JassSyntaxKind.RealKeyword, JassKeyword.Real, JassSyntaxTriviaList.SingleSpace));
        public static readonly JassPredefinedTypeSyntax String = new(new JassSyntaxToken(JassSyntaxKind.StringKeyword, JassKeyword.String, JassSyntaxTriviaList.SingleSpace));

        internal JassPredefinedTypeSyntax(
            JassSyntaxToken token)
        {
            Token = token;
        }

        public JassSyntaxToken Token { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.PredefinedType;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassPredefinedTypeSyntax predefinedType
                && Token.IsEquivalentTo(predefinedType.Token);
        }

        public override void WriteTo(TextWriter writer)
        {
            Token.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return Token;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return Token;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return Token;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return Token;
        }

        public override string ToString() => Token.ToString();

        public override JassSyntaxToken GetFirstToken() => Token;

        public override JassSyntaxToken GetLastToken() => Token;

        protected internal override JassPredefinedTypeSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassPredefinedTypeSyntax(newToken);
        }

        protected internal override JassPredefinedTypeSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassPredefinedTypeSyntax(newToken);
        }
    }
}