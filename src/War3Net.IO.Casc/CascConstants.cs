// ------------------------------------------------------------------------------
// <copyright file="CascConstants.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc
{
    /// <summary>
    /// Contains constant values used throughout the CASC library.
    /// </summary>
    public static class CascConstants
    {
        /// <summary>
        /// Size of MD5 hash in bytes.
        /// </summary>
        public const int MD5HashSize = 0x10;

        /// <summary>
        /// Size of SHA1 hash in bytes.
        /// </summary>
        public const int SHA1HashSize = 0x14;

        /// <summary>
        /// Size of the content key.
        /// </summary>
        public const int CKeySize = 0x10;

        /// <summary>
        /// Size of the encoded key (truncated).
        /// </summary>
        public const int PartialEKeySize = 0x09;

        /// <summary>
        /// Size of the encoded key (full).
        /// </summary>
        public const int EKeySize = 0x10;

        /// <summary>
        /// String length of the hexadecimal string representation of the content key.
        /// </summary>
        public const int CKeyStringLength = CKeySize * 2;

        /// <summary>
        /// String length of the hexadecimal string representation of the encoded key (truncated).
        /// </summary>
        public const int PartialEKeyStringLength = PartialEKeySize * 2;

        /// <summary>
        /// String length of the hexadecimal string representation of the encoded key (full).
        /// </summary>
        public const int EKeyStringLength = EKeySize * 2;

        /// <summary>
        /// Maximum number of data files.
        /// </summary>
        public const int MaxDataFiles = 0x1000;

        /// <summary>
        /// Invalid index value.
        /// </summary>
        public const uint InvalidIndex = 0xFFFFFFFF;

        /// <summary>
        /// Invalid size value.
        /// </summary>
        public const uint InvalidSize = 0xFFFFFFFF;

        /// <summary>
        /// Invalid position value.
        /// </summary>
        public const uint InvalidPos = 0xFFFFFFFF;

        /// <summary>
        /// Invalid ID value.
        /// </summary>
        public const uint InvalidId = 0xFFFFFFFF;

        /// <summary>
        /// Invalid 32-bit offset value.
        /// </summary>
        public const uint InvalidOffset32 = 0xFFFFFFFF;

        /// <summary>
        /// Invalid 64-bit offset value.
        /// </summary>
        public const ulong InvalidOffset64 = 0xFFFFFFFFFFFFFFFF;

        /// <summary>
        /// Invalid 64-bit size value.
        /// </summary>
        public const ulong InvalidSize64 = 0xFFFFFFFFFFFFFFFF;

        /// <summary>
        /// Maximum length of encryption key.
        /// </summary>
        public const int KeyLength = 0x10;

        /// <summary>
        /// Default format string for file ID.
        /// </summary>
        public const string FileIdFormat = "FILE{0:X8}.dat";

        /// <summary>
        /// Separator character for path-product delimiter.
        /// </summary>
        public const char ParamSeparator = '*';

        /// <summary>
        /// Separator character for vfs root directories.
        /// </summary>
        public const char VfsSeparator = ':';

        /// <summary>
        /// Magic number for ENCODING file.
        /// </summary>
        public const ushort FileMagicEncoding = 0x4E45; // 'EN'

        /// <summary>
        /// Magic number for DOWNLOAD file.
        /// </summary>
        public const ushort FileMagicDownload = 0x4C44; // 'DL'

        /// <summary>
        /// Magic number for INSTALL file.
        /// </summary>
        public const ushort FileMagicInstall = 0x494E; // 'IN'

        /// <summary>
        /// Maximum size of an online file.
        /// </summary>
        public const uint MaxOnlineFileSize = 0x40000000;

        /// <summary>
        /// Size of one page in the index file.
        /// </summary>
        public const int FileIndexPageSize = 0x200;
    }
}