// ------------------------------------------------------------------------------
// <copyright file="TriggerData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using War3Net.Build.Resources;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        private static readonly Lazy<TriggerData> _defaultTriggerData = new(GetDefaultTriggerData);

        internal TriggerData(StringReader reader)
        {
            ReadFrom(reader);

            TriggerCategories ??= ImmutableDictionary<string, TriggerCategory>.Empty;
            TriggerTypes ??= ImmutableDictionary<string, TriggerType>.Empty;
            TriggerTypeDefaults ??= ImmutableDictionary<string, TriggerTypeDefault>.Empty;
            TriggerParams ??= ImmutableDictionary<string, TriggerParam>.Empty;
            TriggerEvents ??= ImmutableDictionary<string, TriggerEvent>.Empty;
            TriggerConditions ??= ImmutableDictionary<string, TriggerCondition>.Empty;
            TriggerActions ??= ImmutableDictionary<string, TriggerAction>.Empty;
            TriggerCalls ??= ImmutableDictionary<string, TriggerCall>.Empty;
            DefaultTriggerCategories ??= ImmutableDictionary<int, DefaultTriggerCategory>.Empty;
            DefaultTriggers ??= ImmutableDictionary<int, DefaultTrigger>.Empty;
        }

        public static TriggerData Default => _defaultTriggerData.Value;

        public ImmutableDictionary<string, TriggerCategory> TriggerCategories { get; private set; }

        public ImmutableDictionary<string, TriggerType> TriggerTypes { get; private set; }

        public ImmutableDictionary<string, TriggerTypeDefault> TriggerTypeDefaults { get; private set; }

        public ImmutableDictionary<string, TriggerParam> TriggerParams { get; private set; }

        public ImmutableDictionary<string, TriggerEvent> TriggerEvents { get; private set; }

        public ImmutableDictionary<string, TriggerCondition> TriggerConditions { get; private set; }

        public ImmutableDictionary<string, TriggerAction> TriggerActions { get; private set; }

        public ImmutableDictionary<string, TriggerCall> TriggerCalls { get; private set; }

        public ImmutableDictionary<int, DefaultTriggerCategory> DefaultTriggerCategories { get; private set; }

        public ImmutableDictionary<int, DefaultTrigger> DefaultTriggers { get; private set; }

        internal void ReadFrom(StringReader reader)
        {
            while (true)
            {
                if (TryGetNextSection(reader, out var sectionName))
                {
                    while (!string.IsNullOrEmpty(sectionName))
                    {
                        sectionName = ReadNextSection(reader, sectionName);
                    }

                    return;
                }
            }
        }

        internal bool TryGetNextSection(StringReader reader, out string? result)
        {
            result = reader.ReadLine();

            if (result is null)
            {
                return true;
            }

            if (result.StartsWith('[') && result.EndsWith(']'))
            {
                result = result[1..^1];
                return true;
            }

            return false;
        }

        internal string? ReadNextSection(StringReader reader, string sectionName)
        {
            return sectionName switch
            {
                nameof(TriggerCategories) => ReadTriggerCategories(reader),
                nameof(TriggerTypes) => ReadTriggerType(reader),
                nameof(TriggerTypeDefaults) => ReadTriggerTypeDefaults(reader),
                nameof(TriggerParams) => ReadTriggerParams(reader),
                nameof(TriggerEvents) => ReadTriggerEvents(reader),
                nameof(TriggerConditions) => ReadTriggerConditions(reader),
                nameof(TriggerActions) => ReadTriggerActions(reader),
                nameof(TriggerCalls) => ReadTriggerCalls(reader),
                nameof(DefaultTriggerCategories) => ReadDefaultTriggerCategories(reader),
                nameof(DefaultTriggers) => ReadDefaultTriggers(reader),

                _ => throw new InvalidDataException($"Unknown {nameof(TriggerData)} section: '{sectionName}'"),
            };
        }

        internal string? ReadTriggerCategories(StringReader reader)
        {
            if (TriggerCategories is not null)
            {
                throw new InvalidOperationException();
            }

            var builder = ImmutableDictionary.CreateBuilder<string, TriggerCategory>(StringComparer.Ordinal);

            while (true)
            {
                if (TryGetNextSection(reader, out var line))
                {
                    TriggerCategories = builder.ToImmutable();
                    return line;
                }

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                var split = line.Split('=', 2);

                var key = split[0];
                var values = split[1].Split(',', 3);

                var triggerCategory = new TriggerCategory(
                    key,
                    values[0],
                    values[1],
                    values.Length > 2 ? int.Parse(values[2], CultureInfo.InvariantCulture).ToBool() : false);

                builder.Add(key, triggerCategory);
            }
        }

        internal string? ReadTriggerType(StringReader reader)
        {
            if (TriggerTypes is not null)
            {
                throw new InvalidOperationException();
            }

            var builder = ImmutableDictionary.CreateBuilder<string, TriggerType>(StringComparer.Ordinal);

            while (true)
            {
                if (TryGetNextSection(reader, out var line))
                {
                    TriggerTypes = builder.ToImmutable();
                    return line;
                }

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                var split = line.Split('=', 2);

                var key = split[0];
                var values = split[1].Split(',', 7);

                var triggerType = new TriggerType(
                    key,
                    int.Parse(values[0], CultureInfo.InvariantCulture),
                    int.Parse(values[1], CultureInfo.InvariantCulture).ToBool(),
                    int.Parse(values[2], CultureInfo.InvariantCulture).ToBool(),
                    values[3],
                    values.Length > 4 ? values[4] : null,
                    values.Length > 5 ? values[5] : null,
                    values.Length > 6 ? int.Parse(values[6], CultureInfo.InvariantCulture).ToBool() : true);

                builder.Add(key, triggerType);
            }
        }

        internal string? ReadTriggerTypeDefaults(StringReader reader)
        {
            if (TriggerTypeDefaults is not null)
            {
                throw new InvalidOperationException();
            }

            var builder = ImmutableDictionary.CreateBuilder<string, TriggerTypeDefault>(StringComparer.Ordinal);

            while (true)
            {
                if (TryGetNextSection(reader, out var line))
                {
                    TriggerTypeDefaults = builder.ToImmutable();
                    return line;
                }

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                var split = line.Split('=', 2);

                var key = split[0];
                var values = split[1].Split(',', 2);

                var triggerTypeDefault = new TriggerTypeDefault(
                    key,
                    values[0],
                    values.Length > 1 ? values[1] : null);

                builder.Add(key, triggerTypeDefault);
            }
        }

        internal string? ReadTriggerParams(StringReader reader)
        {
            if (TriggerParams is not null)
            {
                throw new InvalidOperationException();
            }

            var builder = ImmutableDictionary.CreateBuilder<string, TriggerParam>(StringComparer.Ordinal);

            while (true)
            {
                if (TryGetNextSection(reader, out var line))
                {
                    TriggerParams = builder.ToImmutable();
                    return line;
                }

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                var split = line.Split('=', 2);

                var key = split[0];
                var values = split[1].Split(',', 4);

                var triggerParam = new TriggerParam(
                    key,
                    int.Parse(values[0], CultureInfo.InvariantCulture),
                    values[1],
                    values[2],
                    values[3]);

                builder.Add(key, triggerParam);
            }
        }

        internal string? ReadTriggerEvents(StringReader reader)
        {
            if (TriggerEvents is not null)
            {
                throw new InvalidOperationException();
            }

            var builder = ImmutableDictionary.CreateBuilder<string, TriggerEvent>(StringComparer.Ordinal);

            TriggerEvent.Builder? triggerEventBuilder = null;

            while (true)
            {
                if (TryGetNextSection(reader, out var line))
                {
                    if (triggerEventBuilder is not null)
                    {
                        builder.Add(triggerEventBuilder.FunctionName, triggerEventBuilder.ToImmutable());
                    }

                    TriggerEvents = builder.ToImmutable();
                    return line;
                }

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                var split = line.Split('=', 2);
                var key = split[0];

                if (key.StartsWith('_'))
                {
                    if (triggerEventBuilder is null || !key.StartsWith($"_{triggerEventBuilder.FunctionName}_", StringComparison.Ordinal))
                    {
                        throw new InvalidDataException(line);
                    }

                    switch (key.Split('_', 2, StringSplitOptions.RemoveEmptyEntries)[1])
                    {
                        case nameof(TriggerEvent.DisplayName): triggerEventBuilder.DisplayName = split[1]; break;
                        case nameof(TriggerEvent.Parameters): triggerEventBuilder.Parameters = split[1]; break;
                        case nameof(TriggerEvent.Defaults): triggerEventBuilder.Defaults = split[1].Split(',').ToImmutableArray(); break;
                        case nameof(TriggerEvent.Limits): triggerEventBuilder.Limits = split[1].Split(',').Select(value => int.TryParse(value, out var result) ? (int?)result : null).ToImmutableArray(); break;
                        case nameof(TriggerEvent.Category): triggerEventBuilder.Category = split[1]; break;

                        default: throw new InvalidDataException(key);
                    }
                }
                else
                {
                    if (triggerEventBuilder is not null)
                    {
                        builder.Add(triggerEventBuilder.FunctionName, triggerEventBuilder.ToImmutable());
                    }

                    var values = split[1].Split(',');

                    triggerEventBuilder = new TriggerEvent.Builder(
                        key,
                        int.Parse(values[0], CultureInfo.InvariantCulture),
                        values[1..].ToImmutableArray());
                }
            }
        }

        internal string? ReadTriggerConditions(StringReader reader)
        {
            if (TriggerConditions is not null)
            {
                throw new InvalidOperationException();
            }

            var builder = ImmutableDictionary.CreateBuilder<string, TriggerCondition>(StringComparer.Ordinal);

            TriggerCondition.Builder? triggerConditionBuilder = null;

            while (true)
            {
                if (TryGetNextSection(reader, out var line))
                {
                    if (triggerConditionBuilder is not null)
                    {
                        builder.Add(triggerConditionBuilder.FunctionName, triggerConditionBuilder.ToImmutable());
                    }

                    TriggerConditions = builder.ToImmutable();
                    return line;
                }

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                var split = line.Split('=', 2);
                var key = split[0];

                if (key.StartsWith('_'))
                {
                    if (triggerConditionBuilder is null || !key.StartsWith($"_{triggerConditionBuilder.FunctionName}_", StringComparison.Ordinal))
                    {
                        throw new InvalidDataException(line);
                    }

                    switch (key.Split('_', 2, StringSplitOptions.RemoveEmptyEntries)[1])
                    {
                        case nameof(TriggerCondition.DisplayName): triggerConditionBuilder.DisplayName = split[1]; break;
                        case nameof(TriggerCondition.Parameters): triggerConditionBuilder.Parameters = split[1]; break;
                        case nameof(TriggerCondition.Defaults): triggerConditionBuilder.Defaults = split[1].Split(',').ToImmutableArray(); break;
                        case nameof(TriggerCondition.Category): triggerConditionBuilder.Category = split[1]; break;
                        case nameof(TriggerCondition.UseWithAI): triggerConditionBuilder.UseWithAI = int.Parse(split[1], CultureInfo.InvariantCulture).ToBool(); break;
                        case nameof(TriggerCondition.AIDefaults): triggerConditionBuilder.AIDefaults = split[1].Split(',').ToImmutableArray(); break;

                        default: throw new InvalidDataException(key);
                    }
                }
                else
                {
                    if (triggerConditionBuilder is not null)
                    {
                        builder.Add(triggerConditionBuilder.FunctionName, triggerConditionBuilder.ToImmutable());
                    }

                    var values = split[1].Split(',');

                    triggerConditionBuilder = new TriggerCondition.Builder(
                        key,
                        int.Parse(values[0], CultureInfo.InvariantCulture),
                        values[1..].ToImmutableArray());
                }
            }
        }

        internal string? ReadTriggerActions(StringReader reader)
        {
            if (TriggerActions is not null)
            {
                throw new InvalidOperationException();
            }

            var builder = ImmutableDictionary.CreateBuilder<string, TriggerAction>(StringComparer.Ordinal);

            TriggerAction.Builder? triggerActionBuilder = null;

            while (true)
            {
                if (TryGetNextSection(reader, out var line))
                {
                    if (triggerActionBuilder is not null)
                    {
                        builder.Add(triggerActionBuilder.FunctionName, triggerActionBuilder.ToImmutable());
                    }

                    TriggerActions = builder.ToImmutable();
                    return line;
                }

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                var split = line.Split('=', 2);
                var key = split[0];

                if (key.StartsWith('_'))
                {
                    if (triggerActionBuilder is null || !key.StartsWith($"_{triggerActionBuilder.FunctionName}_", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidDataException(line);
                    }

                    switch (key.Split('_', 2, StringSplitOptions.RemoveEmptyEntries)[1])
                    {
                        case nameof(TriggerAction.DisplayName): triggerActionBuilder.DisplayName = split[1]; break;
                        case nameof(TriggerAction.Parameters): triggerActionBuilder.Parameters = split[1]; break;
                        case nameof(TriggerAction.Defaults): triggerActionBuilder.Defaults = split[1].Split(',').ToImmutableArray(); break;
                        case nameof(TriggerAction.Limits): triggerActionBuilder.Limits = split[1].Split(',').Select(value => int.TryParse(value, out var result) ? (int?)result : null).ToImmutableArray(); break;
                        case nameof(TriggerAction.Category): triggerActionBuilder.Category = split[1]; break;
                        case nameof(TriggerAction.ScriptName): triggerActionBuilder.ScriptName = split[1]; break;

                        default: throw new InvalidDataException(key);
                    }
                }
                else
                {
                    if (triggerActionBuilder is not null)
                    {
                        builder.Add(triggerActionBuilder.FunctionName, triggerActionBuilder.ToImmutable());
                    }

                    var values = split[1].Split(',');

                    triggerActionBuilder = new TriggerAction.Builder(
                        key,
                        int.Parse(values[0], CultureInfo.InvariantCulture),
                        values[1..].ToImmutableArray());
                }
            }
        }

        internal string? ReadTriggerCalls(StringReader reader)
        {
            if (TriggerCalls is not null)
            {
                throw new InvalidOperationException();
            }

            var builder = ImmutableDictionary.CreateBuilder<string, TriggerCall>(StringComparer.Ordinal);

            TriggerCall.Builder? triggerCallBuilder = null;

            while (true)
            {
                if (TryGetNextSection(reader, out var line))
                {
                    if (triggerCallBuilder is not null)
                    {
                        builder.Add(triggerCallBuilder.FunctionName, triggerCallBuilder.ToImmutable());
                    }

                    TriggerCalls = builder.ToImmutable();
                    return line;
                }

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                var split = line.Split('=', 2);
                var key = split[0];

                if (key.StartsWith('_'))
                {
                    if (triggerCallBuilder is null || !key.StartsWith($"_{triggerCallBuilder.FunctionName}", StringComparison.Ordinal))
                    {
                        throw new InvalidDataException(line);
                    }

                    var keys = key.Split('_', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (keys.Length == 1)
                    {
                        triggerCallBuilder.Category = split[1];
                    }
                    else
                    {
                        switch (keys[1])
                        {
                            case nameof(TriggerCall.DisplayName): triggerCallBuilder.DisplayName = split[1]; break;
                            case nameof(TriggerCall.Parameters): triggerCallBuilder.Parameters = split[1]; break;
                            case nameof(TriggerCall.Defaults): triggerCallBuilder.Defaults = split[1].Split(',').ToImmutableArray(); break;
                            case nameof(TriggerCall.Limits):
                            case "Limites": triggerCallBuilder.Limits = split[1].Split(',').Select(value => int.TryParse(value, out var result) ? (int?)result : null).ToImmutableArray(); break;
                            case nameof(TriggerCall.Category):
                            case "CATEGORY": triggerCallBuilder.Category = split[1]; break;
                            case nameof(TriggerCall.UseWithAI): triggerCallBuilder.UseWithAI = int.Parse(split[1], CultureInfo.InvariantCulture).ToBool(); break;

                            default: throw new InvalidDataException(key);
                        }
                    }
                }
                else
                {
                    if (triggerCallBuilder is not null)
                    {
                        builder.Add(triggerCallBuilder.FunctionName, triggerCallBuilder.ToImmutable());
                    }

                    var values = split[1].Split(',');

                    triggerCallBuilder = new TriggerCall.Builder(
                        key,
                        int.Parse(values[0], CultureInfo.InvariantCulture),
                        int.Parse(values[1], CultureInfo.InvariantCulture).ToBool(),
                        values[2],
                        values[3..].ToImmutableArray());
                }
            }
        }

        internal string? ReadDefaultTriggerCategories(StringReader reader)
        {
            if (DefaultTriggerCategories is not null)
            {
                throw new InvalidOperationException();
            }

            var builder = ImmutableDictionary.CreateBuilder<int, DefaultTriggerCategory>();

            while (true)
            {
                if (TryGetNextSection(reader, out var line))
                {
                    DefaultTriggerCategories = builder.ToImmutable();
                    return line;
                }

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                var split = line.Split('=', 2);

                var key = split[0];
                var value = split[1];

                if (string.Equals(key, "NumCategories", StringComparison.Ordinal))
                {
                }
                else
                {
                    var defaultTriggerCategory = new DefaultTriggerCategory(value);

                    builder.Add(builder.Count + 1, defaultTriggerCategory);
                }
            }
        }

        internal string? ReadDefaultTriggers(StringReader reader)
        {
            if (DefaultTriggers is not null)
            {
                throw new InvalidOperationException();
            }

            var builder = ImmutableDictionary.CreateBuilder<int, DefaultTrigger>();

            DefaultTrigger.Builder? defaultTriggerBuilder = null;

            while (true)
            {
                if (TryGetNextSection(reader, out var line))
                {
                    if (defaultTriggerBuilder is not null)
                    {
                        builder.Add(defaultTriggerBuilder.TriggerNumber, defaultTriggerBuilder.ToImmutable());
                    }

                    DefaultTriggers = builder.ToImmutable();
                    return line;
                }

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                var split = line.Split('=', 2);

                var key = split[0];
                var value = split[1];

                if (string.Equals(key, "NumTriggers", StringComparison.Ordinal))
                {
                }
                else
                {
                    if (defaultTriggerBuilder is null || !key.StartsWith($"Trigger{defaultTriggerBuilder.TriggerNumber:D2}", StringComparison.Ordinal))
                    {
                        if (defaultTriggerBuilder is not null)
                        {
                            builder.Add(defaultTriggerBuilder.TriggerNumber, defaultTriggerBuilder.ToImmutable());
                        }

                        var triggerNumber = int.Parse(new string(key["Trigger".Length..].TakeWhile(c => char.IsDigit(c)).ToArray()), CultureInfo.InvariantCulture);
                        defaultTriggerBuilder = new DefaultTrigger.Builder(triggerNumber);
                    }

                    var triggerKey = $"Trigger{defaultTriggerBuilder.TriggerNumber:D2}";

                    if (string.Equals(key, $"{triggerKey}Name", StringComparison.Ordinal))
                    {
                        defaultTriggerBuilder.TriggerName = value;
                    }
                    else if (string.Equals(key, $"{triggerKey}Comment", StringComparison.Ordinal))
                    {
                        defaultTriggerBuilder.Comment = value;
                    }
                    else if (string.Equals(key, $"{triggerKey}Category", StringComparison.Ordinal))
                    {
                        defaultTriggerBuilder.Category = int.Parse(value, CultureInfo.InvariantCulture);
                    }
                    else if (Regex.IsMatch(key, $"{triggerKey}Event[0-9]{{2,}}", RegexOptions.None))
                    {
                        defaultTriggerBuilder.Events.Add(value);
                    }
                    else if (Regex.IsMatch(key, $"{triggerKey}Condition[0-9]{{2,}}", RegexOptions.None))
                    {
                        defaultTriggerBuilder.Conditions.Add(value);
                    }
                    else if (Regex.IsMatch(key, $"{triggerKey}Action[0-9]{{2,}}", RegexOptions.None))
                    {
                        defaultTriggerBuilder.Actions.Add(value);
                    }
                }
            }
        }

        private static TriggerData GetDefaultTriggerData()
        {
            using var reader = new StringReader(War3Resources.TriggerData);

            return new TriggerData(reader);
        }
    }
}