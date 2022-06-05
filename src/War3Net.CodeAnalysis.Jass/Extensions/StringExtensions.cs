// ------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class StringExtensions
    {
        public static int ParseDecimal(this string s)
        {
            return int.TryParse(s, out var result)
                ? result
                : BitConverter.ToInt32(BigInteger.Parse(s, CultureInfo.InvariantCulture).ToByteArray().AsSpan()[0..sizeof(int)]);
        }

        public static int ParseOctal(this string s)
        {
            if (s.Length > 11)
            {
                s = s[^11..];
            }

            if (s.Length == 11 && s[0] >= '4')
            {
                s = (char)(s[0] - 4) + s[1..];
            }

            return Convert.ToInt32(s, 8);
        }

        public static int ParseHexadecimal(this string s)
        {
            s = s[(s[0] == '$' ? 1 : 2)..];
            if (s.Length > 8)
            {
                s = s[^8..];
            }

            return Convert.ToInt32(s, 16);
        }

        public static int FromJassRawcode(this string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            if (bytes.Length != 4)
            {
                var p437 = CodePagesEncodingProvider.Instance.GetEncoding(437);
                bytes = p437!.GetBytes(s);

                if (bytes.Length != 4)
                {
                    throw new InvalidDataException($"'{s}' is not a valid fourCC value.");
                }
            }

            var bytesToRead = 4;
            var result = 0;
            for (var i = 0; i < 4; i++)
            {
                var b = bytes[i];
                result |= b << (--bytesToRead * 8);
                if (b >= 0x80 && bytesToRead < 3)
                {
                    result -= 1 << ((bytesToRead + 1) * 8);
                }
            }

            return result;
        }

        public static bool IsJassRawcode(this string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            if (bytes.Length != 4)
            {
                var p437 = CodePagesEncodingProvider.Instance.GetEncoding(437);
                bytes = p437!.GetBytes(s);

                if (bytes.Length != 4)
                {
                    return false;
                }
            }

            return true;
        }
    }
}