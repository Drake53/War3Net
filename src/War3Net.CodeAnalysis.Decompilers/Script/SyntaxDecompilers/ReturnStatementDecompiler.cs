using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Extensions;
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

        private bool TryDecompileReturnStatement(
            JassReturnStatementSyntax returnStatement,
            [NotNullWhen(true)] out TriggerFunction? function)
        {
            var returnExpression = returnStatement.Value.Deparenthesize();

            return TryDecompileConditionExpression(returnExpression, out function);
        }
    }
}