// ------------------------------------------------------------------------------
// <copyright file="BinaryReaderExtensionsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Utilities;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class BinaryReaderExtensionsTests
    {
        [TestMethod]
        public void TestReadUInt16BE()
        {
            // Test data: 0x1234 in big-endian
            var data = new byte[] { 0x12, 0x34 };
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var result = reader.ReadUInt16BE();
            Assert.AreEqual((ushort)0x1234, result);
        }

        [TestMethod]
        public void TestReadUInt32BE()
        {
            // Test data: 0x12345678 in big-endian
            var data = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var result = reader.ReadUInt32BE();
            Assert.AreEqual(0x12345678u, result);
        }

        [TestMethod]
        public void TestReadUInt64BE()
        {
            // Test data: 0x123456789ABCDEF0 in big-endian
            var data = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var result = reader.ReadUInt64BE();
            Assert.AreEqual(0x123456789ABCDEF0ul, result);
        }

        [TestMethod]
        public void TestReadBigEndianValue()
        {
            // Test reading 3 bytes as big-endian
            var data = new byte[] { 0x12, 0x34, 0x56 };
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var result = reader.ReadBigEndianValue(3);
            Assert.AreEqual(0x123456ul, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestReadBigEndianValueTooManyBytes()
        {
            var data = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            reader.ReadBigEndianValue(9); // Should throw
        }

        [TestMethod]
        public void TestReadCString()
        {
            var testString = "Hello, World!";
            var data = System.Text.Encoding.UTF8.GetBytes(testString + "\0");
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var result = reader.ReadCString();
            Assert.AreEqual(testString, result);
        }

        [TestMethod]
        public void TestReadCStringEmpty()
        {
            var data = new byte[] { 0 }; // Just null terminator
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var result = reader.ReadCString();
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void TestReadCStringWithMaxLength()
        {
            var testString = "Hello, World!";
            var data = System.Text.Encoding.UTF8.GetBytes(testString + "\0Extra data");
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var result = reader.ReadCString(20);
            Assert.AreEqual(testString, result);
        }

        [TestMethod]
        public void TestReadCStringWithoutNullTerminator()
        {
            var testString = "Hello";
            var data = System.Text.Encoding.UTF8.GetBytes(testString);
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var result = reader.ReadCString(5);
            Assert.AreEqual(testString, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestReadCStringEndOfStream()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("No null terminator");
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            reader.ReadCString(); // Should throw
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestReadCStringExceedsMaxLength()
        {
            // Create a very long string without null terminator
            var data = new byte[BinaryReaderExtensions.MaxStringLength + 1];
            Array.Fill(data, (byte)'A');

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            reader.ReadCString(); // Should throw
        }

        [TestMethod]
        public void TestSkipWithSeekableStream()
        {
            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            reader.Skip(3);
            Assert.AreEqual(3, stream.Position);

            var nextByte = reader.ReadByte();
            Assert.AreEqual(4, nextByte);
        }

        [TestMethod]
        public void TestSkipWithNonSeekableStream()
        {
            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using var baseStream = new MemoryStream(data);
            using var nonSeekableStream = new NonSeekableStream(baseStream);
            using var reader = new BinaryReader(nonSeekableStream);

            reader.Skip(3);
            var nextByte = reader.ReadByte();
            Assert.AreEqual(4, nextByte);
        }

        [TestMethod]
        public void TestReadMD5Hash()
        {
            var hash = new byte[CascConstants.MD5HashSize];
            for (var i = 0; i < hash.Length; i++)
            {
                hash[i] = (byte)i;
            }

            using var stream = new MemoryStream(hash);
            using var reader = new BinaryReader(stream);

            var result = reader.ReadMD5Hash();
            CollectionAssert.AreEqual(hash, result);
            Assert.AreEqual(CascConstants.MD5HashSize, result.Length);
        }

        [TestMethod]
        public void TestReadSHA1Hash()
        {
            var hash = new byte[CascConstants.SHA1HashSize];
            for (var i = 0; i < hash.Length; i++)
            {
                hash[i] = (byte)i;
            }

            using var stream = new MemoryStream(hash);
            using var reader = new BinaryReader(stream);

            var result = reader.ReadSHA1Hash();
            CollectionAssert.AreEqual(hash, result);
            Assert.AreEqual(CascConstants.SHA1HashSize, result.Length);
        }

        [TestMethod]
        public void TestMaxStringLengthProperty()
        {
            var originalMax = BinaryReaderExtensions.MaxStringLength;
            try
            {
                // Test valid range
                BinaryReaderExtensions.MaxStringLength = 1000;
                Assert.AreEqual(1000, BinaryReaderExtensions.MaxStringLength);

                // Test minimum
                BinaryReaderExtensions.MaxStringLength = 1;
                Assert.AreEqual(1, BinaryReaderExtensions.MaxStringLength);
            }
            finally
            {
                BinaryReaderExtensions.MaxStringLength = originalMax;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestMaxStringLengthTooSmall()
        {
            BinaryReaderExtensions.MaxStringLength = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestMaxStringLengthTooLarge()
        {
            BinaryReaderExtensions.MaxStringLength = 0x10000001;
        }

        // Helper class for testing non-seekable streams
        private class NonSeekableStream : Stream
        {
            private readonly Stream _innerStream;

            public NonSeekableStream(Stream innerStream)
            {
                _innerStream = innerStream;
            }

            public override bool CanRead => _innerStream.CanRead;

            public override bool CanSeek => false;

            public override bool CanWrite => _innerStream.CanWrite;

            public override long Length => throw new NotSupportedException();

            public override long Position
            {
                get => throw new NotSupportedException();
                set => throw new NotSupportedException();
            }

            public override void Flush() => _innerStream.Flush();

            public override int Read(byte[] buffer, int offset, int count) => _innerStream.Read(buffer, offset, count);

            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

            public override void SetLength(long value) => throw new NotSupportedException();

            public override void Write(byte[] buffer, int offset, int count) => _innerStream.Write(buffer, offset, count);
        }
    }
}