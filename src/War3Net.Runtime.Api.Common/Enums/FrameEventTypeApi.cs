// ------------------------------------------------------------------------------
// <copyright file="FrameEventTypeApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums;

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class FrameEventTypeApi
    {
        public static readonly FrameEventType FRAMEEVENT_CONTROL_CLICK = ConvertFrameEventType((int)FrameEventType.Type.ControlClick);
        public static readonly FrameEventType FRAMEEVENT_MOUSE_ENTER = ConvertFrameEventType((int)FrameEventType.Type.MouseEnter);
        public static readonly FrameEventType FRAMEEVENT_MOUSE_LEAVE = ConvertFrameEventType((int)FrameEventType.Type.MouseLeave);
        public static readonly FrameEventType FRAMEEVENT_MOUSE_UP = ConvertFrameEventType((int)FrameEventType.Type.MouseUp);
        public static readonly FrameEventType FRAMEEVENT_MOUSE_DOWN = ConvertFrameEventType((int)FrameEventType.Type.MouseDown);
        public static readonly FrameEventType FRAMEEVENT_MOUSE_WHEEL = ConvertFrameEventType((int)FrameEventType.Type.MouseWheel);
        public static readonly FrameEventType FRAMEEVENT_CHECKBOX_CHECKED = ConvertFrameEventType((int)FrameEventType.Type.CheckboxChecked);
        public static readonly FrameEventType FRAMEEVENT_CHECKBOX_UNCHECKED = ConvertFrameEventType((int)FrameEventType.Type.CheckboxUnchecked);
        public static readonly FrameEventType FRAMEEVENT_EDITBOX_TEXT_CHANGED = ConvertFrameEventType((int)FrameEventType.Type.EditboxTextChanged);
        public static readonly FrameEventType FRAMEEVENT_POPUPMENU_ITEM_CHANGED = ConvertFrameEventType((int)FrameEventType.Type.PopupMenuItemChanged);
        public static readonly FrameEventType FRAMEEVENT_MOUSE_DOUBLECLICK = ConvertFrameEventType((int)FrameEventType.Type.MouseDoubleClick);
        public static readonly FrameEventType FRAMEEVENT_SPRITE_ANIM_UPDATE = ConvertFrameEventType((int)FrameEventType.Type.SpriteAnimationUpdate);
        public static readonly FrameEventType FRAMEEVENT_SLIDER_VALUE_CHANGED = ConvertFrameEventType((int)FrameEventType.Type.SliderValueChanged);
        public static readonly FrameEventType FRAMEEVENT_DIALOG_CANCEL = ConvertFrameEventType((int)FrameEventType.Type.DialogCancel);
        public static readonly FrameEventType FRAMEEVENT_DIALOG_ACCEPT = ConvertFrameEventType((int)FrameEventType.Type.DialogAccept);
        public static readonly FrameEventType FRAMEEVENT_EDITBOX_ENTER = ConvertFrameEventType((int)FrameEventType.Type.EditboxEnter);

        public static FrameEventType ConvertFrameEventType(int i)
        {
            return FrameEventType.GetFrameEventType(i);
        }
    }
}