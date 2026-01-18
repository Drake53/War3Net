// ------------------------------------------------------------------------------
// <copyright file="JassLiteral.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;

using War3Net.Common.Extensions;

namespace War3Net.CodeAnalysis.Jass
{
    public static class JassLiteral
    {
        public static string Int(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string Real(float value, int decimals = 1)
        {
            var rounded = MathF.Round(value, decimals, MidpointRounding.AwayFromZero);
            var format = $"0.{new string('0', decimals)}";
            return rounded.ToString(format, CultureInfo.InvariantCulture);
        }

        public static string Real(double value, int decimals = 1)
        {
            var rounded = Math.Round(value, decimals, MidpointRounding.AwayFromZero);
            var format = $"0.{new string('0', decimals)}";
            return rounded.ToString(format, CultureInfo.InvariantCulture);
        }

        public static string String(string? value)
        {
            return $"{JassSymbol.DoubleQuoteChar}{EscapedStringProvider.GetEscapedString(value ?? string.Empty)}{JassSymbol.DoubleQuoteChar}";
        }

        public static string Bool(bool value)
        {
            return value ? JassKeyword.True : JassKeyword.False;
        }

        public static string FourCC(int value)
        {
            return $"{JassSymbol.SingleQuoteChar}{value.ToRawcode()}{JassSymbol.SingleQuoteChar}";
        }

        public static string FourCC(string value)
        {
            return $"{JassSymbol.SingleQuoteChar}{value}{JassSymbol.SingleQuoteChar}";
        }
    }
}