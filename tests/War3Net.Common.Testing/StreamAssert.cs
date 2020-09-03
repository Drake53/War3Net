// ------------------------------------------------------------------------------
// <copyright file="StreamAssert.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#define BUFFER_STREAM_DATA

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#if DEBUG
using System.Collections.Generic;
#endif

namespace War3Net.Common.Testing
{
    public static class StreamAssert
    {
        public static void AreEqual(Stream expected, Stream actual, bool resetPositions = false, bool allowPadding = false)
        {
            var expectedSize = expected.Length;
            var actualSize = actual.Length;
            if (resetPositions)
            {
                if (allowPadding)
                {
                    while (expected.Position < expected.Length)
                    {
                        Assert.AreEqual(0, expected.ReadByte(), "Padding bytes must be zeroes.");
                        actual.WriteByte(0);
                    }
                }
                else
                {
                    Assert.AreEqual(expected.Length, expected.Position, "Expected to be at end of stream.");
                }

                expected.Position = 0;
                actual.Position = 0;
            }
            else
            {
                expectedSize -= expected.Position;
                actualSize -= actual.Position;
            }

            AreEqual(expected, actual, expectedSize > actualSize ? expectedSize : actualSize);
        }

        public static void AreEqualText(Stream expected, Stream actual, bool resetPositions = false)
        {
            var expectedSize = expected.Length;
            var actualSize = actual.Length;
            if (resetPositions)
            {
                Assert.AreEqual(expected.Length, expected.Position, "Expected to be at end of stream.");

                expected.Position = 0;
                actual.Position = 0;
            }
            else
            {
                expectedSize -= expected.Position;
                actualSize -= actual.Position;
            }

            AreEqualText(expected, actual, expectedSize > actualSize ? expectedSize : actualSize);
        }

        public static void AreEqual(Stream expected, Stream actual, long lengthToCheck)
        {
            Assert.IsTrue(AreStreamsEqual(expected, actual, lengthToCheck, false, out var message), message);
        }

        public static void AreEqualText(Stream expected, Stream actual, long lengthToCheck)
        {
            Assert.IsTrue(AreStreamsEqual(expected, actual, lengthToCheck, true, out var message), message);
        }

        private static bool AreStreamsEqual(Stream expected, Stream actual, long lengthToCheck, bool isText, out string message)
        {
            var result = true;
            var incorrectBytes = 0;
            message = "\r\n";

            var lengthRemaining1 = expected.Length - expected.Position;
            var lengthRemaining2 = actual.Length - actual.Position;

            var line = 1;
            var offset = 1;

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
                    message += $"[Error]: Streams have different length: expected {lengthRemaining1}, actual {lengthRemaining2}.\r\n";
                    result = false;
                }
            }

#if BUFFER_STREAM_DATA
            var data1 = new byte[lengthToCheck];
            if (expected.Read(data1, 0, (int)lengthToCheck) != lengthToCheck)
            {
                message += $"[Error]: Could not buffer stream 1.\r\n";
                result = false;
            }

            var data2 = new byte[lengthToCheck];
            if (actual.Read(data2, 0, (int)lengthToCheck) != lengthToCheck)
            {
                message += $"[Error]: Could not buffer stream 2.\r\n";
                result = false;
            }
#endif

#if DEBUG
            var incorrectBytePositions = new List<int>();
#endif

            for (var bytesRead = 0; bytesRead < lengthToCheck; bytesRead++)
            {
#if BUFFER_STREAM_DATA
                if (data1[bytesRead] != data2[bytesRead])
#else
                if (s1.ReadByte() != s2.ReadByte())
#endif
                {
#if DEBUG
                    incorrectBytePositions.Add(bytesRead);
#endif

                    incorrectBytes++;
                    if (incorrectBytes == 1)
                    {
#if BUFFER_STREAM_DATA
                        if (isText)
                        {
                            message += $"[Error]: First mismatch at ({line},{offset}) (expected '{((char)data1[bytesRead]).Escape()}', actual '{((char)data2[bytesRead]).Escape()}')";
                        }
                        else
                        {
                            message += $"[Error]: First mismatch at byte {bytesRead} (expected {data1[bytesRead]}, actual {data2[bytesRead]})";
                        }
#else
                        message += $"[Error]: First mismatch at byte {bytesRead}";
#endif
                        result = false;
                    }
                }
                else
                {
                    offset++;
                    if (data1[bytesRead] == '\n')
                    {
                        line++;
                        offset = 1;
                    }
                }
            }

            if (incorrectBytes > 0)
            {
                message += $", {100 * incorrectBytes / (float)lengthToCheck}% of {lengthToCheck} bytes were incorrect";
            }

            return result;
        }

        private static string Escape(this char @char)
        {
            return @char switch
            {
                '\n' => @"\n",
                '\r' => @"\r",
                '\t' => @"\t",
                '\f' => @"\f",

                _ => $"{@char}",
            };
        }
    }
}