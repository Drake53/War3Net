// ------------------------------------------------------------------------------
// <copyright file="BitsExtractorTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.Drawing.Tga.Tests
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