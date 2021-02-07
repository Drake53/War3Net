// ------------------------------------------------------------------------------
// <copyright file="TriggerType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Globalization;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        public sealed class TriggerType
        {
            public TriggerType(string key, params string[] values)
            {
                TypeName = key;
                GameVersion = int.Parse(values[0], CultureInfo.InvariantCulture);
                UsableAsGlobalVariable = int.Parse(values[1], CultureInfo.InvariantCulture).ToBool();
                UsableWithComparisonOperators = int.Parse(values[2], CultureInfo.InvariantCulture).ToBool();
                DisplayString = values[3];
                BaseType = values.Length > 4 ? values[4] : null;
                ImportType = values.Length > 5 ? values[5] : null;
                TreatAsBaseType = values.Length > 6 ? int.Parse(values[6], CultureInfo.InvariantCulture).ToBool() : null;
            }

            public string TypeName { get; }

            public int GameVersion { get; }

            public bool UsableAsGlobalVariable { get; }

            public bool UsableWithComparisonOperators { get; }

            public string DisplayString { get; }

            public string? BaseType { get; }

            public string? ImportType { get; }

            public bool? TreatAsBaseType { get; }
        }
    }
}