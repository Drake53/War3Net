// ------------------------------------------------------------------------------
// <copyright file="Header.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Drawing.Tga
{
    /// <summary>
    /// Represents TGA header.
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        public Header(BinaryReader reader)
        {
            IDLength = reader.ReadByte();
            ColorMapType = reader.ReadByte();
            ImageType = reader.ReadByte();
            ColorMapStart = reader.ReadUInt16();
            ColorMapLength = reader.ReadUInt16();
            ColorMapDepth = reader.ReadByte();
            XOffset = reader.ReadUInt16();
            YOffset = reader.ReadUInt16();
            Width = reader.ReadUInt16();
            Height = reader.ReadUInt16();
            PixelDepth = reader.ReadByte();
            ImageDescriptor = reader.ReadByte();
        }

        /// <summary>
        /// Gets or sets a length of Image ID field.
        /// </summary>
        public byte IDLength { get; set; }

        /// <summary>
        /// Gets or sets a Color Map type(<see cref="ColorMapTypes"/>).
        /// </summary>
        public byte ColorMapType { get; set; }

        /// <summary>
        /// Gets or sets a Image type(<see cref="ImageTypes"/>).
        /// </summary>
        public byte ImageType { get; set; }

        /// <summary>
        /// Gets or sets an offset of first entry in the palette.
        /// </summary>
        public ushort ColorMapStart { get; set; }

        /// <summary>
        /// Gets or sets an entry count in the palette.
        /// </summary>
        public ushort ColorMapLength { get; set; }

        /// <summary>
        /// Gets or sets a number of bits per pixel in the palette entry.
        /// </summary>
        public byte ColorMapDepth { get; set; }

        /// <summary>
        /// Gets or sets X offset.
        /// </summary>
        public ushort XOffset { get; set; }

        /// <summary>
        /// Gets or sets Y offset.
        /// </summary>
        public ushort YOffset { get; set; }

        /// <summary>
        /// Gets or sets a width of image.
        /// </summary>
        public ushort Width { get; set; }

        /// <summary>
        /// Gets or sets a height of image.
        /// </summary>
        public ushort Height { get; set; }

        /// <summary>
        /// Gets or sets a pixel depth.
        /// </summary>
        public byte PixelDepth { get; set; }

        /// <summary>
        /// Gets or sets an image descriptor.
        /// </summary>
        public byte ImageDescriptor { get; set; }

        /// <summary>
        /// Gets a number of bits of attributes per pixel.
        /// </summary>
        public byte AttributeBits => BitsExtractor.Extract(ImageDescriptor, 0, 4);

        /// <summary>
        /// Gets an image origin.
        /// </summary>
        public byte ImageOrigin => BitsExtractor.Extract(ImageDescriptor, 4, 2);

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>Returns a string that represents the current object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("IDLength       : {0}\r\n", IDLength);
            sb.AppendFormat("ColorMapType   : {0}({1})\r\n", ColorMapType, ColorMapTypes.ToFormattedText(ColorMapType));
            sb.AppendFormat("ImageType      : {0}({1})\r\n", ImageType, ImageTypes.ToFormattedText(ImageType));
            sb.AppendFormat("ColorMapStart  : {0}\r\n", ColorMapStart);
            sb.AppendFormat("ColorMapLength : {0}\r\n", ColorMapLength);
            sb.AppendFormat("ColorMapDepth  : {0}\r\n", ColorMapDepth);
            sb.AppendFormat("XOffset        : {0}\r\n", XOffset);
            sb.AppendFormat("YOffset        : {0}\r\n", YOffset);
            sb.AppendFormat("Width          : {0}\r\n", Width);
            sb.AppendFormat("Height         : {0}\r\n", Height);
            sb.AppendFormat("PixelDepth     : {0}\r\n", PixelDepth);
            sb.AppendFormat(
                            "ImageDescriptor: 0x{0:X02}(attribute bits: {1}, image origin: {2})\r\n",
                            ImageDescriptor,
                            AttributeBits,
                            ImageOriginTypes.ToFormattedText(ImageOrigin));
            return sb.ToString();
        }
    }
}