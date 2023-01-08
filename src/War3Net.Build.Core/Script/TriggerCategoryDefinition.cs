// ------------------------------------------------------------------------------
// <copyright file="TriggerCategoryDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed partial class TriggerCategoryDefinition : TriggerItem
    {
        public TriggerCategoryDefinition(TriggerItemType triggerItemType = TriggerItemType.Category)
            : base(triggerItemType)
        {
        }

        public bool IsComment { get; set; }

        /// <summary>
        /// If <see langword="false"/>, the category is collapsed, hiding its children in the trigger editor.
        /// </summary>
        public bool IsExpanded { get; set; }
    }
}