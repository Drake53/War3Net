// ------------------------------------------------------------------------------
// <copyright file="CascLocaleFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Locale flags for CASC files.
    /// </summary>
    [Flags]
    public enum CascLocaleFlags : uint
    {
        /// <summary>
        /// No locale.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// Unknown locale 1.
        /// </summary>
        Unknown1 = 0x00000001,

        /// <summary>
        /// English (US).
        /// </summary>
        EnUS = 0x00000002,

        /// <summary>
        /// Korean.
        /// </summary>
        KoKR = 0x00000004,

        /// <summary>
        /// Reserved.
        /// </summary>
        Reserved = 0x00000008,

        /// <summary>
        /// French (France).
        /// </summary>
        FrFR = 0x00000010,

        /// <summary>
        /// German (Germany).
        /// </summary>
        DeDE = 0x00000020,

        /// <summary>
        /// Chinese (China).
        /// </summary>
        ZhCN = 0x00000040,

        /// <summary>
        /// Spanish (Spain).
        /// </summary>
        EsES = 0x00000080,

        /// <summary>
        /// Chinese (Taiwan).
        /// </summary>
        ZhTW = 0x00000100,

        /// <summary>
        /// English (Great Britain).
        /// </summary>
        EnGB = 0x00000200,

        /// <summary>
        /// English (China).
        /// </summary>
        EnCN = 0x00000400,

        /// <summary>
        /// English (Taiwan).
        /// </summary>
        EnTW = 0x00000800,

        /// <summary>
        /// Spanish (Mexico).
        /// </summary>
        EsMX = 0x00001000,

        /// <summary>
        /// Russian.
        /// </summary>
        RuRU = 0x00002000,

        /// <summary>
        /// Portuguese (Brazil).
        /// </summary>
        PtBR = 0x00004000,

        /// <summary>
        /// Italian (Italy).
        /// </summary>
        ItIT = 0x00008000,

        /// <summary>
        /// Portuguese (Portugal).
        /// </summary>
        PtPT = 0x00010000,

        /// <summary>
        /// All locales.
        /// </summary>
        All = 0xFFFFFFFF,

        /// <summary>
        /// All WoW locales except EnCN and EnTW.
        /// </summary>
        AllWoW = 0x0001F3F6,
    }
}