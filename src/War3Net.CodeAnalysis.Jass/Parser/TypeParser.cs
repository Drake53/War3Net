// ------------------------------------------------------------------------------
// <copyright file="TypeParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        private static readonly Dictionary<string, JassSyntaxKind> _predefinedTypeSyntaxKinds = GetPredefinedTypeSyntaxKinds();

        internal static Parser<char, JassTypeSyntax> GetTypeParser(
            Parser<char, string> identifierParser,
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (text, trivia) => new JassSyntaxToken(
                    _predefinedTypeSyntaxKinds.GetValueOrDefault(text, JassSyntaxKind.IdentifierToken),
                    text,
                    trivia),
                identifierParser.Assert(JassSyntaxFacts.IsNotReservedKeyword),
                triviaParser).Select<JassTypeSyntax>(token => token.SyntaxKind == JassSyntaxKind.IdentifierToken
                    ? new JassIdentifierNameSyntax(token)
                    : new JassPredefinedTypeSyntax(token));
        }

        private static Dictionary<string, JassSyntaxKind> GetPredefinedTypeSyntaxKinds()
        {
            return new Dictionary<string, JassSyntaxKind>
            {
                { JassKeyword.Boolean, JassSyntaxKind.BooleanKeyword },
                { JassKeyword.Code, JassSyntaxKind.CodeKeyword },
                { JassKeyword.Handle, JassSyntaxKind.HandleKeyword },
                { JassKeyword.Integer, JassSyntaxKind.IntegerKeyword },
                { JassKeyword.Nothing, JassSyntaxKind.NothingKeyword },
                { JassKeyword.Real, JassSyntaxKind.RealKeyword },
                { JassKeyword.String, JassSyntaxKind.StringKeyword },
            };
        }
    }
}