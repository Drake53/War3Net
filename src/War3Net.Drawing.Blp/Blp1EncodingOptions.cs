// ------------------------------------------------------------------------------
// <copyright file="Blp1EncodingOptions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Drawing.Blp
{
    /// <summary>
    /// Options for encoding a BLP1 file with JPEG compression.
    /// </summary>
    public class Blp1EncodingOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Blp1EncodingOptions"/> class.
        /// </summary>
        public Blp1EncodingOptions()
        {
            GenerateMipmaps = true;
            MipmapLevels = 0;
            JpegQuality = 85;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to generate mipmaps automatically.
        /// </summary>
        public bool GenerateMipmaps { get; set; }

        /// <summary>
        /// Gets or sets the number of mipmap levels to generate (1-16).
        /// Only used if <see cref="GenerateMipmaps"/> is <see langword="true"/>.
        /// Set to 0 to generate all mipmaps.
        /// </summary>
        public int MipmapLevels { get; set; }

        /// <summary>
        /// Gets or sets the JPEG quality (1-100).
        /// </summary>
        public int JpegQuality { get; set; }

        /// <summary>
        /// Gets or sets the extra flags field (team colors/alpha info).
        /// </summary>
        public uint ExtraFlags { get; set; }
    }
}