﻿// ------------------------------------------------------------------------------
// <copyright file="VJassSyntaxFacts.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace War3Net.CodeAnalysis.VJass
{
    public static class VJassSyntaxFacts
    {
        public static bool IsWhitespaceCharacter(char ch)
        {
            return char.IsWhiteSpace(ch)
                && ch != VJassSymbol.CarriageReturnChar
                && ch != VJassSymbol.LineFeedChar;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the character can be the starting character of a JASS identifier.
        /// </summary>
        /// <param name="ch">The character.</param>
        public static bool IsIdentifierStartCharacter(char ch)
        {
            return (ch >= 'A' && ch <= 'Z')
                || (ch >= 'a' && ch <= 'z');
        }

        /// <summary>
        /// Returns <see langword="true"/> if the character can be a part of a JASS identifier.
        /// </summary>
        /// <param name="ch">The character.</param>
        public static bool IsIdentifierPartCharacter(char ch)
        {
            return (ch >= 'A' && ch <= 'Z')
                || (ch >= 'a' && ch <= 'z')
                || (ch >= '0' && ch <= '9')
                || ch == '_';
        }

        /// <summary>
        /// Returns <see langword="true"/> if the character can be the ending character of a JASS identifier.
        /// </summary>
        /// <param name="ch">The character.</param>
        public static bool IsIdentifierEndCharacter(char ch)
        {
            return (ch >= 'A' && ch <= 'Z')
                || (ch >= 'a' && ch <= 'z')
                || (ch >= '0' && ch <= '9');
        }

        /// <summary>
        /// Returns <see langword="true"/> if the character can be the ending character of a vJASS property identifier.
        /// </summary>
        /// <param name="ch">The character.</param>
        public static bool IsPropertyIdentifierEndCharacter(char ch)
        {
            return (ch >= 'A' && ch <= 'Z')
                || (ch >= 'a' && ch <= 'z')
                || (ch >= '0' && ch <= '9')
                || ch == '=';
        }

        /// <summary>
        /// Check that the name is a valid identifier.
        /// </summary>
        public static bool IsValidIdentifier([NotNullWhen(true)] string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (!IsIdentifierStartCharacter(name[0]))
            {
                return false;
            }

            var lastPartCharacterIndex = name.Length - 1;
            for (var i = 1; i < lastPartCharacterIndex; i++)
            {
                if (!IsIdentifierPartCharacter(name[i]))
                {
                    return false;
                }
            }

            return IsIdentifierEndCharacter(name[^1]);
        }

        /// <summary>
        /// Check that the name is a valid property identifier.
        /// </summary>
        public static bool IsValidPropertyIdentifier([NotNullWhen(true)] string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (!IsIdentifierStartCharacter(name[0]))
            {
                return false;
            }

            var lastPartCharacterIndex = name.Length - 1;
            for (var i = 1; i < lastPartCharacterIndex; i++)
            {
                if (!IsIdentifierPartCharacter(name[i]))
                {
                    return false;
                }
            }

            return IsPropertyIdentifierEndCharacter(name[^1]);
        }
    }
}