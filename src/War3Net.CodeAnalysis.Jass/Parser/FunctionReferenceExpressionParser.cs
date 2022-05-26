// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, IExpressionSyntax> GetFunctionReferenceExpressionParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, Unit> whitespaceParser)
        {
            return Keyword.Function.Then(whitespaceParser).Then(identifierNameParser)
                .Select<IExpressionSyntax>(name => new JassFunctionReferenceExpressionSyntax(name))
                .Labelled("function reference");
        }
    }
}