// ------------------------------------------------------------------------------
// <copyright file="BLTEDecoder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.IO.Compression;

using War3Net.IO.Casc.Crypto;
using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Utilities;
using War3Net.IO.Compression;

namespace War3Net.IO.Casc.Compression
{
    /// <summary>
    /// Provides BLTE decompression functionality.
    /// </summary>
    public static class BLTEDecoder
    {
        private const int MaxRecursionDepth = 10;
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
            var header = BLTEHeader.Parse(reader);

            if (header.IsMultiChunk)
            {
                // Process multiple frames
                foreach (var frame in header.Frames)
                {
                    // Read frame data
                    frame.Data = reader.ReadBytes((int)frame.EncodedSize);

                    // Decode frame
                    var decodedData = DecodeFrameWithDepth(frame, recursionDepth);
                    outputStream.Write(decodedData, 0, decodedData.Length);
                }
            }
            else
            {
                // Single frame - read remaining data
                var remainingBytes = inputStream.Length - inputStream.Position;
                var frameData = reader.ReadBytes((int)remainingBytes);

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
        public static byte[] DecodeFrame(BLTEFrame frame)
        {
            return DecodeFrameWithDepth(frame, 0);
        }

        private static byte[] DecodeFrameWithDepth(BLTEFrame frame, int recursionDepth)
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

        private static byte[] DecodeUncompressed(BinaryReader reader, BLTEFrame frame)
        {
            // Read remaining data as-is
            var dataSize = (int)(frame.Data!.Length - 1); // -1 for compression type byte
            return reader.ReadBytes(dataSize);
        }

        private static byte[] DecodeZLib(BinaryReader reader, BLTEFrame frame)
        {
            // Read compressed data
            var compressedSize = (int)(frame.Data!.Length - 1); // -1 for compression type byte
            var compressedData = reader.ReadBytes(compressedSize);

            // Use War3Net.IO.Compression for zlib decompression
            using var compressedStream = new MemoryStream(compressedData);
            using var decompressor = new ZLibStream(compressedStream, CompressionMode.Decompress);
            using var outputStream = new MemoryStream();

            decompressor.CopyTo(outputStream);
            return outputStream.ToArray();
        }

        private static byte[] DecodeEncrypted(BinaryReader reader, BLTEFrame frame, int recursionDepth)
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

            // Read encrypted data
            var encryptedSize = (int)(frame.Data!.Length - 1 - 1 - keyNameLength - 4 - 1);
            var encryptedData = reader.ReadBytes(encryptedSize);

            // Decrypt the data
            var decryptedData = CascEncryption.Decrypt(encryptedData, keyName, iv);

            // Validate decrypted data
            if (decryptedData == null || decryptedData.Length == 0)
            {
                throw new CascEncryptionException(keyName, "Decryption failed or produced empty data");
            }

            // The decrypted data should be processed based on the compression type
            using var decryptedStream = new MemoryStream(decryptedData);
            using var decryptedReader = new BinaryReader(decryptedStream);

            // Process based on the actual compression type without recursion
            switch ((CompressionType)encryptedType)
            {
                case CompressionType.None:
                    return DecodeUncompressed(decryptedReader, new BLTEFrame { Data = decryptedData });

                case CompressionType.ZLib:
                    return DecodeZLib(decryptedReader, new BLTEFrame { Data = decryptedData });

                case CompressionType.Frame:
                    // Nested BLTE frame with depth check
                    return DecodeWithDepth(decryptedData, recursionDepth + 1);

                case CompressionType.LZMA:
                    return DecodeLZMA(decryptedReader, new BLTEFrame { Data = decryptedData });

                case CompressionType.LZ4:
                    return DecodeLZ4(decryptedReader, new BLTEFrame { Data = decryptedData });

                case CompressionType.ZStandard:
                    return DecodeZStandard(decryptedReader, new BLTEFrame { Data = decryptedData });

                default:
                    throw new CascException($"Unsupported compression type in encrypted frame: 0x{encryptedType:X2}");
            }
        }

        private static byte[] DecodeNestedFrame(BinaryReader reader, BLTEFrame frame, int recursionDepth)
        {
            // Nested BLTE frame - recurse with depth check
            var nestedData = reader.ReadBytes((int)(frame.Data!.Length - 1));
            return DecodeWithDepth(nestedData, recursionDepth + 1);
        }

        private static byte[] DecodeLZMA(BinaryReader reader, BLTEFrame frame)
        {
            // LZMA is not commonly used in modern CASC
            // For full support, consider adding LZMA.NET or similar library
            var dataSize = frame.Data!.Length - 1;
            throw new NotSupportedException($"LZMA decompression is not yet implemented in War3Net.IO.Casc. " +
                $"Frame contains {dataSize} bytes of LZMA compressed data. " +
                $"To add support, consider using LZMA SDK or 7-Zip LZMA SDK NuGet package.");
        }

        private static byte[] DecodeLZ4(BinaryReader reader, BLTEFrame frame)
        {
            // LZ4 is used in some newer CASC implementations
            // For full support, consider adding K4os.Compression.LZ4 library
            var dataSize = frame.Data!.Length - 1;
            throw new NotSupportedException($"LZ4 decompression is not yet implemented in War3Net.IO.Casc. " +
                $"Frame contains {dataSize} bytes of LZ4 compressed data. " +
                $"To add support, consider using K4os.Compression.LZ4 NuGet package.");
        }

        private static byte[] DecodeZStandard(BinaryReader reader, BLTEFrame frame)
        {
            // Zstandard is used in the newest CASC implementations (Battle.net games post-2018)
            // For full support, consider adding ZstdNet library
            var dataSize = frame.Data!.Length - 1;
            throw new NotSupportedException($"Zstandard decompression is not yet implemented in War3Net.IO.Casc. " +
                $"Frame contains {dataSize} bytes of Zstandard compressed data. " +
                $"To add support, consider using ZstdNet or ZstdSharp NuGet package.");
        }

        /// <summary>
        /// Checks if data is BLTE-encoded.
        /// </summary>
        /// <param name="data">The data to check.</param>
        /// <returns>true if the data is BLTE-encoded; otherwise, false.</returns>
        public static bool IsBLTE(byte[] data)
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
        public static bool IsBLTE(Stream stream)
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