// ------------------------------------------------------------------------------
// <copyright file="TypeFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        private static readonly HashSet<SyntaxTokenType> _typeKeywords = GetTypeKeywordTokenTypes().ToHashSet();

        public static TypeSyntax Type(SyntaxTokenType typeKeyword)
        {
            return _typeKeywords.Contains(typeKeyword) ? new TypeSyntax(Token(typeKeyword)) : throw new ArgumentException("Tokentype must be a type keyword.", nameof(typeKeyword));
        }

        public static TypeSyntax ParseTypeName(string typeName)
        {
            if (SyntaxToken.TryTokenizeKeyword(typeName, out var token))
            {
                // TODO: filter keywords that are not a typename
                return new TypeSyntax(new TokenNode(token, 0));
            }
            else
            {
                // return new TypeSyntax(new TokenNode(new SyntaxToken(SyntaxToken.GetAlphanumericalTokenType(typeName, TokenizerMode.Content), typeName), 0));
                return new TypeSyntax(Token(SyntaxTokenType.AlphanumericIdentifier, typeName));
            }
        }

        private static IEnumerable<SyntaxTokenType> GetTypeKeywordTokenTypes()
        {
            return new[]
            {
                SyntaxTokenType.BooleanKeyword,
                SyntaxTokenType.RealKeyword,
                SyntaxTokenType.IntegerKeyword,
                SyntaxTokenType.HandleKeyword,
                SyntaxTokenType.StringKeyword,
                // todo: SyntaxTokenType.CodeKeyword?
            };
        }
    }
}