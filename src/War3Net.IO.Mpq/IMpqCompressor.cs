﻿// ------------------------------------------------------------------------------
// <copyright file="IMpqCompressor.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Mpq
{
    public interface IMpqCompressor
    {
        MpqCompressionType CompressionType { get; }

        Stream Compress(Stream stream, int bytesToCompress);
    }
}