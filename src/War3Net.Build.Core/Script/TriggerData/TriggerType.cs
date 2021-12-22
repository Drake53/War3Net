// ------------------------------------------------------------------------------
// <copyright file="TriggerType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        public sealed class TriggerType
        {
            internal TriggerType(
                string typeName,
                int gameVersion,
                bool usableAsGlobalVariable,
                bool usableWithComparisonOperators,
                string displayText,
                string? baseType,
                string? importType,
                bool treatAsBaseType)
            {
                TypeName = typeName;
                GameVersion = gameVersion;
                UsableAsGlobalVariable = usableAsGlobalVariable;
                UsableWithComparisonOperators = usableWithComparisonOperators;
                DisplayText = displayText;
                BaseType = baseType;
                ImportType = importType;
                TreatAsBaseType = treatAsBaseType;
            }

            public string TypeName { get; }

            public int GameVersion { get; }

            public bool UsableAsGlobalVariable { get; }

            public bool UsableWithComparisonOperators { get; }

            public string DisplayText { get; }

            public string? BaseType { get; }

            public string? ImportType { get; }

            public bool TreatAsBaseType { get; }

            public override string ToString() => TypeName;
        }
    }
}