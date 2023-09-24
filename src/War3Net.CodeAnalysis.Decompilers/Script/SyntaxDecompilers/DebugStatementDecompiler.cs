// ------------------------------------------------------------------------------
// <copyright file="DebugStatementDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileDebugStatement(
            JassDebugStatementSyntax debugStatement,
            ref List<TriggerFunction> functions)
        {
            if (debugStatement.Statement is JassLoopStatementSyntax || debugStatement.Statement is JassIfStatementSyntax)
            {
                throw new NotImplementedException();
            }

            functions.Add(DecompileCustomScriptAction(debugStatement.ToString()));
            return true;
        }
    }
}