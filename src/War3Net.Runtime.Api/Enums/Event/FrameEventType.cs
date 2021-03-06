// ------------------------------------------------------------------------------
// <copyright file="FrameEventType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor

namespace War3Net.Runtime.Api.Enums
{
    public class FrameEventType : Handle
    {
        /// @CSharpLua.Template = "FRAMEEVENT_CONTROL_CLICK"
        public static readonly FrameEventType ControlClick;

        /// @CSharpLua.Template = "FRAMEEVENT_MOUSE_ENTER"
        public static readonly FrameEventType MouseEnter;

        /// @CSharpLua.Template = "FRAMEEVENT_MOUSE_LEAVE"
        public static readonly FrameEventType MouseLeave;

        /// @CSharpLua.Template = "FRAMEEVENT_MOUSE_UP"
        public static readonly FrameEventType MouseUp;

        /// @CSharpLua.Template = "FRAMEEVENT_MOUSE_DOWN"
        public static readonly FrameEventType MouseDown;

        /// @CSharpLua.Template = "FRAMEEVENT_MOUSE_WHEEL"
        public static readonly FrameEventType MouseWheel;

        /// @CSharpLua.Template = "FRAMEEVENT_CHECKBOX_CHECKED"
        public static readonly FrameEventType CheckboxChecked;

        /// @CSharpLua.Template = "FRAMEEVENT_CHECKBOX_UNCHECKED"
        public static readonly FrameEventType CheckboxUnchecked;

        /// @CSharpLua.Template = "FRAMEEVENT_EDITBOX_TEXT_CHANGED"
        public static readonly FrameEventType EditboxTextChanged;

        /// @CSharpLua.Template = "FRAMEEVENT_POPUPMENU_ITEM_CHANGED"
        public static readonly FrameEventType PopupMenuItemChanged;

        /// @CSharpLua.Template = "FRAMEEVENT_MOUSE_DOUBLECLICK"
        public static readonly FrameEventType MouseDoubleClick;

        /// @CSharpLua.Template = "FRAMEEVENT_SPRITE_ANIM_UPDATE"
        public static readonly FrameEventType SpriteAnimUpdate;

        /// @CSharpLua.Template = "FRAMEEVENT_SLIDER_VALUE_CHANGED"
        public static readonly FrameEventType SliderValueChanged;

        /// @CSharpLua.Template = "FRAMEEVENT_DIALOG_CANCEL"
        public static readonly FrameEventType DialogCancel;

        /// @CSharpLua.Template = "FRAMEEVENT_DIALOG_ACCEPT"
        public static readonly FrameEventType DialogAccept;

        /// @CSharpLua.Template = "FRAMEEVENT_EDITBOX_ENTER"
        public static readonly FrameEventType EditboxEnter;

        /// @CSharpLua.Template = "ConvertFrameEventType({0})"
        public extern FrameEventType(int id);

        /// @CSharpLua.Template = "ConvertFrameEventType({0})"
        public static extern FrameEventType GetById(int id);
    }
}