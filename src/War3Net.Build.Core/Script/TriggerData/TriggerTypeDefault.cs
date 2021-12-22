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
            internal TriggerTypeDefault(
                string variableType,
                string scriptText,
                string? displayText)
            {
                VariableType = variableType;
                ScriptText = scriptText;
                DisplayText = displayText;
            }

            public string VariableType { get; }

            public string ScriptText { get; }

            public string? DisplayText { get; }

            public override string ToString() => VariableType;
        }
    }
}