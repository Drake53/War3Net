// ------------------------------------------------------------------------------
// <copyright file="DialogEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor

namespace War3Net.Runtime.Api.Enums
{
    public class DialogEvent : EventId
    {
        /// @CSharpLua.Template = "EVENT_DIALOG_BUTTON_CLICK"
        public static readonly DialogEvent ButtonClick;

        /// @CSharpLua.Template = "EVENT_DIALOG_CLICK"
        public static readonly DialogEvent Click;

        /// @CSharpLua.Template = "ConvertDialogEvent({0})"
        public extern DialogEvent(int id);

        /// @CSharpLua.Template = "ConvertDialogEvent({0})"
        public static extern DialogEvent GetById(int id);
    }
}