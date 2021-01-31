// ------------------------------------------------------------------------------
// <copyright file="VariableDefinitionExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Script;

namespace War3Net.Build.Extensions
{
    public static class VariableDefinitionExtensions
    {
        public static string GetVariableName(this VariableDefinition variable)
        {
            return $"udg_{variable.Name}";
        }
    }
}