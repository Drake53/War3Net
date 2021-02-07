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

        private TriggerData()
        {
            _triggerTypes = new();
            _triggerTypeDefaults = new();
            _triggerEvents = new();
            _triggerConditions = new();
            _triggerActions = new();
            _triggerCalls = new();
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
            ConstructorInfo? constructor = null;
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

                        _ => null,
                    };

                    if (target is not null)
                    {
                        // Get Dictionary<string, T> add method and constructor.
                        addMethod = target.GetType().GetMethod("Add");
                        constructor = target.GetType().GetGenericArguments()[1].GetConstructors().Single();
                    }

                    continue;
                }

                if (line.StartsWith('_'))
                {
                    continue;
                }

                if (target is not null)
                {
                    var split = line.Split('=');
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
    }
}