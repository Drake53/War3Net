// ------------------------------------------------------------------------------
// <copyright file="BitsExtractorTest.cs" company="shns">
// Copyright (c) 2016 shns. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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