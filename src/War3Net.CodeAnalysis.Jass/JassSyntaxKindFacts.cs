// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxKindFacts.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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
                JassSyntaxKind.OrKeyword => JassSyntaxKind.LogicalOrExpression,
                JassSyntaxKind.AndKeyword => JassSyntaxKind.LogicalAndExpression,
                JassSyntaxKind.EqualsEqualsToken => JassSyntaxKind.EqualsExpression,
                JassSyntaxKind.ExclamationEqualsToken => JassSyntaxKind.NotEqualsExpression,
                JassSyntaxKind.LessThanToken => JassSyntaxKind.LessThanExpression,
                JassSyntaxKind.LessThanEqualsToken => JassSyntaxKind.LessThanOrEqualExpression,
                JassSyntaxKind.GreaterThanToken => JassSyntaxKind.GreaterThanExpression,
                JassSyntaxKind.GreaterThanEqualsToken => JassSyntaxKind.GreaterThanOrEqualExpression,

                _ => JassSyntaxKind.None,
            };
        }

        public static JassSyntaxKind GetLiteralExpressionKind(JassSyntaxKind literalExpressionTokenSyntaxKind)
        {
            return literalExpressionTokenSyntaxKind switch
            {
                JassSyntaxKind.RealLiteralToken => JassSyntaxKind.RealLiteralExpression,
                JassSyntaxKind.StringLiteralToken => JassSyntaxKind.StringLiteralExpression,
                JassSyntaxKind.CharacterLiteralToken => JassSyntaxKind.CharacterLiteralExpression,
                JassSyntaxKind.TrueKeyword => JassSyntaxKind.TrueLiteralExpression,
                JassSyntaxKind.FalseKeyword => JassSyntaxKind.FalseLiteralExpression,
                JassSyntaxKind.NullKeyword => JassSyntaxKind.NullLiteralExpression,
                JassSyntaxKind.DecimalLiteralToken => JassSyntaxKind.DecimalLiteralExpression,
                JassSyntaxKind.HexadecimalLiteralToken => JassSyntaxKind.HexadecimalLiteralExpression,
                JassSyntaxKind.OctalLiteralToken => JassSyntaxKind.OctalLiteralExpression,
                JassSyntaxKind.FourCCLiteralToken => JassSyntaxKind.FourCCLiteralExpression,

                _ => JassSyntaxKind.None,
            };
        }

        public static JassSyntaxKind GetUnaryExpressionKind(JassSyntaxKind unaryOperatorTokenSyntaxKind)
        {
            return unaryOperatorTokenSyntaxKind switch
            {
                JassSyntaxKind.PlusToken => JassSyntaxKind.UnaryPlusExpression,
                JassSyntaxKind.MinusToken => JassSyntaxKind.UnaryMinusExpression,
                JassSyntaxKind.NotKeyword => JassSyntaxKind.LogicalNotExpression,

                _ => JassSyntaxKind.None,
            };
        }

        public static JassSyntaxKind GetDebugStatementKind(JassSyntaxKind statementSyntaxKind)
        {
            return statementSyntaxKind switch
            {
                JassSyntaxKind.SetStatement => JassSyntaxKind.DebugSetStatement,
                JassSyntaxKind.CallStatement => JassSyntaxKind.DebugCallStatement,
                JassSyntaxKind.LoopStatement => JassSyntaxKind.DebugLoopStatement,
                JassSyntaxKind.IfStatement => JassSyntaxKind.DebugIfStatement,

                _ => JassSyntaxKind.None,
            };
        }

        public static JassSyntaxKind GetGlobalDeclarationKind(JassSyntaxKind declaratorSyntaxKind)
        {
            return declaratorSyntaxKind switch
            {
                JassSyntaxKind.VariableDeclarator => JassSyntaxKind.GlobalVariableDeclaration,
                JassSyntaxKind.ArrayDeclarator => JassSyntaxKind.GlobalArrayDeclaration,

                _ => JassSyntaxKind.None,
            };
        }

        public static JassSyntaxKind GetLocalDeclarationStatementKind(JassSyntaxKind declaratorSyntaxKind)
        {
            return declaratorSyntaxKind switch
            {
                JassSyntaxKind.VariableDeclarator => JassSyntaxKind.LocalVariableDeclarationStatement,
                JassSyntaxKind.ArrayDeclarator => JassSyntaxKind.LocalArrayDeclarationStatement,

                _ => JassSyntaxKind.None,
            };
        }
    }
}