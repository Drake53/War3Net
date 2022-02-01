// ------------------------------------------------------------------------------
// <copyright file="JassCommentSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassCommentSyntax : IDeclarationSyntax, IStatementSyntax, ICustomScriptAction
    {
        public JassCommentSyntax(string comment)
        {
            Comment = comment;
        }

        public string Comment { get; init; }

        public bool Equals(IDeclarationSyntax? other)
        {
            return other is JassCommentSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public bool Equals(IStatementSyntax? other)
        {
            return other is JassCommentSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public bool Equals(ICustomScriptAction? other)
        {
            return other is JassCommentSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public override string ToString() => $"{JassSymbol.Slash}{JassSymbol.Slash}{Comment}";
    }
}