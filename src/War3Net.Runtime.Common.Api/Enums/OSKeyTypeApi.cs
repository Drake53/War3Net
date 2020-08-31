// ------------------------------------------------------------------------------
// <copyright file="OSKeyTypeApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums;

namespace War3Net.Runtime.Common.Api.Enums
{
    public static class OSKeyTypeApi
    {
        public static readonly OSKeyType OSKEY_BACKSPACE = ConvertOsKeyType((int)OSKeyType.Type.Backspace);
        public static readonly OSKeyType OSKEY_TAB = ConvertOsKeyType((int)OSKeyType.Type.Tab);
        public static readonly OSKeyType OSKEY_CLEAR = ConvertOsKeyType((int)OSKeyType.Type.Clear);
        public static readonly OSKeyType OSKEY_RETURN = ConvertOsKeyType((int)OSKeyType.Type.Return);
        public static readonly OSKeyType OSKEY_SHIFT = ConvertOsKeyType((int)OSKeyType.Type.Shift);
        public static readonly OSKeyType OSKEY_CONTROL = ConvertOsKeyType((int)OSKeyType.Type.Control);
        public static readonly OSKeyType OSKEY_ALT = ConvertOsKeyType((int)OSKeyType.Type.Alt);
        public static readonly OSKeyType OSKEY_PAUSE = ConvertOsKeyType((int)OSKeyType.Type.Pause);
        public static readonly OSKeyType OSKEY_CAPSLOCK = ConvertOsKeyType((int)OSKeyType.Type.Capslock);
        public static readonly OSKeyType OSKEY_KANA = ConvertOsKeyType((int)OSKeyType.Type.Kana);
        public static readonly OSKeyType OSKEY_HANGUL = ConvertOsKeyType((int)OSKeyType.Type.Hangul);
        public static readonly OSKeyType OSKEY_JUNJA = ConvertOsKeyType((int)OSKeyType.Type.Junja);
        public static readonly OSKeyType OSKEY_FINAL = ConvertOsKeyType((int)OSKeyType.Type.Final);
        public static readonly OSKeyType OSKEY_HANJA = ConvertOsKeyType((int)OSKeyType.Type.Hanja);
        public static readonly OSKeyType OSKEY_KANJI = ConvertOsKeyType((int)OSKeyType.Type./*Kanji*/Hanja);
        public static readonly OSKeyType OSKEY_ESCAPE = ConvertOsKeyType((int)OSKeyType.Type.Escape);
        public static readonly OSKeyType OSKEY_CONVERT = ConvertOsKeyType((int)OSKeyType.Type.Convert);
        public static readonly OSKeyType OSKEY_NONCONVERT = ConvertOsKeyType((int)OSKeyType.Type.NonConvert);
        public static readonly OSKeyType OSKEY_ACCEPT = ConvertOsKeyType((int)OSKeyType.Type.Accept);
        public static readonly OSKeyType OSKEY_MODECHANGE = ConvertOsKeyType((int)OSKeyType.Type.ModeChange);
        public static readonly OSKeyType OSKEY_SPACE = ConvertOsKeyType((int)OSKeyType.Type.Space);
        public static readonly OSKeyType OSKEY_PAGEUP = ConvertOsKeyType((int)OSKeyType.Type.PageUp);
        public static readonly OSKeyType OSKEY_PAGEDOWN = ConvertOsKeyType((int)OSKeyType.Type.PageDown);
        public static readonly OSKeyType OSKEY_END = ConvertOsKeyType((int)OSKeyType.Type.End);
        public static readonly OSKeyType OSKEY_HOME = ConvertOsKeyType((int)OSKeyType.Type.Home);
        public static readonly OSKeyType OSKEY_LEFT = ConvertOsKeyType((int)OSKeyType.Type.Left);
        public static readonly OSKeyType OSKEY_UP = ConvertOsKeyType((int)OSKeyType.Type.Up);
        public static readonly OSKeyType OSKEY_RIGHT = ConvertOsKeyType((int)OSKeyType.Type.Right);
        public static readonly OSKeyType OSKEY_DOWN = ConvertOsKeyType((int)OSKeyType.Type.Down);
        public static readonly OSKeyType OSKEY_SELECT = ConvertOsKeyType((int)OSKeyType.Type.Select);
        public static readonly OSKeyType OSKEY_PRINT = ConvertOsKeyType((int)OSKeyType.Type.Print);
        public static readonly OSKeyType OSKEY_EXECUTE = ConvertOsKeyType((int)OSKeyType.Type.Execute);
        public static readonly OSKeyType OSKEY_PRINTSCREEN = ConvertOsKeyType((int)OSKeyType.Type.PrintScreen);
        public static readonly OSKeyType OSKEY_INSERT = ConvertOsKeyType((int)OSKeyType.Type.Insert);
        public static readonly OSKeyType OSKEY_DELETE = ConvertOsKeyType((int)OSKeyType.Type.Delete);
        public static readonly OSKeyType OSKEY_HELP = ConvertOsKeyType((int)OSKeyType.Type.Help);
        public static readonly OSKeyType OSKEY_0 = ConvertOsKeyType((int)OSKeyType.Type.Number0);
        public static readonly OSKeyType OSKEY_1 = ConvertOsKeyType((int)OSKeyType.Type.Number1);
        public static readonly OSKeyType OSKEY_2 = ConvertOsKeyType((int)OSKeyType.Type.Number2);
        public static readonly OSKeyType OSKEY_3 = ConvertOsKeyType((int)OSKeyType.Type.Number3);
        public static readonly OSKeyType OSKEY_4 = ConvertOsKeyType((int)OSKeyType.Type.Number4);
        public static readonly OSKeyType OSKEY_5 = ConvertOsKeyType((int)OSKeyType.Type.Number5);
        public static readonly OSKeyType OSKEY_6 = ConvertOsKeyType((int)OSKeyType.Type.Number6);
        public static readonly OSKeyType OSKEY_7 = ConvertOsKeyType((int)OSKeyType.Type.Number7);
        public static readonly OSKeyType OSKEY_8 = ConvertOsKeyType((int)OSKeyType.Type.Number8);
        public static readonly OSKeyType OSKEY_9 = ConvertOsKeyType((int)OSKeyType.Type.Number9);
        public static readonly OSKeyType OSKEY_A = ConvertOsKeyType((int)OSKeyType.Type.A);
        public static readonly OSKeyType OSKEY_B = ConvertOsKeyType((int)OSKeyType.Type.B);
        public static readonly OSKeyType OSKEY_C = ConvertOsKeyType((int)OSKeyType.Type.C);
        public static readonly OSKeyType OSKEY_D = ConvertOsKeyType((int)OSKeyType.Type.D);
        public static readonly OSKeyType OSKEY_E = ConvertOsKeyType((int)OSKeyType.Type.E);
        public static readonly OSKeyType OSKEY_F = ConvertOsKeyType((int)OSKeyType.Type.F);
        public static readonly OSKeyType OSKEY_G = ConvertOsKeyType((int)OSKeyType.Type.G);
        public static readonly OSKeyType OSKEY_H = ConvertOsKeyType((int)OSKeyType.Type.H);
        public static readonly OSKeyType OSKEY_I = ConvertOsKeyType((int)OSKeyType.Type.I);
        public static readonly OSKeyType OSKEY_J = ConvertOsKeyType((int)OSKeyType.Type.J);
        public static readonly OSKeyType OSKEY_K = ConvertOsKeyType((int)OSKeyType.Type.K);
        public static readonly OSKeyType OSKEY_L = ConvertOsKeyType((int)OSKeyType.Type.L);
        public static readonly OSKeyType OSKEY_M = ConvertOsKeyType((int)OSKeyType.Type.M);
        public static readonly OSKeyType OSKEY_N = ConvertOsKeyType((int)OSKeyType.Type.N);
        public static readonly OSKeyType OSKEY_O = ConvertOsKeyType((int)OSKeyType.Type.O);
        public static readonly OSKeyType OSKEY_P = ConvertOsKeyType((int)OSKeyType.Type.P);
        public static readonly OSKeyType OSKEY_Q = ConvertOsKeyType((int)OSKeyType.Type.Q);
        public static readonly OSKeyType OSKEY_R = ConvertOsKeyType((int)OSKeyType.Type.R);
        public static readonly OSKeyType OSKEY_S = ConvertOsKeyType((int)OSKeyType.Type.S);
        public static readonly OSKeyType OSKEY_T = ConvertOsKeyType((int)OSKeyType.Type.T);
        public static readonly OSKeyType OSKEY_U = ConvertOsKeyType((int)OSKeyType.Type.U);
        public static readonly OSKeyType OSKEY_V = ConvertOsKeyType((int)OSKeyType.Type.V);
        public static readonly OSKeyType OSKEY_W = ConvertOsKeyType((int)OSKeyType.Type.W);
        public static readonly OSKeyType OSKEY_X = ConvertOsKeyType((int)OSKeyType.Type.X);
        public static readonly OSKeyType OSKEY_Y = ConvertOsKeyType((int)OSKeyType.Type.Y);
        public static readonly OSKeyType OSKEY_Z = ConvertOsKeyType((int)OSKeyType.Type.Z);
        public static readonly OSKeyType OSKEY_LMETA = ConvertOsKeyType((int)OSKeyType.Type.LMeta);
        public static readonly OSKeyType OSKEY_RMETA = ConvertOsKeyType((int)OSKeyType.Type.RMeta);
        public static readonly OSKeyType OSKEY_APPS = ConvertOsKeyType((int)OSKeyType.Type.Apps);
        public static readonly OSKeyType OSKEY_SLEEP = ConvertOsKeyType((int)OSKeyType.Type.Sleep);
        public static readonly OSKeyType OSKEY_NUMPAD0 = ConvertOsKeyType((int)OSKeyType.Type.Numpad0);
        public static readonly OSKeyType OSKEY_NUMPAD1 = ConvertOsKeyType((int)OSKeyType.Type.Numpad1);
        public static readonly OSKeyType OSKEY_NUMPAD2 = ConvertOsKeyType((int)OSKeyType.Type.Numpad2);
        public static readonly OSKeyType OSKEY_NUMPAD3 = ConvertOsKeyType((int)OSKeyType.Type.Numpad3);
        public static readonly OSKeyType OSKEY_NUMPAD4 = ConvertOsKeyType((int)OSKeyType.Type.Numpad4);
        public static readonly OSKeyType OSKEY_NUMPAD5 = ConvertOsKeyType((int)OSKeyType.Type.Numpad5);
        public static readonly OSKeyType OSKEY_NUMPAD6 = ConvertOsKeyType((int)OSKeyType.Type.Numpad6);
        public static readonly OSKeyType OSKEY_NUMPAD7 = ConvertOsKeyType((int)OSKeyType.Type.Numpad7);
        public static readonly OSKeyType OSKEY_NUMPAD8 = ConvertOsKeyType((int)OSKeyType.Type.Numpad8);
        public static readonly OSKeyType OSKEY_NUMPAD9 = ConvertOsKeyType((int)OSKeyType.Type.Numpad9);
        public static readonly OSKeyType OSKEY_MULTIPLY = ConvertOsKeyType((int)OSKeyType.Type.Multiply);
        public static readonly OSKeyType OSKEY_ADD = ConvertOsKeyType((int)OSKeyType.Type.Add);
        public static readonly OSKeyType OSKEY_SEPARATOR = ConvertOsKeyType((int)OSKeyType.Type.Separator);
        public static readonly OSKeyType OSKEY_SUBTRACT = ConvertOsKeyType((int)OSKeyType.Type.Subtract);
        public static readonly OSKeyType OSKEY_DECIMAL = ConvertOsKeyType((int)OSKeyType.Type.Decimal);
        public static readonly OSKeyType OSKEY_DIVIDE = ConvertOsKeyType((int)OSKeyType.Type.Divide);
        public static readonly OSKeyType OSKEY_F1 = ConvertOsKeyType((int)OSKeyType.Type.F1);
        public static readonly OSKeyType OSKEY_F2 = ConvertOsKeyType((int)OSKeyType.Type.F2);
        public static readonly OSKeyType OSKEY_F3 = ConvertOsKeyType((int)OSKeyType.Type.F3);
        public static readonly OSKeyType OSKEY_F4 = ConvertOsKeyType((int)OSKeyType.Type.F4);
        public static readonly OSKeyType OSKEY_F5 = ConvertOsKeyType((int)OSKeyType.Type.F5);
        public static readonly OSKeyType OSKEY_F6 = ConvertOsKeyType((int)OSKeyType.Type.F6);
        public static readonly OSKeyType OSKEY_F7 = ConvertOsKeyType((int)OSKeyType.Type.F7);
        public static readonly OSKeyType OSKEY_F8 = ConvertOsKeyType((int)OSKeyType.Type.F8);
        public static readonly OSKeyType OSKEY_F9 = ConvertOsKeyType((int)OSKeyType.Type.F9);
        public static readonly OSKeyType OSKEY_F10 = ConvertOsKeyType((int)OSKeyType.Type.F10);
        public static readonly OSKeyType OSKEY_F11 = ConvertOsKeyType((int)OSKeyType.Type.F11);
        public static readonly OSKeyType OSKEY_F12 = ConvertOsKeyType((int)OSKeyType.Type.F12);
        public static readonly OSKeyType OSKEY_F13 = ConvertOsKeyType((int)OSKeyType.Type.F13);
        public static readonly OSKeyType OSKEY_F14 = ConvertOsKeyType((int)OSKeyType.Type.F14);
        public static readonly OSKeyType OSKEY_F15 = ConvertOsKeyType((int)OSKeyType.Type.F15);
        public static readonly OSKeyType OSKEY_F16 = ConvertOsKeyType((int)OSKeyType.Type.F16);
        public static readonly OSKeyType OSKEY_F17 = ConvertOsKeyType((int)OSKeyType.Type.F17);
        public static readonly OSKeyType OSKEY_F18 = ConvertOsKeyType((int)OSKeyType.Type.F18);
        public static readonly OSKeyType OSKEY_F19 = ConvertOsKeyType((int)OSKeyType.Type.F19);
        public static readonly OSKeyType OSKEY_F20 = ConvertOsKeyType((int)OSKeyType.Type.F20);
        public static readonly OSKeyType OSKEY_F21 = ConvertOsKeyType((int)OSKeyType.Type.F21);
        public static readonly OSKeyType OSKEY_F22 = ConvertOsKeyType((int)OSKeyType.Type.F22);
        public static readonly OSKeyType OSKEY_F23 = ConvertOsKeyType((int)OSKeyType.Type.F23);
        public static readonly OSKeyType OSKEY_F24 = ConvertOsKeyType((int)OSKeyType.Type.F24);
        public static readonly OSKeyType OSKEY_NUMLOCK = ConvertOsKeyType((int)OSKeyType.Type.Numlock);
        public static readonly OSKeyType OSKEY_SCROLLLOCK = ConvertOsKeyType((int)OSKeyType.Type.Scrolllock);
        public static readonly OSKeyType OSKEY_OEM_NEC_EQUAL = ConvertOsKeyType((int)OSKeyType.Type./*OemNecEqual*/OemFJJisho);
        public static readonly OSKeyType OSKEY_OEM_FJ_JISHO = ConvertOsKeyType((int)OSKeyType.Type.OemFJJisho);
        public static readonly OSKeyType OSKEY_OEM_FJ_MASSHOU = ConvertOsKeyType((int)OSKeyType.Type.OemFJMasshou);
        public static readonly OSKeyType OSKEY_OEM_FJ_TOUROKU = ConvertOsKeyType((int)OSKeyType.Type.OemFJTouroku);
        public static readonly OSKeyType OSKEY_OEM_FJ_LOYA = ConvertOsKeyType((int)OSKeyType.Type.OemFJLoya);
        public static readonly OSKeyType OSKEY_OEM_FJ_ROYA = ConvertOsKeyType((int)OSKeyType.Type.OemFJRoya);
        public static readonly OSKeyType OSKEY_LSHIFT = ConvertOsKeyType((int)OSKeyType.Type.LShift);
        public static readonly OSKeyType OSKEY_RSHIFT = ConvertOsKeyType((int)OSKeyType.Type.RShift);
        public static readonly OSKeyType OSKEY_LCONTROL = ConvertOsKeyType((int)OSKeyType.Type.LControl);
        public static readonly OSKeyType OSKEY_RCONTROL = ConvertOsKeyType((int)OSKeyType.Type.RControl);
        public static readonly OSKeyType OSKEY_LALT = ConvertOsKeyType((int)OSKeyType.Type.LAlt);
        public static readonly OSKeyType OSKEY_RALT = ConvertOsKeyType((int)OSKeyType.Type.RAlt);
        public static readonly OSKeyType OSKEY_BROWSER_BACK = ConvertOsKeyType((int)OSKeyType.Type.BrowserBack);
        public static readonly OSKeyType OSKEY_BROWSER_FORWARD = ConvertOsKeyType((int)OSKeyType.Type.BrowserForward);
        public static readonly OSKeyType OSKEY_BROWSER_REFRESH = ConvertOsKeyType((int)OSKeyType.Type.BrowserRefresh);
        public static readonly OSKeyType OSKEY_BROWSER_STOP = ConvertOsKeyType((int)OSKeyType.Type.BrowserStop);
        public static readonly OSKeyType OSKEY_BROWSER_SEARCH = ConvertOsKeyType((int)OSKeyType.Type.BrowserSearch);
        public static readonly OSKeyType OSKEY_BROWSER_FAVORITES = ConvertOsKeyType((int)OSKeyType.Type.BrowserFavorites);
        public static readonly OSKeyType OSKEY_BROWSER_HOME = ConvertOsKeyType((int)OSKeyType.Type.BrowserHome);
        public static readonly OSKeyType OSKEY_VOLUME_MUTE = ConvertOsKeyType((int)OSKeyType.Type.VolumeMute);
        public static readonly OSKeyType OSKEY_VOLUME_DOWN = ConvertOsKeyType((int)OSKeyType.Type.VolumeDown);
        public static readonly OSKeyType OSKEY_VOLUME_UP = ConvertOsKeyType((int)OSKeyType.Type.VolumeUp);
        public static readonly OSKeyType OSKEY_MEDIA_NEXT_TRACK = ConvertOsKeyType((int)OSKeyType.Type.MediaNextTrack);
        public static readonly OSKeyType OSKEY_MEDIA_PREV_TRACK = ConvertOsKeyType((int)OSKeyType.Type.MediaPreviousTrack);
        public static readonly OSKeyType OSKEY_MEDIA_STOP = ConvertOsKeyType((int)OSKeyType.Type.MediaStop);
        public static readonly OSKeyType OSKEY_MEDIA_PLAY_PAUSE = ConvertOsKeyType((int)OSKeyType.Type.MediaPlayPause);
        public static readonly OSKeyType OSKEY_LAUNCH_MAIL = ConvertOsKeyType((int)OSKeyType.Type.LaunchMail);
        public static readonly OSKeyType OSKEY_LAUNCH_MEDIA_SELECT = ConvertOsKeyType((int)OSKeyType.Type.LaunchMediaSelect);
        public static readonly OSKeyType OSKEY_LAUNCH_APP1 = ConvertOsKeyType((int)OSKeyType.Type.LaunchApp1);
        public static readonly OSKeyType OSKEY_LAUNCH_APP2 = ConvertOsKeyType((int)OSKeyType.Type.LaunchApp2);
        public static readonly OSKeyType OSKEY_OEM_1 = ConvertOsKeyType((int)OSKeyType.Type.Oem1);
        public static readonly OSKeyType OSKEY_OEM_PLUS = ConvertOsKeyType((int)OSKeyType.Type.OemPlus);
        public static readonly OSKeyType OSKEY_OEM_COMMA = ConvertOsKeyType((int)OSKeyType.Type.OemComma);
        public static readonly OSKeyType OSKEY_OEM_MINUS = ConvertOsKeyType((int)OSKeyType.Type.OemMinus);
        public static readonly OSKeyType OSKEY_OEM_PERIOD = ConvertOsKeyType((int)OSKeyType.Type.OemPeriod);
        public static readonly OSKeyType OSKEY_OEM_2 = ConvertOsKeyType((int)OSKeyType.Type.Oem2);
        public static readonly OSKeyType OSKEY_OEM_3 = ConvertOsKeyType((int)OSKeyType.Type.Oem3);
        public static readonly OSKeyType OSKEY_OEM_4 = ConvertOsKeyType((int)OSKeyType.Type.Oem4);
        public static readonly OSKeyType OSKEY_OEM_5 = ConvertOsKeyType((int)OSKeyType.Type.Oem5);
        public static readonly OSKeyType OSKEY_OEM_6 = ConvertOsKeyType((int)OSKeyType.Type.Oem6);
        public static readonly OSKeyType OSKEY_OEM_7 = ConvertOsKeyType((int)OSKeyType.Type.Oem7);
        public static readonly OSKeyType OSKEY_OEM_8 = ConvertOsKeyType((int)OSKeyType.Type.Oem8);
        public static readonly OSKeyType OSKEY_OEM_AX = ConvertOsKeyType((int)OSKeyType.Type.OemAX);
        public static readonly OSKeyType OSKEY_OEM_102 = ConvertOsKeyType((int)OSKeyType.Type.Oem102);
        public static readonly OSKeyType OSKEY_ICO_HELP = ConvertOsKeyType((int)OSKeyType.Type.IcoHelp);
        public static readonly OSKeyType OSKEY_ICO_00 = ConvertOsKeyType((int)OSKeyType.Type.Ico00);
        public static readonly OSKeyType OSKEY_PROCESSKEY = ConvertOsKeyType((int)OSKeyType.Type.ProcessKey);
        public static readonly OSKeyType OSKEY_ICO_CLEAR = ConvertOsKeyType((int)OSKeyType.Type.IcoClear);
        public static readonly OSKeyType OSKEY_PACKET = ConvertOsKeyType((int)OSKeyType.Type.Packet);
        public static readonly OSKeyType OSKEY_OEM_RESET = ConvertOsKeyType((int)OSKeyType.Type.OemReset);
        public static readonly OSKeyType OSKEY_OEM_JUMP = ConvertOsKeyType((int)OSKeyType.Type.OemJump);
        public static readonly OSKeyType OSKEY_OEM_PA1 = ConvertOsKeyType((int)OSKeyType.Type.OemPA1);
        public static readonly OSKeyType OSKEY_OEM_PA2 = ConvertOsKeyType((int)OSKeyType.Type.OemPA2);
        public static readonly OSKeyType OSKEY_OEM_PA3 = ConvertOsKeyType((int)OSKeyType.Type.OemPA3);
        public static readonly OSKeyType OSKEY_OEM_WSCTRL = ConvertOsKeyType((int)OSKeyType.Type.OemWSCtrl);
        public static readonly OSKeyType OSKEY_OEM_CUSEL = ConvertOsKeyType((int)OSKeyType.Type.OemCuSel);
        public static readonly OSKeyType OSKEY_OEM_ATTN = ConvertOsKeyType((int)OSKeyType.Type.OemAttn);
        public static readonly OSKeyType OSKEY_OEM_FINISH = ConvertOsKeyType((int)OSKeyType.Type.OemFinish);
        public static readonly OSKeyType OSKEY_OEM_COPY = ConvertOsKeyType((int)OSKeyType.Type.OemCopy);
        public static readonly OSKeyType OSKEY_OEM_AUTO = ConvertOsKeyType((int)OSKeyType.Type.OemAuto);
        public static readonly OSKeyType OSKEY_OEM_ENLW = ConvertOsKeyType((int)OSKeyType.Type.OemEnlW);
        public static readonly OSKeyType OSKEY_OEM_BACKTAB = ConvertOsKeyType((int)OSKeyType.Type.OemBackTab);
        public static readonly OSKeyType OSKEY_ATTN = ConvertOsKeyType((int)OSKeyType.Type.Attn);
        public static readonly OSKeyType OSKEY_CRSEL = ConvertOsKeyType((int)OSKeyType.Type.CrSel);
        public static readonly OSKeyType OSKEY_EXSEL = ConvertOsKeyType((int)OSKeyType.Type.ExSel);
        public static readonly OSKeyType OSKEY_EREOF = ConvertOsKeyType((int)OSKeyType.Type.ErEof);
        public static readonly OSKeyType OSKEY_PLAY = ConvertOsKeyType((int)OSKeyType.Type.Play);
        public static readonly OSKeyType OSKEY_ZOOM = ConvertOsKeyType((int)OSKeyType.Type.Zoom);
        public static readonly OSKeyType OSKEY_NONAME = ConvertOsKeyType((int)OSKeyType.Type.NoName);
        public static readonly OSKeyType OSKEY_PA1 = ConvertOsKeyType((int)OSKeyType.Type.PA1);
        public static readonly OSKeyType OSKEY_OEM_CLEAR = ConvertOsKeyType((int)OSKeyType.Type.OemClear);

        public static OSKeyType ConvertOsKeyType(int i)
        {
            return OSKeyType.GetOSKeyType(i);
        }
    }
}