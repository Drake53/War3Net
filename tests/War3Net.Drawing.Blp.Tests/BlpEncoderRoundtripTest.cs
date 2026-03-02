using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Drawing.Blp.Tests
{
    [TestClass]
    public class BlpEncoderRoundtripTest
    {
        /// <summary>
        /// Performs a roundtrip test: PNG -> BLP -> PNG for all mipmaps.
        /// Uses VillageFallStonePath.png as input, encodes it to BLP1 JPEG,
        /// then decodes all mipmap levels and saves them as separate PNG files.
        /// </summary>
        [TestMethod]
        public void TestRoundtrip_VillageFallStonePath()
        {
            // Load the test image
            var inputPngPath = TestDataProvider.GetPath("Blp/VillageFallStonePath.png");

            if (!File.Exists(inputPngPath))
            {
                Assert.Inconclusive($"Test image not found: {inputPngPath}");
                return;
            }

            // Load PNG
            var bgra = LoadPngAsBgra(inputPngPath, out var width, out var height);
            Console.WriteLine($"Loaded PNG: {width}x{height} from {inputPngPath}");

            // Create output directory
            var outputDir = Path.Combine(Path.GetTempPath(), "BlpRoundtripTest_VillageFallStonePath");
            Directory.CreateDirectory(outputDir);

            try
            {
                // Save copy of original for comparison
                var originalCopyPath = Path.Combine(outputDir, "original.png");
                SaveAsPng(bgra, width, height, originalCopyPath);
                Console.WriteLine($"Saved original copy: {originalCopyPath}");

                // Encode to BLP
                var blpPath = Path.Combine(outputDir, "encoded.blp");

                // Use default options
                var options = new Blp1EncodingOptions();

                var encoder = new BlpEncoder(options);
                using (var blpStream = File.Create(blpPath))
                {
                    encoder.Encode(blpStream, width, height, bgra);
                }

                Console.WriteLine($"Encoded to BLP: {blpPath}");

                // Decode BLP and save all mipmaps as PNG
                using (var blpStream = File.OpenRead(blpPath))
                {
                    using var blpFile = new BlpFile(blpStream);

                    Console.WriteLine($"BLP contains {blpFile.MipMapCount} mipmap levels");
                    Console.WriteLine($"Base dimensions: {blpFile.Width}x{blpFile.Height}");

                    // Decode and save each mipmap
                    for (var level = 0; level < blpFile.MipMapCount; level++)
                    {
                        var pixels = blpFile.GetPixels(level, out var mipWidth, out var mipHeight, bgra: true);
                        var mipPngPath = Path.Combine(outputDir, $"mipmap_{level}_{mipWidth}x{mipHeight}.png");

                        SaveAsPng(pixels, mipWidth, mipHeight, mipPngPath);
                        Console.WriteLine($"  Saved mipmap {level}: {mipPngPath} ({mipWidth}x{mipHeight})");

                        // Verify dimensions
                        var expectedWidth = Math.Max(1, width >> level);
                        var expectedHeight = Math.Max(1, height >> level);
                        Assert.AreEqual(expectedWidth, mipWidth, $"Mipmap {level} width mismatch");
                        Assert.AreEqual(expectedHeight, mipHeight, $"Mipmap {level} height mismatch");

                        // For level 0, compare with original (allowing for JPEG compression artifacts)
                        if (level == 0)
                        {
                            var differences = 0;
                            var maxDiff = 0;
                            long totalDiff = 0;

                            for (var i = 0; i < bgra.Length; i++)
                            {
                                var diff = Math.Abs(bgra[i] - pixels[i]);
                                if (diff > 0)
                                {
                                    differences++;
                                    totalDiff += diff;
                                    maxDiff = Math.Max(maxDiff, diff);
                                }
                            }

                            var avgDiff = differences > 0 ? (double)totalDiff / differences : 0;
                            Console.WriteLine($"  Comparison with original:");
                            Console.WriteLine($"    Pixels different: {differences} / {bgra.Length} ({100.0 * differences / bgra.Length:F2}%)");
                            Console.WriteLine($"    Max difference: {maxDiff}");
                            Console.WriteLine($"    Avg difference: {avgDiff:F2}");

                            // Assert that images are reasonably similar (JPEG is lossy)
                            Assert.IsTrue(maxDiff < 50, $"Max pixel difference too high: {maxDiff}");
                        }
                    }
                }

                Console.WriteLine();
                Console.WriteLine($"Roundtrip test completed successfully!");
                Console.WriteLine($"Output directory: {outputDir}");
            }
            finally
            {
                // Note: Not deleting output directory so results can be inspected
                Console.WriteLine();
                Console.WriteLine($"Test files kept in: {outputDir}");
            }
        }

        private static void SaveAsPng(byte[] bgra, int width, int height, string path)
        {
            using (var image = Image.LoadPixelData<Bgra32>(bgra, width, height))
            {
                image.SaveAsPng(path);
            }
        }

        private static byte[] LoadPngAsBgra(string path, out int width, out int height)
        {
            using (var image = Image.Load<Bgra32>(path))
            {
                width = image.Width;
                height = image.Height;

                var bgra = new byte[width * height * 4];
                image.CopyPixelDataTo(bgra);

                return bgra;
            }
        }
    }
}