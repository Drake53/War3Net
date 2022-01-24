// ------------------------------------------------------------------------------
// <copyright file="ForLoopRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        private void RenderForLoopA(TriggerFunction function, TriggerRendererContext context)
        {
            RenderForLoop(function, context, "bj_forLoopAIndex", "bj_forLoopAIndexEnd");
        }

        private void RenderForLoopB(TriggerFunction function, TriggerRendererContext context)
        {
            RenderForLoop(function, context, "bj_forLoopBIndex", "bj_forLoopBIndexEnd");
        }

        private void RenderForLoop(TriggerFunction function, TriggerRendererContext context, string indexName, string indexEndName)
        {
            var argumentTypes = GetArgumentTypes(function);

            context.Renderer.Render(SyntaxFactory.SetStatement(indexName, GetParameter(function.Parameters[0], argumentTypes[0], 0, context)));
            context.Renderer.RenderNewLine();
            context.Renderer.Render(SyntaxFactory.SetStatement(indexEndName, GetParameter(function.Parameters[1], argumentTypes[1], 1, context)));
            context.Renderer.RenderNewLine();

            context.Renderer.Render(JassLoopCustomScriptAction.Value);
            context.Renderer.RenderNewLine();

            context.Renderer.Render(SyntaxFactory.ExitStatement(SyntaxFactory.BinaryGreaterThanExpression(
                SyntaxFactory.VariableReferenceExpression(indexName),
                SyntaxFactory.VariableReferenceExpression(indexEndName))));
            context.Renderer.RenderNewLine();

            RenderTriggerAction(function.Parameters[2].Function, context);

            context.Renderer.Render(SyntaxFactory.SetStatement(
                indexName,
                SyntaxFactory.BinaryAdditionExpression(
                    SyntaxFactory.VariableReferenceExpression(indexName),
                    SyntaxFactory.LiteralExpression(1))));
            context.Renderer.RenderNewLine();

            context.Renderer.Render(JassEndLoopCustomScriptAction.Value);
            context.Renderer.RenderNewLine();
        }

        private void RenderForLoopVar(TriggerFunction function, TriggerRendererContext context)
        {
            throw new NotImplementedException();
        }

        private void RenderForLoopAMultiple(TriggerFunction function, TriggerRendererContext context)
        {
            throw new NotImplementedException();
        }

        private void RenderForLoopBMultiple(TriggerFunction function, TriggerRendererContext context)
        {
            throw new NotImplementedException();
        }

        private void RenderForLoopVarMultiple(TriggerFunction function, TriggerRendererContext context)
        {
            throw new NotImplementedException();
        }
    }
}