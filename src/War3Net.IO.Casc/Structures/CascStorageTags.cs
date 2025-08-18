// ------------------------------------------------------------------------------
// <copyright file="CascStorageTags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.IO.Casc.Structures
{
    /// <summary>
    /// Represents a collection of storage tags in CASC.
    /// </summary>
    public class CascStorageTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascStorageTags"/> class.
        /// </summary>
        public CascStorageTags()
        {
            Tags = new List<CascStorageTag>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascStorageTags"/> class.
        /// </summary>
        /// <param name="tags">The initial tags.</param>
        public CascStorageTags(IEnumerable<CascStorageTag> tags)
        {
            Tags = new List<CascStorageTag>(tags);
        }

        /// <summary>
        /// Gets the collection of tags.
        /// </summary>
        public List<CascStorageTag> Tags { get; }

        /// <summary>
        /// Gets the number of tags.
        /// </summary>
        public int TagCount => Tags.Count;

        /// <summary>
        /// Adds a tag to the collection.
        /// </summary>
        /// <param name="tag">The tag to add.</param>
        public void AddTag(CascStorageTag tag)
        {
            Tags.Add(tag);
        }

        /// <summary>
        /// Adds a tag to the collection.
        /// </summary>
        /// <param name="name">The tag name.</param>
        /// <param name="value">The tag value.</param>
        public void AddTag(string name, uint value)
        {
            Tags.Add(new CascStorageTag(name, value));
        }

        /// <summary>
        /// Gets a tag by name.
        /// </summary>
        /// <param name="name">The tag name.</param>
        /// <returns>The tag, or null if not found.</returns>
        public CascStorageTag? GetTag(string name)
        {
            return Tags.Find(t => t.TagName == name);
        }

        /// <summary>
        /// Gets a tag value by name.
        /// </summary>
        /// <param name="name">The tag name.</param>
        /// <param name="defaultValue">The default value if tag is not found.</param>
        /// <returns>The tag value, or the default value if not found.</returns>
        public uint GetTagValue(string name, uint defaultValue = 0)
        {
            var tag = GetTag(name);
            return tag?.TagValue ?? defaultValue;
        }
    }
}