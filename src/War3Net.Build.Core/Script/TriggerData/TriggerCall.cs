// ------------------------------------------------------------------------------
// <copyright file="TriggerCall.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        public sealed class TriggerCall
        {
            internal TriggerCall(
                string functionName,
                int gameVersion,
                bool usableInEvents,
                string returnType,
                ImmutableArray<string> argumentTypes,
                string displayName,
                string parameters,
                ImmutableArray<string>? defaults,
                ImmutableArray<int?>? limits,
                string category,
                bool useWithAI)
            {
                FunctionName = functionName;
                GameVersion = gameVersion;
                UsableInEvents = usableInEvents;
                ReturnType = returnType;
                ArgumentTypes = argumentTypes;
                DisplayName = displayName;
                Parameters = parameters;
                Defaults = defaults;
                Limits = limits;
                Category = category;
                UseWithAI = useWithAI;
            }

            public string FunctionName { get; }

            public int GameVersion { get; }

            public bool UsableInEvents { get; }

            public string ReturnType { get; }

            public ImmutableArray<string> ArgumentTypes { get; }

            public string DisplayName { get; }

            public string Parameters { get; }

            public ImmutableArray<string>? Defaults { get; }

            public ImmutableArray<int?>? Limits { get; }

            public string Category { get; }

            public bool UseWithAI { get; }

            public override string ToString() => FunctionName;

            internal sealed class Builder
            {
                public Builder(
                    string functionName,
                    int gameVersion,
                    bool usableInEvents,
                    string returnType,
                    ImmutableArray<string> argumentTypes)
                {
                    FunctionName = functionName;
                    GameVersion = gameVersion;
                    UsableInEvents = usableInEvents;
                    ReturnType = returnType;
                    ArgumentTypes = argumentTypes;
                }

                public string FunctionName { get; }

                public int GameVersion { get; }

                public bool UsableInEvents { get; }

                public string ReturnType { get; }

                public ImmutableArray<string> ArgumentTypes { get; }

                public string? DisplayName { get; set; }

                public string? Parameters { get; set; }

                public ImmutableArray<string>? Defaults { get; set; }

                public ImmutableArray<int?>? Limits { get; set; }

                public string? Category { get; set; }

                public bool UseWithAI { get; set; }

                public TriggerCall ToImmutable()
                {
                    return new TriggerCall(
                        FunctionName,
                        GameVersion,
                        UsableInEvents,
                        ReturnType,
                        ArgumentTypes,
                        DisplayName,
                        Parameters,
                        Defaults,
                        Limits,
                        Category,
                        UseWithAI);
                }

                public override string ToString() => FunctionName;
            }
        }
    }
}