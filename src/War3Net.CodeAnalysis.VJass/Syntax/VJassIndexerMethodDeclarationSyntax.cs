// ------------------------------------------------------------------------------
// <copyright file="VJassIndexerMethodDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassIndexerMethodDeclarationSyntax : IMethodDeclaratorSyntax
    {
        public VJassIndexerMethodDeclarationSyntax(
            AccessorType accessorType,
            VJassParameterListSyntax parameterList,
            VJassTypeSyntax returnType)
        {
            AccessorType = accessorType;
            ParameterList = parameterList;
            ReturnType = returnType;
        }

        public AccessorType AccessorType { get; }

        public VJassParameterListSyntax ParameterList { get; }

        public VJassTypeSyntax ReturnType { get; }

        public bool Equals(IMethodDeclaratorSyntax? other)
        {
            return other is VJassIndexerMethodDeclarationSyntax indexerMethodDeclaration
                && AccessorType.Equals(indexerMethodDeclaration.AccessorType)
                && ParameterList.Equals(indexerMethodDeclaration.ParameterList)
                && ReturnType.Equals(indexerMethodDeclaration.ReturnType);
        }

        public override string ToString() => $"{VJassKeyword.Method} {VJassKeyword.Operator} {VJassSymbol.LeftSquareBracket}{VJassSymbol.RightSquareBracket}{AccessorType.GetSymbol()} {VJassKeyword.Takes} {ParameterList} {VJassKeyword.Returns} {ReturnType}";
    }
}