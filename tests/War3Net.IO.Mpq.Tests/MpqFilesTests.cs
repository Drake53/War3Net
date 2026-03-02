namespace War3Net.IO.Mpq.Tests
{
    [TestClass]
    public class MpqFilesTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestGetMpqFiles(string mpqFilePath)
        {
            Assert.IsTrue(TestDataProvider.IsArchiveFile(mpqFilePath, out _));

            using var archive = MpqArchive.Open(mpqFilePath, true);
            var mpqFiles = archive.GetMpqFiles();

            Assert.AreEqual((int)archive.BlockTable.Size, mpqFiles.Count());
            foreach (var mpqFile in mpqFiles)
            {
                Assert.IsNotNull(mpqFile);
            }
        }

        private static IEnumerable<object[]> GetTestData()
        {
            return TestDataProvider.GetDynamicData("*", SearchOption.TopDirectoryOnly, "Maps");
        }
    }
}