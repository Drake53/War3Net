// ------------------------------------------------------------------------------
// <copyright file="VJassMethodDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassMethodDeclaratorSyntax : IMethodDeclaratorSyntax
    {
        public VJassMethodDeclaratorSyntax(
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

        public bool Equals(IMethodDeclaratorSyntax? other)
        {
            return other is VJassMethodDeclaratorSyntax methodDeclarator
                && IdentifierName.Equals(methodDeclarator.IdentifierName)
                && ParameterList.Equals(methodDeclarator.ParameterList)
                && ReturnType.Equals(methodDeclarator.ReturnType);
        }

        public override string ToString() => $"{VJassKeyword.Method} {IdentifierName} {VJassKeyword.Takes} {ParameterList} {VJassKeyword.Returns} {ReturnType}";
    }
}