// ------------------------------------------------------------------------------
// <copyright file="TriggerCondition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        public sealed class TriggerCondition
        {
            internal TriggerCondition(
                string functionName,
                int gameVersion,
                ImmutableArray<string> argumentTypes,
                string displayName,
                string parameters,
                ImmutableArray<string>? defaults,
                string category,
                bool useWithAI,
                ImmutableArray<string>? aiDefaults)
            {
                FunctionName = functionName;
                GameVersion = gameVersion;
                ArgumentTypes = argumentTypes;
                DisplayName = displayName;
                Parameters = parameters;
                Defaults = defaults;
                Category = category;
                UseWithAI = useWithAI;
                AIDefaults = aiDefaults;
            }

            public string FunctionName { get; }

            public int GameVersion { get; }

            public ImmutableArray<string> ArgumentTypes { get; }

            public string DisplayName { get; }

            public string Parameters { get; }

            public ImmutableArray<string>? Defaults { get; }

            public string Category { get; }

            public bool UseWithAI { get; }

            public ImmutableArray<string>? AIDefaults { get; }

            public override string ToString() => FunctionName;

            internal class Builder
            {
                public Builder(
                    string functionName,
                    int gameVersion,
                    ImmutableArray<string> argumentTypes)
                {
                    FunctionName = functionName;
                    GameVersion = gameVersion;

                    if (argumentTypes.Length == 1 && string.Equals(ArgumentTypes[0], JassKeyword.Nothing, StringComparison.Ordinal))
                    {
                        ArgumentTypes = ImmutableArray<string>.Empty;
                    }
                    else
                    {
                        ArgumentTypes = argumentTypes;
                    }
                }

                public string FunctionName { get; }

                public int GameVersion { get; }

                public ImmutableArray<string> ArgumentTypes { get; }

                public string? DisplayName { get; set; }

                public string? Parameters { get; set; }

                public ImmutableArray<string>? Defaults { get; set; }

                public ImmutableArray<int?>? Limits { get; set; }

                public string? Category { get; set; }

                public bool UseWithAI { get; set; }

                public ImmutableArray<string>? AIDefaults { get; set; }

                public TriggerCondition ToImmutable()
                {
                    return new TriggerCondition(
                        FunctionName,
                        GameVersion,
                        ArgumentTypes,
                        DisplayName,
                        Parameters,
                        Defaults,
                        Category,
                        UseWithAI,
                        AIDefaults);
                }

                public override string ToString() => FunctionName;
            }
        }
    }
}