// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxKindFacts.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFacts
    {
        private static readonly Dictionary<JassSyntaxKind, string> _defaultTokenText = GetDefaultTokenText();
        private static readonly Dictionary<string, JassSyntaxKind> _textToKind = _defaultTokenText.ToDictionary(pair => pair.Value, pair => pair.Key);

        public static bool IsPunctuation(JassSyntaxKind syntaxKind)
        {
            return syntaxKind >= JassSyntaxKind.AsteriskToken && syntaxKind <= JassSyntaxKind.GreaterThanEqualsToken;
        }

        public static bool IsKeyword(JassSyntaxKind syntaxKind)
        {
            return syntaxKind >= JassSyntaxKind.BooleanKeyword && syntaxKind <= JassSyntaxKind.NotKeyword;
        }

        public static bool IsPredefinedTypeKeyword(JassSyntaxKind syntaxKind)
        {
            return syntaxKind >= JassSyntaxKind.BooleanKeyword && syntaxKind <= JassSyntaxKind.CodeKeyword;
        }

        public static bool IsReservedKeyword(JassSyntaxKind syntaxKind)
        {
            return syntaxKind >= JassSyntaxKind.NullKeyword && syntaxKind <= JassSyntaxKind.NotKeyword;
        }

        internal static bool IsLiteralToken(JassSyntaxKind syntaxKind)
        {
            return syntaxKind >= JassSyntaxKind.RealLiteralToken && syntaxKind <= JassSyntaxKind.FourCCLiteralToken;
        }

        public static bool IsAnyToken(JassSyntaxKind syntaxKind)
        {
            return syntaxKind >= JassSyntaxKind.AsteriskToken && syntaxKind <= JassSyntaxKind.FourCCLiteralToken;
        }

        public static bool IsTrivia(JassSyntaxKind syntaxKind)
        {
            return syntaxKind >= JassSyntaxKind.NewlineTrivia && syntaxKind <= JassSyntaxKind.SingleLineCommentTrivia;
        }

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

        public static JassSyntaxKind GetKeywordKind(string text)
        {
            return _textToKind.GetValueOrDefault(text, JassSyntaxKind.None); 
        }

        private static Dictionary<JassSyntaxKind, string> GetDefaultTokenText()
        {
            return new Dictionary<JassSyntaxKind, string>
            {
                { JassSyntaxKind.AsteriskToken, JassSymbol.Asterisk },
                { JassSyntaxKind.OpenParenToken, JassSymbol.OpenParen },
                { JassSyntaxKind.CloseParenToken, JassSymbol.CloseParen },
                { JassSyntaxKind.MinusToken, JassSymbol.Minus },
                { JassSyntaxKind.PlusToken, JassSymbol.Plus },
                { JassSyntaxKind.EqualsToken, JassSymbol.Equals },
                { JassSyntaxKind.OpenBracketToken, JassSymbol.OpenBracket },
                { JassSyntaxKind.CloseBracketToken, JassSymbol.CloseBracket },
                { JassSyntaxKind.LessThanToken, JassSymbol.LessThan },
                { JassSyntaxKind.CommaToken, JassSymbol.Comma },
                { JassSyntaxKind.GreaterThanToken, JassSymbol.GreaterThan },
                { JassSyntaxKind.SlashToken, JassSymbol.Slash },
                { JassSyntaxKind.ExclamationEqualsToken, JassSymbol.ExclamationEquals },
                { JassSyntaxKind.EqualsEqualsToken, JassSymbol.EqualsEquals },
                { JassSyntaxKind.LessThanEqualsToken, JassSymbol.LessThanEquals },
                { JassSyntaxKind.GreaterThanEqualsToken, JassSymbol.GreaterThanEquals },
                { JassSyntaxKind.BooleanKeyword, JassKeyword.Boolean },
                { JassSyntaxKind.IntegerKeyword, JassKeyword.Integer },
                { JassSyntaxKind.RealKeyword, JassKeyword.Real },
                { JassSyntaxKind.StringKeyword, JassKeyword.String },
                { JassSyntaxKind.NothingKeyword, JassKeyword.Nothing },
                { JassSyntaxKind.HandleKeyword, JassKeyword.Handle },
                { JassSyntaxKind.NullKeyword, JassKeyword.Null },
                { JassSyntaxKind.TrueKeyword, JassKeyword.True },
                { JassSyntaxKind.FalseKeyword, JassKeyword.False },
                { JassSyntaxKind.IfKeyword, JassKeyword.If },
                { JassSyntaxKind.ElseIfKeyword, JassKeyword.ElseIf },
                { JassSyntaxKind.ThenKeyword, JassKeyword.Then },
                { JassSyntaxKind.ElseKeyword, JassKeyword.Else },
                { JassSyntaxKind.EndIfKeyword, JassKeyword.EndIf },
                { JassSyntaxKind.LoopKeyword, JassKeyword.Loop },
                { JassSyntaxKind.ExitWhenKeyword, JassKeyword.ExitWhen },
                { JassSyntaxKind.EndLoopKeyword, JassKeyword.EndLoop },
                { JassSyntaxKind.ReturnKeyword, JassKeyword.Return },
                { JassSyntaxKind.CallKeyword, JassKeyword.Call },
                { JassSyntaxKind.SetKeyword, JassKeyword.Set },
                { JassSyntaxKind.LocalKeyword, JassKeyword.Local },
                { JassSyntaxKind.DebugKeyword, JassKeyword.Debug },
                { JassSyntaxKind.ConstantKeyword, JassKeyword.Constant },
                { JassSyntaxKind.FunctionKeyword, JassKeyword.Function },
                { JassSyntaxKind.TakesKeyword, JassKeyword.Takes },
                { JassSyntaxKind.ReturnsKeyword, JassKeyword.Returns },
                { JassSyntaxKind.EndFunctionKeyword, JassKeyword.EndFunction },
                { JassSyntaxKind.NativeKeyword, JassKeyword.Native },
                { JassSyntaxKind.ExtendsKeyword, JassKeyword.Extends },
                { JassSyntaxKind.CodeKeyword, JassKeyword.Code },
                { JassSyntaxKind.AliasKeyword, JassKeyword.Alias },
                { JassSyntaxKind.ArrayKeyword, JassKeyword.Array },
                { JassSyntaxKind.GlobalsKeyword, JassKeyword.Globals },
                { JassSyntaxKind.EndGlobalsKeyword, JassKeyword.EndGlobals },
                { JassSyntaxKind.TypeKeyword, JassKeyword.Type },
                { JassSyntaxKind.OrKeyword, JassKeyword.Or },
                { JassSyntaxKind.AndKeyword, JassKeyword.And },
                { JassSyntaxKind.NotKeyword, JassKeyword.Not },
            };
        }
    }
}