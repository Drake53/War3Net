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
            var actualHex = Convert.ToHexString(hash);
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
    }
}