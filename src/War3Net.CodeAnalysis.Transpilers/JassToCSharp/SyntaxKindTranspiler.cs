using Microsoft.CodeAnalysis.CSharp;

using War3Net.CodeAnalysis.Jass;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public SyntaxKind TranspileBinaryExpressionKind(JassSyntaxKind expressionKind)
        {
            return expressionKind switch
            {
                JassSyntaxKind.AddExpression => SyntaxKind.AddExpression,
                JassSyntaxKind.SubtractExpression => SyntaxKind.SubtractExpression,
                JassSyntaxKind.MultiplyExpression => SyntaxKind.MultiplyExpression,
                JassSyntaxKind.DivideExpression => SyntaxKind.DivideExpression,
                JassSyntaxKind.GreaterThanExpression => SyntaxKind.GreaterThanExpression,
                JassSyntaxKind.LessThanExpression => SyntaxKind.LessThanExpression,
                JassSyntaxKind.EqualsExpression => SyntaxKind.EqualsExpression,
                JassSyntaxKind.NotEqualsExpression => SyntaxKind.NotEqualsExpression,
                JassSyntaxKind.GreaterThanOrEqualExpression => SyntaxKind.GreaterThanOrEqualExpression,
                JassSyntaxKind.LessThanOrEqualExpression => SyntaxKind.LessThanOrEqualExpression,
                JassSyntaxKind.LogicalAndExpression => SyntaxKind.LogicalAndExpression,
                JassSyntaxKind.LogicalOrExpression => SyntaxKind.LogicalOrExpression,
            };
        }

        public SyntaxKind TranspileBinaryOperatorKind(JassSyntaxKind operatorKind)
        {
            return operatorKind switch
            {
                JassSyntaxKind.PlusToken => SyntaxKind.PlusToken,
                JassSyntaxKind.MinusToken => SyntaxKind.MinusToken,
                JassSyntaxKind.AsteriskToken => SyntaxKind.AsteriskToken,
                JassSyntaxKind.SlashToken => SyntaxKind.SlashToken,
                JassSyntaxKind.GreaterThanToken => SyntaxKind.GreaterThanToken,
                JassSyntaxKind.LessThanToken => SyntaxKind.LessThanToken,
                JassSyntaxKind.EqualsEqualsToken => SyntaxKind.EqualsEqualsToken,
                JassSyntaxKind.ExclamationEqualsToken => SyntaxKind.ExclamationEqualsToken,
                JassSyntaxKind.GreaterThanEqualsToken => SyntaxKind.GreaterThanEqualsToken,
                JassSyntaxKind.LessThanEqualsToken => SyntaxKind.LessThanEqualsToken,
                JassSyntaxKind.AndKeyword => SyntaxKind.AmpersandAmpersandToken,
                JassSyntaxKind.OrKeyword => SyntaxKind.BarBarToken,
            };
        }

        public SyntaxKind TranspileTypeKeyword(JassSyntaxKind keyword)
        {
            return keyword switch
            {
                JassSyntaxKind.BooleanKeyword => SyntaxKind.BoolKeyword,
                JassSyntaxKind.HandleKeyword => SyntaxKind.ObjectKeyword,
                JassSyntaxKind.IntegerKeyword => SyntaxKind.IntKeyword,
                JassSyntaxKind.NothingKeyword => SyntaxKind.VoidKeyword,
                JassSyntaxKind.RealKeyword => SyntaxKind.FloatKeyword,
                JassSyntaxKind.StringKeyword => SyntaxKind.StringKeyword,
            };
        }

        public SyntaxKind TranspileUnaryExpressionKind(JassSyntaxKind expressionKind)
        {
            return expressionKind switch
            {
                JassSyntaxKind.UnaryPlusExpression => SyntaxKind.UnaryPlusExpression,
                JassSyntaxKind.UnaryMinusExpression => SyntaxKind.UnaryMinusExpression,
                JassSyntaxKind.LogicalNotExpression => SyntaxKind.LogicalNotExpression,
            };
        }

        public SyntaxKind TranspileUnaryOperatorKind(JassSyntaxKind operatorKind)
        {
            return operatorKind switch
            {
                JassSyntaxKind.PlusToken => SyntaxKind.PlusToken,
                JassSyntaxKind.MinusToken => SyntaxKind.MinusToken,
                JassSyntaxKind.NotKeyword => SyntaxKind.ExclamationToken,
            };
        }
    }
}