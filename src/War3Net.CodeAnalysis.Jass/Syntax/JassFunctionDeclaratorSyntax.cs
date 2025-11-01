// ------------------------------------------------------------------------------
// <copyright file="JassFunctionDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassFunctionDeclaratorSyntax : IEquatable<JassFunctionDeclaratorSyntax>, IJassSyntaxToken
    {
        public JassFunctionDeclaratorSyntax(JassIdentifierNameSyntax identifierName, JassParameterListSyntax parameterList, JassTypeSyntax returnType)
        {
            IdentifierName = identifierName;
            ParameterList = parameterList;
            ReturnType = returnType;
        }

        public JassIdentifierNameSyntax IdentifierName { get; init; }

        public JassParameterListSyntax ParameterList { get; init; }

        public JassTypeSyntax ReturnType { get; init; }

        public bool Equals(JassFunctionDeclaratorSyntax? other)
        {
            return other is not null
                && IdentifierName.Equals(other.IdentifierName)
                && ParameterList.Equals(other.ParameterList)
                && ReturnType.Equals(other.ReturnType);
        }

        public override string ToString() => $"{IdentifierName} {JassKeyword.Takes} {ParameterList} {JassKeyword.Returns} {ReturnType}";
    }
}