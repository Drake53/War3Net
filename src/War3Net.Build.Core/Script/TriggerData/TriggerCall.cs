// ------------------------------------------------------------------------------
// <copyright file="TriggerCall.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Globalization;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        public sealed class TriggerCall
        {
            public TriggerCall(string key, params string[] values)
            {
                FunctionName = key;
                GameVersion = int.Parse(values[0], CultureInfo.InvariantCulture);
                UsableInEvents = int.Parse(values[1], CultureInfo.InvariantCulture).ToBool();
                ReturnType = values[2];
                ArgumentTypes = values[3..].ToImmutableArray();
            }

            public string FunctionName { get; }

            public int GameVersion { get; }

            public bool UsableInEvents { get; }

            public string ReturnType { get; }

            public ImmutableArray<string> ArgumentTypes { get; }
        }
    }
}