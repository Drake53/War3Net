// ------------------------------------------------------------------------------
// <copyright file="TriggerConditionRenderer.cs" company="Drake53">
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
        private void RenderConditionFunction(TrigFunctionIdentifierBuilder identifierBuilder, string functionName, TriggerFunctionParameter parameter)
        {
            if (parameter.Type != TriggerFunctionParameterType.Function || parameter.Function is null)
            {
                throw new ArgumentException("Parameter must have a function and be of type 'Function'.", nameof(parameter));
            }

            var function = parameter.Function;
            if (function.Type != TriggerFunctionType.Condition || !function.IsEnabled)
            {
                throw new ArgumentException("Parameter function must be enabled and of type 'Condition'.", nameof(parameter));
            }

            var stringBuilder = new StringBuilder();
            using var stringWriter = new StringWriter(stringBuilder);
            var renderer = new JassRenderer(stringWriter);

            var context = new TriggerRendererContext(renderer, identifierBuilder);

            renderer.Render(SyntaxFactory.ConditionFunctionDeclarator(functionName));
            renderer.RenderNewLine();

            var expression = GetTriggerConditionExpression(function, context);

            renderer.Render(SyntaxFactory.ReturnStatement(expression));
            renderer.RenderNewLine();

            renderer.Render(SyntaxFactory.Token(JassSyntaxKind.EndFunctionKeyword));
            renderer.RenderNewLine();

            _writer.WriteLine(stringBuilder.ToString());
        }

        private void RenderConditionFunction(TrigFunctionIdentifierBuilder identifierBuilder, string functionName, bool returnValue, List<TriggerFunction> functions)
        {
            identifierBuilder.Append("Func");

            var stringBuilder = new StringBuilder();
            using var stringWriter = new StringWriter(stringBuilder);
            var renderer = new JassRenderer(stringWriter);

            var context = new TriggerRendererContext(renderer, identifierBuilder);

            renderer.Render(SyntaxFactory.ConditionFunctionDeclarator(functionName));
            renderer.RenderNewLine();

            for (var i = 0; i < functions.Count; i++)
            {
                var function = functions[i];
                if (function.Type != TriggerFunctionType.Condition || !function.IsEnabled)
                {
                    continue;
                }

                context.TrigFunctionIdentifierBuilder.Append(i + 1);
                var expression = GetTriggerConditionExpression(function, context);
                context.TrigFunctionIdentifierBuilder.Remove();

                if (returnValue)
                {
                    context.Renderer.Render(SyntaxFactory.IfStatement(
                        SyntaxFactory.ParenthesizedExpression(SyntaxFactory.UnaryNotExpression(expression)),
                        SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(false))));
                    context.Renderer.RenderNewLine();
                }
                else
                {
                    context.Renderer.Render(SyntaxFactory.IfStatement(
                        SyntaxFactory.ParenthesizedExpression(expression),
                        SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(true))));
                    context.Renderer.RenderNewLine();
                }
            }

            context.Renderer.Render(SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(returnValue)));
            context.Renderer.RenderNewLine();

            renderer.Render(SyntaxFactory.Token(JassSyntaxKind.EndFunctionKeyword));
            renderer.RenderNewLine();

            _writer.WriteLine(stringBuilder.ToString());

            identifierBuilder.Remove();
        }

        private JassExpressionSyntax GetTriggerConditionExpression(TriggerFunction function, TriggerRendererContext context)
        {
            if (function.Type != TriggerFunctionType.Condition || !function.IsEnabled)
            {
                throw new ArgumentException("Function must be enabled and of type 'Condition'.", nameof(function));
            }

            if (function.Name == "OrMultiple" || function.Name == "AndMultiple")
            {
                var conditionFunctionName = $"{context.TrigFunctionIdentifierBuilder}C";
                RenderConditionFunction(context.TrigFunctionIdentifierBuilder, conditionFunctionName, function.Name == "AndMultiple", function.ChildFunctions);

                return SyntaxFactory.InvocationExpression(conditionFunctionName);
            }
            else if (function.Name == "GetBooleanAnd" || function.Name == "GetBooleanOr")
            {
                context.TrigFunctionIdentifierBuilder.Append(1);
                var conditionFunctionName1 = context.TrigFunctionIdentifierBuilder.ToString();
                RenderConditionFunction(context.TrigFunctionIdentifierBuilder, conditionFunctionName1, function.Parameters[0]);
                context.TrigFunctionIdentifierBuilder.Remove();

                context.TrigFunctionIdentifierBuilder.Append(2);
                var conditionFunctionName2 = context.TrigFunctionIdentifierBuilder.ToString();
                RenderConditionFunction(context.TrigFunctionIdentifierBuilder, conditionFunctionName2, function.Parameters[1]);
                context.TrigFunctionIdentifierBuilder.Remove();

                return SyntaxFactory.InvocationExpression(
                    function.Name,
                    SyntaxFactory.InvocationExpression(conditionFunctionName1),
                    SyntaxFactory.InvocationExpression(conditionFunctionName2));
            }
            else
            {
                var parameters = GetParameters(function, context);

                return SyntaxFactory.ParenthesizedExpression(SyntaxFactory.BinaryExpression(
                    parameters.ArgumentList.Items[0],
                    SyntaxFactory.ParseBinaryOperator(((JassLiteralExpressionSyntax)parameters.ArgumentList.Items[1]).Token.Text),
                    parameters.ArgumentList.Items[2]));
            }
        }
    }
}