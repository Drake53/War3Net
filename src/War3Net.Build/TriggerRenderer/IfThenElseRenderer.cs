// ------------------------------------------------------------------------------
// <copyright file="IfThenElseRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        private void RenderIfThenElse(TriggerFunction function, TriggerRendererContext context)
        {
            context.TrigFunctionIdentifierBuilder.Append(1);
            var conditionFunctionName = context.TrigFunctionIdentifierBuilder.ToString();
            RenderConditionFunction(context.TrigFunctionIdentifierBuilder, conditionFunctionName, function.Parameters[0].Function);
            context.TrigFunctionIdentifierBuilder.Remove();

            context.Renderer.Render(new JassIfCustomScriptAction(SyntaxFactory.ParenthesizedExpression(SyntaxFactory.InvocationExpression(conditionFunctionName))));
            context.Renderer.RenderNewLine();

            context.TrigFunctionIdentifierBuilder.Append(2);
            RenderTriggerAction(function.Parameters[1].Function, context);
            context.TrigFunctionIdentifierBuilder.Remove();

            context.Renderer.Render(JassElseCustomScriptAction.Value);
            context.Renderer.RenderNewLine();

            context.TrigFunctionIdentifierBuilder.Append(3);
            RenderTriggerAction(function.Parameters[2].Function, context);
            context.TrigFunctionIdentifierBuilder.Remove();

            context.Renderer.Render(JassEndIfCustomScriptAction.Value);
            context.Renderer.RenderNewLine();
        }

        private void RenderIfThenElseMultiple(TriggerFunction function, TriggerRendererContext context)
        {
            var conditionFunctionName = $"{context.TrigFunctionIdentifierBuilder}C";
            RenderConditionFunction(context.TrigFunctionIdentifierBuilder, conditionFunctionName, true, function.ChildFunctions);

            context.Renderer.Render(new JassIfCustomScriptAction(SyntaxFactory.ParenthesizedExpression(SyntaxFactory.InvocationExpression(conditionFunctionName))));
            context.Renderer.RenderNewLine();

            context.TrigFunctionIdentifierBuilder.Append("Func");

            for (var i = 0; i < function.ChildFunctions.Count; i++)
            {
                var childFunction = function.ChildFunctions[i];
                if (childFunction.Branch == 1 && childFunction.IsEnabled)
                {
                    context.TrigFunctionIdentifierBuilder.Append(i + 1);
                    RenderTriggerAction(childFunction, context);
                    context.TrigFunctionIdentifierBuilder.Remove();
                }
            }

            context.Renderer.Render(JassElseCustomScriptAction.Value);
            context.Renderer.RenderNewLine();

            for (var i = 0; i < function.ChildFunctions.Count; i++)
            {
                var childFunction = function.ChildFunctions[i];
                if (childFunction.Branch == 2 && childFunction.IsEnabled)
                {
                    context.TrigFunctionIdentifierBuilder.Append(i + 1);
                    RenderTriggerAction(childFunction, context);
                    context.TrigFunctionIdentifierBuilder.Remove();
                }
            }

            context.TrigFunctionIdentifierBuilder.Remove();

            context.Renderer.Render(JassEndIfCustomScriptAction.Value);
            context.Renderer.RenderNewLine();
        }
    }
}