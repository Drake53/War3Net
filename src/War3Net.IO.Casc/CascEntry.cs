// ------------------------------------------------------------------------------
// <copyright file="CascEntry.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc
{
    /// <summary>
    /// Represents an entry in a CASC archive.
    /// </summary>
    public class CascEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascEntry"/> class.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public CascEntry(string fileName)
        {
            FileName = fileName;
            CKey = CascKey.Empty;
            EKey = EKey.Empty;
            FileDataId = CascConstants.InvalidId;
            LocaleFlags = CascLocaleFlags.None;
            ContentFlags = CascContentFlags.None;
        }

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Gets or sets the content key.
        /// </summary>
        public CascKey CKey { get; set; }

        /// <summary>
        /// Gets or sets the encoded key.
        /// </summary>
        public EKey EKey { get; set; }

        /// <summary>
        /// Gets or sets the file size.
        /// </summary>
        public ulong FileSize { get; set; }

        /// <summary>
        /// Gets or sets the compressed size.
        /// </summary>
        public ulong CompressedSize { get; set; }

        /// <summary>
        /// Gets or sets the file data ID.
        /// </summary>
        public uint FileDataId { get; set; }

        /// <summary>
        /// Gets or sets the locale flags.
        /// </summary>
        public CascLocaleFlags LocaleFlags { get; set; }

        /// <summary>
        /// Gets or sets the content flags.
        /// </summary>
        public CascContentFlags ContentFlags { get; set; }

        /// <summary>
        /// Gets or sets the tag bitmask.
        /// </summary>
        public ulong TagBitMask { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the file is available locally.
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the name type.
        /// </summary>
        public CascNameType NameType { get; set; }

        /// <summary>
        /// Gets a value indicating whether the file is encrypted.
        /// </summary>
        public bool IsEncrypted => (ContentFlags & CascContentFlags.Encrypted) != 0;

        /// <summary>
        /// Gets a value indicating whether the file is compressed.
        /// </summary>
        public bool IsCompressed => (ContentFlags & CascContentFlags.NoCompression) == 0;

        /// <inheritdoc/>
        public override string ToString()
        {
            return FileName;
        }
    }
}