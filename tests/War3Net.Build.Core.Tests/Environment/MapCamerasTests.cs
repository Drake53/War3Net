// ------------------------------------------------------------------------------
// <copyright file="MapCamerasTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapCamerasTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCameraFiles), DynamicDataSourceType.Method)]
        public void TestParseMapCameras(string camerasFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(camerasFilePath, typeof(MapCameras));
        }

        private static IEnumerable<object[]> GetCameraFiles()
        {
            return TestDataProvider.GetDynamicData(
                MapCameras.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Camera"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapCameras.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}