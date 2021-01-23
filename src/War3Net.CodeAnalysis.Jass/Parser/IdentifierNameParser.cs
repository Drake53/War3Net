// ------------------------------------------------------------------------------
// <copyright file="IdentifierNameParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassIdentifierNameSyntax> GetIdentifierNameParser()
        {
            return Try(Token(c => char.IsLetterOrDigit(c) || c == '_').AtLeastOnceString().Assert(value => !char.IsDigit(value[0])))
                .Then(value => value[0] != '_' && value[^1] != '_' && !JassKeyword.IsKeyword(value)
                    ? Return(new JassIdentifierNameSyntax(value))
                    : Fail<JassIdentifierNameSyntax>($"'{value}' is not a valid identifier name"))
                .SkipWhitespaces()
                .Labelled("identifier name");
        }
    }
}