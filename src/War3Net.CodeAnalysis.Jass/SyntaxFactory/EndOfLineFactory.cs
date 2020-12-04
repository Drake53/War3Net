// ------------------------------------------------------------------------------
// <copyright file="EndOfLineFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static EndOfLineSyntax Newline()
        {
            return Comment(null);
        }

        public static EndOfLineSyntax Comment()
        {
            return Comment(string.Empty);
        }

        public static EndOfLineSyntax Comment(string? comment)
        {
            if (comment is null)
            {
                return new EndOfLineSyntax(Token(SyntaxTokenType.NewlineSymbol));
            }

            if (string.IsNullOrEmpty(comment))
            {
                return new EndOfLineSyntax(new CommentSyntax(
                    Token(SyntaxTokenType.DoubleForwardSlash),
                    Empty(),
                    Token(SyntaxTokenType.NewlineSymbol)));
            }

            return new EndOfLineSyntax(new CommentSyntax(
                    Token(SyntaxTokenType.DoubleForwardSlash),
                    Token(SyntaxTokenType.Comment, comment),
                    Token(SyntaxTokenType.NewlineSymbol)));
        }
    }
}