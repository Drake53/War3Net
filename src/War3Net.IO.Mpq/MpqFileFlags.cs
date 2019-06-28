// ------------------------------------------------------------------------------
// <copyright file="MpqFileFlags.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace Foole.Mpq
{
    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum MpqFileFlags : uint
    {
        /// <summary>
        /// AKA Imploded
        /// </summary>
        CompressedPK = 0x100,

        /// <summary>
        ///
        /// </summary>
        CompressedMulti = 0x200,

        /// <summary>
        ///
        /// </summary>
        Compressed = 0xff00,

        /// <summary>
        ///
        /// </summary>
        Encrypted = 0x10000,

        /// <summary>
        /// AKA FixSeed
        /// </summary>
        BlockOffsetAdjustedKey = 0x020000,

        /// <summary>
        ///
        /// </summary>
        SingleUnit = 0x1000000,

        /// <summary>
        /// Appears in WoW 1.10 or newer. Indicates the file has associated metadata.
        /// </summary>
        FileHasMetadata = 0x04000000,

        /// <summary>
        ///
        /// </summary>
        Exists = 0x80000000,
    }
}