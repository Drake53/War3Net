// ------------------------------------------------------------------------------
// <copyright file="MapObjectTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;
using War3Net.Build.Providers;
using War3Net.Common.Testing;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class MapObjectTest
    {
        [TestMethod]
        public void TestCreateNewObjectData()
        {
            var objectData = new MapObjectData(
                new MapUnitObjectData(Array.Empty<ObjectModification>()),
                new MapItemObjectData(Array.Empty<ObjectModification>()),
                new MapDestructableObjectData(Array.Empty<ObjectModification>()),
                new MapDoodadObjectData(Array.Empty<ObjectModification>()),
                new MapAbilityObjectData(Array.Empty<ObjectModification>()),
                new MapBuffObjectData(Array.Empty<ObjectModification>()),
                new MapUpgradeObjectData(Array.Empty<ObjectModification>()));

            using var memoryStream = new MemoryStream();
            objectData.SerializeTo(memoryStream, true);

            memoryStream.Position = 0;
            MapObjectData.Parse(memoryStream);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapUnitObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapUnitObjectData(string mapUnitObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapUnitObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapUnitObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapItemObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapItemObjectData(string mapItemObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapItemObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapItemObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapDestructableObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapDestructableObjectData(string mapDestructableObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapDestructableObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapDestructableObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapDoodadObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapDoodadObjectData(string mapDoodadObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapDoodadObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapDoodadObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapAbilityObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapAbilityObjectData(string mapAbilityObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapAbilityObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapAbilityObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapBuffObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapBuffObjectData(string mapBuffObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapBuffObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapBuffObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapUpgradeObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapUpgradeObjectData(string mapUpgradeObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapUpgradeObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapUpgradeObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapAllObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapObjectData(string mapObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        private static IEnumerable<object[]> GetMapUnitObjectData()
        {
            return GetMapObjectData("Unit", MapUnitObjectData.FileName);
        }

        private static IEnumerable<object[]> GetMapItemObjectData()
        {
            return GetMapObjectData("Item", MapItemObjectData.FileName);
        }

        private static IEnumerable<object[]> GetMapDestructableObjectData()
        {
            return GetMapObjectData("Destructable", MapDestructableObjectData.FileName);
        }

        private static IEnumerable<object[]> GetMapDoodadObjectData()
        {
            return GetMapObjectData("Doodad", MapDoodadObjectData.FileName);
        }

        private static IEnumerable<object[]> GetMapAbilityObjectData()
        {
            return GetMapObjectData("Ability", MapAbilityObjectData.FileName);
        }

        private static IEnumerable<object[]> GetMapBuffObjectData()
        {
            return GetMapObjectData("Buff", MapBuffObjectData.FileName);
        }

        private static IEnumerable<object[]> GetMapUpgradeObjectData()
        {
            return GetMapObjectData("Upgrade", MapUpgradeObjectData.FileName);
        }

        private static IEnumerable<object[]> GetMapAllObjectData()
        {
            return GetMapObjectData("All", MapObjectData.FileName);
        }

        private static IEnumerable<object[]> GetMapObjectData(string directory, string fileName)
        {
            return TestDataProvider.GetDynamicData(
                fileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", directory))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                fileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}