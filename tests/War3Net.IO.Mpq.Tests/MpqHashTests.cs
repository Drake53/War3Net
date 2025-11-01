// ------------------------------------------------------------------------------
// <copyright file="MpqHashTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.IO.Mpq.Tests
{
    [TestClass]
    public class MpqHashTests
    {
        [TestMethod]
        [DataRow("EXAMPLE", 6869011987399665552UL)]
        public void TestGetHashedFileName(string fileName, ulong expectedHash)
        {
            Assert.AreEqual(expectedHash, MpqHash.GetHashedFileName(fileName));
        }
    }
}