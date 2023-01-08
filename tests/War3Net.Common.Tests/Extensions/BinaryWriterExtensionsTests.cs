// ------------------------------------------------------------------------------
// <copyright file="BinaryWriterExtensionsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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
            using var writer = new BinaryWriter(memoryStream);

            try
            {
                writer.WriteString(s);
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

            if (!s.EndsWith(char.MinValue))
            {
                s += char.MinValue;
                expectedStringLength++;
            }

            foreach (var c in s)
            {
                if (char.IsSurrogate(c))
                {
                    expectedStringLength++;
                }
            }

            Assert.AreEqual(expectedStringLength, memoryStream.Length);

            memoryStream.Position = 0;
            using var reader = new BinaryReader(memoryStream);
            Assert.AreEqual(s, new string(reader.ReadChars(expectedStringLength)));
        }

        private static IEnumerable<object?[]> GetTestWriteStrings()
        {
            return TestData.GetTestStrings();
        }
    }
}