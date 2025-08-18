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
        /// <summary>
        /// Decodes BLTE-encoded data.
        /// </summary>
        /// <param name="data">The BLTE-encoded data.</param>
        /// <returns>The decoded data.</returns>
        public static byte[] Decode(byte[] data)
        {
            using var inputStream = new MemoryStream(data);
            using var outputStream = new MemoryStream();
            Decode(inputStream, outputStream);
            return outputStream.ToArray();
        }

        /// <summary>
        /// Decodes BLTE-encoded data from a stream.
        /// </summary>
        /// <param name="inputStream">The input stream containing BLTE data.</param>
        /// <param name="outputStream">The output stream for decoded data.</param>
        public static void Decode(Stream inputStream, Stream outputStream)
        {
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
                    var decodedData = DecodeFrame(frame);
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
                var decodedData = DecodeFrame(frame);
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
                    return DecodeEncrypted(reader, frame);

                case CompressionType.Frame:
                    return DecodeNestedFrame(reader, frame);

                default:
                    throw new CascException($"Unsupported compression type: 0x{frame.CompressionType:X2}");
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

        private static byte[] DecodeEncrypted(BinaryReader reader, BLTEFrame frame)
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

            // Read encrypted data
            var encryptedSize = (int)(frame.Data!.Length - 1 - 1 - keyNameLength - 4 - 1);
            var encryptedData = reader.ReadBytes(encryptedSize);

            // Decrypt the data
            var decryptedData = CascEncryption.Decrypt(encryptedData, keyName, iv);

            // The decrypted data contains the actual compression type followed by the data
            using var decryptedStream = new MemoryStream(decryptedData);
            using var decryptedReader = new BinaryReader(decryptedStream);

            // Create a new frame with decrypted data
            var decryptedFrame = new BLTEFrame
            {
                CompressionType = (CompressionType)encryptedType,
                Data = decryptedData,
            };

            // Decode based on the actual compression type
            return DecodeFrame(decryptedFrame);
        }

        private static byte[] DecodeNestedFrame(BinaryReader reader, BLTEFrame frame)
        {
            // Nested BLTE frame - recurse
            var nestedData = reader.ReadBytes((int)(frame.Data!.Length - 1));
            return Decode(nestedData);
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

            var position = stream.Position;
            var signature = new byte[4];
            stream.Read(signature, 0, 4);
            stream.Position = position;

            return signature[0] == 'B' && signature[1] == 'L' && signature[2] == 'T' && signature[3] == 'E';
        }
    }
}