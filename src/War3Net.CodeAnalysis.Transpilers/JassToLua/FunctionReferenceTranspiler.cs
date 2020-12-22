// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceTranspiler.cs" company="Drake53">
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
        [return: NotNullIfNotNull("functionReference")]
        public LuaExpressionSyntax? Transpile(FunctionReferenceSyntax? functionReference, out SyntaxTokenType expressionType)
        {
            if (functionReference is null)
            {
                expressionType = SyntaxTokenType.NullKeyword;
                return null;
            }

            expressionType = SyntaxTokenType.CodeKeyword;

            return TranspileExpression(functionReference.IdentifierNameNode);
        }
    }
}