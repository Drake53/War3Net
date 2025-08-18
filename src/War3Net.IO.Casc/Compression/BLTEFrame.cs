// ------------------------------------------------------------------------------
// <copyright file="BLTEFrame.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.IO.Casc.Enums;

namespace War3Net.IO.Casc.Compression
{
    /// <summary>
    /// Represents a single frame in BLTE-encoded data.
    /// </summary>
    public class BLTEFrame
    {
        /// <summary>
        /// Gets or sets the encoded size of the frame.
        /// </summary>
        public uint EncodedSize { get; set; }

        /// <summary>
        /// Gets or sets the decoded (content) size of the frame.
        /// </summary>
        public uint ContentSize { get; set; }

        /// <summary>
        /// Gets or sets the MD5 hash of the frame (optional).
        /// </summary>
        public byte[]? Hash { get; set; }

        /// <summary>
        /// Gets or sets the compression type.
        /// </summary>
        public CompressionType CompressionType { get; set; }

        /// <summary>
        /// Gets or sets the frame data.
        /// </summary>
        public byte[]? Data { get; set; }

        /// <summary>
        /// Gets a value indicating whether this frame has a hash.
        /// </summary>
        public bool HasHash => Hash != null && Hash.Length == CascConstants.MD5HashSize;

        /// <summary>
        /// Gets a value indicating whether this frame is encrypted.
        /// </summary>
        public bool IsEncrypted => CompressionType == CompressionType.Encrypted;

        /// <summary>
        /// Gets a value indicating whether this frame is compressed.
        /// </summary>
        public bool IsCompressed => CompressionType == CompressionType.ZLib;

        /// <summary>
        /// Gets a value indicating whether this frame contains nested frames.
        /// </summary>
        public bool IsNested => CompressionType == CompressionType.Frame;
    }
}