// ------------------------------------------------------------------------------
// <copyright file="CascKeyTests.cs" company="Drake53">
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
    public class CascKeyTests
    {
        [TestMethod]
        public void TestCascKeyCreation()
        {
            var keyBytes = new byte[CascConstants.CKeySize];
            for (int i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] = (byte)i;
            }

            var key = new CascKey(keyBytes);
            Assert.IsNotNull(key);
            Assert.IsFalse(key.IsEmpty);
        }

        [TestMethod]
        public void TestCascKeyEmpty()
        {
            var emptyKey = CascKey.Empty;
            Assert.IsTrue(emptyKey.IsEmpty);
            Assert.AreEqual(CascConstants.CKeySize, emptyKey.Value.Length);
        }

        [TestMethod]
        public void TestCascKeyParse()
        {
            const string hexString = "0123456789ABCDEF0123456789ABCDEF";
            var key = CascKey.Parse(hexString);
            
            Assert.IsNotNull(key);
            Assert.IsFalse(key.IsEmpty);
            
            var toString = key.ToString();
            Assert.AreEqual(hexString, toString);
        }

        [TestMethod]
        public void TestCascKeyTryParse()
        {
            const string validHex = "0123456789ABCDEF0123456789ABCDEF";
            Assert.IsTrue(CascKey.TryParse(validHex, out var key));
            Assert.IsNotNull(key);
            Assert.IsFalse(key.IsEmpty);

            const string invalidHex = "INVALID";
            Assert.IsFalse(CascKey.TryParse(invalidHex, out var invalidKey));
            Assert.IsTrue(invalidKey.IsEmpty);
        }

        [TestMethod]
        public void TestCascKeyEquality()
        {
            var bytes1 = new byte[CascConstants.CKeySize];
            var bytes2 = new byte[CascConstants.CKeySize];
            for (int i = 0; i < bytes1.Length; i++)
            {
                bytes1[i] = bytes2[i] = (byte)i;
            }

            var key1 = new CascKey(bytes1);
            var key2 = new CascKey(bytes2);

            Assert.AreEqual(key1, key2);
            Assert.IsTrue(key1 == key2);
            Assert.IsFalse(key1 != key2);
            Assert.AreEqual(key1.GetHashCode(), key2.GetHashCode());
        }

        [TestMethod]
        public void TestCascKeyInequality()
        {
            var bytes1 = new byte[CascConstants.CKeySize];
            var bytes2 = new byte[CascConstants.CKeySize];
            bytes1[0] = 1;
            bytes2[0] = 2;

            var key1 = new CascKey(bytes1);
            var key2 = new CascKey(bytes2);

            Assert.AreNotEqual(key1, key2);
            Assert.IsFalse(key1 == key2);
            Assert.IsTrue(key1 != key2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCascKeyInvalidSize()
        {
            var invalidBytes = new byte[10]; // Wrong size
            _ = new CascKey(invalidBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCascKeyNullBytes()
        {
            _ = new CascKey((byte[])null!);
        }

        [TestMethod]
        public void TestCascKeyToArray()
        {
            var originalBytes = new byte[CascConstants.CKeySize];
            for (int i = 0; i < originalBytes.Length; i++)
            {
                originalBytes[i] = (byte)i;
            }

            var key = new CascKey(originalBytes);
            var arrayBytes = key.ToArray();

            CollectionAssert.AreEqual(originalBytes, arrayBytes);
        }
    }
}