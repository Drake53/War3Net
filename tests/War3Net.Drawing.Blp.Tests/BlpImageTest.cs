// ------------------------------------------------------------------------------
// <copyright file="BlpImageTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.Drawing.Blp.Tests
{
    public static class SKColorFormatter
    {
        /// <summary>
        /// Displays the <see cref="SkiaSharp.SKColor"/> in the style of <see cref="Color.ToString()"/>.
        /// </summary>
        public static string ToColorString(this SkiaSharp.SKColor color)
        {
            return $"Color [A={color.Alpha}, R={color.Red}, G={color.Green}, B={color.Blue}]";
        }
    }

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
                        // Allow pixel values to be slightly different, since some testcases were decoded with WPF (BitmapSource), not SkiaSharp.
                        const int delta = 1;

                        var expectedPixel = expectedImage.GetPixel(x, y);
                        var actualPixel = actualImage.GetPixel(x, y);

                        var message = $"Expected:<{expectedPixel}>. Actual:<{actualPixel.ToColorString()}>";

                        Assert.IsTrue(Math.Abs(expectedPixel.A - actualPixel.Alpha) <= delta, message);
                        Assert.IsTrue(Math.Abs(expectedPixel.R - actualPixel.Red) <= delta, message);
                        Assert.IsTrue(Math.Abs(expectedPixel.G - actualPixel.Green) <= delta, message);
                        Assert.IsTrue(Math.Abs(expectedPixel.B - actualPixel.Blue) <= delta, message);
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
                        // Allow pixel values to be slightly different, since some testcases were decoded with WPF (BitmapSource), not SkiaSharp.
                        const int delta = 1;

                        var expectedPixel = expectedImage.GetPixel(x, y);
                        var actualPixel = actualImage.GetPixel(x, y);

                        var message = $"Expected:<{expectedPixel}>. Actual:<{actualPixel}>";

                        Assert.IsTrue(Math.Abs(expectedPixel.A - actualPixel.A) <= delta, message);
                        Assert.IsTrue(Math.Abs(expectedPixel.R - actualPixel.R) <= delta, message);
                        Assert.IsTrue(Math.Abs(expectedPixel.G - actualPixel.G) <= delta, message);
                        Assert.IsTrue(Math.Abs(expectedPixel.B - actualPixel.B) <= delta, message);
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

            yield return new object[] { "TestData/map101.blp", "TestData/map101.png", 0 };
        }
    }
}