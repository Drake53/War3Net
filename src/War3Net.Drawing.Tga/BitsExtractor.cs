using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgaLib
{
    /// <summary>
    /// Bits extraction utility.
    /// </summary>
    public /*internal*/ static class BitsExtractor
    {
        #region public methods

        /// <summary>
        /// Extract bits from byte.
        /// </summary>
        /// <param name="value">The 8-bit unsigned value to extract bits.</param>
        /// <param name="bitOffset">A bit offset that starts to extract bits.</param>
        /// <param name="extractBitCount">A bit count to extract.</param>
        /// <returns>Returns extracted bits.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The sum of <paramref name="bitOffset"/> and <paramref name="extractBitCount"/> is larger than the 8-bit.
        /// </exception>
        public static byte Extract(byte value, byte bitOffset, byte extractBitCount)
        {
            const byte BitCount = 8;

            if (bitOffset + extractBitCount > BitCount)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("The sum of {0}({1}) and {2}({3}) is larger than the {4} bit.",
                                  nameof(bitOffset),
                                  bitOffset,
                                  nameof(extractBitCount),
                                  extractBitCount,
                                  BitCount));
            }

            return (byte)(((uint)value >> bitOffset) & ((uint)byte.MaxValue >> (BitCount - extractBitCount)));
        }

        #endregion  // public methods
    }
}