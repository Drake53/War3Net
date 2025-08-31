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
            for (var i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] = (byte)i;
            }

            var key = EKey.FromBytes(keyBytes);
            Assert.IsNotNull(key);
            Assert.IsFalse(key.IsEmpty);
            Assert.AreEqual(CascConstants.EKeySize, key.Length);
        }

        [TestMethod]
        public void TestEKeyEmpty()
        {
            var emptyKey = EKey.Empty;
            Assert.IsTrue(emptyKey.IsEmpty);
            Assert.AreEqual(0, emptyKey.Value.Length);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(8)]
        [DataRow(10)]
        [DataRow(11)]
        [DataRow(15)]
        [DataRow(17)]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEKeyInvalidLength(int length)
        {
            var bytes = new byte[length];
            for (var i = 0; i < length; i++)
            {
                bytes[i] = (byte)i;
            }

            _ = EKey.FromBytes(bytes);
        }

        [TestMethod]
        public void TestEKeyPartialEquality()
        {
            var partialKeyBytes = new byte[CascConstants.PartialEKeySize];
            for (var i = 0; i < partialKeyBytes.Length; i++)
            {
                partialKeyBytes[i] = (byte)i;
            }

            var fullKeyBytes = new byte[CascConstants.EKeySize];
            for (var i = 0; i < fullKeyBytes.Length; i++)
            {
                fullKeyBytes[i] = (byte)i;
            }

            var partialKey = EKey.FromBytes(partialKeyBytes);
            var fullKey = EKey.FromBytes(fullKeyBytes);

            Assert.IsTrue(partialKey.Equals(fullKey));
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
            var bytes1 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var bytes2 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var key1 = EKey.FromBytes(bytes1);
            var key2 = EKey.FromBytes(bytes2);

            Assert.AreEqual(key1, key2);
            Assert.IsTrue(key1 == key2);
            Assert.IsFalse(key1 != key2);
            Assert.AreEqual(key1.GetHashCode(), key2.GetHashCode());
        }

        [TestMethod]
        public void TestEKeyInequalityDifferentValues()
        {
            var bytes1 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var bytes2 = new byte[] { 1, 2, 3, 4, 0, 6, 7, 8, 9 };

            var key1 = EKey.FromBytes(bytes1);
            var key2 = EKey.FromBytes(bytes2);

            Assert.AreNotEqual(key1, key2);
            Assert.IsFalse(key1 == key2);
            Assert.IsTrue(key1 != key2);
        }

        [TestMethod]
        public void TestEKeyInequalityDifferentLengths()
        {
            var bytes1 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var bytes2 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var key1 = EKey.FromBytes(bytes1);
            var key2 = EKey.FromBytes(bytes2);

            Assert.AreEqual(key1, key2);
            Assert.IsTrue(key1 == key2);
            Assert.IsFalse(key1 != key2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEKeyTooLarge()
        {
            var invalidBytes = new byte[CascConstants.EKeySize + 1]; // Too large
            _ = EKey.FromBytes(invalidBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEKeyNullBytes()
        {
            _ = EKey.FromBytes((byte[]?)null);
        }

        [TestMethod]
        public void TestEKeyToArray()
        {
            var originalBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var key = EKey.FromBytes(originalBytes);
            var arrayBytes = key.ToArray();

            CollectionAssert.AreEqual(originalBytes, arrayBytes);
        }
    }
}