// ------------------------------------------------------------------------------
// <copyright file="TriggerActionRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Script;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        private void RenderActionFunction(TrigFunctionIdentifierBuilder identifierBuilder, string functionName, TriggerFunctionParameter parameter)
        {
            if (parameter.Type != TriggerFunctionParameterType.Function || parameter.Function is null)
            {
                throw new ArgumentException("Parameter must have a function and be of type 'Function'.", nameof(parameter));
            }

            var function = parameter.Function;
            if (function.Type != TriggerFunctionType.Action || !function.IsEnabled)
            {
                throw new ArgumentException("Parameter function must be enabled and of type 'Action'.", nameof(parameter));
            }

            using var writer = IndentedTextWriter.New(_writer);

            var context = new TriggerRendererContext(writer, identifierBuilder);

            writer.WriteFunction(functionName);
            RenderTriggerAction(function, context);
            writer.EndFunction();

            _writer.WriteLine(writer.ToString());
        }

        private void RenderActionFunction(TrigFunctionIdentifierBuilder identifierBuilder, string functionName, List<TriggerFunction> functions)
        {
            identifierBuilder.Append("Func");

            using var writer = IndentedTextWriter.New(_writer);

            var context = new TriggerRendererContext(writer, identifierBuilder);

            writer.WriteFunction(functionName);

            for (var i = 0; i < functions.Count; i++)
            {
                var function = functions[i];
                if (function.Type != TriggerFunctionType.Action || !function.IsEnabled)
                {
                    continue;
                }

                context.TrigFunctionIdentifierBuilder.Append(i + 1);
                RenderTriggerAction(function, context);
                context.TrigFunctionIdentifierBuilder.Remove();
            }

            writer.EndFunction();

            _writer.WriteLine(writer.ToString());

            identifierBuilder.Remove();
        }

        private void RenderTriggerAction(TriggerFunction function, TriggerRendererContext context)
        {
            if (function.Type != TriggerFunctionType.Action || !function.IsEnabled)
            {
                throw new ArgumentException("Function must be enabled and of type 'Action'.", nameof(function));
            }

            switch (function.Name)
            {
                case "SetVariable": RenderSetVariable(function, context); break;
                case "WaitForCondition": RenderWaitForCondition(function, context); break;

                case "ForLoopA": RenderForLoopA(function, context); break;
                case "ForLoopAMultiple": RenderForLoopAMultiple(function, context); break;

                case "ForLoopB": RenderForLoopB(function, context); break;
                case "ForLoopBMultiple": RenderForLoopBMultiple(function, context); break;

                case "ForLoopVar": RenderForLoopVar(function, context); break;
                case "ForLoopVarMultiple": RenderForLoopVarMultiple(function, context); break;

                case "IfThenElse": RenderIfThenElse(function, context); break;
                case "IfThenElseMultiple": RenderIfThenElseMultiple(function, context); break;

                case "EnumDestructablesInCircleBJ":
                case "EnumDestructablesInRectAll":
                case "EnumItemsInRectBJ":
                case "ForForce":
                case "ForGroup":
                    RenderForeachLoop(function, context);
                    break;

                case "EnumDestructablesInCircleBJMultiple":
                case "EnumDestructablesInRectAllMultiple":
                case "EnumItemsInRectBJMultiple":
                case "ForForceMultiple":
                case "ForGroupMultiple":
                    RenderForeachLoopMultiple(function, context);
                    break;

                case "CommentString":
                    context.Writer.WriteComment(function.Parameters[0].Value);
                    break;

                case "CustomScriptCode":
                    if (_isLuaTrigger)
                    {
                        context.Writer.WriteLine("//! beginusercode");
                        context.Writer.WriteLine(function.Parameters[0].Value);
                        context.Writer.WriteLine("//! endusercode");
                    }
                    else
                    {
                        context.Writer.WriteLine(function.Parameters[0].Value);
                    }

                    break;

                case "ReturnAction":
                    context.Writer.WriteReturn();
                    break;

                default:
                    context.Writer.WriteCall(
                        GetScriptName(function),
                        GetParameters(function, context.TrigFunctionIdentifierBuilder).ToArray());

                    break;
            }
        }
    }
}