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