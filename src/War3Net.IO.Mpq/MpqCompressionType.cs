// ------------------------------------------------------------------------------
// <copyright file="MpqCompressionType.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Mpq
{
    [Flags]
    internal enum MpqCompressionType : byte
    {
        Huffman = 0x01,

        ZLib = 0x02,

        UNK = 0x04,

        PKLib = 0x08,

        BZip2 = 0x10,

        /// <summary>
        /// Lempel–Ziv–Markov chain
        /// </summary>
        Lzma = 0x12,

        Sparse = 0x20,

        ImaAdpcmMono = 0x40,

        ImaAdpcmStereo = 0x80,
    }
}