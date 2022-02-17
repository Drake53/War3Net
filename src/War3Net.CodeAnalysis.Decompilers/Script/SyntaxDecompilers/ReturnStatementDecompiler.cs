// ------------------------------------------------------------------------------
// <copyright file="ReturnStatementDecompiler.cs" company="Drake53">
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
        private bool TryDecompileReturnStatement(
            JassReturnStatementSyntax returnStatement,
            ref List<TriggerFunction> functions)
        {
            if (returnStatement.Value is null)
            {
                functions.Add(new TriggerFunction
                {
                    Type = TriggerFunctionType.Action,
                    IsEnabled = true,
                    Name = "ReturnAction",
                });

                return true;
            }

            return false;
        }
    }
}