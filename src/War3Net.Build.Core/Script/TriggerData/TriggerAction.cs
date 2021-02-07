// ------------------------------------------------------------------------------
// <copyright file="TriggerAction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

using War3Net.CodeAnalysis.Jass;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        public sealed class TriggerAction
        {
            public TriggerAction(string key, params string[] values)
            {
                ActionFunctionName = key;
                GameVersion = int.Parse(values[0], CultureInfo.InvariantCulture);
                ArgumentTypes = values[1..].ToImmutableArray();

                if (ArgumentTypes.Length == 1 && string.Equals(ArgumentTypes.Single(), JassKeyword.Nothing, StringComparison.Ordinal))
                {
                    ArgumentTypes = ImmutableArray<string>.Empty;
                }
            }

            public string ActionFunctionName { get; }

            public int GameVersion { get; }

            public ImmutableArray<string> ArgumentTypes { get; }
        }
    }
}