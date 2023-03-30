// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxFacts.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace War3Net.CodeAnalysis.Jass
{
    public static class JassSyntaxFacts
    {
        public static bool IsWhitespaceCharacter(char ch)
        {
            return char.IsWhiteSpace(ch)
                && ch != JassSymbol.CarriageReturnChar
                && ch != JassSymbol.LineFeedChar;
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

        public static JassSyntaxKind GetBinaryExpressionKind(JassSyntaxKind binaryOperatorTokenSyntaxKind)
        {
            return binaryOperatorTokenSyntaxKind switch
            {
                JassSyntaxKind.PlusToken => JassSyntaxKind.AddExpression,
                JassSyntaxKind.MinusToken => JassSyntaxKind.SubtractExpression,
                JassSyntaxKind.AsteriskToken => JassSyntaxKind.MultiplyExpression,
                JassSyntaxKind.SlashToken => JassSyntaxKind.DivideExpression,
                JassSyntaxKind.GreaterThanToken => JassSyntaxKind.GreaterThanExpression,
                JassSyntaxKind.LessThanToken => JassSyntaxKind.LessThanExpression,
                JassSyntaxKind.EqualsEqualsToken => JassSyntaxKind.EqualsExpression,
                JassSyntaxKind.ExclamationEqualsToken => JassSyntaxKind.NotEqualsExpression,
                JassSyntaxKind.GreaterThanEqualsToken => JassSyntaxKind.GreaterThanOrEqualExpression,
                JassSyntaxKind.LessThanEqualsToken => JassSyntaxKind.LessThanOrEqualExpression,
                JassSyntaxKind.AndKeyword => JassSyntaxKind.AndExpression,
                JassSyntaxKind.OrKeyword => JassSyntaxKind.OrExpression,

                _ => throw new InvalidEnumArgumentException(nameof(binaryOperatorTokenSyntaxKind), (int)binaryOperatorTokenSyntaxKind, typeof(JassSyntaxKind)),
            };
        }

        public static JassSyntaxKind GetLiteralExpressionKind(JassSyntaxKind literalExpressionTokenSyntaxKind)
        {
            return literalExpressionTokenSyntaxKind switch
            {
                JassSyntaxKind.TrueKeyword => JassSyntaxKind.TrueLiteralExpression,
                JassSyntaxKind.FalseKeyword => JassSyntaxKind.FalseLiteralExpression,
                JassSyntaxKind.NullKeyword => JassSyntaxKind.NullLiteralExpression,
                JassSyntaxKind.CharacterLiteralToken => JassSyntaxKind.CharacterLiteralExpression,
                JassSyntaxKind.DecimalLiteralToken => JassSyntaxKind.DecimalLiteralExpression,
                JassSyntaxKind.FourCCLiteralToken => JassSyntaxKind.FourCCLiteralExpression,
                JassSyntaxKind.HexadecimalLiteralToken => JassSyntaxKind.HexadecimalLiteralExpression,
                JassSyntaxKind.OctalLiteralToken => JassSyntaxKind.OctalLiteralExpression,
                JassSyntaxKind.RealLiteralToken => JassSyntaxKind.RealLiteralExpression,
                JassSyntaxKind.StringLiteralToken => JassSyntaxKind.StringLiteralExpression,

                _ => throw new InvalidEnumArgumentException(nameof(literalExpressionTokenSyntaxKind), (int)literalExpressionTokenSyntaxKind, typeof(JassSyntaxKind)),
            };
        }

        public static JassSyntaxKind GetUnaryExpressionKind(JassSyntaxKind unaryOperatorTokenSyntaxKind)
        {
            return unaryOperatorTokenSyntaxKind switch
            {
                JassSyntaxKind.PlusToken => JassSyntaxKind.UnaryPlusExpression,
                JassSyntaxKind.MinusToken => JassSyntaxKind.UnaryMinusExpression,
                JassSyntaxKind.NotKeyword => JassSyntaxKind.NotExpression,

                _ => throw new InvalidEnumArgumentException(nameof(unaryOperatorTokenSyntaxKind), (int)unaryOperatorTokenSyntaxKind, typeof(JassSyntaxKind)),
            };
        }

        public static JassSyntaxKind GetDebugStatementKind(JassSyntaxKind statementSyntaxKind)
        {
            return statementSyntaxKind switch
            {
                JassSyntaxKind.SetStatement => JassSyntaxKind.DebugSetStatement,
                JassSyntaxKind.CallStatement => JassSyntaxKind.DebugCallStatement,
                JassSyntaxKind.IfStatement => JassSyntaxKind.DebugIfStatement,
                JassSyntaxKind.LoopStatement => JassSyntaxKind.DebugLoopStatement,

                _ => throw new InvalidEnumArgumentException(nameof(statementSyntaxKind), (int)statementSyntaxKind, typeof(JassSyntaxKind)),
            };
        }

        public static JassSyntaxKind GetGlobalDeclarationKind(JassSyntaxKind declaratorSyntaxKind)
        {
            return declaratorSyntaxKind switch
            {
                JassSyntaxKind.VariableDeclarator => JassSyntaxKind.GlobalVariableDeclaration,
                JassSyntaxKind.ArrayDeclarator => JassSyntaxKind.GlobalArrayDeclaration,

                _ => throw new InvalidEnumArgumentException(nameof(declaratorSyntaxKind), (int)declaratorSyntaxKind, typeof(JassSyntaxKind)),
            };
        }

        public static JassSyntaxKind GetLocalDeclarationStatementKind(JassSyntaxKind declaratorSyntaxKind)
        {
            return declaratorSyntaxKind switch
            {
                JassSyntaxKind.VariableDeclarator => JassSyntaxKind.LocalVariableDeclarationStatement,
                JassSyntaxKind.ArrayDeclarator => JassSyntaxKind.LocalArrayDeclarationStatement,

                _ => throw new InvalidEnumArgumentException(nameof(declaratorSyntaxKind), (int)declaratorSyntaxKind, typeof(JassSyntaxKind)),
            };
        }
    }
}