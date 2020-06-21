// ------------------------------------------------------------------------------
// <copyright file="BinaryWriterExtensionsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Common.Extensions;

namespace War3Net.Common.Tests.Extensions
{
    [TestClass]
    public sealed class BinaryWriterExtensionsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestWriteStrings), DynamicDataSourceType.Method)]
        public void TestWriteString(string? s, Type? expectedExceptionType = null)
        {
            using var memoryStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(memoryStream);

            try
            {
                binaryWriter.WriteString(s);
            }
            catch (Exception e)
            {
                if (expectedExceptionType is null)
                {
                    Assert.Fail(e.Message);
                }

                Assert.AreEqual(expectedExceptionType, e.GetType());
                return;
            }

            s ??= string.Empty;
            var expectedStringLength = s.Length;

            if (s.EndsWith(char.MinValue))
            {
                s = s.TrimEnd(char.MinValue);
            }
            else
            {
                expectedStringLength++;
            }

            Assert.AreEqual(expectedStringLength, memoryStream.Length);

            memoryStream.Position = 0;
            using var binaryReader = new BinaryReader(memoryStream);
            Assert.AreEqual(s, binaryReader.ReadChars());
        }

        private static IEnumerable<object?[]> GetTestWriteStrings()
        {
            yield return new[] { "Hello world!" };
            yield return new[] { "\uD83D\uDCAB" };
            yield return new[] { string.Empty };
            yield return new[] { (string?)null };

            yield return new object?[] { "String cannot contain \0 characters.", typeof(ArgumentException) };
            yield return new[] { "But it can if it's the last character: \0" };
            yield return new[] { "\0" };
        }
    }
}