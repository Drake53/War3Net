// ------------------------------------------------------------------------------
// <copyright file="TypeFactory.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
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
                return new TypeSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, typeName), 0));
            }
        }
    }
}