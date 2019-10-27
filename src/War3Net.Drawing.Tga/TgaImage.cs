// ------------------------------------------------------------------------------
// <copyright file="TgaImage.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace War3Net.Drawing.Tga
{
    /// <summary>
    /// Represents a TGA image.
    /// </summary>
    public class TgaImage
    {
        private readonly bool _useAlphaChannelForcefully;
        private readonly Header _header;
        private readonly byte[] _imageID;
        private readonly byte[] _colorMap;
        private readonly byte[] _imageBytes;
        private readonly DeveloperArea _developerArea;
        private readonly ExtensionArea _extensionArea;
        private readonly Footer _footer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TgaImage"/> class.
        /// </summary>
        /// <param name="baseStream">A <see cref="Stream"/> that contains TGA file.</param>
        /// <param name="useAlphaChannelForcefully">Use the alpha channel forcefully, if true.</param>
        public TgaImage(Stream baseStream, bool useAlphaChannelForcefully = false)
        {
            _ = baseStream ?? throw new ArgumentNullException(nameof(baseStream));

            using (var reader = new BinaryReader(baseStream))
            {
                _useAlphaChannelForcefully = useAlphaChannelForcefully;

                _header = new Header(reader);

                _imageID = new byte[Header.IDLength];
                reader.Read(ImageID, 0, ImageID.Length);

                var bytesPerPixel = GetBytesPerPixel();

                if (GetPixelFormat() == PixelFormat.Format8bppIndexed)
                {
                    if (Header.ColorMapLength != 0)
                    {
                        throw new NotSupportedException("Potentially non-greyscale 8bpp images are not supported.");
                    }

                    // Fill colormap with all shades of grey.
                    _colorMap = new byte[256 * bytesPerPixel];
                    for (var i = 0; i < 256; i++)
                    {
                        ColorMap[i] = (byte)i;
                    }
                }
                else
                {
                    _colorMap = new byte[Header.ColorMapLength * bytesPerPixel];
                    reader.Read(ColorMap, 0, ColorMap.Length);
                }

                var position = reader.BaseStream.Position;
                if (Footer.HasFooter(reader))
                {
                    _footer = new Footer(reader);

                    if (Footer.ExtensionAreaOffset != 0)
                    {
                        _extensionArea = new ExtensionArea(reader, Footer.ExtensionAreaOffset);
                    }

                    if (Footer.DeveloperDirectoryOffset != 0)
                    {
                        _developerArea = new DeveloperArea(reader, Footer.DeveloperDirectoryOffset);
                    }
                }

                reader.BaseStream.Seek(position, SeekOrigin.Begin);
                _imageBytes = new byte[Header.Width * Header.Height * bytesPerPixel];
                ReadImageBytes(reader);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the alpha channel is forcefully used.
        /// </summary>
        public bool UseAlphaChannelForcefully => _useAlphaChannelForcefully;

        /// <summary>
        /// Gets a <see cref="Tga.Header"/>.
        /// </summary>
        public Header Header => _header;

        /// <summary>
        /// Gets an image ID.
        /// </summary>
        public byte[] ImageID => _imageID;

        /// <summary>
        /// Gets a color map (palette).
        /// </summary>
        public byte[] ColorMap => _colorMap;

        /// <summary>
        /// Gets an image bytes array.
        /// </summary>
        public byte[] ImageBytes => _imageBytes;

        /// <summary>
        /// Gets a developer area.
        /// </summary>
        public DeveloperArea DeveloperArea => _developerArea;

        /// <summary>
        /// Gets an <see cref="Tga.ExtensionArea"/>.
        /// </summary>
        public ExtensionArea ExtensionArea => _extensionArea;

        /// <summary>
        /// Gets a <see cref="Tga.Footer"/>.
        /// </summary>
        public Footer Footer => _footer;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>Returns a string that represents the current object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("[Header]");
            sb.AppendFormat("{0}", Header);
            sb.AppendLine();

            sb.AppendLine("[Extension Area]");
            if (Footer != null)
            {
                sb.AppendFormat("{0}", ExtensionArea);
            }
            else
            {
                sb.AppendLine("No extension area");
            }

            sb.AppendLine();

            sb.AppendLine("[Footer]");
            if (Footer != null)
            {
                sb.AppendFormat("{0}", Footer);
            }
            else
            {
                sb.AppendLine("No footer");
            }

            return sb.ToString();
        }

        public Bitmap GetBitmap()
        {
            int width = Header.Width;
            int height = Header.Height;
            var pixelFormat = GetPixelFormat();
            var bytesPerPixel = GetBytesPerPixel();
            var offset = 0;

            var leftToRight = Header.ImageOrigin == ImageOriginTypes.TopLeft || Header.ImageOrigin == ImageOriginTypes.BottomLeft;
            var topToBottom = Header.ImageOrigin == ImageOriginTypes.TopLeft || Header.ImageOrigin == ImageOriginTypes.TopRight;

            var xinc = leftToRight ? 1 : -1;
            var yinc = topToBottom ? 1 : -1;
            var xstart = leftToRight ? 0 : width - 1;
            var ystart = topToBottom ? 0 : height - 1;

            var bitmap = new Bitmap(width, height);
            for (var y = ystart; y >= 0 && y < height; y += yinc)
            {
                for (var x = xstart; x >= 0 && x < width; x += xinc)
                {
                    bitmap.SetPixel(x, y, GetColor(pixelFormat, offset, bytesPerPixel));
                    offset += bytesPerPixel;
                }
            }

            return bitmap;
        }

        private static int StretchColorChannel(int value, int from, int to)
        {
            if (from != 5 || to != 8)
            {
                throw new NotImplementedException();
            }

            return value | (value >> from);
        }

        private static int GetBytesPerPixel(PixelFormat pixelFormat)
        {
            return (GetBitsPerPixel(pixelFormat) + 7) / 8;
        }

        private static int GetBitsPerPixel(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    return 8;
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    return 16;
                case PixelFormat.Format24bppRgb:
                    return 24;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets a palette index.
        /// </summary>
        /// <param name="indexData">An index data.</param>
        /// <returns>Returns a palette index.</returns>
        private static long GetPaletteIndex(byte[] indexData)
        {
            switch (indexData.Length)
            {
                case 1:
                    return indexData[0];

                case 2:
                    return BitConverter.ToUInt16(indexData, 0);

                case 4:
                    return BitConverter.ToUInt32(indexData, 0);

                default:
                    throw new NotSupportedException(string.Format("A byte length of index data is not supported({0}bytes).", indexData.Length));
            }
        }

        private Color GetColor(PixelFormat pixelFormat, int offset, int bytesPerPixel)
        {
            var byte1 = bytesPerPixel >= 1 ? ImageBytes[offset + 0] : -1;
            var byte2 = bytesPerPixel >= 2 ? ImageBytes[offset + 1] : -1;
            var byte3 = bytesPerPixel >= 3 ? ImageBytes[offset + 2] : -1;
            var byte4 = bytesPerPixel >= 4 ? ImageBytes[offset + 3] : -1;

            switch (pixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    // Assumes that the image is greyscale.
                    return Color.FromArgb(byte1, byte1, byte1);
                case PixelFormat.Format16bppRgb555:
                    return Color.FromArgb(
                        StretchColorChannel((byte2 & 0x7c) << 1, 5, 8),
                        StretchColorChannel(((byte2 & 0x03) << 6) | ((byte1 & 0xe0) >> 2), 5, 8),
                        StretchColorChannel((byte1 & 0x1f) << 3, 5, 8));
                case PixelFormat.Format16bppRgb565:
                    throw new NotSupportedException();
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppRgb:
                    return Color.FromArgb(byte3, byte2, byte1);
                case PixelFormat.Format32bppArgb:
                    return Color.FromArgb(byte4, byte3, byte2, byte1);

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a pixel format of TGA image.
        /// </summary>
        /// <returns>Returns a pixel format of TGA image.</returns>
        private PixelFormat GetPixelFormat()
        {
            switch (Header.ImageType)
            {
                case ImageTypes.ColorMapped:
                case ImageTypes.CompressedColorMapped:
                    {
                        // color depth of color-mapped image is defined in the palette
                        switch (Header.ColorMapDepth)
                        {
                            case ColorDepth.Bpp15:
                                return PixelFormat.Format16bppRgb555;
                            case ColorDepth.Bpp16:
                                // return PixelFormats.Bgr555;
                                // return PixelFormat.Format16bppRgb565;
                                return PixelFormat.Format16bppRgb555;

                            case ColorDepth.Bpp24:
                                // return PixelFormats.Bgr24;
                                return PixelFormat.Format24bppRgb;

                            case ColorDepth.Bpp32:
                                // return PixelFormats.Bgra32;
                                // todo: verify, since PixelFormatConverter doesn't have corresponding format for Bgra32
                                return PixelFormat.Format32bppArgb;

                            default:
                                throw new NotSupportedException(string.Format("Color depth isn't supported({0}bpp).", Header.ColorMapDepth));
                        }
                    }

                case ImageTypes.TrueColor:
                case ImageTypes.CompressedTrueColor:
                    {
                        switch (Header.PixelDepth)
                        {
                            case ColorDepth.Bpp15:
                                return PixelFormat.Format16bppRgb555;
                            case ColorDepth.Bpp16:
                                // return PixelFormats.Bgr555;
                                // return PixelFormat.Format16bppRgb565;
                                return PixelFormat.Format16bppRgb555;

                            case ColorDepth.Bpp24:
                                // return PixelFormats.Bgr24;
                                return PixelFormat.Format24bppRgb;

                            case ColorDepth.Bpp32:
                                // return PixelFormats.Bgra32;
                                // todo: verify, since PixelFormatConverter doesn't have corresponding format for Bgra32
                                return PixelFormat.Format32bppArgb;

                            default:
                                throw new NotSupportedException(string.Format("Color depth isn't supported({0}bpp).", Header.PixelDepth));
                        }
                    }

                case ImageTypes.Monochrome:
                case ImageTypes.CompressedMonochrome:
                    {
                        switch (Header.PixelDepth)
                        {
                            case ColorDepth.Bpp8:
                                // return PixelFormats.Gray8;
                                return PixelFormat.Format8bppIndexed;

                            default:
                                throw new NotSupportedException(string.Format("Color depth isn't supported({0}bpp).", Header.PixelDepth));
                        }
                    }

                default:
                    throw new NotSupportedException(
                        string.Format("Image type \"{0}({1})\" isn't supported.", Header.ImageType, ImageTypes.ToFormattedText(Header.ImageType)));
            }
        }

        /// <summary>
        /// Gets bytes per pixel.
        /// </summary>
        /// <returns>Returns bytes per pixel.</returns>
        private int GetBytesPerPixel()
        {
            var pixelFormat = GetPixelFormat();
            return GetBytesPerPixel(pixelFormat);
        }

        /// <summary>
        /// Read an image data.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        private void ReadImageBytes(BinaryReader reader)
        {
            switch (Header.ImageType)
            {
                case ImageTypes.ColorMapped:
                case ImageTypes.TrueColor:
                case ImageTypes.Monochrome:
                    ReadUncompressedData(reader);
                    break;

                case ImageTypes.CompressedColorMapped:
                case ImageTypes.CompressedTrueColor:
                case ImageTypes.CompressedMonochrome:
                    DecodeRunLengthEncoding(reader);
                    break;

                default:
                    throw new NotSupportedException(
                        string.Format("Image type \"{0}({1})\" isn't supported.", Header.ImageType, ImageTypes.ToFormattedText(Header.ImageType)));
            }
        }

        /// <summary>
        /// Reads an uncompressed image data.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        private void ReadUncompressedData(BinaryReader reader)
        {
            // Use a pixel depth, not a color depth. So don't use GetBytesPerPixel().
            // (Pixel data is an index data, if an image type is color-mapped.)
            var bytesPerPixel = (Header.PixelDepth + 7) / 8;

            var numberOfPixels = Header.Width * Header.Height;

            for (var i = 0; i < numberOfPixels; ++i)
            {
                var pixelData = ExtractPixelData(reader.ReadBytes(bytesPerPixel));
                Array.Copy(pixelData, 0, ImageBytes, i * pixelData.Length, pixelData.Length);
            }
        }

        /// <summary>
        /// Decode a run-length encoded data.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        private void DecodeRunLengthEncoding(BinaryReader reader)
        {
            // most significant bit of repetitionCountField deetermins whether run-length packet or raw packet.
            const byte RunLengthPacketMask = 0x80;

            // rest of repetitionCountField represents number of pixels encoded by the packet - 1
            // (actual nmber of pixels encoded by the packet is repetitionCountField + 1)
            const byte RepetitionCountMask = 0x7F;

            // Use a pixel depth, not a color depth. So don't use GetBytesPerPixel().
            // (Pixel data is an index data, if an image type is color-mapped.)
            var bytesPerPixel = (Header.PixelDepth + 7) / 8;

            var numberOfPixels = Header.Width * Header.Height;
            int repetitionCount;
            for (var processedPixels = 0; processedPixels < numberOfPixels; processedPixels += repetitionCount)
            {
                var repetitionCountField = reader.ReadByte();
                var isRunLengthPacket = (repetitionCountField & RunLengthPacketMask) != 0x00;
                repetitionCount = (repetitionCountField & RepetitionCountMask) + 1;

                if (isRunLengthPacket)
                {
                    // Run-length packet
                    var pixelData = ExtractPixelData(reader.ReadBytes(bytesPerPixel));

                    // Repeats same pixel data
                    for (var i = 0; i < repetitionCount; ++i)
                    {
                        Array.Copy(pixelData, 0, ImageBytes, (processedPixels + i) * pixelData.Length, pixelData.Length);
                    }
                }
                else
                {
                    // Raw packet
                    // Repeats different pixel data
                    for (var i = 0; i < repetitionCount; ++i)
                    {
                        var pixelData = ExtractPixelData(reader.ReadBytes(bytesPerPixel));
                        Array.Copy(pixelData, 0, ImageBytes, (processedPixels + i) * pixelData.Length, pixelData.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Extracts a pixel data.
        /// </summary>
        /// <param name="rawPixelData">A raw pixel data in the TGA file.</param>
        /// <returns>
        /// Returns a pixel data in the palette, if an image type is color-mapped.
        /// Returns a raw pixel data, if an image type is RGB or grayscale.
        /// </returns>
        private byte[] ExtractPixelData(byte[] rawPixelData)
        {
            byte[] pixelData;

            switch (Header.ImageType)
            {
                case ImageTypes.ColorMapped:
                case ImageTypes.CompressedColorMapped:
                    {
                        // Extracts a pixel data in the palette.
                        var paletteIndex = GetPaletteIndex(rawPixelData);
                        var bytesPerPixel = GetBytesPerPixel();
                        var realPixelData = new byte[bytesPerPixel];
                        Array.Copy(
                                   ColorMap,
                                   (Header.ColorMapStart + paletteIndex) * bytesPerPixel,
                                   realPixelData,
                                   0,
                                   realPixelData.Length);
                        pixelData = realPixelData;
                    }

                    break;

                case ImageTypes.TrueColor:
                case ImageTypes.Monochrome:
                case ImageTypes.CompressedTrueColor:
                case ImageTypes.CompressedMonochrome:
                    // Returns a raw pixel data as is.
                    pixelData = rawPixelData;
                    break;

                default:
                    throw new NotSupportedException(
                        string.Format("Image type \"{0}({1})\" isn't supported.", Header.ImageType, ImageTypes.ToFormattedText(Header.ImageType)));
            }

            if (!HasAlpha() && !_useAlphaChannelForcefully && (GetBytesPerPixel(GetPixelFormat()) >= 4))
            {
                pixelData[ArgbOffset.Alpha] = 0xFF;
            }

            return pixelData;
        }

        /// <summary>
        /// Gets whether has an alpha value or not.
        /// </summary>
        /// <returns>
        /// Returns true, if TGA image has an alpha value.
        /// Returns false, if TGA image don't have an alpha value.
        /// </returns>
        private bool HasAlpha()
        {
            var hasAlpha = (Header.AttributeBits == 8) || (GetBytesPerPixel(GetPixelFormat()) >= 4);

            if (ExtensionArea != null)
            {
                hasAlpha = (ExtensionArea.AttributesType == AttributeTypes.HasAlpha) ||
                           (ExtensionArea.AttributesType == AttributeTypes.HasPreMultipliedAlpha);
            }

            return hasAlpha;
        }
    }
}