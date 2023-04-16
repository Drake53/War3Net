// ------------------------------------------------------------------------------
// <copyright file="SyntaxTokenFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        private static readonly Dictionary<JassSyntaxKind, JassSyntaxToken> _defaultTokens = new();
        private static readonly Dictionary<JassSyntaxKind, string> _defaultTokenText = GetDefaultTokenText();

        public static JassSyntaxToken Token(JassSyntaxKind syntaxKind)
        {
            if (_defaultTokens.TryGetValue(syntaxKind, out var token))
            {
                return token;
            }

            if (!_defaultTokenText.TryGetValue(syntaxKind, out var text))
            {
                throw new InvalidEnumArgumentException(nameof(syntaxKind), (int)syntaxKind, typeof(JassSyntaxKind));
            }

            token = new JassSyntaxToken(JassSyntaxTriviaList.Empty, syntaxKind, text, JassSyntaxTriviaList.Empty);
            _defaultTokens.Add(syntaxKind, token);
            return token;
        }

        public static JassSyntaxToken Token(JassSyntaxTriviaList leadingTrivia, JassSyntaxKind syntaxKind)
        {
            return Token(leadingTrivia, syntaxKind, JassSyntaxTriviaList.Empty);
        }

        public static JassSyntaxToken Token(JassSyntaxKind syntaxKind, JassSyntaxTriviaList trailingTrivia)
        {
            return Token(JassSyntaxTriviaList.Empty, syntaxKind, trailingTrivia);
        }

        public static JassSyntaxToken Token(JassSyntaxKind syntaxKind, string text)
        {
            return Token(JassSyntaxTriviaList.Empty, syntaxKind, text, JassSyntaxTriviaList.Empty);
        }

        public static JassSyntaxToken Token(JassSyntaxTriviaList leadingTrivia, JassSyntaxKind syntaxKind, JassSyntaxTriviaList trailingTrivia)
        {
            if (!_defaultTokenText.TryGetValue(syntaxKind, out var text))
            {
                throw new InvalidEnumArgumentException(nameof(syntaxKind), (int)syntaxKind, typeof(JassSyntaxKind));
            }

            return new JassSyntaxToken(leadingTrivia, syntaxKind, text, trailingTrivia);
        }

        public static JassSyntaxToken Token(JassSyntaxTriviaList leadingTrivia, JassSyntaxKind syntaxKind, string text, JassSyntaxTriviaList trailingTrivia)
        {
            return new JassSyntaxToken(leadingTrivia, syntaxKind, text, trailingTrivia);
        }

        private static Dictionary<JassSyntaxKind, string> GetDefaultTokenText()
        {
            return new Dictionary<JassSyntaxKind, string>
            {
                { JassSyntaxKind.OpenParenToken, JassSymbol.OpenParen },
                { JassSyntaxKind.CloseParenToken, JassSymbol.CloseParen },
                { JassSyntaxKind.AsteriskToken, JassSymbol.Asterisk },
                { JassSyntaxKind.PlusToken, JassSymbol.Plus },
                { JassSyntaxKind.CommaToken, JassSymbol.Comma },
                { JassSyntaxKind.MinusToken, JassSymbol.Minus },
                { JassSyntaxKind.SlashToken, JassSymbol.Slash },
                { JassSyntaxKind.LessThanToken, JassSymbol.LessThan },
                { JassSyntaxKind.EqualsToken, JassSymbol.Equals },
                { JassSyntaxKind.GreaterThanToken, JassSymbol.GreaterThan },
                { JassSyntaxKind.OpenBracketToken, JassSymbol.OpenBracket },
                { JassSyntaxKind.CloseBracketToken, JassSymbol.CloseBracket },

                { JassSyntaxKind.ExclamationEqualsToken, JassSymbol.ExclamationEquals },
                { JassSyntaxKind.LessThanEqualsToken, JassSymbol.LessThanEquals },
                { JassSyntaxKind.GreaterThanEqualsToken, JassSymbol.GreaterThanEquals },
                { JassSyntaxKind.EqualsEqualsToken, JassSymbol.EqualsEquals },
                { JassSyntaxKind.EndOfFileToken, string.Empty },

                { JassSyntaxKind.AliasKeyword, JassKeyword.Alias },
                { JassSyntaxKind.AndKeyword, JassKeyword.And },
                { JassSyntaxKind.ArrayKeyword, JassKeyword.Array },
                { JassSyntaxKind.BooleanKeyword, JassKeyword.Boolean },
                { JassSyntaxKind.CallKeyword, JassKeyword.Call },
                { JassSyntaxKind.CodeKeyword, JassKeyword.Code },
                { JassSyntaxKind.ConstantKeyword, JassKeyword.Constant },
                { JassSyntaxKind.DebugKeyword, JassKeyword.Debug },
                { JassSyntaxKind.ElseKeyword, JassKeyword.Else },
                { JassSyntaxKind.ElseIfKeyword, JassKeyword.ElseIf },
                { JassSyntaxKind.EndFunctionKeyword, JassKeyword.EndFunction },
                { JassSyntaxKind.EndGlobalsKeyword, JassKeyword.EndGlobals },
                { JassSyntaxKind.EndIfKeyword, JassKeyword.EndIf },
                { JassSyntaxKind.EndLoopKeyword, JassKeyword.EndLoop },
                { JassSyntaxKind.ExitWhenKeyword, JassKeyword.ExitWhen },
                { JassSyntaxKind.ExtendsKeyword, JassKeyword.Extends },
                { JassSyntaxKind.FalseKeyword, JassKeyword.False },
                { JassSyntaxKind.FunctionKeyword, JassKeyword.Function },
                { JassSyntaxKind.GlobalsKeyword, JassKeyword.Globals },
                { JassSyntaxKind.HandleKeyword, JassKeyword.Handle },
                { JassSyntaxKind.IfKeyword, JassKeyword.If },
                { JassSyntaxKind.IntegerKeyword, JassKeyword.Integer },
                { JassSyntaxKind.LocalKeyword, JassKeyword.Local },
                { JassSyntaxKind.LoopKeyword, JassKeyword.Loop },
                { JassSyntaxKind.NativeKeyword, JassKeyword.Native },
                { JassSyntaxKind.NotKeyword, JassKeyword.Not },
                { JassSyntaxKind.NothingKeyword, JassKeyword.Nothing },
                { JassSyntaxKind.NullKeyword, JassKeyword.Null },
                { JassSyntaxKind.OrKeyword, JassKeyword.Or },
                { JassSyntaxKind.RealKeyword, JassKeyword.Real },
                { JassSyntaxKind.ReturnKeyword, JassKeyword.Return },
                { JassSyntaxKind.ReturnsKeyword, JassKeyword.Returns },
                { JassSyntaxKind.SetKeyword, JassKeyword.Set },
                { JassSyntaxKind.StringKeyword, JassKeyword.String },
                { JassSyntaxKind.TakesKeyword, JassKeyword.Takes },
                { JassSyntaxKind.ThenKeyword, JassKeyword.Then },
                { JassSyntaxKind.TrueKeyword, JassKeyword.True },
                { JassSyntaxKind.TypeKeyword, JassKeyword.Type },
            };
        }
    }
}