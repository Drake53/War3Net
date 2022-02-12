// ------------------------------------------------------------------------------
// <copyright file="TriggerActionRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

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

            var stringBuilder = new StringBuilder();
            using var stringWriter = new StringWriter(stringBuilder);
            var renderer = new JassRenderer(stringWriter);

            var context = new TriggerRendererContext(renderer, identifierBuilder);

            renderer.Render(new JassFunctionCustomScriptAction(SyntaxFactory.FunctionDeclarator(functionName)));
            renderer.RenderNewLine();

            RenderTriggerAction(function, context);

            renderer.Render(JassEndFunctionCustomScriptAction.Value);
            renderer.RenderNewLine();

            _writer.WriteLine(stringBuilder.ToString());
        }

        private void RenderActionFunction(TrigFunctionIdentifierBuilder identifierBuilder, string functionName, List<TriggerFunction> functions)
        {
            identifierBuilder.Append("Func");

            var stringBuilder = new StringBuilder();
            using var stringWriter = new StringWriter(stringBuilder);
            var renderer = new JassRenderer(stringWriter);

            var context = new TriggerRendererContext(renderer, identifierBuilder);

            renderer.Render(new JassFunctionCustomScriptAction(SyntaxFactory.FunctionDeclarator(functionName)));
            renderer.RenderNewLine();

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

            renderer.Render(JassEndFunctionCustomScriptAction.Value);
            renderer.RenderNewLine();

            _writer.WriteLine(stringBuilder.ToString());

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
                    context.Renderer.Render(new JassCommentSyntax(" " + function.Parameters[0].Value));
                    context.Renderer.RenderNewLine();
                    break;

                case "CustomScriptCode":
                    if (_isLuaTrigger)
                    {
                        context.Renderer.Render(new JassCommentSyntax("! beginusercode"));
                        context.Renderer.RenderNewLine();

                        context.Renderer.RenderLine(function.Parameters[0].Value);

                        context.Renderer.Render(new JassCommentSyntax("! endusercode"));
                        context.Renderer.RenderNewLine();
                    }
                    else
                    {
                        context.Renderer.Render(SyntaxFactory.ParseStatementLine(function.Parameters[0].Value));
                        context.Renderer.RenderNewLine();
                    }

                    break;

                case "ReturnAction":
                    context.Renderer.Render(JassReturnStatementSyntax.Empty);
                    context.Renderer.RenderNewLine();
                    break;

                default:
                    context.Renderer.Render(SyntaxFactory.CallStatement(GetScriptName(function), GetParameters(function, context)));
                    context.Renderer.RenderNewLine();
                    break;
            }
        }
    }
}