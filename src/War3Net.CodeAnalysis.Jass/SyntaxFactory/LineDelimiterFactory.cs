// ------------------------------------------------------------------------------
// <copyright file="LineDelimiterFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static LineDelimiterSyntax LineDelimiter(params EndOfLineSyntax[] eols)
        {
            return new LineDelimiterSyntax(eols);
        }

        public static LineDelimiterSyntax LineDelimiter(IEnumerable<EndOfLineSyntax> eols)
        {
            return LineDelimiter(eols.ToArray());
        }

        public static LineDelimiterSyntax LineDelimiter(params string?[] comments)
        {
            return LineDelimiter(comments.Select(comment => Comment(comment)));
        }

        public static LineDelimiterSyntax Newlines(int amount = 1)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            return LineDelimiter(new string?[amount]);
        }
    }
}