// ------------------------------------------------------------------------------
// <copyright file="TriggerParam.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Globalization;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        public sealed class TriggerParam
        {
            public TriggerParam(string key, params string[] values)
            {
                ParameterName = key;
                GameVersion = int.Parse(values[0], CultureInfo.InvariantCulture);
                VariableType = values[1];
                CodeText = values[2].StartsWith('`') && values[2].EndsWith('`') ? $"\"{values[2].Trim('`')}\"" : values[2].Trim('"');
                DisplayText = values[3];
            }

            public string ParameterName { get; }

            public int GameVersion { get; }

            public string VariableType { get; }

            public string CodeText { get; }

            public string DisplayText { get; }
        }
    }
}