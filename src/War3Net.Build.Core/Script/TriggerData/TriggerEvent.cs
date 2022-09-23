// ------------------------------------------------------------------------------
// <copyright file="TriggerEvent.cs" company="Drake53">
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
        public sealed class TriggerEvent
        {
            internal TriggerEvent(
                string functionName,
                int gameVersion,
                ImmutableArray<string> argumentTypes,
                string? displayName,
                string? parameters,
                ImmutableArray<string> defaults,
                ImmutableArray<int?>? limits,
                string category)
            {
                FunctionName = functionName;
                GameVersion = gameVersion;
                ArgumentTypes = argumentTypes;
                DisplayName = displayName;
                Parameters = parameters;
                Defaults = defaults;
                Limits = limits;
                Category = category;
            }

            public string FunctionName { get; }

            public int GameVersion { get; }

            public ImmutableArray<string> ArgumentTypes { get; }

            public string? DisplayName { get; }

            public string? Parameters { get; }

            public ImmutableArray<string> Defaults { get; }

            public ImmutableArray<int?>? Limits { get; }

            public string Category { get; }

            public override string ToString() => FunctionName;

            internal sealed class Builder
            {
                public Builder(
                    string functionName,
                    int gameVersion,
                    ImmutableArray<string> argumentTypes)
                {
                    FunctionName = functionName;
                    GameVersion = gameVersion;

                    if (argumentTypes.Length == 1 && string.Equals(argumentTypes[0], JassKeyword.Nothing, StringComparison.Ordinal))
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

                public TriggerEvent ToImmutable()
                {
                    return new TriggerEvent(
                        FunctionName,
                        GameVersion,
                        ArgumentTypes,
                        DisplayName,
                        Parameters,
                        Defaults.Value,
                        Limits,
                        Category);
                }

                public override string ToString() => FunctionName;
            }
        }
    }
}