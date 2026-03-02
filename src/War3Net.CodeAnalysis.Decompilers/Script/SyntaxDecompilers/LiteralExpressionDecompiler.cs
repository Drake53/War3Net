// ------------------------------------------------------------------------------
// <copyright file="LiteralExpressionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileLiteralExpression(
            JassLiteralExpressionSyntax literalExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            return literalExpression.SyntaxKind switch
            {
                JassSyntaxKind.CharacterLiteralExpression => TryDecompileCharacterLiteralExpression(literalExpression, expectedType, out functionParameter),
                JassSyntaxKind.FourCCLiteralExpression => TryDecompileFourCCLiteralExpression(literalExpression, expectedType, out functionParameter),
                JassSyntaxKind.HexadecimalLiteralExpression => TryDecompileHexadecimalLiteralExpression(literalExpression, expectedType, out functionParameter),
                JassSyntaxKind.RealLiteralExpression => TryDecompileRealLiteralExpression(literalExpression, expectedType, out functionParameter),
                JassSyntaxKind.OctalLiteralExpression => TryDecompileOctalLiteralExpression(literalExpression, expectedType, out functionParameter),
                JassSyntaxKind.DecimalLiteralExpression => TryDecompileDecimalLiteralExpression(literalExpression, expectedType, out functionParameter),
                JassSyntaxKind.TrueLiteralExpression => TryDecompileBooleanLiteralExpression(literalExpression, expectedType, out functionParameter),
                JassSyntaxKind.FalseLiteralExpression => TryDecompileBooleanLiteralExpression(literalExpression, expectedType, out functionParameter),
                JassSyntaxKind.StringLiteralExpression => TryDecompileStringLiteralExpression(literalExpression, expectedType, out functionParameter),
                JassSyntaxKind.NullLiteralExpression => TryDecompileNullLiteralExpression(literalExpression, expectedType, out functionParameter),

                _ => throw new NotSupportedException($"Unsupported literal expression kind: {literalExpression.SyntaxKind}"),
            };
        }

        private bool TryDecompileLiteralExpression(
            JassLiteralExpressionSyntax literalExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            return literalExpression.SyntaxKind switch
            {
                JassSyntaxKind.CharacterLiteralExpression => TryDecompileCharacterLiteralExpression(literalExpression, out decompileOptions),
                JassSyntaxKind.FourCCLiteralExpression => TryDecompileFourCCLiteralExpression(literalExpression, out decompileOptions),
                JassSyntaxKind.HexadecimalLiteralExpression => TryDecompileHexadecimalLiteralExpression(literalExpression, out decompileOptions),
                JassSyntaxKind.RealLiteralExpression => TryDecompileRealLiteralExpression(literalExpression, out decompileOptions),
                JassSyntaxKind.OctalLiteralExpression => TryDecompileOctalLiteralExpression(literalExpression, out decompileOptions),
                JassSyntaxKind.DecimalLiteralExpression => TryDecompileDecimalLiteralExpression(literalExpression, out decompileOptions),
                JassSyntaxKind.TrueLiteralExpression => TryDecompileBooleanLiteralExpression(literalExpression, out decompileOptions),
                JassSyntaxKind.FalseLiteralExpression => TryDecompileBooleanLiteralExpression(literalExpression, out decompileOptions),
                JassSyntaxKind.StringLiteralExpression => TryDecompileStringLiteralExpression(literalExpression, out decompileOptions),
                JassSyntaxKind.NullLiteralExpression => TryDecompileNullLiteralExpression(literalExpression, out decompileOptions),

                _ => throw new NotSupportedException($"Unsupported literal expression kind: {literalExpression.SyntaxKind}"),
            };
        }
    }
}