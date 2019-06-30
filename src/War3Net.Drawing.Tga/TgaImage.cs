using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace TgaLib
{
    /// <summary>
    /// Represents TGA image.
    /// </summary>
    public class TgaImage
    {
        /// <summary>
        /// Use the alpha channel forcefully, if true.
        /// </summary>
        private bool useAlphaChannelForcefully_;


        /// <summary>
        /// Gets or sets a header.
        /// </summary>
        public Header Header { get; set; }

        /// <summary>
        /// Gets or sets an image ID.
        /// </summary>
        public byte[] ImageID { get; set; }

        /// <summary>
        /// Gets or sets a color map(palette).
        /// </summary>
        public byte[] ColorMap { get; set; }

        /// <summary>
        /// Gets or sets an image bytes array.
        /// </summary>
        public byte[] ImageBytes { get; set; }

        /// <summary>
        /// Gets or sets a developer area.
        /// </summary>
        public DeveloperArea DeveloperArea { get; set; }

        /// <summary>
        /// Gets or sets an extension area.
        /// </summary>
        public ExtensionArea ExtensionArea { get; set; }

        /// <summary>
        /// Gets or sets a footer.
        /// </summary>
        public Footer Footer { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        public TgaImage(BinaryReader reader) : this(reader, false)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        /// <param name="useAlphaChannelForcefully">Use the alpha channel forcefully, if true.</param>
        public TgaImage(BinaryReader reader, bool useAlphaChannelForcefully)
        {
            useAlphaChannelForcefully_ = useAlphaChannelForcefully;

            Header = new Header(reader);

            ImageID = new byte[Header.IDLength];
            reader.Read(ImageID, 0, ImageID.Length);

            var bytesPerPixel = GetBytesPerPixel();

            if (GetPixelFormat() == PixelFormat.Format8bppIndexed)
            {
                if (Header.ColorMapLength != 0)
                {
                    throw new NotSupportedException("Potentially non-greyscale 8bpp images are not supported.");
                }

                // Fill colormap with all shades of grey.
                ColorMap = new byte[256 * bytesPerPixel];
                for (var i = 0; i < 256; i++)
                {
                    ColorMap[i] = (byte)i;
                }
            }
            else
            {
                ColorMap = new byte[Header.ColorMapLength * bytesPerPixel];
                reader.Read(ColorMap, 0, ColorMap.Length);
            }

            var position = reader.BaseStream.Position;
            if (Footer.HasFooter(reader))
            {
                Footer = new Footer(reader);

                if (Footer.ExtensionAreaOffset != 0)
                {
                    ExtensionArea = new ExtensionArea(reader, Footer.ExtensionAreaOffset);
                }

                if (Footer.DeveloperDirectoryOffset != 0)
                {
                    DeveloperArea = new DeveloperArea(reader, Footer.DeveloperDirectoryOffset);
                }
            }

            reader.BaseStream.Seek(position, SeekOrigin.Begin);
            ImageBytes = new byte[Header.Width * Header.Height * bytesPerPixel];
            ReadImageBytes(reader);
        }


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
            var stride = bytesPerPixel * width;

            var bitmap = new Bitmap(width, height);
            for (var y = 0; y < height; y++)
            {
                for ( var x = 0; x < width; x++)
                {
                    var offset = (x * bytesPerPixel) + (y * stride);
                    bitmap.SetPixel(x, y, GetColor(pixelFormat, offset, bytesPerPixel));
                }
            }

            return bitmap;
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
                    //return Color.FromArgb(( byte2 & 0x7c ) << 1, ( ( byte2 & 0x03 ) >> 5 ) | ( byte1 & 0xe0 ), ( byte1 & 0x1f ) >> 3);
                    return Color.FromArgb(StretchColorChannel(( byte2 & 0x7c ) << 1, 5, 8), StretchColorChannel(( ( byte2 & 0x03 ) << 6 ) | (( byte1 & 0xe0 ) >> 2), 5, 8), StretchColorChannel(( byte1 & 0x1f ) << 3, 5, 8));
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

        private static int StretchColorChannel(int value, int from, int to)
        {
            if (from != 5 || to != 8)
                throw new NotImplementedException();

            return value | ( value >> from );
        }

        /// <summary>
        /// Gets a bitmap image.
        /// </summary>
        /// <returns>Returns a bitmap image.</returns>
        /*public BitmapSource GetBitmapSource()
        {
            int width = Header.Width;
            int height = Header.Height;
            var dpi = 96d;
            var pixelFormat = GetPixelFormat();
            var stride = GetBytesPerPixel() * width;
            var source = BitmapSource.Create(width, height, dpi, dpi, pixelFormat, null, ImageBytes, stride);
            source.Freeze();

            var transformedSource = Transform(source);

            return transformedSource;
        }*/

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
                                return PixelFormat.Format16bppRgb555;
                                // return PixelFormat.Format16bppRgb565;

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
                                return PixelFormat.Format16bppRgb555;
                                // return PixelFormat.Format16bppRgb565;

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
                                // throw new NotSupportedException();
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
            //return ( pixelFormat.BitsPerPixel + 7 ) / 8;
            return GetBytesPerPixel(pixelFormat);
        }

        private static int GetBytesPerPixel(PixelFormat pixelFormat)
        {
            return ( GetBitsPerPixel(pixelFormat) + 7 ) / 8;
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

            int numberOfPixels = Header.Width * Header.Height;

            for (int i = 0; i < numberOfPixels; ++i)
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
            int repetitionCount = 0;
            for (int processedPixels = 0; processedPixels < numberOfPixels; processedPixels += repetitionCount)
            {
                var repetitionCountField = reader.ReadByte();
                bool isRunLengthPacket = ((repetitionCountField & RunLengthPacketMask) != 0x00);
                repetitionCount = (repetitionCountField & RepetitionCountMask) + 1;

                if (isRunLengthPacket)
                {
                    // Run-length packet
                    var pixelData = ExtractPixelData(reader.ReadBytes(bytesPerPixel));
                    // Repeats same pixel data
                    for (int i = 0; i < repetitionCount; ++i)
                    {
                        Array.Copy(pixelData, 0, ImageBytes, (processedPixels + i) * pixelData.Length, pixelData.Length);
                    }
                }
                else
                {
                    // Raw packet
                    // Repeats different pixel data
                    for (int i = 0; i < repetitionCount; ++i)
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
            byte[] pixelData = null;

            switch (Header.ImageType)
            {
                case ImageTypes.ColorMapped:
                case ImageTypes.CompressedColorMapped:
                    {
                        // Extracts a pixel data in the palette.
                        var paletteIndex = GetPaletteIndex(rawPixelData);
                        var bytesPerPixel = GetBytesPerPixel();
                        var realPixelData = new byte[bytesPerPixel];
                        Array.Copy(ColorMap,
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

            //if (!HasAlpha() && !useAlphaChannelForcefully_ && (GetPixelFormat() == PixelFormats.Bgra32))
            if (!HasAlpha() && !useAlphaChannelForcefully_ && ( GetBytesPerPixel(GetPixelFormat()) >= 4 ))
            {
                pixelData[ArgbOffset.Alpha] = 0xFF;
            }

            return pixelData;
        }

        /// <summary>
        /// Gets a palette index.
        /// </summary>
        /// <param name="indexData">An index data.</param>
        /// <returns>Returns a palette index.</returns>
        private long GetPaletteIndex(byte[] indexData)
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

        /// <summary>
        /// Gets whether has an alpha value or not.
        /// </summary>
        /// <returns>
        /// Returns true, if TGA image has an alpha value.
        /// Returns false, if TGA image don't have an alpha value.
        /// </returns>
        private bool HasAlpha()
        {
            bool hasAlpha = (Header.AttributeBits == 8) || (GetBytesPerPixel(GetPixelFormat()) >= 4 );

            if (ExtensionArea != null)
            {
                hasAlpha = (ExtensionArea.AttributesType == AttributeTypes.HasAlpha) ||
                           (ExtensionArea.AttributesType == AttributeTypes.HasPreMultipliedAlpha);
            }

            return hasAlpha;
        }

        /// <summary>
        /// Transforms an image according to <see cref="Header.ImageOrigin"/>.
        /// </summary>
        /// <param name="source">A source image.</param>
        /// <returns>Returns a transformed image.</returns>
        /*private BitmapSource Transform(BitmapSource source)
        {
            double scaleX = 1.0;
            double scaleY = 1.0;

            switch (Header.ImageOrigin)
            {
                case ImageOriginTypes.BottomLeft:
                    scaleX = 1.0;
                    scaleY = -1.0;
                    break;

                case ImageOriginTypes.BottomRight:
                    scaleX = -1.0;
                    scaleY = -1.0;
                    break;

                case ImageOriginTypes.TopLeft:
                    scaleX = 1.0;
                    scaleY = 1.0;
                    break;

                case ImageOriginTypes.TopRight:
                    scaleX = -1.0;
                    scaleY = 1.0;
                    break;

                default:
                    throw new NotSupportedException(string.Format("Image origin \"{0}\" isn't supported.", Header.ImageOrigin));
            }

            var transform = new ScaleTransform(scaleX, scaleY, 0.5, 0.5);
            return new TransformedBitmap(source, transform);
        }*/
    }
}