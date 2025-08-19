// ------------------------------------------------------------------------------
// <copyright file="BlteDecoder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using War3Net.IO.Casc.Crypto;
using War3Net.IO.Casc.Enums;

namespace War3Net.IO.Casc.Compression
{
    /// <summary>
    /// Provides BLTE decompression functionality.
    /// </summary>
    public static class BlteDecoder
    {
        /// <summary>
        /// Default maximum recursion depth for nested BLTE frames.
        /// </summary>
        public const int DefaultMaxRecursionDepth = 20;

        private static int _maxRecursionDepth = DefaultMaxRecursionDepth;

        /// <summary>
        /// Gets or sets the maximum recursion depth for nested BLTE frames.
        /// </summary>
        public static int MaxRecursionDepth
        {
            get => _maxRecursionDepth;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Maximum recursion depth must be at least 1.");
                }

                if (value > 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Maximum recursion depth cannot exceed 100.");
                }

                _maxRecursionDepth = value;
            }
        }

        /// <summary>
        /// Decodes BLTE-encoded data.
        /// </summary>
        /// <param name="data">The BLTE-encoded data.</param>
        /// <returns>The decoded data.</returns>
        public static byte[] Decode(byte[] data)
        {
            return DecodeWithDepth(data, 0);
        }

        private static byte[] DecodeWithDepth(byte[] data, int recursionDepth)
        {
            if (recursionDepth > MaxRecursionDepth)
            {
                throw new CascException($"Maximum BLTE recursion depth ({MaxRecursionDepth}) exceeded. Possible corrupted data or infinite loop.");
            }

            using var inputStream = new MemoryStream(data);
            using var outputStream = new MemoryStream();
            DecodeWithDepth(inputStream, outputStream, recursionDepth);
            return outputStream.ToArray();
        }

        /// <summary>
        /// Decodes BLTE-encoded data from a stream.
        /// </summary>
        /// <param name="inputStream">The input stream containing BLTE data.</param>
        /// <param name="outputStream">The output stream for decoded data.</param>
        public static void Decode(Stream inputStream, Stream outputStream)
        {
            DecodeWithDepth(inputStream, outputStream, 0);
        }

        private static void DecodeWithDepth(Stream inputStream, Stream outputStream, int recursionDepth)
        {
            if (recursionDepth > MaxRecursionDepth)
            {
                throw new CascException($"Maximum BLTE recursion depth ({MaxRecursionDepth}) exceeded. Possible corrupted data or infinite loop.");
            }

            using var reader = new BinaryReader(inputStream, System.Text.Encoding.UTF8, true);

            // Parse BLTE header
            var header = BlteHeader.Parse(reader);

            // Validate frame sizes to prevent excessive memory allocation
            const uint MaxFrameSize = 100 * 1024 * 1024; // 100 MB per frame
            const uint MaxTotalSize = 500 * 1024 * 1024; // 500 MB total

            if (header.IsMultiChunk)
            {
                uint totalSize = 0;

                // Validate all frame sizes before processing
                foreach (var frame in header.Frames)
                {
                    if (frame.EncodedSize > MaxFrameSize)
                    {
                        throw new CascException($"BLTE frame encoded size ({frame.EncodedSize} bytes) exceeds maximum allowed size ({MaxFrameSize} bytes)");
                    }

                    if (frame.ContentSize > MaxFrameSize)
                    {
                        throw new CascException($"BLTE frame decoded size ({frame.ContentSize} bytes) exceeds maximum allowed size ({MaxFrameSize} bytes)");
                    }

                    totalSize += frame.EncodedSize;
                    if (totalSize > MaxTotalSize)
                    {
                        throw new CascException($"Total BLTE frames size ({totalSize} bytes) exceeds maximum allowed size ({MaxTotalSize} bytes)");
                    }
                }

                // Process multiple frames
                foreach (var frame in header.Frames)
                {
                    // Validate stream position is not negative
                    if (inputStream.Position < 0)
                    {
                        throw new CascException($"Invalid stream position: {inputStream.Position}. Stream may be corrupted.");
                    }

                    // Validate we have enough data to read
                    if (inputStream.Position + frame.EncodedSize > inputStream.Length)
                    {
                        throw new CascException($"Insufficient data in stream. Expected {frame.EncodedSize} bytes but only {inputStream.Length - inputStream.Position} bytes available.");
                    }

                    // Additional safety check for allocation size
                    try
                    {
                        // Read frame data
                        frame.Data = reader.ReadBytes((int)frame.EncodedSize);
                    }
                    catch (OutOfMemoryException)
                    {
                        throw new CascException($"Unable to allocate {frame.EncodedSize} bytes for BLTE frame. File may be corrupted.");
                    }

                    // Decode frame
                    var decodedData = DecodeFrameWithDepth(frame, recursionDepth);

                    // Validate decoded size if specified
                    if (frame.ContentSize > 0 && decodedData.Length != frame.ContentSize)
                    {
                        throw new CascException($"Decoded frame size mismatch. Expected {frame.ContentSize} bytes but got {decodedData.Length} bytes.");
                    }

                    outputStream.Write(decodedData, 0, decodedData.Length);
                }
            }
            else
            {
                // Validate stream position
                if (inputStream.Position < 0)
                {
                    throw new CascException($"Invalid stream position: {inputStream.Position}. Stream may be corrupted.");
                }

                // Single frame - read remaining data
                var remainingBytes = inputStream.Length - inputStream.Position;

                if (remainingBytes < 0)
                {
                    throw new CascException($"Invalid remaining bytes calculation. Stream position ({inputStream.Position}) exceeds stream length ({inputStream.Length}).");
                }

                if (remainingBytes > MaxFrameSize)
                {
                    throw new CascException($"Single BLTE frame size ({remainingBytes} bytes) exceeds maximum allowed size ({MaxFrameSize} bytes)");
                }

                // Additional safety check for allocation
                byte[] frameData;
                try
                {
                    frameData = reader.ReadBytes((int)remainingBytes);
                }
                catch (OutOfMemoryException)
                {
                    throw new CascException($"Unable to allocate {remainingBytes} bytes for BLTE frame. File may be corrupted.");
                }

                // Create frame info
                var frame = header.Frames[0];
                frame.EncodedSize = (uint)frameData.Length;
                frame.Data = frameData;

                // Decode frame
                var decodedData = DecodeFrameWithDepth(frame, recursionDepth);
                outputStream.Write(decodedData, 0, decodedData.Length);
            }
        }

        /// <summary>
        /// Decodes a single BLTE frame.
        /// </summary>
        /// <param name="frame">The frame to decode.</param>
        /// <returns>The decoded data.</returns>
        public static byte[] DecodeFrame(BlteFrame frame)
        {
            return DecodeFrameWithDepth(frame, 0);
        }

        private static byte[] DecodeFrameWithDepth(BlteFrame frame, int recursionDepth)
        {
            if (recursionDepth > MaxRecursionDepth)
            {
                throw new CascException($"Maximum BLTE recursion depth ({MaxRecursionDepth}) exceeded. Possible corrupted data or infinite loop.");
            }

            if (frame.Data == null || frame.Data.Length == 0)
            {
                return Array.Empty<byte>();
            }

            using var inputStream = new MemoryStream(frame.Data);
            using var reader = new BinaryReader(inputStream);

            // Read compression type
            frame.CompressionType = (CompressionType)reader.ReadByte();

            switch (frame.CompressionType)
            {
                case CompressionType.None:
                    return DecodeUncompressed(reader, frame);

                case CompressionType.ZLib:
                    return DecodeZLib(reader, frame);

                case CompressionType.Encrypted:
                    return DecodeEncrypted(reader, frame, recursionDepth);

                case CompressionType.Frame:
                    return DecodeNestedFrame(reader, frame, recursionDepth);

                case CompressionType.LZMA:
                    return DecodeLZMA(reader, frame);

                case CompressionType.LZ4:
                    return DecodeLZ4(reader, frame);

                case CompressionType.ZStandard:
                    return DecodeZStandard(reader, frame);

                default:
                    throw new CascException($"Unsupported compression type: 0x{frame.CompressionType:X2} ('{(char)frame.CompressionType}')");
            }
        }

        private static byte[] DecodeUncompressed(BinaryReader reader, BlteFrame frame)
        {
            // Read remaining data as-is
            var dataSize = (int)(frame.Data!.Length - 1); // -1 for compression type byte
            return reader.ReadBytes(dataSize);
        }

        private static byte[] DecodeZLib(BinaryReader reader, BlteFrame frame)
        {
            // Read compressed data
            var compressedSize = (int)(frame.Data!.Length - 1); // -1 for compression type byte
            var compressedData = reader.ReadBytes(compressedSize);

            // Use War3Net.IO.Compression for zlib decompression
            using var compressedStream = new MemoryStream(compressedData);
            using var decompressor = new Ionic.Zlib.ZlibStream(compressedStream, Ionic.Zlib.CompressionMode.Decompress);
            using var outputStream = new MemoryStream();

            decompressor.CopyTo(outputStream);
            return outputStream.ToArray();
        }

        private static byte[] DecodeEncrypted(BinaryReader reader, BlteFrame frame, int recursionDepth)
        {
            // Read encryption key name
            var keyNameLength = reader.ReadByte();
            if (keyNameLength != 8)
            {
                throw new CascException($"Invalid encryption key name length: {keyNameLength}");
            }

            var keyNameBytes = reader.ReadBytes(keyNameLength);
            var keyName = BitConverter.ToUInt64(keyNameBytes, 0);

            // Read IV
            var iv = reader.ReadBytes(4);

            // Read encrypted data type
            var encryptedType = reader.ReadByte();

            // Prevent infinite recursion - encrypted frames should not contain another encrypted frame
            // Check BEFORE attempting decryption to avoid potential exploits
            if ((CompressionType)encryptedType == CompressionType.Encrypted)
            {
                throw new CascException("Nested encrypted frames are not supported");
            }

            // Read encrypted data size before validation to ensure proper bounds checking
            var encryptedSize = (int)(frame.Data!.Length - 1 - 1 - keyNameLength - 4 - 1);
            if (encryptedSize < 0)
            {
                throw new CascException($"Invalid encrypted data size: {encryptedSize}");
            }

            var encryptedData = reader.ReadBytes(encryptedSize);

            // Decrypt the data
            var decryptedData = CascEncryption.Decrypt(encryptedData, keyName, iv);

            // Validate decrypted data
            if (decryptedData == null || decryptedData.Length == 0)
            {
                throw new CascEncryptionException($"Decryption failed or produced empty data for key: 0x{keyName:X16}");
            }

            // The decrypted data should be processed based on the compression type
            using var decryptedStream = new MemoryStream(decryptedData);
            using var decryptedReader = new BinaryReader(decryptedStream);

            // Process based on the actual compression type without recursion
            switch ((CompressionType)encryptedType)
            {
                case CompressionType.None:
                    return DecodeUncompressed(decryptedReader, new BlteFrame { Data = decryptedData });

                case CompressionType.ZLib:
                    return DecodeZLib(decryptedReader, new BlteFrame { Data = decryptedData });

                case CompressionType.Frame:
                    // Nested BLTE frame with depth check
                    return DecodeWithDepth(decryptedData, recursionDepth + 1);

                case CompressionType.LZMA:
                    return DecodeLZMA(decryptedReader, new BlteFrame { Data = decryptedData });

                case CompressionType.LZ4:
                    return DecodeLZ4(decryptedReader, new BlteFrame { Data = decryptedData });

                case CompressionType.ZStandard:
                    return DecodeZStandard(decryptedReader, new BlteFrame { Data = decryptedData });

                default:
                    throw new CascException($"Unsupported compression type in encrypted frame: 0x{encryptedType:X2}");
            }
        }

        private static byte[] DecodeNestedFrame(BinaryReader reader, BlteFrame frame, int recursionDepth)
        {
            // Nested BLTE frame - recurse with depth check
            var nestedData = reader.ReadBytes((int)(frame.Data!.Length - 1));
            return DecodeWithDepth(nestedData, recursionDepth + 1);
        }

        private static byte[] DecodeLZMA(BinaryReader reader, BlteFrame frame)
        {
            // LZMA (Lempel-Ziv-Markov chain algorithm) compression
            // Used in: Some older WoW patches and classic Blizzard games
            // Implementation notes:
            // - Requires LZMA SDK (7-Zip SDK) or LZMA.NET NuGet package
            // - Header format: 5 bytes properties + 8 bytes uncompressed size
            // - Reference: CascLib's CascDecompress.cpp::Decompress_LZMA()
            var dataSize = frame.Data!.Length - 1;

            // TODO: To implement LZMA support:
            // 1. Add NuGet package: LZMA-SDK or SevenZipSharp
            // 2. Read LZMA properties (5 bytes)
            // 3. Read uncompressed size (8 bytes, little-endian)
            // 4. Initialize LZMA decoder with properties
            // 5. Decompress remaining data

            throw new NotSupportedException(
                $"LZMA decompression is not yet implemented in War3Net.IO.Casc.\n" +
                $"Frame contains {dataSize} bytes of LZMA compressed data.\n" +
                $"This compression is used in some older Blizzard games.\n" +
                $"To add support:\n" +
                $"  1. Install NuGet package: LZMA-SDK\n" +
                $"  2. Implement using Decoder.Code() method\n" +
                $"Reference implementation: CascLib/src/CascDecompress.cpp");
        }

        private static byte[] DecodeLZ4(BinaryReader reader, BlteFrame frame)
        {
            // LZ4 (Extremely Fast Compression algorithm)
            // Used in: Modern WoW, Overwatch, Heroes of the Storm
            // Implementation notes:
            // - Very fast decompression speed
            // - Frame format may include uncompressed size in header
            // - Reference: CascLib's CascDecompress.cpp::Decompress_LZ4()
            var dataSize = frame.Data!.Length - 1;

            // TODO: To implement LZ4 support:
            // 1. Add NuGet package: K4os.Compression.LZ4
            // 2. Check for uncompressed size header (4 bytes, optional)
            // 3. Use LZ4Codec.Decode() for decompression
            // 4. Handle both block and frame formats

            throw new NotSupportedException(
                $"LZ4 decompression is not yet implemented in War3Net.IO.Casc.\n" +
                $"Frame contains {dataSize} bytes of LZ4 compressed data.\n" +
                $"This compression is used in modern Blizzard games (2016+).\n" +
                $"To add support:\n" +
                $"  1. Install NuGet package: K4os.Compression.LZ4\n" +
                $"  2. Use LZ4Codec.Decode() method\n" +
                $"Reference implementation: CascLib/src/CascDecompress.cpp");
        }

        private static byte[] DecodeZStandard(BinaryReader reader, BlteFrame frame)
        {
            // Zstandard (Fast real-time compression algorithm by Facebook)
            // Used in: Latest WoW expansions, Diablo IV, newer Battle.net games
            // Implementation notes:
            // - Better compression ratio than LZ4 with similar speed
            // - Self-contained frames with magic number 0xFD2FB528
            // - Reference: CascLib's CascDecompress.cpp::Decompress_ZSTD()
            var dataSize = frame.Data!.Length - 1;

            // TODO: To implement Zstandard support:
            // 1. Add NuGet package: ZstdSharp.Port or ZstdNet
            // 2. Create decompressor context
            // 3. Use Decompress() method with input data
            // 4. Handle streaming decompression for large files

            throw new NotSupportedException(
                $"Zstandard decompression is not yet implemented in War3Net.IO.Casc.\n" +
                $"Frame contains {dataSize} bytes of Zstandard compressed data.\n" +
                $"This compression is used in the newest Blizzard games (2018+).\n" +
                $"To add support:\n" +
                $"  1. Install NuGet package: ZstdSharp.Port\n" +
                $"  2. Use Decompressor.Unwrap() method\n" +
                $"Reference implementation: CascLib/src/CascDecompress.cpp");
        }

        /// <summary>
        /// Checks if data is BLTE-encoded.
        /// </summary>
        /// <param name="data">The data to check.</param>
        /// <returns>true if the data is BLTE-encoded; otherwise, false.</returns>
        public static bool IsBlte(byte[] data)
        {
            if (data == null || data.Length < 8)
            {
                return false;
            }

            // Check for BLTE signature
            return data[0] == 'B' && data[1] == 'L' && data[2] == 'T' && data[3] == 'E';
        }

        /// <summary>
        /// Checks if a stream contains BLTE-encoded data.
        /// </summary>
        /// <param name="stream">The stream to check.</param>
        /// <returns>true if the stream contains BLTE-encoded data; otherwise, false.</returns>
        public static bool IsBlte(Stream stream)
        {
            if (stream == null || stream.Length < 8)
            {
                return false;
            }

            // For non-seekable streams, we cannot check without consuming data
            if (!stream.CanSeek)
            {
                throw new NotSupportedException("Cannot check BLTE signature on non-seekable streams without consuming data");
            }

            // Save the current position
            var originalPosition = stream.Position;

            try
            {
                // Check if we can read 4 bytes from current position
                if (stream.Length - stream.Position < 4)
                {
                    return false;
                }

                var signature = new byte[4];
                var bytesRead = stream.Read(signature, 0, 4);

                if (bytesRead != 4)
                {
                    return false;
                }

                return signature[0] == 'B' && signature[1] == 'L' && signature[2] == 'T' && signature[3] == 'E';
            }
            finally
            {
                // Restore the original position for seekable streams
                stream.Position = originalPosition;
            }
        }
    }
}