// ------------------------------------------------------------------------------
// <copyright file="Salsa20Tests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Crypto;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class Salsa20Tests
    {
        [TestMethod]
        public void TestSalsa20BasicEncryption()
        {
            var key = new byte[16];
            var iv = new byte[8];
            for (int i = 0; i < key.Length; i++)
            {
                key[i] = (byte)i;
            }
            for (int i = 0; i < iv.Length; i++)
            {
                iv[i] = (byte)(i + 16);
            }

            var plaintext = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            
            var salsa20 = new Salsa20(key, iv);
            var encrypted = salsa20.Process(plaintext);
            
            // Salsa20 is symmetric, so encrypting again should give original
            var salsa20_2 = new Salsa20(key, iv);
            var decrypted = salsa20_2.Process(encrypted);
            
            CollectionAssert.AreEqual(plaintext, decrypted);
        }

        [TestMethod]
        public void TestSalsa20InPlace()
        {
            var key = new byte[16];
            var iv = new byte[8];
            Array.Fill(key, (byte)0xAB);
            Array.Fill(iv, (byte)0xCD);

            var data = new byte[] { 10, 20, 30, 40, 50 };
            var original = data.ToArray();
            
            var salsa20 = new Salsa20(key, iv);
            salsa20.ProcessInPlace(data);
            
            // Data should be modified
            Assert.IsFalse(data.SequenceEqual(original));
            
            // Process again to decrypt
            var salsa20_2 = new Salsa20(key, iv);
            salsa20_2.ProcessInPlace(data);
            
            // Should be back to original
            CollectionAssert.AreEqual(original, data);
        }

        [TestMethod]
        public void TestSalsa20With32ByteKey()
        {
            var key = new byte[32];
            var iv = new byte[8];
            for (int i = 0; i < key.Length; i++)
            {
                key[i] = (byte)(i * 2);
            }
            for (int i = 0; i < iv.Length; i++)
            {
                iv[i] = (byte)(i * 3);
            }

            var plaintext = Enumerable.Range(0, 100).Select(i => (byte)i).ToArray();
            
            var salsa20 = new Salsa20(key, iv);
            var encrypted = salsa20.Process(plaintext);
            
            var salsa20_2 = new Salsa20(key, iv);
            var decrypted = salsa20_2.Process(encrypted);
            
            CollectionAssert.AreEqual(plaintext, decrypted);
        }

        [TestMethod]
        public void TestSalsa20LargeData()
        {
            var key = new byte[16];
            var iv = new byte[8];
            Array.Fill(key, (byte)0x42);
            Array.Fill(iv, (byte)0x24);

            // Test with data larger than one keystream block (64 bytes)
            var plaintext = new byte[256];
            for (int i = 0; i < plaintext.Length; i++)
            {
                plaintext[i] = (byte)(i % 256);
            }
            
            var salsa20 = new Salsa20(key, iv);
            var encrypted = salsa20.Process(plaintext);
            
            Assert.IsFalse(plaintext.SequenceEqual(encrypted));
            
            var salsa20_2 = new Salsa20(key, iv);
            var decrypted = salsa20_2.Process(encrypted);
            
            CollectionAssert.AreEqual(plaintext, decrypted);
        }

        [TestMethod]
        public void TestSalsa20EmptyData()
        {
            var key = new byte[16];
            var iv = new byte[8];

            var salsa20 = new Salsa20(key, iv);
            var result = salsa20.Process(Array.Empty<byte>());
            
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSalsa20InvalidKeySize()
        {
            var invalidKey = new byte[15]; // Should be 16 or 32
            var iv = new byte[8];
            
            _ = new Salsa20(invalidKey, iv);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSalsa20InvalidIVSize()
        {
            var key = new byte[16];
            var invalidIv = new byte[7]; // Should be 8
            
            _ = new Salsa20(key, invalidIv);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSalsa20NullKey()
        {
            var iv = new byte[8];
            _ = new Salsa20(null!, iv);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSalsa20NullIV()
        {
            var key = new byte[16];
            _ = new Salsa20(key, null!);
        }

        [TestMethod]
        public void TestSalsa20PartialProcess()
        {
            var key = new byte[16];
            var iv = new byte[8];
            Array.Fill(key, (byte)0x11);
            Array.Fill(iv, (byte)0x22);

            var plaintext = new byte[100];
            Array.Fill(plaintext, (byte)0x33);

            var salsa20 = new Salsa20(key, iv);
            
            // Process in chunks
            var output = new byte[100];
            salsa20.Process(plaintext, 0, 50, output, 0);
            salsa20.Process(plaintext, 50, 50, output, 50);

            // Compare with processing all at once
            var salsa20_2 = new Salsa20(key, iv);
            var expected = salsa20_2.Process(plaintext);

            CollectionAssert.AreEqual(expected, output);
        }
    }
}