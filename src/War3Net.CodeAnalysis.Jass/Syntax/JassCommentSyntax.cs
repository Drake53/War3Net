// ------------------------------------------------------------------------------
// <copyright file="JassCommentSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassCommentSyntax : ITopLevelDeclarationSyntax, IScopedDeclarationSyntax, IGlobalDeclarationSyntax, IScopedGlobalDeclarationSyntax, IMemberDeclarationSyntax, IStatementSyntax, IDeclarationLineSyntax, IGlobalLineSyntax, IStatementLineSyntax, IJassSyntaxToken
    {
        public JassCommentSyntax(string comment)
        {
            Comment = comment;
        }

        public string Comment { get; init; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is JassCommentSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public bool Equals(IScopedDeclarationSyntax? other)
        {
            return other is JassCommentSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public bool Equals(IGlobalDeclarationSyntax? other)
        {
            return other is JassCommentSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public bool Equals(IScopedGlobalDeclarationSyntax? other)
        {
            return other is JassCommentSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public bool Equals(IMemberDeclarationSyntax? other)
        {
            return other is JassCommentSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public bool Equals(IStatementSyntax? other)
        {
            return other is JassCommentSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public bool Equals(IDeclarationLineSyntax? other)
        {
            return other is JassCommentSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public bool Equals(IGlobalLineSyntax? other)
        {
            return other is JassCommentSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public bool Equals(IStatementLineSyntax? other)
        {
            return other is JassCommentSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public override string ToString() => $"{JassSymbol.Slash}{JassSymbol.Slash}{Comment}";
    }
}