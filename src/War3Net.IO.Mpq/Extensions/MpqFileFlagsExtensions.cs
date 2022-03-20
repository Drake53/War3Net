// ------------------------------------------------------------------------------
// <copyright file="MpqFileFlagsExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Mpq.Extensions
{
    public static class MpqFileFlagsExtensions
    {
        /// <summary>
        /// Returns <see langword="true"/> if <paramref name="mpqFileFlags"/> has both the <see cref="MpqFileFlags.Encrypted"/> and <see cref="MpqFileFlags.BlockOffsetAdjustedKey"/> flags.
        /// </summary>
        public static bool IsOffsetEncrypted(this MpqFileFlags mpqFileFlags)
        {
            return mpqFileFlags.HasFlag(MpqFileFlags.Encrypted | MpqFileFlags.BlockOffsetAdjustedKey);
        }
    }
}