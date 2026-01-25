// ------------------------------------------------------------------------------
// <copyright file="ParameterFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassParameterSyntax Parameter(JassTypeSyntax type, JassIdentifierNameSyntax identifierName)
        {
            return new JassParameterSyntax(
                type,
                identifierName);
        }

        public static JassParameterSyntax Parameter(JassTypeSyntax type, string name)
        {
            return new JassParameterSyntax(
                type,
                ParseIdentifierName(name));
        }

        public static JassParameterSyntax Parameter(string type, JassIdentifierNameSyntax identifierName)
        {
            return new JassParameterSyntax(
                ParseTypeName(type),
                identifierName);
        }

        public static JassParameterSyntax Parameter(string type, string name)
        {
            return new JassParameterSyntax(
                ParseTypeName(type),
                ParseIdentifierName(name));
        }
    }
}