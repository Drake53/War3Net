// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclaratorFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassFunctionDeclaratorSyntax FunctionDeclarator(string name)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(JassTypeSyntax.Nothing));
        }

        public static JassFunctionDeclaratorSyntax ConditionFunctionDeclarator(string name)
        {
            return new JassFunctionDeclaratorSyntax(
                null,
                Token(JassSyntaxKind.FunctionKeyword),
                ParseIdentifierName(name),
                JassEmptyParameterListSyntax.Value,
                ReturnClause(JassTypeSyntax.Boolean));
        }
    }
}