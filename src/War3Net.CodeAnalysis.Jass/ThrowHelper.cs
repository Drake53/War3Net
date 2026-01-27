// ------------------------------------------------------------------------------
// <copyright file="ThrowHelper.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal static class ThrowHelper
    {
        public static void ThrowIfInvalidToken(
            JassSyntaxToken token,
            JassSyntaxKind expectedSyntaxKind,
            [CallerArgumentExpression(nameof(token))] string? paramName = null)
        {
            if (token is null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (token.SyntaxKind != expectedSyntaxKind)
            {
                throw new ArgumentException($"The token's syntax kind must be '{expectedSyntaxKind}'.", paramName);
            }
        }

        public static void ThrowIfInvalidSeparatedSyntaxList<TNode>(
            SeparatedSyntaxList<TNode, JassSyntaxToken> separatedSyntaxList,
            JassSyntaxKind expectedSyntaxKind,
            [CallerArgumentExpression(nameof(separatedSyntaxList))] string? paramName = null)
        {
            if (separatedSyntaxList is null)
            {
                throw new ArgumentNullException(paramName);
            }

            for (var i = 0; i < separatedSyntaxList.Items.Length; i++)
            {
                var item = separatedSyntaxList.Items[i];
                if (item is null)
                {
                    throw new ArgumentException("Items in list may not be null.", paramName);
                }
            }

            for (var i = 0; i < separatedSyntaxList.Separators.Length; i++)
            {
                var separator = separatedSyntaxList.Separators[i];
                if (separator is null)
                {
                    throw new ArgumentException("Separators in list may not be null.", paramName);
                }

                if (separator.SyntaxKind != expectedSyntaxKind)
                {
                    throw new ArgumentException($"The separator token's syntax kind must be '{expectedSyntaxKind}'.", paramName);
                }
            }
        }
    }
}