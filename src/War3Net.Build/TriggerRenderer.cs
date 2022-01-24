// ------------------------------------------------------------------------------
// <copyright file="TriggerRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

using War3Net.Build.Extensions;
using War3Net.Build.Providers;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        private readonly TextWriter _writer;
        private readonly TriggerData _triggerData;
        private readonly ImmutableDictionary<string, string> _variableTypes;

        public TriggerRenderer(TextWriter writer, TriggerData triggerData, IEnumerable<VariableDefinition> variables)
        {
            _writer = writer;
            _triggerData = triggerData;
            _variableTypes = variables.ToImmutableDictionary(variable => variable.Name, variable => variable.Type, StringComparer.Ordinal);
        }

        public void RenderTrigger(TriggerDefinition triggerDefinition)
        {
            if (triggerDefinition is null)
            {
                throw new ArgumentNullException(nameof(triggerDefinition));
            }

            var commentLine = new JassCommentDeclarationSyntax("===========================================================================").ToString();

            _writer.WriteLine(commentLine);
            _writer.WriteLine(new JassCommentDeclarationSyntax($" Trigger: {triggerDefinition.Name}").ToString());

            if (!string.IsNullOrEmpty(triggerDefinition.Description))
            {
                _writer.WriteLine(new JassCommentDeclarationSyntax(string.Empty));

                using var stringReader = new StringReader(triggerDefinition.Description);
                while (true)
                {
                    var line = stringReader.ReadLine();
                    if (line is null)
                    {
                        break;
                    }

                    _writer.WriteLine(new JassCommentDeclarationSyntax($" {line}").ToString());
                }
            }

            _writer.WriteLine(commentLine);

            var identifierBuilder = new TrigFunctionIdentifierBuilder(triggerDefinition.GetTrigIdentifierBaseName());

            if (triggerDefinition.Functions.Any(function => function.Type == TriggerFunctionType.Condition && function.IsEnabled))
            {
                RenderConditionFunction(identifierBuilder, triggerDefinition.GetTrigConditionsFunctionName(), true, triggerDefinition.Functions);
            }

            RenderActionFunction(identifierBuilder, triggerDefinition.GetTrigActionsFunctionName(), triggerDefinition.Functions);

            _writer.WriteLine(commentLine);
            RenderInitTrig(triggerDefinition);
        }

        private void RenderInitTrig(TriggerDefinition triggerDefinition)
        {
            var stringBuilder = new StringBuilder();
            using var stringWriter = new StringWriter(stringBuilder);
            var renderer = new JassRenderer(stringWriter);

            var triggerVariableName = triggerDefinition.GetVariableName();

            var statements = new List<IStatementSyntax>();

            statements.Add(SyntaxFactory.SetStatement(
                triggerVariableName,
                SyntaxFactory.InvocationExpression(WellKnownNatives.CreateTrigger)));

            if (!triggerDefinition.IsInitiallyOn)
            {
                statements.Add(SyntaxFactory.CallStatement(
                    WellKnownNatives.DisableTrigger,
                    SyntaxFactory.VariableReferenceExpression(triggerVariableName)));
            }

            foreach (var function in triggerDefinition.Functions.Where(function => function.Type == TriggerFunctionType.Event && function.IsEnabled))
            {
                if (string.Equals(function.Name, "MapInitializationEvent", StringComparison.Ordinal))
                {
                    continue;
                }

                var stringBuilder2 = new StringBuilder();
                using var stringWriter2 = new StringWriter(stringBuilder2);
                var renderer2 = new JassRenderer(stringWriter2);

                var identifierBuilder = new TrigFunctionIdentifierBuilder(triggerDefinition.GetTriggerIdentifierName() + "_Func");

                var context = new TriggerRendererContext(renderer2, identifierBuilder);

                var argumentListBuilder = ImmutableArray.CreateBuilder<IExpressionSyntax>();
                argumentListBuilder.Add(SyntaxFactory.VariableReferenceExpression(triggerVariableName));
                BuildParameters(function, context, argumentListBuilder);

                statements.Add(SyntaxFactory.CallStatement(function.Name, new JassArgumentListSyntax(argumentListBuilder.ToImmutable())));
            }

            if (triggerDefinition.Functions.Any(function => function.Type == TriggerFunctionType.Condition && function.IsEnabled))
            {
                statements.Add(SyntaxFactory.CallStatement(
                    WellKnownNatives.TriggerAddCondition,
                    SyntaxFactory.VariableReferenceExpression(triggerVariableName),
                    SyntaxFactory.InvocationExpression(WellKnownNatives.Condition, SyntaxFactory.FunctionReferenceExpression(triggerDefinition.GetTrigConditionsFunctionName()))));
            }

            statements.Add(SyntaxFactory.CallStatement(
                WellKnownNatives.TriggerAddAction,
                SyntaxFactory.VariableReferenceExpression(triggerVariableName),
                SyntaxFactory.FunctionReferenceExpression(triggerDefinition.GetTrigActionsFunctionName())));

            renderer.Render(SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(triggerDefinition.GetInitTrigFunctionName()), statements));

            _writer.WriteLine(stringBuilder.ToString());
        }

        private ImmutableArray<string> GetArgumentTypes(TriggerFunction function)
        {
            var argumentTypes = function.Type switch
            {
                TriggerFunctionType.Event => _triggerData.TriggerEvents[function.Name].ArgumentTypes,
                TriggerFunctionType.Condition => _triggerData.TriggerConditions[function.Name].ArgumentTypes,
                TriggerFunctionType.Action => _triggerData.TriggerActions[function.Name].ArgumentTypes,
                TriggerFunctionType.Call => _triggerData.TriggerCalls[function.Name].ArgumentTypes,

                _ => throw new InvalidEnumArgumentException(nameof(function.Type)),
            };

            if (argumentTypes.Length != function.Parameters.Count)
            {
                throw new ArgumentException("", nameof(function));
            }

            return argumentTypes;
        }

        private void BuildParameters(TriggerFunction function, TriggerRendererContext context, ImmutableArray<IExpressionSyntax>.Builder argumentListBuilder)
        {
            var argumentTypes = GetArgumentTypes(function);

            for (var i = 0; i < argumentTypes.Length; i++)
            {
                argumentListBuilder.Add(GetParameter(function.Parameters[i], argumentTypes[i], i, context));
            }
        }

        private void BuildParametersSkipLast(TriggerFunction function, TriggerRendererContext context, ImmutableArray<IExpressionSyntax>.Builder argumentListBuilder)
        {
            var argumentTypes = GetArgumentTypes(function);

            for (var i = 0; i + 1 < argumentTypes.Length; i++)
            {
                argumentListBuilder.Add(GetParameter(function.Parameters[i], argumentTypes[i], i, context));
            }
        }

        private ImmutableArray<IExpressionSyntax>.Builder BuildParameters(TriggerFunction function, TriggerRendererContext context)
        {
            var argumentTypes = GetArgumentTypes(function);

            var argumentListBuilder = ImmutableArray.CreateBuilder<IExpressionSyntax>();
            for (var i = 0; i < argumentTypes.Length; i++)
            {
                argumentListBuilder.Add(GetParameter(function.Parameters[i], argumentTypes[i], i, context));
            }

            return argumentListBuilder;
        }

        private JassArgumentListSyntax GetParameters(TriggerFunction function, TriggerRendererContext context)
        {
            return new JassArgumentListSyntax(BuildParameters(function, context).ToImmutable());
        }

        private IExpressionSyntax GetParameter(TriggerFunctionParameter parameter, string type, int parameterIndex, TriggerRendererContext context)
        {
            context.TrigFunctionIdentifierBuilder.Append(parameterIndex + 1);
            try
            {
                switch (parameter.Type)
                {
                    case TriggerFunctionParameterType.Preset:
                        var triggerParam = _triggerData.TriggerParams[parameter.Value];
                        if (triggerParam.ScriptText.StartsWith('`') && triggerParam.ScriptText.EndsWith('`'))
                        {
                            return SyntaxFactory.ParseExpression($"\"{triggerParam.ScriptText[1..^1]}\"");
                        }

                        return SyntaxFactory.ParseExpression(triggerParam.ScriptText);

                    case TriggerFunctionParameterType.Variable:
                        var variableeName = parameter.Value.StartsWith("gg_", StringComparison.Ordinal)
                            ? parameter.Value
                            : $"udg_{parameter.Value}";

                        return parameter.ArrayIndexer is null
                            ? SyntaxFactory.VariableReferenceExpression(variableeName)
                            : SyntaxFactory.ArrayReferenceExpression(variableeName, GetParameter(parameter.ArrayIndexer, JassKeyword.Integer, 0, context));

                    case TriggerFunctionParameterType.Function:
                        if (parameter.Function is null)
                        {
                            throw new ArgumentException("", nameof(parameter));
                        }

                        if (type == "boolexpr")
                        {
                            var conditionFunctionName = context.TrigFunctionIdentifierBuilder.ToString();
                            RenderConditionFunction(context.TrigFunctionIdentifierBuilder, conditionFunctionName, parameter.Function);

                            return SyntaxFactory.InvocationExpression(WellKnownNatives.Condition, SyntaxFactory.FunctionReferenceExpression(conditionFunctionName));
                        }

                        var scriptName = GetScriptName(parameter.Function);

                        if (string.Equals(scriptName, "OperatorInt", StringComparison.Ordinal) ||
                            string.Equals(scriptName, "OperatorReal", StringComparison.Ordinal))
                        {
                            var parameters = GetParameters(parameter.Function, context);

                            return SyntaxFactory.ParenthesizedExpression(SyntaxFactory.BinaryExpression(
                                parameters.Arguments[0],
                                parameters.Arguments[2],
                                SyntaxFactory.ParseBinaryOperator(((JassStringLiteralExpressionSyntax)parameters.Arguments[1]).Value)));
                        }
                        else if (string.Equals(scriptName, "OperatorString", StringComparison.Ordinal))
                        {
                            var parameters = GetParameters(parameter.Function, context);

                            return SyntaxFactory.ParenthesizedExpression(SyntaxFactory.BinaryExpression(
                                parameters.Arguments[0],
                                parameters.Arguments[1],
                                BinaryOperatorType.Add));
                        }
                        else
                        {
                            return SyntaxFactory.InvocationExpression(scriptName, GetParameters(parameter.Function, context));
                        }

                    case TriggerFunctionParameterType.String:
                        var knownStringTypes = new HashSet<string>(StringComparer.Ordinal)
                        {
                            "StringExt",
                            "stringnoformat",
                            "string",
                            "String",
                            "imagefile",
                            "modelfile",
                            "animationname",
                            "attachpoint",
                        };

                        var knownFourCCTypes = new HashSet<string>(StringComparer.Ordinal)
                        {
                            "unitcode",
                            "techcode",
                            "abilcode",
                            "itemcode",
                        };

                        if (knownStringTypes.Contains(type))
                        {
                            return SyntaxFactory.ParseExpression($"\"{EscapedStringProvider.GetEscapedString(parameter.Value)}\"");
                        }
                        else if (knownFourCCTypes.Contains(type))
                        {
                            return SyntaxFactory.ParseExpression($"'{parameter.Value}'");
                        }
                        else
                        {
                            return SyntaxFactory.ParseExpression(parameter.Value);
                        }

                    default:
                        throw new InvalidEnumArgumentException(nameof(parameter.Type));
                }
            }
            finally
            {
                context.TrigFunctionIdentifierBuilder.Remove();
            }
        }

        private string GetScriptName(TriggerFunction function)
        {
            if (function.Type == TriggerFunctionType.Action)
            {
                var triggerAction = _triggerData.TriggerActions[function.Name];
                return triggerAction.ScriptName ?? triggerAction.FunctionName;
            }
            else
            {
                return function.Name;
            }
        }
    }
}