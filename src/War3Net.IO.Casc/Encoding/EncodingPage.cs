// ------------------------------------------------------------------------------
// <copyright file="EncodingPage.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.IO.Casc.Utilities;

namespace War3Net.IO.Casc.Encoding
{
    /// <summary>
    /// Represents a page header in the ENCODING manifest.
    /// </summary>
    public class EncodingPage
    {
        /// <summary>
        /// Gets or sets the first key in the page.
        /// </summary>
        public byte[] FirstKey { get; set; } = new byte[CascConstants.MD5HashSize];

        /// <summary>
        /// Gets or sets the MD5 hash of the entire page.
        /// </summary>
        public byte[] PageHash { get; set; } = new byte[CascConstants.MD5HashSize];

        /// <summary>
        /// Parses an encoding page header from a binary reader.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <returns>The parsed page header.</returns>
        public static EncodingPage Parse(BinaryReader reader)
        {
            var page = new EncodingPage
            {
                FirstKey = reader.ReadMD5Hash(),
                PageHash = reader.ReadMD5Hash(),
            };

            return page;
        }

        /// <summary>
        /// Writes the page header to a binary writer.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        public void WriteTo(BinaryWriter writer)
        {
            writer.WriteMD5Hash(FirstKey);
            writer.WriteMD5Hash(PageHash);
        }

        /// <summary>
        /// Gets the size of the page header in bytes.
        /// </summary>
        public static int Size => CascConstants.MD5HashSize * 2;
    }
}