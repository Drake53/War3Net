// ------------------------------------------------------------------------------
// <copyright file="CascStorageProduct.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Structures
{
    /// <summary>
    /// Represents product information for CASC storage.
    /// </summary>
    public class CascStorageProduct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascStorageProduct"/> class.
        /// </summary>
        public CascStorageProduct()
        {
            CodeName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascStorageProduct"/> class.
        /// </summary>
        /// <param name="codeName">The product code name.</param>
        /// <param name="buildNumber">The build number.</param>
        public CascStorageProduct(string codeName, uint buildNumber)
        {
            CodeName = codeName;
            BuildNumber = buildNumber;
        }

        /// <summary>
        /// Gets or sets the code name of the product.
        /// </summary>
        /// <remarks>
        /// Examples: "wowt" = World of Warcraft PTR, "wow" = World of Warcraft, "d3" = Diablo III.
        /// </remarks>
        public string CodeName { get; set; }

        /// <summary>
        /// Gets or sets the build number.
        /// </summary>
        /// <remarks>
        /// If zero, then CascLib didn't recognize the build number.
        /// </remarks>
        public uint BuildNumber { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return BuildNumber > 0 ? $"{CodeName} (Build {BuildNumber})" : CodeName;
        }
    }
}