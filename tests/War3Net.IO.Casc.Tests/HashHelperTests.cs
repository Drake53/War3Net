// ------------------------------------------------------------------------------
// <copyright file="HashHelperTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Utilities;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class HashHelperTests
    {
        [TestMethod]
        public void TestMD5Hash()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("Hello, World!");
            var hash = HashHelper.ComputeMD5(data);

            Assert.IsNotNull(hash);
            Assert.AreEqual(16, hash.Length);

            // Known MD5 hash of "Hello, World!"
            var expectedHex = "65A8E27D8879283831B664BD8B7F0AD4";
            var actualHex = HashHelper.ToHexString(hash);
            Assert.AreEqual(expectedHex, actualHex);
        }

        [TestMethod]
        public void TestMD5HashString()
        {
            const string text = "Test String";
            var hash = HashHelper.ComputeMD5(text);

            Assert.IsNotNull(hash);
            Assert.AreEqual(16, hash.Length);

            // Should be same as hashing the bytes
            var bytesHash = HashHelper.ComputeMD5(System.Text.Encoding.UTF8.GetBytes(text));
            CollectionAssert.AreEqual(bytesHash, hash);
        }

        [TestMethod]
        public void TestSHA1Hash()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("Test Data");
            var hash = HashHelper.ComputeSHA1(data);

            Assert.IsNotNull(hash);
            Assert.AreEqual(20, hash.Length);
        }

        [TestMethod]
        public void TestSHA256Hash()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("Test Data");
            var hash = HashHelper.ComputeSHA256(data);

            Assert.IsNotNull(hash);
            Assert.AreEqual(32, hash.Length);
        }

        [TestMethod]
        public void TestJenkinsHash()
        {
            const string text = "test.txt";
            var hash = HashHelper.ComputeJenkinsHash(text);

            Assert.IsTrue(hash > 0);

            // Same string should give same hash
            var hash2 = HashHelper.ComputeJenkinsHash(text);
            Assert.AreEqual(hash, hash2);

            // Different string should give different hash
            var hash3 = HashHelper.ComputeJenkinsHash("other.txt");
            Assert.AreNotEqual(hash, hash3);
        }

        [TestMethod]
        public void TestJenkinsHashBytes()
        {
            var data = new byte[] { 1, 2, 3, 4, 5 };
            var hash = HashHelper.ComputeJenkinsHash(data);

            Assert.IsTrue(hash > 0);

            // Same data should give same hash
            var hash2 = HashHelper.ComputeJenkinsHash(data);
            Assert.AreEqual(hash, hash2);
        }

        [TestMethod]
        public void TestJenkinsHashLookup3()
        {
            var data = new byte[] { 10, 20, 30, 40, 50 };
            uint initVal = 0x12345678;

            var hash = HashHelper.JenkinsHashLookup3(data, initVal);
            Assert.IsTrue(hash > 0);

            // Different init value should give different hash
            var hash2 = HashHelper.JenkinsHashLookup3(data, 0x87654321);
            Assert.AreNotEqual(hash, hash2);
        }

        [TestMethod]
        public void TestToHexString()
        {
            var bytes = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            var hex = HashHelper.ToHexString(bytes);

            Assert.AreEqual("0123456789ABCDEF", hex);
        }

        [TestMethod]
        public void TestFromHexString()
        {
            const string hex = "0123456789ABCDEF";
            var bytes = HashHelper.FromHexString(hex);

            Assert.IsNotNull(bytes);
            Assert.AreEqual(8, bytes.Length);
            Assert.AreEqual(0x01, bytes[0]);
            Assert.AreEqual(0x23, bytes[1]);
            Assert.AreEqual(0x45, bytes[2]);
            Assert.AreEqual(0x67, bytes[3]);
            Assert.AreEqual(0x89, bytes[4]);
            Assert.AreEqual(0xAB, bytes[5]);
            Assert.AreEqual(0xCD, bytes[6]);
            Assert.AreEqual(0xEF, bytes[7]);
        }

        [TestMethod]
        public void TestFromHexStringWithSeparators()
        {
            const string hex = "01-23-45-67-89-AB-CD-EF";
            var bytes = HashHelper.FromHexString(hex);

            Assert.IsNotNull(bytes);
            Assert.AreEqual(8, bytes.Length);

            const string hex2 = "01 23 45 67 89 AB CD EF";
            var bytes2 = HashHelper.FromHexString(hex2);

            CollectionAssert.AreEqual(bytes, bytes2);
        }

        [TestMethod]
        public void TestFromHexStringLowerCase()
        {
            const string hex = "0123456789abcdef";
            var bytes = HashHelper.FromHexString(hex);

            Assert.IsNotNull(bytes);
            Assert.AreEqual(8, bytes.Length);
            Assert.AreEqual(0xAB, bytes[5]);
            Assert.AreEqual(0xCD, bytes[6]);
            Assert.AreEqual(0xEF, bytes[7]);
        }

        [TestMethod]
        public void TestFromHexStringEmpty()
        {
            var bytes = HashHelper.FromHexString(string.Empty);
            Assert.IsNotNull(bytes);
            Assert.AreEqual(0, bytes.Length);

            var bytes2 = HashHelper.FromHexString(null!);
            Assert.IsNotNull(bytes2);
            Assert.AreEqual(0, bytes2.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFromHexStringInvalidLength()
        {
            // Odd number of characters
            HashHelper.FromHexString("123");
        }

        [TestMethod]
        public void TestHashRoundTrip()
        {
            var originalData = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
            var hex = HashHelper.ToHexString(originalData);
            var recovered = HashHelper.FromHexString(hex);

            CollectionAssert.AreEqual(originalData, recovered);
        }
    }
}