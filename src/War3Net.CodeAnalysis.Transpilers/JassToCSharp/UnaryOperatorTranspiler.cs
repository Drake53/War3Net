// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;

using War3Net.CodeAnalysis.Jass;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public SyntaxKind TranspileUnary(JassSyntaxKind unaryOperatorTokenSyntaxKind)
        {
            return unaryOperatorTokenSyntaxKind switch
            {
                JassSyntaxKind.PlusToken => SyntaxKind.UnaryPlusExpression,
                JassSyntaxKind.MinusToken => SyntaxKind.UnaryPlusExpression,
                JassSyntaxKind.NotKeyword => SyntaxKind.LogicalNotExpression,
            };
        }
    }
}