// ------------------------------------------------------------------------------
// <copyright file="StreamAssert.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#define BUFFER_STREAM_DATA

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.IO.Mpq.Tests
{
    internal static class StreamAssert
    {
        internal static void AreEqual(Stream s1, Stream s2)
        {
            var size1 = s1.Length;
            var size2 = s2.Length;
            AreEqual(s1, s2, size1 > size2 ? size1 : size2);
        }

        internal static void AreEqual(Stream s1, Stream s2, long lengthToCheck)
        {
            Assert.IsTrue(AreStreamsEqual(s1, s2, lengthToCheck, out var message), message);
        }

        private static bool AreStreamsEqual(Stream s1, Stream s2, long lengthToCheck, out string message)
        {
            var result = true;
            var incorrectBytes = 0;
            message = "\r\n";

            var lengthRemaining1 = s1.Length - s1.Position;
            var lengthRemaining2 = s2.Length - s2.Position;

            if (lengthRemaining1 == lengthRemaining2)
            {
                if (lengthToCheck > lengthRemaining1)
                {
                    message += $"[Warning]: Checking {lengthToCheck} bytes in streams, when they only have {lengthRemaining1} bytes remaining to be read.\r\n";
                }
            }
            else
            {
                if (lengthToCheck - lengthRemaining1 > 0 || lengthToCheck - lengthRemaining2 > 0)
                {
                    message += $"[Error]: Streams have different length: {lengthRemaining1} vs {lengthRemaining2}.\r\n";
                    result = false;
                }
            }

#if BUFFER_STREAM_DATA
            var data1 = new byte[lengthRemaining1];
            if (s1.Read(data1, 0, (int)lengthRemaining1) != lengthRemaining1)
            {
                message += $"[Error]: Could not buffer stream 1.\r\n";
                result = false;
            }

            var data2 = new byte[lengthRemaining2];
            if (s2.Read(data2, 0, (int)lengthRemaining2) != lengthRemaining2)
            {
                message += $"[Error]: Could not buffer stream 2.\r\n";
                result = false;
            }
#endif

            for (var bytesRead = 0; bytesRead < lengthToCheck; bytesRead++)
            {
#if BUFFER_STREAM_DATA
                if (data1[bytesRead] != data2[bytesRead])
#else
                if (s1.ReadByte() != s2.ReadByte())
#endif
                {
                    incorrectBytes++;
                    if (incorrectBytes == 1)
                    {
                        message += $"[Error]: First mismatch at byte {bytesRead}";
                        result = false;
                    }
                }
            }

            if (incorrectBytes > 0)
            {
                message += $", {incorrectBytes}/{lengthToCheck} bytes were incorrect";
            }

            return result;
        }
    }
}