// ------------------------------------------------------------------------------
// <copyright file="MapCamerasDecompilerTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build;
using War3Net.Build.Info;
using War3Net.TestTools.UnitTesting;

namespace War3Net.CodeAnalysis.Decompilers.Tests.Environment
{
    [TestClass]
    public class MapCamerasDecompilerTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestDecompileMap(Map map)
        {
            Assert.IsTrue(new JassScriptDecompiler(map).TryDecompileMapCameras(map.Cameras.FormatVersion, map.Cameras.UseNewFormat, out var decompiledMapCameras), "Failed to decompile map cameras.");

            Assert.AreEqual(map.Cameras.Cameras.Count, decompiledMapCameras.Cameras.Count);
            for (var i = 0; i < decompiledMapCameras.Cameras.Count; i++)
            {
                var expectedCamera = map.Cameras.Cameras[i];
                var actualCamera = decompiledMapCameras.Cameras[i];

                //const float delta = 0.0500001f;
                const float delta = 0.055f;

                Assert.AreEqual(expectedCamera.Name.Replace('_', ' '), actualCamera.Name, ignoreCase: false, CultureInfo.InvariantCulture);
                Assert.AreEqual(expectedCamera.ZOffset, actualCamera.ZOffset, delta);
                Assert.AreEqual(expectedCamera.Rotation, actualCamera.Rotation, delta);
                Assert.AreEqual(expectedCamera.AngleOfAttack, actualCamera.AngleOfAttack, delta);
                Assert.AreEqual(expectedCamera.TargetDistance, actualCamera.TargetDistance, delta);
                Assert.AreEqual(expectedCamera.Roll, actualCamera.Roll, delta);
                Assert.AreEqual(expectedCamera.FieldOfView, actualCamera.FieldOfView, delta);
                Assert.AreEqual(expectedCamera.FarClippingPlane, actualCamera.FarClippingPlane, delta);
                Assert.AreEqual(expectedCamera.NearClippingPlane, actualCamera.NearClippingPlane, delta);
                if (map.Cameras.UseNewFormat)
                {
                    Assert.AreEqual(expectedCamera.LocalPitch, actualCamera.LocalPitch, delta);
                    Assert.AreEqual(expectedCamera.LocalYaw, actualCamera.LocalYaw, delta);
                    Assert.AreEqual(expectedCamera.LocalRoll, actualCamera.LocalRoll, delta);
                }

                Assert.AreEqual(expectedCamera.TargetPosition.X, actualCamera.TargetPosition.X, delta);
                Assert.AreEqual(expectedCamera.TargetPosition.Y, actualCamera.TargetPosition.Y, delta);
            }
        }

        private static IEnumerable<object[]> GetTestData()
        {
            foreach (var mapPath in TestDataProvider.GetDynamicData("*", SearchOption.AllDirectories, "Maps"))
            {
                if (Map.TryOpen((string)mapPath[0], out var map, MapFiles.Info | MapFiles.Script | MapFiles.Cameras) &&
                    map.Info is not null &&
                    map.Cameras is not null &&
                    map.Cameras.Cameras.Count > 0 &&
                    map.Info.ScriptLanguage == ScriptLanguage.Jass &&
                    !string.IsNullOrEmpty(map.Script))
                {
                    yield return new[] { map };
                }
            }
        }
    }
}