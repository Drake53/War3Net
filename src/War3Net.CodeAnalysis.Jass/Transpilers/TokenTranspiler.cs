// ------------------------------------------------------------------------------
// <copyright file="TokenTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    [Flags]
    public enum TokenTranspileFlags
    {
        ReturnArray = 1 << 0,
        ReturnBoolFunc = 1 << 1,
    }

    public static partial class JassToCSharpTranspiler
    {
        private const string AntiReservedKeywordConflictPrefix = "@";

        // TODO: use SyntaxFacts.IsValidIdentifier for alphanumeric tokens
        private static Lazy<HashSet<string>> _reservedKeywords = new Lazy<HashSet<string>>(() => new HashSet<string>(GetReservedKeywords()));

        public static TypeSyntax TranspileType(this TokenNode tokenNode, TokenTranspileFlags flags = (TokenTranspileFlags)0)
        {
            _ = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));

            return SyntaxFactory.ParseTypeName(flags.HasFlag(TokenTranspileFlags.ReturnArray)
                ? $"{tokenNode.TranspileTypeString(flags)}[]"
                : tokenNode.TranspileTypeString(flags));
        }

        public static string TranspileTypeString(this TokenNode tokenNode, TokenTranspileFlags flags = (TokenTranspileFlags)0)
        {
            _ = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));

            switch (tokenNode.TokenType)
            {
                // TODO: don't use hardcoded strings
                case SyntaxTokenType.HandleKeyword: return "object"; // SyntaxFactory.PredefinedType(SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.ObjectKeyword))
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

        public static Microsoft.CodeAnalysis.CSharp.SyntaxKind TranspileUnaryOperator(this TokenNode tokenNode)
        {
            _ = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));

            switch (tokenNode.TokenType)
            {
                case SyntaxTokenType.PlusOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.UnaryPlusExpression;
                case SyntaxTokenType.MinusOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.UnaryMinusExpression;
                case SyntaxTokenType.NotOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.LogicalNotExpression;

                default:
                    throw new ArgumentException($"Cannot transpile token of type {tokenNode.TokenType} to an operator of a unary expression.");
            }
        }

        public static Microsoft.CodeAnalysis.CSharp.SyntaxKind TranspileBinaryOperator(this TokenNode tokenNode)
        {
            _ = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));

            switch (tokenNode.TokenType)
            {
                case SyntaxTokenType.PlusOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.AddExpression;
                case SyntaxTokenType.MinusOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.SubtractExpression;
                case SyntaxTokenType.MultiplicationOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.MultiplyExpression;
                case SyntaxTokenType.DivisionOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.DivideExpression;
                case SyntaxTokenType.GreaterThanOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.GreaterThanExpression;
                case SyntaxTokenType.LessThanOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.LessThanExpression;
                case SyntaxTokenType.EqualityOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.EqualsExpression; // rename EqualsOperator?
                case SyntaxTokenType.UnequalityOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.NotEqualsExpression; // rename NotEqualsOperator?
                case SyntaxTokenType.GreaterOrEqualOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.GreaterThanOrEqualExpression;
                case SyntaxTokenType.LessOrEqualOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.LessThanOrEqualExpression;
                case SyntaxTokenType.AndOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.LogicalAndExpression;
                case SyntaxTokenType.OrOperator: return Microsoft.CodeAnalysis.CSharp.SyntaxKind.LogicalOrExpression;

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
                case SyntaxTokenType.TrueKeyword: return SyntaxFactory.LiteralExpression(Microsoft.CodeAnalysis.CSharp.SyntaxKind.TrueLiteralExpression);
                case SyntaxTokenType.FalseKeyword: return SyntaxFactory.LiteralExpression(Microsoft.CodeAnalysis.CSharp.SyntaxKind.FalseLiteralExpression);
                case SyntaxTokenType.String: return SyntaxFactory.ParseExpression($"\"{text}\"");
                case SyntaxTokenType.NullKeyword: return SyntaxFactory.LiteralExpression(Microsoft.CodeAnalysis.CSharp.SyntaxKind.NullLiteralExpression);

                default:
                    throw new ArgumentException($"Cannot transpile token of type {tokenNode.TokenType} to an expression.");
            }
        }

        private static IEnumerable<string> GetReservedKeywords()
        {
            foreach (var syntaxKind in Enum.GetValues(typeof(Microsoft.CodeAnalysis.CSharp.SyntaxKind)))
            {
                var kind = (Microsoft.CodeAnalysis.CSharp.SyntaxKind)syntaxKind;

                if (SyntaxFacts.IsReservedKeyword(kind))
                {
                    yield return SyntaxFactory.Token(kind).ValueText;
                }
            }
        }
    }
}