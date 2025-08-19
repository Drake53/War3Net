// ------------------------------------------------------------------------------
// <copyright file="BinaryWriterExtensionsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Structures;
using War3Net.IO.Casc.Utilities;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class BinaryWriterExtensionsTests
    {
        [TestMethod]
        public void TestWriteUInt16BE()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteUInt16BE(0x1234);

            var result = stream.ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0x12, result[0]);
            Assert.AreEqual(0x34, result[1]);
        }

        [TestMethod]
        public void TestWriteUInt32BE()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteUInt32BE(0x12345678);

            var result = stream.ToArray();
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(0x12, result[0]);
            Assert.AreEqual(0x34, result[1]);
            Assert.AreEqual(0x56, result[2]);
            Assert.AreEqual(0x78, result[3]);
        }

        [TestMethod]
        public void TestWriteUInt64BE()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteUInt64BE(0x123456789ABCDEF0);

            var result = stream.ToArray();
            Assert.AreEqual(8, result.Length);
            Assert.AreEqual(0x12, result[0]);
            Assert.AreEqual(0x34, result[1]);
            Assert.AreEqual(0x56, result[2]);
            Assert.AreEqual(0x78, result[3]);
            Assert.AreEqual(0x9A, result[4]);
            Assert.AreEqual(0xBC, result[5]);
            Assert.AreEqual(0xDE, result[6]);
            Assert.AreEqual(0xF0, result[7]);
        }

        [TestMethod]
        public void TestWriteBigEndianValue()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteBigEndianValue(0x123456, 3);

            var result = stream.ToArray();
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(0x12, result[0]);
            Assert.AreEqual(0x34, result[1]);
            Assert.AreEqual(0x56, result[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWriteBigEndianValueTooManyBytes()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteBigEndianValue(0x123456789ABCDEF0, 9); // Should throw
        }

        [TestMethod]
        public void TestWriteCString()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var testString = "Hello, World!";
            writer.WriteCString(testString);

            var result = stream.ToArray();
            var expectedBytes = System.Text.Encoding.UTF8.GetBytes(testString);
            Assert.AreEqual(expectedBytes.Length + 1, result.Length);

            // Check string bytes
            for (var i = 0; i < expectedBytes.Length; i++)
            {
                Assert.AreEqual(expectedBytes[i], result[i]);
            }

            // Check null terminator
            Assert.AreEqual(0, result[result.Length - 1]);
        }

        [TestMethod]
        public void TestWriteCStringEmpty()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteCString(string.Empty);

            var result = stream.ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(0, result[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWriteCStringNull()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteCString(null!);
        }

        [TestMethod]
        public void TestWriteCStringFixedLength()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteCString("Hello", 10);

            var result = stream.ToArray();
            Assert.AreEqual(10, result.Length);

            // Check "Hello"
            Assert.AreEqual((byte)'H', result[0]);
            Assert.AreEqual((byte)'e', result[1]);
            Assert.AreEqual((byte)'l', result[2]);
            Assert.AreEqual((byte)'l', result[3]);
            Assert.AreEqual((byte)'o', result[4]);

            // Rest should be zeros
            for (var i = 5; i < 10; i++)
            {
                Assert.AreEqual(0, result[i]);
            }
        }

        [TestMethod]
        public void TestWriteCStringFixedLengthTruncated()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteCString("HelloWorld", 6);

            var result = stream.ToArray();
            Assert.AreEqual(6, result.Length);

            // Should be "Hello\0" (truncated)
            Assert.AreEqual((byte)'H', result[0]);
            Assert.AreEqual((byte)'e', result[1]);
            Assert.AreEqual((byte)'l', result[2]);
            Assert.AreEqual((byte)'l', result[3]);
            Assert.AreEqual((byte)'o', result[4]);
            Assert.AreEqual(0, result[5]); // Null terminator
        }

        [TestMethod]
        public void TestWriteMD5Hash()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var hash = new byte[CascConstants.MD5HashSize];
            for (var i = 0; i < hash.Length; i++)
            {
                hash[i] = (byte)i;
            }

            writer.WriteMD5Hash(hash);

            var result = stream.ToArray();
            CollectionAssert.AreEqual(hash, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWriteMD5HashInvalidSize()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var hash = new byte[10]; // Wrong size
            writer.WriteMD5Hash(hash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWriteMD5HashNull()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteMD5Hash(null!);
        }

        [TestMethod]
        public void TestWriteSHA1Hash()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var hash = new byte[CascConstants.SHA1HashSize];
            for (var i = 0; i < hash.Length; i++)
            {
                hash[i] = (byte)i;
            }

            writer.WriteSHA1Hash(hash);

            var result = stream.ToArray();
            CollectionAssert.AreEqual(hash, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWriteSHA1HashInvalidSize()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var hash = new byte[10]; // Wrong size
            writer.WriteSHA1Hash(hash);
        }

        [TestMethod]
        public void TestWritePadding()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WritePadding(5);

            var result = stream.ToArray();
            Assert.AreEqual(5, result.Length);
            foreach (var b in result)
            {
                Assert.AreEqual(0, b);
            }
        }

        [TestMethod]
        public void TestWriteCKey()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var keyBytes = new byte[CascConstants.CKeySize];
            for (var i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] = (byte)i;
            }

            var cKey = new CascKey(keyBytes);
            writer.WriteCKey(cKey);

            var result = stream.ToArray();
            CollectionAssert.AreEqual(keyBytes, result);
        }

        [TestMethod]
        public void TestWriteEKey()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var keyBytes = new byte[CascConstants.EKeySize];
            for (var i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] = (byte)i;
            }

            var eKey = new EKey(keyBytes);
            writer.WriteEKey(eKey);

            var result = stream.ToArray();
            CollectionAssert.AreEqual(keyBytes, result);
        }

        [TestMethod]
        public void TestWriteEKeyWithSpecificLength()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var keyBytes = new byte[5] { 1, 2, 3, 4, 5 };
            var eKey = new EKey(keyBytes);

            // Write with padding to 10 bytes
            writer.WriteEKey(eKey, 10);

            var result = stream.ToArray();
            Assert.AreEqual(10, result.Length);

            // First 5 bytes should match
            for (var i = 0; i < 5; i++)
            {
                Assert.AreEqual(keyBytes[i], result[i]);
            }

            // Last 5 bytes should be zero (padding)
            for (var i = 5; i < 10; i++)
            {
                Assert.AreEqual(0, result[i]);
            }
        }

        [TestMethod]
        public void TestRoundTripBigEndianValues()
        {
            // Test that reading and writing produce the same values
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteUInt16BE(0x1234);
            writer.WriteUInt32BE(0x56789ABC);
            writer.WriteUInt64BE(0xDEF0123456789ABC);

            stream.Position = 0;
            using var reader = new BinaryReader(stream);

            Assert.AreEqual((ushort)0x1234, reader.ReadUInt16BE());
            Assert.AreEqual(0x56789ABCu, reader.ReadUInt32BE());
            Assert.AreEqual(0xDEF0123456789ABCul, reader.ReadUInt64BE());
        }
    }
}