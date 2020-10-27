// ------------------------------------------------------------------------------
// <copyright file="TriggerData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace War3Net.Build.Script
{
    public sealed class TriggerData
    {
        private static readonly Lazy<TriggerData> _defaultTriggerData = new Lazy<TriggerData>(() => ParseText(DefaultTriggerData.TriggerData));

        private readonly Dictionary<string, string[]> _triggerEvents;
        private readonly Dictionary<string, string[]> _triggerConditions;
        private readonly Dictionary<string, string[]> _triggerActions;
        private readonly Dictionary<string, string[]> _triggerCalls;

        private TriggerData()
        {
            _triggerEvents = new Dictionary<string, string[]>();
            _triggerConditions = new Dictionary<string, string[]>();
            _triggerActions = new Dictionary<string, string[]>();
            _triggerCalls = new Dictionary<string, string[]>();
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

            Dictionary<string, string[]>? target = null;
            var argumentsOffset = -1;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                if (line.StartsWith('[') && line.EndsWith(']'))
                {
                    target = line[1..^1] switch
                    {
                        "TriggerEvents" => result._triggerEvents,
                        "TriggerConditions" => result._triggerConditions,
                        "TriggerActions" => result._triggerActions,
                        "TriggerCalls" => result._triggerCalls,

                        _ => null,
                    };

                    argumentsOffset = result._triggerCalls == target ? 3 : 1;

                    continue;
                }

                if (line.StartsWith('_'))
                {
                    continue;
                }

                if (target != null)
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

                    var values = split[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length - argumentsOffset < 0)
                    {
                        throw new InvalidDataException(
                            argumentsOffset > 1
                            ? $"Expected at least {argumentsOffset} arguments, but only got {values.Length}."
                            : "Expected at least one argument, but got none.");
                    }

                    var parameters = values.Skip(argumentsOffset).ToArray();
                    if (parameters.Length == 1 && string.Equals(parameters[0], "nothing", StringComparison.Ordinal))
                    {
                        parameters = Array.Empty<string>();
                    }

                    target.Add(split[0], parameters);
                }
            }

            return result;
        }

        public int GetParameterCount(TriggerFunctionType functionType, string functionName)
        {
            var target = functionType switch
            {
                TriggerFunctionType.Event => _triggerEvents,
                TriggerFunctionType.Condition => _triggerConditions,
                TriggerFunctionType.Action => _triggerActions,
                TriggerFunctionType.Call => _triggerCalls,

                _ => throw new InvalidEnumArgumentException(nameof(functionType), (int)functionType, typeof(TriggerFunctionType)),
            };

            if (string.IsNullOrWhiteSpace(functionName))
            {
                throw new ArgumentNullException(nameof(functionName));
            }

            return target.TryGetValue(functionName, out var parameters)
                ? parameters.Length
                : throw new KeyNotFoundException($"The {functionType} '{functionName}' was not found.");
        }
    }
}