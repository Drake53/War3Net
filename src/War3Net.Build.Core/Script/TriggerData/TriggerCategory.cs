// ------------------------------------------------------------------------------
// <copyright file="TriggerCategory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        public sealed class TriggerCategory
        {
            internal TriggerCategory(
                string categoryIdentifier,
                string displayText,
                string iconImageFile,
                bool hideCategoryName)
            {
                CategoryIdentifier = categoryIdentifier;
                DisplayText = displayText;
                IconImageFile = iconImageFile;
                HideCategoryName = hideCategoryName;
            }

            public string CategoryIdentifier { get; }

            public string DisplayText { get; }

            public string IconImageFile { get; }

            public bool HideCategoryName { get; }

            public override string ToString() => CategoryIdentifier;
        }
    }
}