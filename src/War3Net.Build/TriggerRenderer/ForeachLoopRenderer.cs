// ------------------------------------------------------------------------------
// <copyright file="ForeachLoopRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Linq;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        private void RenderForeachLoop(TriggerFunction function, TriggerRendererContext context)
        {
            context.TrigFunctionIdentifierBuilder.Append(function.Parameters.Count);
            var actionFunctionName = context.TrigFunctionIdentifierBuilder.ToString();
            RenderActionFunction(context.TrigFunctionIdentifierBuilder, actionFunctionName, function.Parameters.Last().Function);
            context.TrigFunctionIdentifierBuilder.Remove();

            var argumentListBuilder = ImmutableArray.CreateBuilder<IExpressionSyntax>();
            BuildParametersSkipLast(function, context, argumentListBuilder);
            argumentListBuilder.Add(SyntaxFactory.FunctionReferenceExpression(actionFunctionName));

            context.Renderer.Render(SyntaxFactory.CallStatement(GetScriptName(function), new JassArgumentListSyntax(argumentListBuilder.ToImmutable())));
            context.Renderer.RenderNewLine();
        }

        private void RenderForeachLoopMultiple(TriggerFunction function, TriggerRendererContext context)
        {
            var actionFunctionName = $"{context.TrigFunctionIdentifierBuilder}A";
            RenderActionFunction(context.TrigFunctionIdentifierBuilder, actionFunctionName, function.ChildFunctions);

            var argumentListBuilder = BuildParameters(function, context);
            argumentListBuilder.Add(SyntaxFactory.FunctionReferenceExpression(actionFunctionName));

            context.Renderer.Render(SyntaxFactory.CallStatement(GetScriptName(function), new JassArgumentListSyntax(argumentListBuilder.ToImmutable())));
            context.Renderer.RenderNewLine();
        }
    }
}