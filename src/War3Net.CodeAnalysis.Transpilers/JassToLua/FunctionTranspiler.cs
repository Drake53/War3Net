// ------------------------------------------------------------------------------
// <copyright file="FunctionTranspiler.cs" company="Drake53">
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
        public static void Transpile(this FunctionSyntax functionNode, ref StringBuilder sb)
        {
            _ = functionNode ?? throw new ArgumentNullException(nameof(functionNode));

            functionNode.FunctionDeclarationNode.Transpile(ref sb);
            functionNode.DeclarationLineDelimiterNode.Transpile(ref sb);
            functionNode.LocalVariableListNode.Transpile(ref sb);
            functionNode.StatementListNode.Transpile(ref sb);
            sb.Append("end");
            functionNode.LastLineDelimiterNode.Transpile(ref sb);

            TranspileStringConcatenationHandler.ResetLocalVariables();
        }
    }
}