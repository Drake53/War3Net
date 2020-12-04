// ------------------------------------------------------------------------------
// <copyright file="ArgumentListFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static ArgumentListSyntax ArgumentList(params NewExpressionSyntax[] expressions)
        {
            if (expressions.Length == 0)
            {
                throw new ArgumentException("Array must contain at least one element.", nameof(expressions));
            }

            var tail = expressions.Length == 1
                ? new ArgumentListTailSyntax(Empty())
                : new ArgumentListTailSyntax(expressions.Skip(1).Select(expr => new CommaSeparatedExpressionSyntax(Token(SyntaxTokenType.Comma), expr)).ToArray());

            return new ArgumentListSyntax(expressions[0], tail);
        }
    }
}