// ------------------------------------------------------------------------------
// <copyright file="BlpEncoder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using JpegLibrary;

namespace War3Net.Drawing.Blp
{
    /// <summary>
    /// Encoder for creating BLP1 files with JPEG compression and BGRA channels.
    /// BLP1 uses a non-standard 4-channel JPEG format (BGRA) instead of the typical 3-channel RGB.
    /// The encoder stores normal (non-inverted) pixel values in the JPEG stream.
    /// Decoding behavior is platform-specific: Windows WPF inverts after decoding, JpegLibrary returns raw values.
    /// </summary>
    public sealed class BlpEncoder
    {
        private readonly Blp1EncodingOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlpEncoder"/> class.
        /// </summary>
        /// <param name="options">Encoding options.</param>
        public BlpEncoder(Blp1EncodingOptions? options = null)
        {
            _options = options ?? new Blp1EncodingOptions();
        }

        /// <summary>
        /// Encodes BGRA pixel data to a BLP1 JPEG file.
        /// The encoder stores normal (non-inverted) pixel values using non-standard 4-channel JPEG (BGRA).
        /// </summary>
        /// <param name="stream">The stream to write the BLP1 data to.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        /// <param name="bgra">BGRA pixel data (4 bytes per pixel).</param>
        public void Encode(Stream stream, int width, int height, byte[] bgra)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (bgra is null)
            {
                throw new ArgumentNullException(nameof(bgra));
            }

            if (bgra.Length != width * height * 4)
            {
                throw new ArgumentException("BGRA data length does not match width * height * 4", nameof(bgra));
            }

            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("Width and height must be positive");
            }

            // Generate mipmaps
            var mipmaps = GenerateMipmaps(width, height, bgra);

            // Encode each mipmap to JPEG
            var jpegMipmaps = new byte[mipmaps.Length][];
            for (var i = 0; i < mipmaps.Length; i++)
            {
                var mipWidth = Math.Max(1, width >> i);
                var mipHeight = Math.Max(1, height >> i);
                jpegMipmaps[i] = EncodeJpeg(mipmaps[i], mipWidth, mipHeight);
            }

