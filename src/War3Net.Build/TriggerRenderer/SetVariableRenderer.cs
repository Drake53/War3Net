// ------------------------------------------------------------------------------
// <copyright file="SetVariableRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Build.Script;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        private void RenderSetVariable(TriggerFunction function, TriggerRendererContext context)
        {
            var variableParameter = function.Parameters[0];
            var valueParameter = function.Parameters[1];

            if (!_variableTypes.TryGetValue(variableParameter.Value, out var type))
            {
                throw new InvalidOperationException($"Unable to determine the type of the global variable '{variableParameter.Value}'.");
            }

            if (variableParameter.ArrayIndexer is not null)
            {
                context.Renderer.Render(SyntaxFactory.SetStatement(
                    $"udg_{variableParameter.Value}",
                    GetParameter(variableParameter.ArrayIndexer, "integer", 0, context),
                    GetParameter(valueParameter, type, 1, context)));
            }
            else
            {
                context.Renderer.Render(SyntaxFactory.SetStatement(
                    $"udg_{variableParameter.Value}",
                    GetParameter(valueParameter, type, 1, context)));
            }

            context.Renderer.RenderNewLine();
        }
    }
}