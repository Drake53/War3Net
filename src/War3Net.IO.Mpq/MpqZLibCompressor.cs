// ------------------------------------------------------------------------------
// <copyright file="MpqZLibCompressor.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using Ionic.Zlib;

using War3Net.IO.Compression;

namespace War3Net.IO.Mpq
{
    public class MpqZLibCompressor : IMpqCompressor
    {
        private static readonly Lazy<MpqZLibCompressor> _defaultInstance = new Lazy<MpqZLibCompressor>(() => new MpqZLibCompressor());

        private readonly CompressionLevel _compressionLevel;

        public MpqZLibCompressor(CompressionLevel compressionLevel)
        {
            _compressionLevel = compressionLevel;
        }

        private MpqZLibCompressor()
        {
            _compressionLevel = CompressionLevel.BestCompression;
        }

        public static MpqZLibCompressor Default => _defaultInstance.Value;

        public MpqCompressionType CompressionType => MpqCompressionType.ZLib;

        public Stream Compress(Stream stream, int bytesToCompress)
        {
            return ZLibCompression.Compress(stream, bytesToCompress, _compressionLevel, true);
        }
    }
}