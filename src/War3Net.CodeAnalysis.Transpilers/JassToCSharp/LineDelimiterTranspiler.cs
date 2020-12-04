// ------------------------------------------------------------------------------
// <copyright file="LineDelimiterTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static SyntaxTrivia Transpile(this Jass.Syntax.LineDelimiterSyntax lineDelimiterNode)
        {
            _ = lineDelimiterNode ?? throw new ArgumentNullException(nameof(lineDelimiterNode));

            var comment = SyntaxFactory.Comment(lineDelimiterNode.Select(eol => eol.Transpile()).Aggregate((accum, next) => accum + next));

            return comment;
        }
    }
}