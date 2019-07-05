// ------------------------------------------------------------------------------
// <copyright file="MpqLocale.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1028 // Enum Storage should be Int32

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// The locale of an <see cref="MpqFile"/>.
    /// </summary>
    public enum MpqLocale : uint
    {
        /// <summary>
        /// The default locale, commonly used when the <see cref="MpqArchive"/> does not contain localized files.
        /// </summary>
        Neutral = 0,

        /// <summary>
        /// The zh-TW locale.
        /// </summary>
        Chinese = 0x404,

        /// <summary>
        /// The cs locale.
        /// </summary>
        Czech = 0x405,

        /// <summary>
        /// The de locale.
        /// </summary>
        German = 0x407,

        /// <summary>
        /// The en-US locale.
        /// </summary>
        English = 0x409,

        /// <summary>
        /// The es locale.
        /// </summary>
        Spanish = 0x40a,

        /// <summary>
        /// The fr locale.
        /// </summary>
        French = 0x40c,

        /// <summary>
        /// The it locale.
        /// </summary>
        Italian = 0x410,

        /// <summary>
        /// The ja locale.
        /// </summary>
        Japanese = 0x411,

        /// <summary>
        /// The ko locale.
        /// </summary>
        Korean = 0x412,

        /// <summary>
        /// The pl locale.
        /// </summary>
        Polish = 0x415,

        /// <summary>
        /// The pt locale.
        /// </summary>
        Portuguese = 0x416,

        /// <summary>
        /// The ru locale.
        /// </summary>
        Russian = 0x419,

        /// <summary>
        /// The en-GB locale.
        /// </summary>
        EnglishUK = 0x809,
    }
}