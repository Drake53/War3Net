#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

namespace War3Net.Runtime.Api.Common.Enums.Event
{
    public static class WidgetEventApi
    {
        public static readonly WidgetEvent EVENT_WIDGET_DEATH = ConvertWidgetEvent((int)WidgetEvent.Type.Death);

        public static WidgetEvent ConvertWidgetEvent(int i)
        {
            return WidgetEvent.GetWidgetEvent(i);
        }
    }
}