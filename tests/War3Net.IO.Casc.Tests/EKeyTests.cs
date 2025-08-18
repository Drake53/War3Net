// ------------------------------------------------------------------------------
// <copyright file="EKeyTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class EKeyTests
    {
        [TestMethod]
        public void TestEKeyCreation()
        {
            var keyBytes = new byte[CascConstants.EKeySize];
            for (int i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] = (byte)i;
            }

            var key = new EKey(keyBytes);
            Assert.IsNotNull(key);
            Assert.IsFalse(key.IsEmpty);
            Assert.AreEqual(CascConstants.EKeySize, key.Length);
        }

        [TestMethod]
        public void TestEKeyEmpty()
        {
            var emptyKey = EKey.Empty;
            Assert.IsTrue(emptyKey.IsEmpty);
            Assert.AreEqual(CascConstants.EKeySize, emptyKey.Value.Length);
        }

        [TestMethod]
        public void TestEKeyVariableLength()
        {
            // Test with different lengths
            for (int length = 1; length <= CascConstants.CKeySize; length++)
            {
                var bytes = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    bytes[i] = (byte)i;
                }

                var key = new EKey(bytes);
                Assert.AreEqual(length, key.Length);
                Assert.AreEqual(length, key.Value.Length);
            }
        }

        [TestMethod]
        public void TestEKeyCreateTruncated()
        {
            var fullKey = new byte[CascConstants.CKeySize];
            for (int i = 0; i < fullKey.Length; i++)
            {
                fullKey[i] = (byte)i;
            }

            var truncatedKey = EKey.CreateTruncated(fullKey);
            Assert.AreEqual(CascConstants.EKeySize, truncatedKey.Length);
            
            // Verify first 9 bytes match
            for (int i = 0; i < CascConstants.EKeySize; i++)
            {
                Assert.AreEqual(fullKey[i], truncatedKey.Value[i]);
            }
        }

        [TestMethod]
        public void TestEKeyParse()
        {
            const string hexString = "0123456789ABCDEF01";
            var key = EKey.Parse(hexString);
            
            Assert.IsNotNull(key);
            Assert.IsFalse(key.IsEmpty);
            
            var toString = key.ToString();
            Assert.AreEqual(hexString, toString);
        }

        [TestMethod]
        public void TestEKeyTryParse()
        {
            const string validHex = "0123456789ABCDEF01";
            Assert.IsTrue(EKey.TryParse(validHex, out var key));
            Assert.IsNotNull(key);
            Assert.IsFalse(key.IsEmpty);

            const string invalidHex = "INVALID!";
            Assert.IsFalse(EKey.TryParse(invalidHex, out var invalidKey));
            Assert.IsTrue(invalidKey.IsEmpty);
        }

        [TestMethod]
        public void TestEKeyEquality()
        {
            var bytes1 = new byte[] { 1, 2, 3, 4, 5 };
            var bytes2 = new byte[] { 1, 2, 3, 4, 5 };

            var key1 = new EKey(bytes1);
            var key2 = new EKey(bytes2);

            Assert.AreEqual(key1, key2);
            Assert.IsTrue(key1 == key2);
            Assert.IsFalse(key1 != key2);
            Assert.AreEqual(key1.GetHashCode(), key2.GetHashCode());
        }

        [TestMethod]
        public void TestEKeyInequalityDifferentValues()
        {
            var bytes1 = new byte[] { 1, 2, 3, 4, 5 };
            var bytes2 = new byte[] { 1, 2, 3, 4, 6 };

            var key1 = new EKey(bytes1);
            var key2 = new EKey(bytes2);

            Assert.AreNotEqual(key1, key2);
            Assert.IsFalse(key1 == key2);
            Assert.IsTrue(key1 != key2);
        }

        [TestMethod]
        public void TestEKeyInequalityDifferentLengths()
        {
            var bytes1 = new byte[] { 1, 2, 3, 4, 5 };
            var bytes2 = new byte[] { 1, 2, 3, 4, 5, 6 };

            var key1 = new EKey(bytes1);
            var key2 = new EKey(bytes2);

            Assert.AreNotEqual(key1, key2);
            Assert.IsFalse(key1 == key2);
            Assert.IsTrue(key1 != key2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEKeyTooLarge()
        {
            var invalidBytes = new byte[CascConstants.CKeySize + 1]; // Too large
            _ = new EKey(invalidBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestEKeyNullBytes()
        {
            _ = new EKey((byte[])null!);
        }

        [TestMethod]
        public void TestEKeyToArray()
        {
            var originalBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var key = new EKey(originalBytes);
            var arrayBytes = key.ToArray();

            CollectionAssert.AreEqual(originalBytes, arrayBytes);
        }
    }
}