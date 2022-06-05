// ------------------------------------------------------------------------------
// <copyright file="VJassOperatorMethodDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassOperatorMethodDeclaratorSyntax : IMethodDeclaratorSyntax
    {
        public VJassOperatorMethodDeclaratorSyntax(
            OperatorOverloadType operatorOverloadType,
            VJassParameterListSyntax parameterList,
            VJassTypeSyntax returnType)
        {
            OperatorOverloadType = operatorOverloadType;
            ParameterList = parameterList;
            ReturnType = returnType;
        }

        public OperatorOverloadType OperatorOverloadType { get; }

        public VJassParameterListSyntax ParameterList { get; }

        public VJassTypeSyntax ReturnType { get; }

        public bool Equals(IMethodDeclaratorSyntax? other)
        {
            return other is VJassOperatorMethodDeclaratorSyntax operatorMethodDeclarator
                && OperatorOverloadType.Equals(operatorMethodDeclarator.OperatorOverloadType)
                && ParameterList.Equals(operatorMethodDeclarator.ParameterList)
                && ReturnType.Equals(operatorMethodDeclarator.ReturnType);
        }

        public override string ToString() => $"{VJassKeyword.Method} {VJassKeyword.Operator} {OperatorOverloadType.GetSymbol()} {VJassKeyword.Takes} {ParameterList} {VJassKeyword.Returns} {ReturnType}";
    }
}