// ------------------------------------------------------------------------------
// <copyright file="JassCommentStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassCommentStatementSyntax : IStatementSyntax, ICustomScriptAction
    {
        public JassCommentStatementSyntax(string comment)
        {
            Comment = comment;
        }

        public string Comment { get; init; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is JassCommentStatementSyntax commentStatement
                && string.Equals(Comment, commentStatement.Comment, StringComparison.Ordinal);
        }

        public bool Equals(ICustomScriptAction? other)
        {
            return other is JassCommentStatementSyntax commentStatement
                && string.Equals(Comment, commentStatement.Comment, StringComparison.Ordinal);
        }

        public override string ToString() => $"{JassSymbol.Slash}{JassSymbol.Slash}{Comment}";
    }
}