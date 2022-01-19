﻿// ------------------------------------------------------------------------------
// <copyright file="JassCommentDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassCommentDeclarationSyntax : IDeclarationSyntax
    {
        public JassCommentDeclarationSyntax(string comment)
        {
            Comment = comment;
        }

        public string Comment { get; init; }

        public bool Equals(IDeclarationSyntax? other)
        {
            return other is JassCommentDeclarationSyntax comment
                && string.Equals(Comment, comment.Comment, StringComparison.Ordinal);
        }

        public override string ToString() => $"{JassSymbol.Slash}{JassSymbol.Slash}{Comment}";
    }
}