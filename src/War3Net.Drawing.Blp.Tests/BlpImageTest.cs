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
        public void TestGetBlpBitmap(string inputImagePath, string expectedImagePath, int mipmapLevel)
        {
            using (var fileStream = File.OpenRead(inputImagePath))
            {
                var expectedImage = new Bitmap(expectedImagePath);
                var blpFile = new BlpFile(fileStream);
                var actualImage = blpFile.GetBitmap(mipmapLevel);

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

        private static IEnumerable<object[]> GetBlpImageData()
        {
            yield break;
        }
    }
}