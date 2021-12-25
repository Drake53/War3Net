// ------------------------------------------------------------------------------
// <copyright file="InfoTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.MapFactory
{
    [TestClass]
    public class InfoTests
    {
        [TestMethod]
        public void TestCreateMapInfo()
        {
            var expectedMap = Map.Open(TestDataProvider.GetPath(@"Maps\NewLuaMap.w3m"), MapFiles.Info | MapFiles.TriggerStrings);

            expectedMap.LocalizeInfo();

            var expected = expectedMap.Info;
            var actual = Build.MapFactory.Info();

            using var expectedStream = new MemoryStream();
            using var expectedWriter = new BinaryWriter(expectedStream);
            expectedWriter.Write(expected);

            using var actualStream = new MemoryStream();
            using var actualWriter = new BinaryWriter(actualStream);
            actualWriter.Write(actual);

            StreamAssert.AreEqual(expectedStream, actualStream, true);
        }
    }
}