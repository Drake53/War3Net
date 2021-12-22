// ------------------------------------------------------------------------------
// <copyright file="TriggerAction.cs" company="Drake53">
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
        public sealed class TriggerAction
        {
            internal TriggerAction(
                string functionName,
                int gameVersion,
                ImmutableArray<string> argumentTypes,
                string displayName,
                string parameters,
                ImmutableArray<string>? defaults,
                ImmutableArray<int?>? limits,
                string category,
                string? scriptName)
            {
                FunctionName = functionName;
                GameVersion = gameVersion;
                ArgumentTypes = argumentTypes;
                DisplayName = displayName;
                Parameters = parameters;
                Defaults = defaults;
                Limits = limits;
                Category = category;
                ScriptName = scriptName;
            }

            public string FunctionName { get; }

            public int GameVersion { get; }

            public ImmutableArray<string> ArgumentTypes { get; }

            public string DisplayName { get; }

            public string Parameters { get; }

            public ImmutableArray<string>? Defaults { get; }

            public ImmutableArray<int?>? Limits { get; }

            public string Category { get; }

            public string? ScriptName { get; }

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

                public string? ScriptName { get; set; }

                public TriggerAction ToImmutable()
                {
                    return new TriggerAction(
                        FunctionName,
                        GameVersion,
                        ArgumentTypes,
                        DisplayName,
                        Parameters,
                        Defaults,
                        Limits,
                        Category,
                        ScriptName);
                }

                public override string ToString() => FunctionName;
            }
        }
    }
}