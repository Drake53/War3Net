// ------------------------------------------------------------------------------
// <copyright file="ExitStatementDecompiler.cs" company="Drake53">
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
        private bool TryDecompileExitStatement(
            JassExitStatementSyntax exitStatement,
            ref List<TriggerFunction> functions)
        {
            functions.Add(DecompileCustomScriptAction(exitStatement.ToString()));
            return true;
        }
    }
}