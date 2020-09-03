// ------------------------------------------------------------------------------
// <copyright file="MapObjectTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;

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
    }
}