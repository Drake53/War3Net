// ------------------------------------------------------------------------------
// <copyright file="VJassAccessorMethodDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassAccessorMethodDeclaratorSyntax : IMethodDeclaratorSyntax
    {
        public VJassAccessorMethodDeclaratorSyntax(
            VJassIdentifierNameSyntax identifierName,
            AccessorType accessorType,
            VJassParameterListSyntax parameterList,
            VJassTypeSyntax returnType)
        {
            IdentifierName = identifierName;
            AccessorType = accessorType;
            ParameterList = parameterList;
            ReturnType = returnType;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public AccessorType AccessorType { get; }

        public VJassParameterListSyntax ParameterList { get; }

        public VJassTypeSyntax ReturnType { get; }

        public bool Equals(IMethodDeclaratorSyntax? other)
        {
            return other is VJassAccessorMethodDeclaratorSyntax accessorMethodDeclarator
                && IdentifierName.Equals(accessorMethodDeclarator.IdentifierName)
                && AccessorType.Equals(accessorMethodDeclarator.AccessorType)
                && ParameterList.Equals(accessorMethodDeclarator.ParameterList)
                && ReturnType.Equals(accessorMethodDeclarator.ReturnType);
        }

        public override string ToString() => $"{VJassKeyword.Method} {VJassKeyword.Operator} {IdentifierName}{AccessorType.GetSymbol()} {VJassKeyword.Takes} {ParameterList} {VJassKeyword.Returns} {ReturnType}";
    }
}