// ------------------------------------------------------------------------------
// <copyright file="EndianConverterTests.cs" company="Drake53">
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
    public class EndianConverterTests
    {
        [TestMethod]
        public void TestSwap16()
        {
            ushort value = 0x1234;
            ushort swapped = EndianConverter.Swap(value);
            Assert.AreEqual((ushort)0x3412, swapped);
            
            // Swapping twice should give original
            ushort doubleSwapped = EndianConverter.Swap(swapped);
            Assert.AreEqual(value, doubleSwapped);
        }

        [TestMethod]
        public void TestSwap32()
        {
            uint value = 0x12345678;
            uint swapped = EndianConverter.Swap(value);
            Assert.AreEqual(0x78563412u, swapped);
            
            // Swapping twice should give original
            uint doubleSwapped = EndianConverter.Swap(swapped);
            Assert.AreEqual(value, doubleSwapped);
        }

        [TestMethod]
        public void TestSwap64()
        {
            ulong value = 0x123456789ABCDEF0;
            ulong swapped = EndianConverter.Swap(value);
            Assert.AreEqual(0xF0DEBC9A78563412ul, swapped);
            
            // Swapping twice should give original
            ulong doubleSwapped = EndianConverter.Swap(swapped);
            Assert.AreEqual(value, doubleSwapped);
        }

        [TestMethod]
        public void TestFromBigEndian16()
        {
            var bytes = new byte[] { 0x12, 0x34 };
            ushort value = EndianConverter.FromBigEndian16(bytes);
            Assert.AreEqual((ushort)0x1234, value);
        }

        [TestMethod]
        public void TestFromBigEndian32()
        {
            var bytes = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            uint value = EndianConverter.FromBigEndian32(bytes);
            Assert.AreEqual(0x12345678u, value);
        }

        [TestMethod]
        public void TestFromBigEndian64()
        {
            var bytes = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
            ulong value = EndianConverter.FromBigEndian64(bytes);
            Assert.AreEqual(0x123456789ABCDEF0ul, value);
        }

        [TestMethod]
        public void TestToBigEndian16()
        {
            ushort value = 0x1234;
            var bytes = EndianConverter.ToBigEndian(value);
            
            Assert.AreEqual(2, bytes.Length);
            Assert.AreEqual(0x12, bytes[0]);
            Assert.AreEqual(0x34, bytes[1]);
        }

        [TestMethod]
        public void TestToBigEndian32()
        {
            uint value = 0x12345678;
            var bytes = EndianConverter.ToBigEndian(value);
            
            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(0x12, bytes[0]);
            Assert.AreEqual(0x34, bytes[1]);
            Assert.AreEqual(0x56, bytes[2]);
            Assert.AreEqual(0x78, bytes[3]);
        }

        [TestMethod]
        public void TestToBigEndian64()
        {
            ulong value = 0x123456789ABCDEF0;
            var bytes = EndianConverter.ToBigEndian(value);
            
            Assert.AreEqual(8, bytes.Length);
            Assert.AreEqual(0x12, bytes[0]);
            Assert.AreEqual(0x34, bytes[1]);
            Assert.AreEqual(0x56, bytes[2]);
            Assert.AreEqual(0x78, bytes[3]);
            Assert.AreEqual(0x9A, bytes[4]);
            Assert.AreEqual(0xBC, bytes[5]);
            Assert.AreEqual(0xDE, bytes[6]);
            Assert.AreEqual(0xF0, bytes[7]);
        }

        [TestMethod]
        public void TestReadBigEndianValue()
        {
            var bytes = new byte[] { 0x00, 0x12, 0x34, 0x56, 0x78, 0x9A };
            
            // Read 1 byte
            ulong value1 = EndianConverter.ReadBigEndianValue(bytes, 1, 1);
            Assert.AreEqual(0x12ul, value1);
            
            // Read 2 bytes
            ulong value2 = EndianConverter.ReadBigEndianValue(bytes, 1, 2);
            Assert.AreEqual(0x1234ul, value2);
            
            // Read 3 bytes
            ulong value3 = EndianConverter.ReadBigEndianValue(bytes, 1, 3);
            Assert.AreEqual(0x123456ul, value3);
            
            // Read 4 bytes
            ulong value4 = EndianConverter.ReadBigEndianValue(bytes, 1, 4);
            Assert.AreEqual(0x12345678ul, value4);
            
            // Read 5 bytes
            ulong value5 = EndianConverter.ReadBigEndianValue(bytes, 1, 5);
            Assert.AreEqual(0x123456789Aul, value5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFromBigEndian16TooShort()
        {
            var bytes = new byte[] { 0x12 }; // Need 2 bytes
            EndianConverter.FromBigEndian16(bytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFromBigEndian32TooShort()
        {
            var bytes = new byte[] { 0x12, 0x34, 0x56 }; // Need 4 bytes
            EndianConverter.FromBigEndian32(bytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFromBigEndian64TooShort()
        {
            var bytes = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE }; // Need 8 bytes
            EndianConverter.FromBigEndian64(bytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestReadBigEndianValueTooLarge()
        {
            var bytes = new byte[10];
            EndianConverter.ReadBigEndianValue(bytes, 0, 9); // Max is 8 bytes
        }

        [TestMethod]
        public void TestRoundTrip16()
        {
            ushort original = 0xABCD;
            var bytes = EndianConverter.ToBigEndian(original);
            ushort recovered = EndianConverter.FromBigEndian16(bytes);
            Assert.AreEqual(original, recovered);
        }

        [TestMethod]
        public void TestRoundTrip32()
        {
            uint original = 0xDEADBEEF;
            var bytes = EndianConverter.ToBigEndian(original);
            uint recovered = EndianConverter.FromBigEndian32(bytes);
            Assert.AreEqual(original, recovered);
        }

        [TestMethod]
        public void TestRoundTrip64()
        {
            ulong original = 0xCAFEBABEDEADBEEF;
            var bytes = EndianConverter.ToBigEndian(original);
            ulong recovered = EndianConverter.FromBigEndian64(bytes);
            Assert.AreEqual(original, recovered);
        }
    }
}