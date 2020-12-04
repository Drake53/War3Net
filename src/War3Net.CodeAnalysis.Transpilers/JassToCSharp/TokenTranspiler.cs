// ------------------------------------------------------------------------------
// <copyright file="TokenTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        private const string AntiReservedKeywordConflictPrefix = "@";

        // TODO: use SyntaxFacts.IsValidIdentifier for alphanumeric tokens
        private static readonly Lazy<HashSet<string>> _reservedKeywords = new Lazy<HashSet<string>>(() => new HashSet<string>(GetReservedKeywords()));

        public static TypeSyntax TranspileType(this TokenNode tokenNode, TokenTranspileFlags flags = 0)
        {
            _ = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));

            return SyntaxFactory.ParseTypeName(flags.HasFlag(TokenTranspileFlags.ReturnArray)
                ? $"{tokenNode.TranspileTypeString(flags)}[]"
                : tokenNode.TranspileTypeString(flags));
        }

        public static string TranspileTypeString(this TokenNode tokenNode, TokenTranspileFlags flags = 0)
        {
            _ = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));

            switch (tokenNode.TokenType)
            {
                // TODO: don't use hardcoded strings
                case SyntaxTokenType.HandleKeyword: return "object"; // SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword))
                case SyntaxTokenType.IntegerKeyword: return "int";
                case SyntaxTokenType.RealKeyword: return "float";
                case SyntaxTokenType.StringKeyword: return "string";
                case SyntaxTokenType.BooleanKeyword: return "bool";
                case SyntaxTokenType.CodeKeyword: return flags.HasFlag(TokenTranspileFlags.ReturnBoolFunc) ? "System.Func<bool>" : "System.Action";
                case SyntaxTokenType.AlphanumericIdentifier: return tokenNode.TranspileIdentifier();

                default:
                    throw new ArgumentException($"Cannot transpile token of type {tokenNode.TokenType} to a C# type.");
            }
        }

        public static string TranspileIdentifier(this TokenNode tokenNode)
        {
            if ((tokenNode?.TokenType ?? SyntaxTokenType.Undefined) == SyntaxTokenType.AlphanumericIdentifier)
            {
                return _reservedKeywords.Value.Contains(tokenNode.ValueText)
                    ? $"{AntiReservedKeywordConflictPrefix}{tokenNode.ValueText}"
                    : tokenNode.ValueText;
            }

            throw new ArgumentException($"Identifier token must have type {SyntaxTokenType.AlphanumericIdentifier}.");
        }

        public static SyntaxKind TranspileUnaryOperator(this TokenNode tokenNode)
        {
            _ = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));

            switch (tokenNode.TokenType)
            {
                case SyntaxTokenType.PlusOperator: return SyntaxKind.UnaryPlusExpression;
                case SyntaxTokenType.MinusOperator: return SyntaxKind.UnaryMinusExpression;
                case SyntaxTokenType.NotOperator: return SyntaxKind.LogicalNotExpression;

                default:
                    throw new ArgumentException($"Cannot transpile token of type {tokenNode.TokenType} to an operator of a unary expression.");
            }
        }

        public static SyntaxKind TranspileBinaryOperator(this TokenNode tokenNode)
        {
            _ = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));

            switch (tokenNode.TokenType)
            {
                case SyntaxTokenType.PlusOperator: return SyntaxKind.AddExpression;
                case SyntaxTokenType.MinusOperator: return SyntaxKind.SubtractExpression;
                case SyntaxTokenType.MultiplicationOperator: return SyntaxKind.MultiplyExpression;
                case SyntaxTokenType.DivisionOperator: return SyntaxKind.DivideExpression;
                case SyntaxTokenType.GreaterThanOperator: return SyntaxKind.GreaterThanExpression;
                case SyntaxTokenType.LessThanOperator: return SyntaxKind.LessThanExpression;
                case SyntaxTokenType.EqualityOperator: return SyntaxKind.EqualsExpression; // rename EqualsOperator?
                case SyntaxTokenType.UnequalityOperator: return SyntaxKind.NotEqualsExpression; // rename NotEqualsOperator?
                case SyntaxTokenType.GreaterOrEqualOperator: return SyntaxKind.GreaterThanOrEqualExpression;
                case SyntaxTokenType.LessOrEqualOperator: return SyntaxKind.LessThanOrEqualExpression;
                case SyntaxTokenType.AndOperator: return SyntaxKind.LogicalAndExpression;
                case SyntaxTokenType.OrOperator: return SyntaxKind.LogicalOrExpression;

                default:
                    throw new ArgumentException($"Cannot transpile token of type {tokenNode.TokenType} to an operator of a binary expression.");
            }
        }

        public static ExpressionSyntax TranspileExpression(this TokenNode tokenNode)
        {
            if (tokenNode.TokenType == SyntaxTokenType.AlphanumericIdentifier)
            {
                return SyntaxFactory.ParseExpression(
                    _reservedKeywords.Value.Contains(tokenNode.ValueText)
                        ? $"{AntiReservedKeywordConflictPrefix}{tokenNode.ValueText}"
                        : tokenNode.ValueText);
            }

            return tokenNode.TranspileConstantExpression();
        }

        public static ExpressionSyntax TranspileExpression(this TokenNode tokenNode, out bool @const)
        {
            if (tokenNode.TokenType == SyntaxTokenType.AlphanumericIdentifier)
            {
                // Can be true, but too much effort to detect if the assigned identifier itself is also constant.
                @const = false;

                return SyntaxFactory.ParseExpression(
                    _reservedKeywords.Value.Contains(tokenNode.ValueText)
                        ? $"{AntiReservedKeywordConflictPrefix}{tokenNode.ValueText}"
                        : tokenNode.ValueText);
            }

            @const = true;

            return tokenNode.TranspileConstantExpression();
        }

        private static ExpressionSyntax TranspileConstantExpression(this TokenNode tokenNode)
        {
            var text = tokenNode.ValueText;
            switch (tokenNode.TokenType)
            {
                case SyntaxTokenType.DecimalNumber: return SyntaxFactory.ParseExpression(text);
                case SyntaxTokenType.OctalNumber: return SyntaxFactory.ParseExpression("0"); // TODO: implement octal numbers
                case SyntaxTokenType.HexadecimalNumber: return SyntaxFactory.ParseExpression($"0x{text.Substring(text[0] == '$' ? 1 : 2)}");
                case SyntaxTokenType.FourCCNumber: return SyntaxFactory.ParseExpression(
                    text.Length == 4
                    ? ((int)text[0] << 24 | (int)text[1] << 16 | (int)text[2] << 8 | (int)text[3]).ToString()
                    : ((int)text[0]).ToString());
                case SyntaxTokenType.RealNumber: return SyntaxFactory.ParseExpression($"{(text[text.Length - 1] == '.' ? text.Substring(0, text.Length - 1) : text)}f");
                case SyntaxTokenType.TrueKeyword: return SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression);
                case SyntaxTokenType.FalseKeyword: return SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
                case SyntaxTokenType.String: return SyntaxFactory.ParseExpression($"\"{text}\"");
                case SyntaxTokenType.NullKeyword: return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);

                default:
                    throw new ArgumentException($"Cannot transpile token of type {tokenNode.TokenType} to an expression.");
            }
        }

        private static IEnumerable<string> GetReservedKeywords()
        {
            foreach (var syntaxKind in Enum.GetValues(typeof(SyntaxKind)))
            {
                var kind = (SyntaxKind)syntaxKind;

                if (SyntaxFacts.IsReservedKeyword(kind))
                {
                    yield return SyntaxFactory.Token(kind).ValueText;
                }
            }
        }
    }
}