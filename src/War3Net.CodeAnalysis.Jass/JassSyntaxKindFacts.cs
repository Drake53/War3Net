// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxKindFacts.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.ComponentModel;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFacts
    {
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
                JassSyntaxKind.AndKeyword => JassSyntaxKind.LogicalAndExpression,
                JassSyntaxKind.OrKeyword => JassSyntaxKind.LogicalOrExpression,

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
                JassSyntaxKind.NotKeyword => JassSyntaxKind.LogicalNotExpression,

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