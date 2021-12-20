// ------------------------------------------------------------------------------
// <copyright file="TriggerTypeDefault.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        public sealed class TriggerTypeDefault
        {
            public TriggerTypeDefault(string key, params string[] values)
            {
                TypeName = key;
                ExpressionString = values[0];
                DisplayString = values.Length > 1 ? values[1] : null;
            }

            public string TypeName { get; }

            public string ExpressionString { get; }

            public string? DisplayString { get; }

            public override string ToString() => TypeName;
        }
    }
}