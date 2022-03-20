// ------------------------------------------------------------------------------
// <copyright file="MpqCompressionTypeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.IO.Mpq.Extensions
{
    public static class MpqCompressionTypeExtensions
    {
        private static readonly Lazy<HashSet<MpqCompressionType>> _knownMpqCompressionTypes = new(GetKnownMpqCompressionTypes);

        public static bool IsKnownMpqCompressionType(this MpqCompressionType mpqCompressionType)
        {
            return _knownMpqCompressionTypes.Value.Contains(mpqCompressionType);
        }

        private static HashSet<MpqCompressionType> GetKnownMpqCompressionTypes()
        {
            return new HashSet<MpqCompressionType>
            {
                MpqCompressionType.Huffman,
                MpqCompressionType.ZLib,
                MpqCompressionType.PKLib,
                MpqCompressionType.BZip2,
                MpqCompressionType.Lzma,
                MpqCompressionType.Sparse,
                MpqCompressionType.ImaAdpcmMono,
                MpqCompressionType.ImaAdpcmStereo,

                MpqCompressionType.Sparse | MpqCompressionType.ZLib,
                MpqCompressionType.Sparse | MpqCompressionType.BZip2,

                MpqCompressionType.ImaAdpcmMono | MpqCompressionType.Huffman,
                MpqCompressionType.ImaAdpcmMono | MpqCompressionType.PKLib,

                MpqCompressionType.ImaAdpcmStereo | MpqCompressionType.Huffman,
                MpqCompressionType.ImaAdpcmStereo | MpqCompressionType.PKLib,
            };
        }
    }
}