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
            return widgetData.ItemTableSets.Any(itemTableSet => itemTableSet.Items.Count > 0);
        }
    }
}