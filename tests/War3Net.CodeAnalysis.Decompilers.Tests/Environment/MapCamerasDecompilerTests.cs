// ------------------------------------------------------------------------------
// <copyright file="MapCamerasDecompilerTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build;

namespace War3Net.CodeAnalysis.Decompilers.Tests.Environment
{
    [TestClass]
    public class MapCamerasDecompilerTests
    {
        private const MapFiles FilesToOpen = MapFiles.Info | MapFiles.Script | MapFiles.Cameras;

        [TestMethod]
        [DynamicTestData(FilesToOpen)]
        public void TestDecompileMapCameras(string mapFilePath)
        {
            var map = Map.Open(mapFilePath, FilesToOpen);

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
    }
}