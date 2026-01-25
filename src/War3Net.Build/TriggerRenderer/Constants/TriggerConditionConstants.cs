// ------------------------------------------------------------------------------
// <copyright file="TriggerConditionConstants.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        // [TriggerConditions] in triggerdata.txt
        private static class TriggerConditionConstants
        {
            internal const string GetBooleanAnd = "GetBooleanAnd";
            internal const string GetBooleanOr = "GetBooleanOr";
            internal const string AndMultiple = "AndMultiple";
            internal const string OrMultiple = "OrMultiple";
        }
    }
}