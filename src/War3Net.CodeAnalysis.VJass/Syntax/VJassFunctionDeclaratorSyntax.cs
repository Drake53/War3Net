// ------------------------------------------------------------------------------
// <copyright file="VJassFunctionDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassFunctionDeclaratorSyntax : IEquatable<VJassFunctionDeclaratorSyntax>
    {
        public VJassFunctionDeclaratorSyntax(
            VJassIdentifierNameSyntax identifierName,
            VJassParameterListSyntax parameterList,
            VJassTypeSyntax returnType)
        {
            IdentifierName = identifierName;
            ParameterList = parameterList;
            ReturnType = returnType;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassParameterListSyntax ParameterList { get; }

        public VJassTypeSyntax ReturnType { get; }

        public bool Equals(VJassFunctionDeclaratorSyntax? other)
        {
            return other is not null
                && IdentifierName.Equals(other.IdentifierName)
                && ParameterList.Equals(other.ParameterList)
                && ReturnType.Equals(other.ReturnType);
        }

        public override string ToString() => $"{IdentifierName} {VJassKeyword.Takes} {ParameterList} {VJassKeyword.Returns} {ReturnType}";
    }
}