﻿// ------------------------------------------------------------------------------
// <copyright file="JassFourCCLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    [Obsolete]
    public class JassFourCCLiteralExpressionSyntax : IExpressionSyntax
    {
        public JassFourCCLiteralExpressionSyntax(int value)
        {
            Value = value;
        }

        public int Value { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is JassFourCCLiteralExpressionSyntax fourCCLiteralExpression
                && Value == fourCCLiteralExpression.Value;
        }

        public override string ToString() => $"{JassSymbol.SingleQuoteChar}{Value.ToJassRawcode()}{JassSymbol.SingleQuoteChar}";
    }
}