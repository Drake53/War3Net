// ------------------------------------------------------------------------------
// <copyright file="ForeachLoopRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        private void RenderForeachLoop(TriggerFunction function, TriggerRendererContext context)
        {
            context.TrigFunctionIdentifierBuilder.Append(function.Parameters.Count);
            var actionFunctionName = context.TrigFunctionIdentifierBuilder.ToString();
            RenderActionFunction(context.TrigFunctionIdentifierBuilder, actionFunctionName, function.Parameters[^1]);
            context.TrigFunctionIdentifierBuilder.Remove();

            context.Writer.WriteCall(
                GetScriptName(function),
                GetParameters(function, context.TrigFunctionIdentifierBuilder)
                    .SkipLast(1)
                    .Append(JassExpression.FunctionRef(actionFunctionName))
                    .ToArray());
        }

        private void RenderForeachLoopMultiple(TriggerFunction function, TriggerRendererContext context)
        {
            var actionFunctionName = $"{context.TrigFunctionIdentifierBuilder}A";
            RenderActionFunction(context.TrigFunctionIdentifierBuilder, actionFunctionName, function.ChildFunctions);

            context.Writer.WriteCall(
                GetScriptName(function),
                GetParameters(function, context.TrigFunctionIdentifierBuilder)
                    .Append(JassExpression.FunctionRef(actionFunctionName))
                    .ToArray());
        }
    }
}