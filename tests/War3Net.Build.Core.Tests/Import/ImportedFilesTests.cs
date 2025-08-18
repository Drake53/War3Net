// ------------------------------------------------------------------------------
// <copyright file="ImportedFilesTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Import;

namespace War3Net.Build.Core.Tests.Import
{
    [TestClass]
    public class ImportedFilesTests
    {
        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetImportedFilesFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<ImportedFiles>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetImportedFilesFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<ImportedFiles>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetImportedFilesFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<ImportedFiles>.RunJsonRWTest(filePath, true);
        }
    }
}