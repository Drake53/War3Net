// ------------------------------------------------------------------------------
// <copyright file="DefaultTriggerCategory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed partial class TriggerData
    {
        public sealed class DefaultTriggerCategory
        {
            internal DefaultTriggerCategory(string categoryName)
            {
                CategoryName = categoryName;
            }

            public string CategoryName { get; }

            public override string ToString() => CategoryName;
        }
    }
}