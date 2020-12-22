// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        [return: NotNullIfNotNull("arrayReference")]
        public LuaExpressionSyntax? Transpile(ArrayReferenceSyntax? arrayReference, out SyntaxTokenType expressionType)
        {
            if (arrayReference is null)
            {
                expressionType = SyntaxTokenType.NullKeyword;
                return null;
            }

            expressionType = GetVariableType(arrayReference.IdentifierNameNode.ValueText);

            return new LuaTableIndexAccessExpressionSyntax(
                TranspileIdentifier(arrayReference.IdentifierNameNode),
                Transpile(arrayReference.IndexExpressionNode));
        }
    }
}