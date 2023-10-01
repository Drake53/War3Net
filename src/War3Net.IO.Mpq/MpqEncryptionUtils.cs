// ------------------------------------------------------------------------------
// <copyright file="MpqEncryptionUtils.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.IO.Mpq.Extensions;

namespace War3Net.IO.Mpq
{
    public static class MpqEncryptionUtils
    {
        public static uint CalculateEncryptionSeed(string? fileName)
        {
            return CalculateEncryptionSeed(fileName, out var encryptionSeed) ? encryptionSeed : 0;
        }

        internal static bool CalculateEncryptionSeed(string? fileName, out uint encryptionSeed)
        {
            var name = fileName.GetFileName();
            if (!string.IsNullOrEmpty(name) && StormBuffer.TryGetHashString(name, 0x300, out encryptionSeed))
            {
                return true;
            }

            encryptionSeed = 0;
            return false;
        }

        internal static uint CalculateEncryptionSeed(string? fileName, uint fileOffset, uint fileSize, MpqFileFlags flags)
        {
            if (fileName is null)
            {
                return 0;
            }

            var blockOffsetAdjusted = flags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey);
            var seed = CalculateEncryptionSeed(fileName);
            if (blockOffsetAdjusted)
            {
                seed = AdjustEncryptionSeed(seed, fileOffset, fileSize);
            }

            return seed;
        }

        internal static uint AdjustEncryptionSeed(uint baseSeed, uint fileOffset, uint fileSize)
        {
            return (baseSeed + fileOffset) ^ fileSize;
        }

        internal static uint UnadjustEncryptionSeed(uint adjustedSeed, uint fileOffset, uint fileSize)
        {
            return (adjustedSeed ^ fileSize) - fileOffset;
        }
    }
}