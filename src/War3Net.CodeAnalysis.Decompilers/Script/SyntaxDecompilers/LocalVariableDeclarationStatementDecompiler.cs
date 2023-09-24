// ------------------------------------------------------------------------------
// <copyright file="LocalVariableDeclarationStatementDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileLocalVariableDeclarationStatement(
            JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement,
            ref List<TriggerFunction> functions)
        {
            functions.Add(DecompileCustomScriptAction(localVariableDeclarationStatement.ToString()));
            return true;
        }
    }
}