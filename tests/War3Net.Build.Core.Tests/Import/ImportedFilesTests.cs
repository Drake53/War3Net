using Microsoft.VisualStudio.TestTools.UnitTesting;
using War3Net.Build.Import;

namespace War3Net.Build.Core.Tests.Import
{
    [TestClass]
    public class ImportedFilesTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.ImportedFiles)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<ImportedFiles>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.ImportedFiles)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<ImportedFiles>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.ImportedFiles)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<ImportedFiles>.RunJsonRWTest(filePath, true);
        }
    }
}