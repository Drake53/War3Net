// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, Func<JassIdentifierNameSyntax, JassExpressionSyntax>> GetArrayReferenceExpressionParser(
            Parser<char, JassElementAccessClauseSyntax> elementAccessClauseParser)
        {
            return Map<char, JassElementAccessClauseSyntax, Func<JassIdentifierNameSyntax, JassExpressionSyntax>>(
                (elementAccessClause) => identifierName => new JassArrayReferenceExpressionSyntax(
                    identifierName,
                    elementAccessClause),
                elementAccessClauseParser);
        }
    }
}