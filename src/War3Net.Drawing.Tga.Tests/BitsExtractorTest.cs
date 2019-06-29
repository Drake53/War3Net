using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TgaLib.Test
{
    [TestClass]
    public class BitsExtractorTest
    {
        [TestMethod]
        public void TestExtractByte()
        {
            Assert.AreEqual(19, BitsExtractor.Extract(0xCC, 2, 5));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestExtractByteException()
        {
            _ = BitsExtractor.Extract(0xCC, 2, 7);
        }
    }
}