// ------------------------------------------------------------------------------
// <copyright file="CameraTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Numerics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Runtime.Enums;
using War3Net.Runtime.Rendering;

namespace War3Net.Runtime.Tests.Rendering
{
    [TestClass]
    public class CameraTests
    {
        [TestMethod]
        public void TestPanCamera()
        {
            const float InitialX = 10f;
            const float InitialY = -25f;

            var camera = new Camera(InitialX, InitialY, 1920, 1080);

            const float NewX = 500f;
            const float NewY = 12.34f;

            camera.Pan(NewX, NewY, 0f);
            camera.Pan(999999f, 999999f, 999999f);

            Assert.AreEqual(InitialX, camera.PositionX);
            Assert.AreEqual(InitialY, camera.PositionY);

            camera.Update(0f);

            Assert.AreEqual(NewX, camera.PositionX);
            Assert.AreEqual(NewY, camera.PositionY);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetCameraEyeTestData), DynamicDataSourceType.Method)]
        public void TestCameraEye(Vector3 expectedEyePosition, float x, float y, float zOffset, float targetDistance, float angleOfAttack, float rotation, float roll)
        {
            var camera = new Camera(999f, 999f, 1920, 1080);

            camera.Pan(x, y, 0f);
            camera.SetField(CameraField.GetCameraField((int)CameraField.Type.ZOffset), zOffset, 0f);
            camera.SetField(CameraField.GetCameraField((int)CameraField.Type.TargetDistance), targetDistance, 0f);
            camera.SetField(CameraField.GetCameraField((int)CameraField.Type.AngleOfAttack), angleOfAttack, 0f);
            camera.SetField(CameraField.GetCameraField((int)CameraField.Type.Rotation), rotation, 0f);
            camera.SetField(CameraField.GetCameraField((int)CameraField.Type.Roll), roll, 0f);

            camera.Update(0f);

            const float Delta = 0.001f;
            Assert.AreEqual(expectedEyePosition.X, camera.EyeX, Delta);
            Assert.AreEqual(expectedEyePosition.Y, camera.EyeY, Delta);
            Assert.AreEqual(expectedEyePosition.Z, camera.EyeZ, Delta);
        }

        private static IEnumerable<object[]> GetCameraEyeTestData()
        {
            yield return new object[] { new Vector3((float)4.033111E-05, -922.6683f, 1367.912f), 0f, 0f, 0f, 1650f, 304f, 90f, 0f };
            yield return new object[] { new Vector3(50.00004f, -922.6683f, 1367.912f), 50f, 0f, 0f, 1650f, 304f, 90f, 0f };
            yield return new object[] { new Vector3((float)4.033111E-05, -872.6683f, 1367.912f), 0f, 50f, 0f, 1650f, 304f, 90f, 0f };
            yield return new object[] { new Vector3((float)4.033111E-05, -922.6683f, 1417.912f), 0f, 0f, 50f, 1650f, 304f, 90f, 0f };
            yield return new object[] { new Vector3(50.00004f, -872.6683f, 1417.912f), 50f, 50f, 50f, 1650f, 304f, 90f, 0f };
            yield return new object[] { new Vector3((float)8.600676E-13, (float)-1.967605E-5, 1650.0f), 0f, 0f, 0f, 1650f, 270f, 90f, 0f };
            yield return new object[] { new Vector3((float)7.212379E-5, -1650.0f, 0.0f), 0f, 0f, 0f, 1650f, 360f, 90f, 0f };
            yield return new object[] { new Vector3(-922.6683f, 0.0f, 1367.912f), 0f, 0f, 0f, 1650f, 304f, 0f, 0f };
            yield return new object[] { new Vector3((float)4.033111E-05, -922.6683f, 1367.912f), 0f, 0f, 0f, 1650f, 304f, 90f, 30f };
            yield return new object[] { new Vector3(-269.2401f, -1526.937f, 564.3336f), 0f, 0f, 0f, 1650f, 340f, 80f, 30f };
        }
    }
}