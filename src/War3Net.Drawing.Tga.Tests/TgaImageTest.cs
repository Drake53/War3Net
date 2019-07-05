// ------------------------------------------------------------------------------
// <copyright file="TgaImageTest.cs" company="shns">
// Copyright (c) 2016 shns. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.Drawing.Tga.Tests
{
    [TestClass]
    public class TgaImageTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTgaImageData), DynamicDataSourceType.Method)]
        public void TestGetTgaBitmap(string inputImagePath, string expectedImagePath, bool forceAlpha)
        {
            using (var fileStream = File.OpenRead(inputImagePath))
            {
                var expectedImage = new Bitmap(expectedImagePath);
                var actualImage = new TgaImage(fileStream, forceAlpha).GetBitmap();

                Assert.AreEqual(expectedImage.Width, actualImage.Width);
                Assert.AreEqual(expectedImage.Height, actualImage.Height);

                for (var y = 0; y < expectedImage.Height; y++)
                {
                    for (var x = 0; x < expectedImage.Width; x++)
                    {
                        Assert.AreEqual(expectedImage.GetPixel(x, y), actualImage.GetPixel(x, y));
                    }
                }

                expectedImage.Dispose();
                actualImage.Dispose();
            }
        }

        private static IEnumerable<object[]> GetTgaImageData()
        {
            yield return new object[] { "TestData/UBW8.tga", "TestData/grayscale.png", false };
            yield return new object[] { "TestData/UCM8.tga", "TestData/color.png", false };
            yield return new object[] { "TestData/UTC16.tga", "TestData/color.png", false };
            yield return new object[] { "TestData/UTC24.tga", "TestData/color.png", false };
            yield return new object[] { "TestData/UTC32.tga", "TestData/color.png", false };

            yield return new object[] { "TestData/CBW8.tga", "TestData/grayscale.png", false };
            yield return new object[] { "TestData/CCM8.tga", "TestData/color.png", false };
            yield return new object[] { "TestData/CTC16.tga", "TestData/color.png", false };
            yield return new object[] { "TestData/CTC24.tga", "TestData/color.png", false };
            yield return new object[] { "TestData/CTC32.tga", "TestData/color.png", false };

            yield return new object[] { "TestData/rgb32rle.tga", "TestData/rgb32rle.png", true };
        }
    }
}