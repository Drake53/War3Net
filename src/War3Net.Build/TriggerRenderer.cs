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
using War3Net.Build.Extensions;
using War3Net.Build.Script;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        private readonly IndentedTextWriter _writer;
        private readonly TriggerData _triggerData;
        private readonly ImmutableDictionary<string, string> _variableTypes;
        private readonly bool _isLuaTrigger;

        public TriggerRenderer(
            IndentedTextWriter writer,
            TriggerData triggerData,
            IEnumerable<VariableDefinition> variables,
            bool isLuaTrigger = false)
        {
            _writer = writer;
            _triggerData = triggerData;
            _variableTypes = variables.ToImmutableDictionary(variable => variable.Name, variable => variable.Type, StringComparer.Ordinal);
            _isLuaTrigger = isLuaTrigger;
        }

        public void RenderTrigger(TriggerDefinition triggerDefinition)
        {
            if (triggerDefinition is null)
            {
                throw new ArgumentNullException(nameof(triggerDefinition));
            }

            var commentLine = "//===========================================================================";

            _writer.WriteLine(commentLine);
            _writer.WriteComment($"Trigger: {triggerDefinition.Name}");

            if (!string.IsNullOrEmpty(triggerDefinition.Description))
            {
                _writer.WriteLine(JassSymbol.SlashSlash);

                using var stringReader = new StringReader(triggerDefinition.Description);
                while (true)
                {
                    var line = stringReader.ReadLine();
                    if (line is null)
                    {
                        break;
                    }

                    _writer.WriteComment(line);
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
            var triggerVariableName = triggerDefinition.GetVariableName();

            _writer.WriteFunction(triggerDefinition.GetInitTrigFunctionName());

            _writer.WriteSet(
                triggerVariableName,
                JassExpression.InvokeSpaced(WellKnownNatives.CreateTrigger));

            if (!triggerDefinition.IsInitiallyOn)
            {
                _writer.WriteCall(
                    WellKnownNatives.DisableTrigger,
                    triggerVariableName);
            }

            var identifierBuilder = new TrigFunctionIdentifierBuilder(triggerDefinition.GetTriggerIdentifierName() + "_Func");

            foreach (var function in triggerDefinition.Functions.Where(function => function.Type == TriggerFunctionType.Event && function.IsEnabled))
            {
                if (string.Equals(function.Name, "MapInitializationEvent", StringComparison.Ordinal))
                {
                    continue;
                }

                var arguments = GetParameters(function, identifierBuilder)
                    .Prepend(triggerVariableName)
                    .ToArray();

                _writer.WriteCall(
                    function.Name,
                    arguments);
            }

            if (triggerDefinition.Functions.Any(function => function.Type == TriggerFunctionType.Condition && function.IsEnabled))
            {
                _writer.WriteCall(
                    WellKnownNatives.TriggerAddCondition,
                    triggerVariableName,
                    JassExpression.InvokeSpaced(
                        WellKnownNatives.Condition,
                        JassExpression.FunctionRef(triggerDefinition.GetTrigConditionsFunctionName())));
            }

            _writer.WriteCall(
                WellKnownNatives.TriggerAddAction,
                triggerVariableName,
                JassExpression.FunctionRef(triggerDefinition.GetTrigActionsFunctionName()));

            _writer.EndFunction();
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

        private IEnumerable<string> GetParameters(TriggerFunction function, TrigFunctionIdentifierBuilder identifierBuilder)
        {
            var argumentTypes = GetArgumentTypes(function);

            for (var i = 0; i < argumentTypes.Length; i++)
            {
                yield return GetParameter(function.Parameters[i], argumentTypes[i], i, identifierBuilder);
            }
        }

        private string GetParameter(TriggerFunctionParameter parameter, string type, int parameterIndex, TrigFunctionIdentifierBuilder identifierBuilder)
        {
            identifierBuilder.Append(parameterIndex + 1);
            try
            {
                switch (parameter.Type)
                {
                    case TriggerFunctionParameterType.Preset:
                        var triggerParam = _triggerData.TriggerParams[parameter.Value];
                        if (triggerParam.ScriptText.StartsWith('`') && triggerParam.ScriptText.EndsWith('`'))
                        {
                            return $"{JassSymbol.DoubleQuoteChar}{triggerParam.ScriptText[1..^1]}{JassSymbol.DoubleQuoteChar}";
                        }

                        return triggerParam.ScriptText;

                    case TriggerFunctionParameterType.Variable:
                        var variableName = parameter.Value.StartsWith("gg_", StringComparison.Ordinal)
                            ? parameter.Value
                            : $"udg_{parameter.Value}";

                        return parameter.ArrayIndexer is null
                            ? variableName
                            : JassExpression.ElementAccess(variableName, GetParameter(parameter.ArrayIndexer, JassKeyword.Integer, 0, identifierBuilder));

                    case TriggerFunctionParameterType.Function:
                        if (parameter.Function is null)
                        {
                            throw new ArgumentException("", nameof(parameter));
                        }

                        if (type == "boolexpr")
                        {
                            var conditionFunctionName = identifierBuilder.ToString();
                            RenderConditionFunction(identifierBuilder, conditionFunctionName, parameter);

                            return JassExpression.Invoke(WellKnownNatives.Condition, JassExpression.FunctionRef(conditionFunctionName));
                        }

                        var scriptName = GetScriptName(parameter.Function);

                        if (string.Equals(scriptName, "OperatorInt", StringComparison.Ordinal) ||
                            string.Equals(scriptName, "OperatorReal", StringComparison.Ordinal))
                        {
                            var parameters = GetParameters(parameter.Function, identifierBuilder).ToArray();

                            var @operator = parameters[1];
                            if (@operator.StartsWith('"') && @operator.EndsWith('"'))
                            {
                                @operator = @operator[1..^1];
                            }

                            return JassExpression.ParenthesizedCompact(JassExpression.Binary(
                                parameters[0],
                                @operator,
                                parameters[2]));
                        }
                        else if (string.Equals(scriptName, "OperatorString", StringComparison.Ordinal))
                        {
                            var parameters = GetParameters(parameter.Function, identifierBuilder).ToArray();

                            return JassExpression.ParenthesizedCompact(JassExpression.Add(
                                parameters[0],
                                parameters[1]));
                        }
                        else
                        {
                            return JassExpression.Invoke(
                                scriptName,
                                GetParameters(parameter.Function, identifierBuilder).ToArray());
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
                            return JassLiteral.String(parameter.Value);
                        }
                        else if (knownFourCCTypes.Contains(type))
                        {
                            return JassLiteral.FourCC(parameter.Value);
                        }
                        else
                        {
                            return parameter.Value;
                        }

                    default:
                        throw new InvalidEnumArgumentException(nameof(parameter.Type));
                }
            }
            finally
            {
                identifierBuilder.Remove();
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