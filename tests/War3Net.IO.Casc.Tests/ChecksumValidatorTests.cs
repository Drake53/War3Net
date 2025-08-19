// ------------------------------------------------------------------------------
// <copyright file="ChecksumValidatorTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Security.Cryptography;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Utilities;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class ChecksumValidatorTests
    {
        [TestMethod]
        public void TestValidateMD5_ValidChecksum()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("Hello, World!");
            var expectedHash = ComputeMD5Hash(data);

            var result = ChecksumValidator.ValidateMD5(data, expectedHash);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestValidateMD5_InvalidChecksum()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("Hello, World!");
            var wrongHash = new byte[CascConstants.MD5HashSize];
            Array.Fill(wrongHash, (byte)0xFF);

            var result = ChecksumValidator.ValidateMD5(data, wrongHash);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestValidateMD5_EmptyData()
        {
            var data = Array.Empty<byte>();
            var expectedHash = ComputeMD5Hash(data);

            var result = ChecksumValidator.ValidateMD5(data, expectedHash);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateMD5_NullData()
        {
            var hash = new byte[CascConstants.MD5HashSize];
            ChecksumValidator.ValidateMD5((byte[])null!, hash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestValidateMD5_NullExpectedHash()
        {
            var data = new byte[] { 1, 2, 3 };
            ChecksumValidator.ValidateMD5(data, null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestValidateMD5_WrongHashSize()
        {
            var data = new byte[] { 1, 2, 3 };
            var wrongSizeHash = new byte[10]; // Wrong size
            ChecksumValidator.ValidateMD5(data, wrongSizeHash);
        }

        [TestMethod]
        public void TestValidateMD5Stream_ValidChecksum()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("Stream test data");
            var expectedHash = ComputeMD5Hash(data);

            using var stream = new MemoryStream(data);
            var result = ChecksumValidator.ValidateMD5(stream, expectedHash);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestValidateMD5Stream_InvalidChecksum()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("Stream test data");
            var wrongHash = new byte[CascConstants.MD5HashSize];
            Array.Fill(wrongHash, (byte)0x42);

            using var stream = new MemoryStream(data);
            var result = ChecksumValidator.ValidateMD5(stream, wrongHash);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateMD5Stream_NullStream()
        {
            var hash = new byte[CascConstants.MD5HashSize];
            ChecksumValidator.ValidateMD5((Stream)null!, hash);
        }

        [TestMethod]
        public void TestComputeMD5()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("Test data");
            var hash = ChecksumValidator.ComputeMD5(data);

            Assert.IsNotNull(hash);
            Assert.AreEqual(CascConstants.MD5HashSize, hash.Length);

            // Verify it's consistent
            var hash2 = ChecksumValidator.ComputeMD5(data);
            CollectionAssert.AreEqual(hash, hash2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestComputeMD5_NullData()
        {
            ChecksumValidator.ComputeMD5(null!);
        }

        [TestMethod]
        public void TestValidateSHA1_ValidChecksum()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("SHA1 test data");
            var expectedHash = ComputeSHA1Hash(data);

            var result = ChecksumValidator.ValidateSHA1(data, expectedHash);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestValidateSHA1_InvalidChecksum()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("SHA1 test data");
            var wrongHash = new byte[CascConstants.SHA1HashSize];
            Array.Fill(wrongHash, (byte)0xAA);

            var result = ChecksumValidator.ValidateSHA1(data, wrongHash);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateSHA1_NullData()
        {
            var hash = new byte[CascConstants.SHA1HashSize];
            ChecksumValidator.ValidateSHA1((byte[])null!, hash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestValidateSHA1_WrongHashSize()
        {
            var data = new byte[] { 1, 2, 3 };
            var wrongSizeHash = new byte[15]; // Wrong size
            ChecksumValidator.ValidateSHA1(data, wrongSizeHash);
        }

        [TestMethod]
        public void TestValidateSHA256_ValidChecksum()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("SHA256 test data");
            var expectedHash = ComputeSHA256Hash(data);

            var result = ChecksumValidator.ValidateSHA256(data, expectedHash);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestValidateSHA256_InvalidChecksum()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("SHA256 test data");
            var wrongHash = new byte[32];
            Array.Fill(wrongHash, (byte)0xBB);

            var result = ChecksumValidator.ValidateSHA256(data, wrongHash);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateSHA256_NullData()
        {
            var hash = new byte[32];
            ChecksumValidator.ValidateSHA256((byte[])null!, hash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestValidateSHA256_WrongHashSize()
        {
            var data = new byte[] { 1, 2, 3 };
            var wrongSizeHash = new byte[20]; // Wrong size
            ChecksumValidator.ValidateSHA256(data, wrongSizeHash);
        }

        [TestMethod]
        public void TestCompareHashes_Equal()
        {
            var hash1 = new byte[] { 1, 2, 3, 4, 5 };
            var hash2 = new byte[] { 1, 2, 3, 4, 5 };

            var result = ChecksumValidator.CompareHashes(hash1, hash2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCompareHashes_NotEqual()
        {
            var hash1 = new byte[] { 1, 2, 3, 4, 5 };
            var hash2 = new byte[] { 1, 2, 3, 4, 6 };

            var result = ChecksumValidator.CompareHashes(hash1, hash2);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestCompareHashes_DifferentLengths()
        {
            var hash1 = new byte[] { 1, 2, 3, 4, 5 };
            var hash2 = new byte[] { 1, 2, 3, 4 };

            var result = ChecksumValidator.CompareHashes(hash1, hash2);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestCompareHashes_EmptyArrays()
        {
            var hash1 = Array.Empty<byte>();
            var hash2 = Array.Empty<byte>();

            var result = ChecksumValidator.CompareHashes(hash1, hash2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCompareHashes_ConstantTimeComparison()
        {
            // Test that comparison is constant-time by checking all positions
            var hash1 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            // Test difference at each position
            for (var i = 0; i < hash1.Length; i++)
            {
                var hash2 = (byte[])hash1.Clone();
                hash2[i] = (byte)(hash2[i] ^ 1); // Flip one bit

                var result = ChecksumValidator.CompareHashes(hash1, hash2);
                Assert.IsFalse(result, $"Failed at position {i}");
            }
        }

        [TestMethod]
        public void TestLargeDataValidation()
        {
            // Test with larger data
            var data = new byte[1024 * 1024]; // 1MB
            var random = new Random(42); // Fixed seed for reproducibility
            random.NextBytes(data);

            var md5Hash = ComputeMD5Hash(data);
            var sha1Hash = ComputeSHA1Hash(data);
            var sha256Hash = ComputeSHA256Hash(data);

            Assert.IsTrue(ChecksumValidator.ValidateMD5(data, md5Hash));
            Assert.IsTrue(ChecksumValidator.ValidateSHA1(data, sha1Hash));
            Assert.IsTrue(ChecksumValidator.ValidateSHA256(data, sha256Hash));
        }

        [TestMethod]
        public void TestKnownMD5Values()
        {
            // Test with known MD5 values
            var testCases = new[]
            {
                (Data: string.Empty, Hash: "D41D8CD98F00B204E9800998ECF8427E"),
                (Data: "a", Hash: "0CC175B9C0F1B6A831C399E269772661"),
                (Data: "abc", Hash: "900150983CD24FB0D6963F7D28E17F72"),
                (Data: "message digest", Hash: "F96B697D7CB7938D525A2F31AAF161D0"),
            };

            foreach (var testCase in testCases)
            {
                var data = System.Text.Encoding.ASCII.GetBytes(testCase.Data);
                var expectedHash = HexStringToBytes(testCase.Hash);

                var result = ChecksumValidator.ValidateMD5(data, expectedHash);
                Assert.IsTrue(result, $"Failed for input: '{testCase.Data}'");
            }
        }

        // Helper methods
        private static byte[] ComputeMD5Hash(byte[] data)
        {
            using var md5 = MD5.Create();
            return md5.ComputeHash(data);
        }

        private static byte[] ComputeSHA1Hash(byte[] data)
        {
            using var sha1 = SHA1.Create();
            return sha1.ComputeHash(data);
        }

        private static byte[] ComputeSHA256Hash(byte[] data)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(data);
        }

        private static byte[] HexStringToBytes(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return bytes;
        }
    }
}