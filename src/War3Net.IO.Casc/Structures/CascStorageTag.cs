// ------------------------------------------------------------------------------
// <copyright file="CascStorageTag.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Structures
{
    /// <summary>
    /// Represents a storage tag in CASC.
    /// </summary>
    public class CascStorageTag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascStorageTag"/> class.
        /// </summary>
        public CascStorageTag()
        {
            TagName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascStorageTag"/> class.
        /// </summary>
        /// <param name="name">The tag name.</param>
        /// <param name="value">The tag value.</param>
        public CascStorageTag(string name, uint value)
        {
            TagName = name;
            TagValue = value;
        }

        /// <summary>
        /// Gets or sets the tag name.
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Gets the length of the tag name.
        /// </summary>
        public int TagNameLength => TagName?.Length ?? 0;

        /// <summary>
        /// Gets or sets the tag value.
        /// </summary>
        public uint TagValue { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{TagName}={TagValue}";
        }
    }
}