// ------------------------------------------------------------------------------
// <copyright file="WidgetDataExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using War3Net.Build.Widget;

namespace War3Net.Build.Extensions
{
    public static class WidgetDataExtensions
    {
        public static bool HasItemTable(this WidgetData widgetData)
        {
            return widgetData.MapItemTableId != -1 || widgetData.HasItemTableSets();
        }

        public static bool HasItemTableSets(this WidgetData widgetData)
        {
            return widgetData.ItemTableSets.Any(itemTableSet => itemTableSet.Items.Any());
        }
    }
}