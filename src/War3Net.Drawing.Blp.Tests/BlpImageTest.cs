// ------------------------------------------------------------------------------
// <copyright file="BlpImageTest.cs" company="Xalcon @ mmowned.com-Forum">
// Copyright (c) 2011 Xalcon @ mmowned.com-Forum. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.Drawing.Blp.Tests
{
    [TestClass]
    public class BlpImageTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetBlpImageData), DynamicDataSourceType.Method)]
        public void TestGetBlpSKBitmap(string inputImagePath, string expectedImagePath, int mipMapLevel)
        {
            using (var fileStream = File.OpenRead(inputImagePath))
            {
                var expectedImage = new Bitmap(expectedImagePath);
                var blpFile = new BlpFile(fileStream);
                var actualImage = blpFile.GetSKBitmap(mipMapLevel);

                Assert.AreEqual(expectedImage.Width, actualImage.Width);
                Assert.AreEqual(expectedImage.Height, actualImage.Height);

                for (var y = 0; y < expectedImage.Height; y++)
                {
                    for (var x = 0; x < expectedImage.Width; x++)
                    {
                        Assert.AreEqual(expectedImage.GetPixel(x, y).A, actualImage.GetPixel(x, y).Alpha);
                        Assert.AreEqual(expectedImage.GetPixel(x, y).R, actualImage.GetPixel(x, y).Red);
                        Assert.AreEqual(expectedImage.GetPixel(x, y).G, actualImage.GetPixel(x, y).Green);
                        Assert.AreEqual(expectedImage.GetPixel(x, y).B, actualImage.GetPixel(x, y).Blue);
                    }
                }

                expectedImage.Dispose();
                blpFile.Dispose();
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetBlpImageData), DynamicDataSourceType.Method)]
        public void TestGetBlpBitmap(string inputImagePath, string expectedImagePath, int mipMapLevel)
        {
            using (var fileStream = File.OpenRead(inputImagePath))
            {
                var expectedImage = new Bitmap(expectedImagePath);
                var blpFile = new BlpFile(fileStream);
                var actualImage = blpFile.GetBitmap(mipMapLevel);

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
                blpFile.Dispose();
            }
        }

#if NETFRAMEWORK || NETCOREAPP3_0
        [DataTestMethod]
        [DynamicData(nameof(GetBlpImageData), DynamicDataSourceType.Method)]
        public void TestGetBlpBitmapSource(string inputImagePath, string expectedImagePath, int mipMapLevel)
        {
            using (var fileStream = File.OpenRead(inputImagePath))
            {
                var expectedImage = new Bitmap(expectedImagePath);
                var blpFile = new BlpFile(fileStream);
                var actualImage = blpFile.GetBitmapSource(mipMapLevel);

                Assert.AreEqual(expectedImage.Width, actualImage.PixelWidth);
                Assert.AreEqual(expectedImage.Height, actualImage.PixelHeight);

                var bytesPerPixel = (actualImage.Format.BitsPerPixel + 7) / 8;
                var stride = bytesPerPixel * actualImage.PixelWidth;
                var bytes = new byte[stride * actualImage.PixelHeight];
                actualImage.CopyPixels(bytes, stride, 0);

                for (var y = 0; y < expectedImage.Height; y++)
                {
                    for (var x = 0; x < expectedImage.Width; x++)
                    {
                        var offset = (y * stride) + (x * bytesPerPixel);

                        // Assumes actualImage.Format is either PixelFormats.Bgr32 or PixelFormats.Bgra32
                        Assert.AreEqual(expectedImage.GetPixel(x, y).B, bytes[offset + 0]);
                        Assert.AreEqual(expectedImage.GetPixel(x, y).G, bytes[offset + 1]);
                        Assert.AreEqual(expectedImage.GetPixel(x, y).R, bytes[offset + 2]);

                        if (bytesPerPixel > 3)
                        {
                            Assert.AreEqual(expectedImage.GetPixel(x, y).A, bytes[offset + 3]);
                        }
                    }
                }

                expectedImage.Dispose();
                blpFile.Dispose();
            }
        }
#endif

        private static IEnumerable<object[]> GetBlpImageData()
        {
            yield return new object[] { "TestData/colorJpg75Mip8Blp1.blp", "TestData/color.png", 0 };
            yield return new object[] { "TestData/colorPalettedMip8Blp1.blp", "TestData/color.png", 0 };

            yield return new object[] { "TestData/colorDxtMip8Blp2.blp", "TestData/color.png", 0 };
            yield return new object[] { "TestData/colorPalettedMip8Blp2.blp", "TestData/color.png", 0 };
        }
    }
}