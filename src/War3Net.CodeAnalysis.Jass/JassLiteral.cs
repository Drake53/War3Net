// ------------------------------------------------------------------------------
// <copyright file="JassLiteral.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;

using War3Net.CodeAnalysis.Jass.Extensions;
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
            return value.ToString($"F{decimals}", CultureInfo.InvariantCulture);
        }

        public static string Real(double value, int decimals = 1)
        {
            return value.ToString($"F{decimals}", CultureInfo.InvariantCulture);
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

        public static float ParseReal(string value)
        {
            return float.Parse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
        }

        public static char ParseChar(string value)
        {
            if (value[1] == '\\')
            {
                return value[2] switch
                {
                    'r' => '\r',
                    'n' => '\n',
                    't' => '\t',
                    'b' => '\b',
                    'f' => '\f',
                    _ => value[2],
                };
            }

            return value[1];
        }

        public static string ParseString(string value)
        {
            return value[1..^1];
        }

        public static int ParseInt(string value)
        {
            return int.Parse(value, NumberStyles.None, CultureInfo.InvariantCulture);
        }

        public static int ParseHex(string value)
        {
            var hexDigits = value.StartsWith(JassSymbol.DollarChar) ? value[1..] : value[2..];
            return Convert.ToInt32(hexDigits, 16);
        }

        public static int ParseOctal(string value)
        {
            return Convert.ToInt32(value, 8);
        }

        public static int ParseFourCC(string value)
        {
            return value[1..^1].FromJassRawcode();
        }
    }
}