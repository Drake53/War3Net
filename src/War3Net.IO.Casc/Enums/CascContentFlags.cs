// ------------------------------------------------------------------------------
// <copyright file="CascContentFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Content flags for CASC files (primarily used in WoW).
    /// </summary>
    [Flags]
    public enum CascContentFlags : uint
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// Install file.
        /// </summary>
        Install = 0x00000004,

        /// <summary>
        /// Load on Windows.
        /// </summary>
        LoadOnWindows = 0x00000008,

        /// <summary>
        /// Load on Mac.
        /// </summary>
        LoadOnMac = 0x00000010,

        /// <summary>
        /// x86 32-bit platform.
        /// </summary>
        X86_32 = 0x00000020,

        /// <summary>
        /// x86 64-bit platform.
        /// </summary>
        X86_64 = 0x00000040,

        /// <summary>
        /// Low violence version.
        /// </summary>
        LowViolence = 0x00000080,

        /// <summary>
        /// Don't load this file.
        /// </summary>
        DontLoad = 0x00000100,

        /// <summary>
        /// Update plugin.
        /// </summary>
        UpdatePlugin = 0x00000800,

        /// <summary>
        /// ARM64 platform.
        /// </summary>
        ARM64 = 0x00008000,

        /// <summary>
        /// File is encrypted.
        /// </summary>
        Encrypted = 0x08000000,

        /// <summary>
        /// No name hash.
        /// </summary>
        NoNameHash = 0x10000000,

        /// <summary>
        /// Uncommon resolution.
        /// </summary>
        UncommonResolution = 0x20000000,

        /// <summary>
        /// Bundle file.
        /// </summary>
        Bundle = 0x40000000,

        /// <summary>
        /// No compression.
        /// </summary>
        NoCompression = 0x80000000,
    }
}