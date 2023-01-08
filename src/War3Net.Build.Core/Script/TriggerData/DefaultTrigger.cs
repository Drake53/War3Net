// ------------------------------------------------------------------------------
// <copyright file="DefaultTrigger.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        public sealed class DefaultTrigger
        {
            internal DefaultTrigger(
                string triggerName,
                string? comment,
                int category,
                ImmutableArray<string> events,
                ImmutableArray<string> conditions,
                ImmutableArray<string> actions)
            {
                TriggerName = triggerName;
                Comment = comment;
                Category = category;
                Events = events;
                Conditions = conditions;
                Actions = actions;
            }

            public string TriggerName { get; }

            public string? Comment { get; }

            public int Category { get; }

            public ImmutableArray<string> Events { get; }

            public ImmutableArray<string> Conditions { get; }

            public ImmutableArray<string> Actions { get; }

            public override string ToString() => TriggerName;

            internal sealed class Builder
            {
                public Builder(int triggerNumber)
                {
                    TriggerNumber = triggerNumber;

                    Events = new();
                    Conditions = new();
                    Actions = new();
                }

                public int TriggerNumber { get; }

                public string? TriggerName { get; set; }

                public string? Comment { get; set; }

                public int? Category { get; set; }

                public List<string> Events { get; set; }

                public List<string> Conditions { get; set; }

                public List<string> Actions { get; set; }

                public DefaultTrigger ToImmutable()
                {
                    return new DefaultTrigger(
                        TriggerName,
                        Comment,
                        Category.Value,
                        Events.ToImmutableArray(),
                        Conditions.ToImmutableArray(),
                        Actions.ToImmutableArray());
                }

                public override string ToString() => TriggerName;
            }
        }
    }
}