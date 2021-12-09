// ------------------------------------------------------------------------------
// <copyright file="TriggerData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using War3Net.Build.Resources;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        private static readonly Lazy<TriggerData> _defaultTriggerData = new Lazy<TriggerData>(() => ParseText(DefaultTriggerData.TriggerData));

        private readonly Dictionary<string, TriggerType> _triggerTypes;
        private readonly Dictionary<string, TriggerTypeDefault> _triggerTypeDefaults;
        private readonly Dictionary<string, TriggerEvent> _triggerEvents;
        private readonly Dictionary<string, TriggerCondition> _triggerConditions;
        private readonly Dictionary<string, TriggerAction> _triggerActions;
        private readonly Dictionary<string, TriggerCall> _triggerCalls;
        private readonly Dictionary<string, TriggerParam> _triggerParams;

        private Dictionary<string, TriggerCondition> _triggerConditionsLookup;
        private Dictionary<string, Dictionary<int, TriggerAction>> _triggerActionsLookup;
        private Dictionary<string, TriggerParam> _variableTypePresets;
        private Dictionary<string, Dictionary<string, TriggerParam>> _triggerParamsLookup;

        private TriggerData()
        {
            _triggerTypes = new();
            _triggerTypeDefaults = new();
            _triggerEvents = new();
            _triggerConditions = new();
            _triggerActions = new(StringComparer.Ordinal);
            _triggerCalls = new();
            _triggerParams = new();
        }

        public static TriggerData Default => _defaultTriggerData.Value;

        public static TriggerData ParseFile(string filePath) => ParseStream(File.OpenRead(filePath));

        public static TriggerData ParseText(string text)
        {
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, leaveOpen: true))
            {
                writer.Write(text);
            }

            stream.Position = 0;
            return ParseStream(stream);
        }

        public static TriggerData ParseStream(Stream stream, bool leaveOpen = false)
        {
            var result = new TriggerData();

            using var reader = new StreamReader(stream, leaveOpen: leaveOpen);

            object? target = null;
            MethodInfo? addMethod = null;
            PropertyInfo? getProperty = null;
            ConstructorInfo? constructor = null;
            MethodInfo? additionalPropertySetter = null;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                if (line.StartsWith('[') && line.EndsWith(']'))
                {
                    target = line switch
                    {
                        "[TriggerTypes]" => result._triggerTypes,
                        "[TriggerTypeDefaults]" => result._triggerTypeDefaults,
                        "[TriggerEvents]" => result._triggerEvents,
                        "[TriggerConditions]" => result._triggerConditions,
                        "[TriggerActions]" => result._triggerActions,
                        "[TriggerCalls]" => result._triggerCalls,
                        "[TriggerParams]" => result._triggerParams,

                        _ => null,
                    };

                    if (target is not null)
                    {
                        // Get Dictionary<string, T> add method, this[string], and T constructor, propertySetter.
                        addMethod = target.GetType().GetMethod("Add");
                        getProperty = target.GetType().GetProperty("Item");
                        constructor = target.GetType().GetGenericArguments()[1].GetConstructors().Single();
                        additionalPropertySetter = target.GetType().GetGenericArguments()[1].GetMethod("SetAdditionalProperty");
                    }

                    continue;
                }

                if (line.StartsWith('_'))
                {
                    if (additionalPropertySetter is null)
                    {
                        continue;
                    }

                    var split = line.Split('_', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length != 2)
                    {
                        continue;
                    }

                    var key = split[0];
                    object? obj;
                    try
                    {
                        obj = getProperty!.GetValue(target, new object[] { key });
                    }
                    catch (TargetInvocationException e) when (e.InnerException is KeyNotFoundException)
                    {
                        continue;
                    }

                    var parameters = split[1].Split('=', 2);
                    additionalPropertySetter.Invoke(obj, new object[] { parameters[0], parameters[1] });

                    continue;
                }

                if (target is not null)
                {
                    var split = line.Split('=', 2);
                    if (split.Length != 2)
                    {
                        split = line.Split('-');
                        if (split.Length != 2)
                        {
                            throw new InvalidDataException(line);
                        }
                    }

                    var key = split[0];
                    var values = split[1].Split(',', StringSplitOptions.RemoveEmptyEntries);

                    addMethod!.Invoke(target, new[] { key, constructor!.Invoke(new object[] { key, values }) });
                }
            }

            result.PrepareLookups();

            return result;
        }

        public bool TryGetTriggerType(string typeName, [NotNullWhen(true)] out TriggerType? triggerType) => _triggerTypes.TryGetValue(typeName, out triggerType);

        public bool TryGetTriggerTypeDefault(string typeName, [NotNullWhen(true)] out TriggerTypeDefault? triggerTypeDefault) => _triggerTypeDefaults.TryGetValue(typeName, out triggerTypeDefault);

        public int GetParameterCount(TriggerFunctionType functionType, string functionName)
        {
            return GetParameters(functionType, functionName).Length;
        }

        public string GetParameterType(TriggerFunctionType functionType, string functionName, int parameterIndex)
        {
            return GetParameters(functionType, functionName)[parameterIndex];
        }

        public ImmutableArray<string> GetParameters(TriggerFunctionType functionType, string functionName)
        {
            if (string.IsNullOrWhiteSpace(functionName))
            {
                throw new ArgumentNullException(nameof(functionName));
            }

            ImmutableArray<string>? parameters = functionType switch
            {
                TriggerFunctionType.Event => _triggerEvents.TryGetValue(functionName, out var @event) ? @event.ArgumentTypes : null,
                TriggerFunctionType.Condition => _triggerConditions.TryGetValue(functionName, out var condition) ? condition.ArgumentTypes : null,
                TriggerFunctionType.Action => _triggerActions.TryGetValue(functionName, out var action) ? action.ArgumentTypes : null,
                TriggerFunctionType.Call => _triggerCalls.TryGetValue(functionName, out var call) ? call.ArgumentTypes : null,

                _ => throw new InvalidEnumArgumentException(nameof(functionType), (int)functionType, typeof(TriggerFunctionType)),
            };

            return parameters ?? throw new KeyNotFoundException($"The {functionType} '{functionName}' was not found.");
        }

        public bool TryGetParametersByScriptName(
            string scriptName,
            int expectedParameterCount,
            [NotNullWhen(true)] out ImmutableArray<string>? parameters,
            [NotNullWhen(true)] out string? functionName)
        {
            if (string.IsNullOrWhiteSpace(scriptName))
            {
                throw new ArgumentNullException(nameof(scriptName));
            }

            if (_triggerActionsLookup.TryGetValue(scriptName, out var triggerActions) && triggerActions.TryGetValue(expectedParameterCount, out var action))
            {
                parameters = action.ArgumentTypes;
                functionName = action.ActionFunctionName;
                return true;
            }

            parameters = null;
            functionName = null;
            return false;
        }

        public bool TryGetReturnType(string functionName, [NotNullWhen(true)] out string? returnType)
        {
            if (_triggerCalls.TryGetValue(functionName, out var call))
            {
                returnType = call.ReturnType;
                return true;
            }

            returnType = null;
            return false;
        }

        public bool TryGetOperatorCompareType(string variableType, [NotNullWhen(true)] out string? operatorCompareType, [NotNullWhen(true)] out string? operatorType)
        {
            if (_triggerConditionsLookup.TryGetValue(variableType, out var triggerCondition))
            {
                operatorCompareType = triggerCondition.ConditionFunctionName;
                operatorType = triggerCondition.ArgumentTypes[1];
                return true;
            }

            operatorCompareType = null;
            operatorType = null;
            return false;
        }

        public bool TryGetTriggerParamPreset(string codeText, [NotNullWhen(true)] out string? presetName, [NotNullWhen(true)] out string? type)
        {
            if (_variableTypePresets.TryGetValue(codeText, out var triggerParam))
            {
                presetName = triggerParam.ParameterName;
                type = triggerParam.VariableType;
                return true;
            }

            presetName = null;
            type = null;
            return false;
        }

        public bool TryGetTriggerParamPreset(string variableType, string codeText, [NotNullWhen(true)] out string? presetName)
        {
            if (_triggerParamsLookup.TryGetValue(variableType, out var variableTypePresets) && variableTypePresets.TryGetValue(codeText, out var triggerParam))
            {
                presetName = triggerParam.ParameterName;
                return true;
            }

            presetName = null;
            return false;
        }

        private void PrepareLookups()
        {
            _triggerConditionsLookup = _triggerConditions
                .Where(triggerCondition => triggerCondition.Value.ArgumentTypes.Length == 3 && string.Equals(triggerCondition.Value.ArgumentTypes[0], triggerCondition.Value.ArgumentTypes[2], StringComparison.Ordinal))
                .ToDictionary(triggerCondition => triggerCondition.Value.ArgumentTypes[0], triggerCondition => triggerCondition.Value);

            _triggerActionsLookup = _triggerActions
                .GroupBy(triggerAction => string.IsNullOrEmpty(triggerAction.Value.ScriptName) ? triggerAction.Key : triggerAction.Value.ScriptName)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.GroupBy(triggerAction => triggerAction.Value.ArgumentTypes.Length).ToDictionary(grouping => grouping.Key, grouping => grouping.First().Value));

            _variableTypePresets = GetTriggerParamDictionary(_triggerParams, GetAllowedOperatorCompareVariableTypes());

            _triggerParamsLookup = _triggerParams
                .GroupBy(triggerParam => triggerParam.Value.VariableType, StringComparer.Ordinal)
                .ToDictionary(grouping => grouping.Key, grouping => GetTriggerParamDictionary(grouping));
        }

        private HashSet<string> GetAllowedOperatorCompareVariableTypes()
        {
            return _triggerConditionsLookup.Keys.ToHashSet(StringComparer.Ordinal);
        }

        private Dictionary<string, TriggerParam> GetTriggerParamDictionary(IEnumerable<KeyValuePair<string, TriggerParam>> items, HashSet<string>? allowedVariableTypes = null)
        {
            var result = new Dictionary<string, TriggerParam>();
            foreach (var item in items)
            {
                if (allowedVariableTypes is not null && (string.Equals(item.Value.CodeText, "null", StringComparison.Ordinal) || !allowedVariableTypes.Contains(item.Value.VariableType)))
                {
                    continue;
                }

                result.TryAdd(item.Value.CodeText, item.Value);
            }

            return result;
        }
    }
}