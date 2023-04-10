// ------------------------------------------------------------------------------
// <copyright file="PredefinedTypeParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassTypeSyntax> GetPredefinedTypeParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return OneOf(
                Keyword.Boolean.AsToken(triviaParser, JassSyntaxKind.BooleanKeyword),
                Keyword.Code.AsToken(triviaParser, JassSyntaxKind.CodeKeyword),
                Keyword.Handle.AsToken(triviaParser, JassSyntaxKind.HandleKeyword),
                Keyword.Integer.AsToken(triviaParser, JassSyntaxKind.IntegerKeyword),
                Keyword.Nothing.AsToken(triviaParser, JassSyntaxKind.NothingKeyword),
                Keyword.Real.AsToken(triviaParser, JassSyntaxKind.RealKeyword),
                Keyword.String.AsToken(triviaParser, JassSyntaxKind.StringKeyword))
                .Select(token => (JassTypeSyntax)new JassPredefinedTypeSyntax(token));
        }
    }
}