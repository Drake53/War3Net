// ------------------------------------------------------------------------------
// <copyright file="RootEntry.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Root
{
    /// <summary>
    /// Represents an entry in a CASC root file.
    /// </summary>
    public class RootEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootEntry"/> class.
        /// </summary>
        public RootEntry()
        {
            FileName = string.Empty;
            CKey = CascKey.Empty;
            FileDataId = CascConstants.InvalidId;
        }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the content key.
        /// </summary>
        public CascKey CKey { get; set; }

        /// <summary>
        /// Gets or sets the file data ID.
        /// </summary>
        public uint FileDataId { get; set; }

        /// <summary>
        /// Gets or sets the file name hash.
        /// </summary>
        public ulong FileNameHash { get; set; }

        /// <summary>
        /// Gets or sets the locale flags.
        /// </summary>
        public CascLocaleFlags LocaleFlags { get; set; }

        /// <summary>
        /// Gets or sets the content flags.
        /// </summary>
        public CascContentFlags ContentFlags { get; set; }

        /// <summary>
        /// Gets a value indicating whether this entry has a valid file name.
        /// </summary>
        public bool HasFileName => !string.IsNullOrEmpty(FileName);

        /// <summary>
        /// Gets a value indicating whether this entry has a valid file data ID.
        /// </summary>
        public bool HasFileDataId => FileDataId != CascConstants.InvalidId;

        /// <summary>
        /// Gets a value indicating whether this entry has a valid content key.
        /// </summary>
        public bool HasCKey => !CKey.IsEmpty;

        /// <inheritdoc/>
        public override string ToString()
        {
            if (HasFileName)
            {
                return FileName;
            }

            if (HasFileDataId)
            {
                return $"FileDataId:{FileDataId}";
            }

            return CKey.ToString();
        }
    }
}