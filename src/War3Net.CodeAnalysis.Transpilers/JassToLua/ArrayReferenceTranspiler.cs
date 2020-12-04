// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this ArrayReferenceSyntax arrayReferenceNode, ref StringBuilder sb, out bool isString)
        {
            _ = arrayReferenceNode ?? throw new ArgumentNullException(nameof(arrayReferenceNode));

            isString = TranspileStringConcatenationHandler.IsStringVariable(arrayReferenceNode.IdentifierNameNode.ValueText);

            arrayReferenceNode.IdentifierNameNode.TranspileExpression(ref sb);
            sb.Append('[');
            arrayReferenceNode.IndexExpressionNode.Transpile(ref sb);
            sb.Append(']');
        }
    }
}