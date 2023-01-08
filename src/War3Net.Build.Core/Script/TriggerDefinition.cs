// ------------------------------------------------------------------------------
// <copyright file="TriggerDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerDefinition : TriggerItem
    {
        public TriggerDefinition(TriggerItemType triggerItemType = TriggerItemType.Gui)
            : base(triggerItemType)
        {
        }

        public string Description { get; set; }

        public bool IsComment { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsCustomTextTrigger { get; set; }

        public bool IsInitiallyOn { get; set; }

        public bool RunOnMapInit { get; set; }

        public List<TriggerFunction> Functions { get; init; } = new();
    }
}