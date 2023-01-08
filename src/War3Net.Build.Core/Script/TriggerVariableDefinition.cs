// ------------------------------------------------------------------------------
// <copyright file="TriggerVariableDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed partial class TriggerVariableDefinition : TriggerItem
    {
        public TriggerVariableDefinition(TriggerItemType triggerItemType = TriggerItemType.Variable)
            : base(triggerItemType)
        {
        }
    }
}