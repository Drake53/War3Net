// ------------------------------------------------------------------------------
// <copyright file="StreamAssert.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
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

            for (var bytesRead = 0; bytesRead < lengthToCheck; bytesRead++)
            {
                var s1read = s1.ReadByte();
                var s2read = s2.ReadByte();

                if (s1read == s2read)
                {
                    // Console.WriteLine($"{bytesRead}: {s1read}");
                }
                else
                {
                    // Console.WriteLine($"{bytesRead}: {s1read} != {s2read}");
                    incorrectBytes++;
                    if (incorrectBytes == 1)
                    {
                        message += $"[Error]: First mismatch at byte {bytesRead}";
                    }
                }
            }

            if (incorrectBytes > 0)
            {
                result = false;
                message += $", {incorrectBytes}/{lengthToCheck} bytes were incorrect";
            }

            return result;
        }
    }
}