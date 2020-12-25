// ------------------------------------------------------------------------------
// <copyright file="EscapedStringProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace War3Net.Build.Providers
{
    // Based on https://referencesource.microsoft.com/#System/regex/system/text/regularexpressions/RegexParser.cs,845bf727ea7a0421
    public static class EscapedStringProvider
    {
        /// <summary>
        /// Modified <see cref="System.Text.RegularExpressions.Regex.Escape(string)"/> to only escape \\, \n, \r, \t, and \f.
        /// </summary>
        [return: NotNullIfNotNull("input")]
        public static string? GetEscapedString(string? input)
        {
            if (input is null)
            {
                return null;
            }

            for (var i = 0; i < input.Length; i++)
            {
                if (IsMetachar(input[i]))
                {
                    var sb = new StringBuilder();
                    var ch = input[i];
                    int lastpos;

                    sb.Append(input, 0, i);
                    do
                    {
                        switch (ch)
                        {
                            case '\\':
                                sb.Append(@"\\");
                                break;
                            case '\n':
                                sb.Append(@"\n");
                                break;
                            case '\r':
                                sb.Append(@"\r");
                                break;
                            case '\t':
                                sb.Append(@"\t");
                                break;
                            case '\f':
                                sb.Append(@"\f");
                                break;
                        }

                        i++;
                        lastpos = i;

                        while (i < input.Length)
                        {
                            ch = input[i];
                            if (IsMetachar(ch))
                            {
                                break;
                            }

                            i++;
                        }

                        sb.Append(input, lastpos, i - lastpos);
                    }
                    while (i < input.Length);

                    return sb.ToString();
                }
            }

            return input;
        }

        private static bool IsMetachar(char ch)
        {
            return ch == '\\'
                || ch == '\n'
                || ch == '\r'
                || ch == '\t'
                || ch == '\f';
        }
    }
}