            using (var writer = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                // Write header
                WriteHeader(writer, width, height, jpegMipmaps.Length);

                // Calculate and write mipmap offsets and sizes
                var (offsets, sizes) = CalculateMipmapOffsetsAndSizes(jpegMipmaps);
                WriteMipmapInfo(writer, offsets, sizes);

                // Write JPEG header size (0 = not using shared header)
                writer.Write(0u);

                // Write JPEG data for each mipmap
                foreach (var jpegData in jpegMipmaps)
                {
                    writer.Write(jpegData);
                }
            }
        }

        private byte[][] GenerateMipmaps(int width, int height, byte[] bgra)
        {
            if (!_options.GenerateMipmaps)
            {
                return new[] { bgra };
            }

            // Calculate number of mipmap levels
            var maxLevels = CalculateMaxMipmapLevels(width, height);
            var levels = _options.MipmapLevels > 0
                ? Math.Min(_options.MipmapLevels, maxLevels)
                : maxLevels;

            var mipmaps = new byte[levels][];
            mipmaps[0] = bgra;

            // Generate each mipmap level
            for (var i = 1; i < levels; i++)
            {
                var prevWidth = Math.Max(1, width >> (i - 1));
                var prevHeight = Math.Max(1, height >> (i - 1));
                var currWidth = Math.Max(1, width >> i);
                var currHeight = Math.Max(1, height >> i);

                mipmaps[i] = GenerateMipmap(mipmaps[i - 1], prevWidth, prevHeight, currWidth, currHeight);
            }

            return mipmaps;
        }

        private byte[] GenerateMipmap(byte[] source, int srcWidth, int srcHeight, int dstWidth, int dstHeight)
        {
            var dest = new byte[dstWidth * dstHeight * 4];

            // Simple box filter downsampling
            for (var y = 0; y < dstHeight; y++)
            {
                for (var x = 0; x < dstWidth; x++)
                {
                    var srcX = x * 2;
                    var srcY = y * 2;

                    // Sample 2x2 pixels and average them
                    int b = 0, g = 0, r = 0, a = 0;
                    var count = 0;

                    for (var dy = 0; dy < 2 && srcY + dy < srcHeight; dy++)
                    {
                        for (var dx = 0; dx < 2 && srcX + dx < srcWidth; dx++)
                        {
                            var srcIdx = ((srcY + dy) * srcWidth + (srcX + dx)) * 4;
                            b += source[srcIdx + 0];
                            g += source[srcIdx + 1];
                            r += source[srcIdx + 2];
                            a += source[srcIdx + 3];
                            count++;
                        }
                    }

                    var dstIdx = (y * dstWidth + x) * 4;
                    dest[dstIdx + 0] = (byte)(b / count);
                    dest[dstIdx + 1] = (byte)(g / count);
                    dest[dstIdx + 2] = (byte)(r / count);
                    dest[dstIdx + 3] = (byte)(a / count);
                }
            }

            return dest;
        }

        private int CalculateMaxMipmapLevels(int width, int height)
        {
            var levels = 1;
            var minDim = Math.Min(width, height);

            while (minDim > 1 && levels < 16)
            {
                minDim >>= 1;
                levels++;
            }

            return levels;
        }

        private byte[] EncodeJpeg(byte[] bgra, int width, int height)
        {
            // BLP1 uses non-standard 4-channel JPEG (BGRA format instead of RGB/CMYK)
            // Pixel values are stored normally (NOT inverted) in the JPEG stream
            // The decoder handles platform-specific interpretation:
            // - Windows WPF: interprets as RGB/RGBA, inverts after decoding
            // - JpegLibrary: reads raw 4-channel data directly
            var encoder = new JpegEncoder();

            // Set quantization tables for all 4 components
            // Each component needs its own quantization table with unique identifier
            var qTable0 = JpegStandardQuantizationTable.ScaleByQuality(
                JpegStandardQuantizationTable.GetLuminanceTable(JpegElementPrecision.Precision8Bit, 0),
                _options.JpegQuality);
            var qTable1 = JpegStandardQuantizationTable.ScaleByQuality(
                JpegStandardQuantizationTable.GetChrominanceTable(JpegElementPrecision.Precision8Bit, 1),
                _options.JpegQuality);
            var qTable2 = JpegStandardQuantizationTable.ScaleByQuality(
                JpegStandardQuantizationTable.GetChrominanceTable(JpegElementPrecision.Precision8Bit, 2),
                _options.JpegQuality);
            var qTable3 = JpegStandardQuantizationTable.ScaleByQuality(
                JpegStandardQuantizationTable.GetChrominanceTable(JpegElementPrecision.Precision8Bit, 3),
                _options.JpegQuality);

            encoder.SetQuantizationTable(qTable0);
            encoder.SetQuantizationTable(qTable1);
            encoder.SetQuantizationTable(qTable2);
            encoder.SetQuantizationTable(qTable3);

            // Set Huffman tables (we'll reuse the same tables for all components)
            encoder.SetHuffmanTable(true, 0, JpegStandardHuffmanEncodingTable.GetLuminanceDCTable());
            encoder.SetHuffmanTable(false, 0, JpegStandardHuffmanEncodingTable.GetLuminanceACTable());
            encoder.SetHuffmanTable(true, 1, JpegStandardHuffmanEncodingTable.GetChrominanceDCTable());
            encoder.SetHuffmanTable(false, 1, JpegStandardHuffmanEncodingTable.GetChrominanceACTable());
            encoder.SetHuffmanTable(true, 2, JpegStandardHuffmanEncodingTable.GetChrominanceDCTable());
            encoder.SetHuffmanTable(false, 2, JpegStandardHuffmanEncodingTable.GetChrominanceACTable());
            encoder.SetHuffmanTable(true, 3, JpegStandardHuffmanEncodingTable.GetChrominanceDCTable());
            encoder.SetHuffmanTable(false, 3, JpegStandardHuffmanEncodingTable.GetChrominanceACTable());

            // Add 4 components (BGRA - non-standard for JPEG!)
            // Parameters: componentId, quantTableId, dcTableId, acTableId, hSampling, vSampling
            encoder.AddComponent(1, 0, 0, 0, 1, 1); // Blue component
            encoder.AddComponent(2, 1, 1, 1, 1, 1); // Green component
            encoder.AddComponent(3, 2, 2, 2, 1, 1); // Red component
            encoder.AddComponent(4, 3, 3, 3, 1, 1); // Alpha component

            // Set input reader with original (non-inverted) pixel data
            encoder.SetInputReader(new JpegBufferInputReader(width, height, 4, bgra));

            // Encode to buffer
            var writer = new System.Buffers.ArrayBufferWriter<byte>();
            encoder.SetOutput(writer);
            encoder.Encode();

            return writer.WrittenSpan.ToArray();
        }

        private void WriteHeader(BinaryWriter writer, int width, int height, int mipmapCount)
        {
            // Magic number - BLP1
            writer.Write((int)FileFormatVersion.BLP1);

            // Content type (0=JPEG)
            writer.Write((int)FileContent.JPG);

            // Alpha depth (0 for JPEG format, even though we encode BGRA)
            writer.Write(0u);

            // Width and height
            writer.Write(width);
            writer.Write(height);

            // Extra flags
            writer.Write(_options.ExtraFlags);

            // Has mipmaps
            writer.Write(mipmapCount > 1 ? 1u : 0u);
        }

        private static (uint[] offsets, uint[] sizes) CalculateMipmapOffsetsAndSizes(byte[][] jpegMipmaps)
        {
            var offsets = new uint[16];
            var sizes = new uint[16];

            // Header structure:
            // 0x00-0x1B: BLP1 header (28 bytes)
            // 0x1C-0x5B: Mipmap offsets (64 bytes)
            // 0x5C-0x9B: Mipmap sizes (64 bytes)
            // 0x9C-0x9F: JPEG header size field (4 bytes)
            // 0xA0+: JPEG data starts here
            uint currentOffset = 0x9C + 4; // After all header data and JPEG header size field

            for (var i = 0; i < jpegMipmaps.Length; i++)
            {
                offsets[i] = currentOffset;
                sizes[i] = (uint)jpegMipmaps[i].Length;
                currentOffset += sizes[i];
            }

            return (offsets, sizes);
        }

        private void WriteMipmapInfo(BinaryWriter writer, uint[] offsets, uint[] sizes)
        {
            // Write 16 mipmap offsets
            for (var i = 0; i < 16; i++)
            {
                writer.Write(offsets[i]);
            }

            // Write 16 mipmap sizes
            for (var i = 0; i < 16; i++)
            {
                writer.Write(sizes[i]);
            }
        }
    }
}