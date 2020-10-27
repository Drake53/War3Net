// ------------------------------------------------------------------------------
// <copyright file="BlpFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using War3Net.Common.Extensions;

namespace War3Net.Drawing.Blp
{
    /// <summary>
    /// Represents a BLP image file.
    /// </summary>
    /// <remarks>
    /// Documentation used for <see cref="FileFormatVersion.BLP1"/>:
    /// https://www.hiveworkshop.com/threads/blp-specifications-wc3.279306/
    /// https://github.com/DrSuperGood/blp-iio-plugin/tree/master/BLP%20IIO%20Plugins/src/com/hiveworkshop/blizzard/blp
    /// </remarks>
    public sealed class BlpFile : IDisposable
    {
        private readonly FileFormatVersion _fileFormatVersion;
        private readonly FileContent _formatVersion;
        private readonly byte _colorEncoding; // 1 = Uncompressed, 2 = DirectX Compressed
        private readonly uint _alphaDepth; // 0 = no alpha, 1 = 1 Bit, 4 = Bit (only DXT3), 8 = 8 Bit Alpha
        private readonly byte _alphaEncoding; // 0: DXT1 alpha (0 or 1 Bit alpha), 1 = DXT2/3 alpha (4 Bit), 7: DXT4/5 (interpolated alpha)
        private readonly uint _hasMipMaps; // If true (1), then there are MipMaps
        private readonly uint _extra; // ?
        private readonly int _width; // X Resolution of the biggest MipMap
        private readonly int _height; // Y Resolution of the biggest MipMap

        private readonly uint[] _mipMapOffsets; // Offset for every MipMap level. If 0 = no more mitmap level
        private readonly uint[] _mipMapSizes; // Size for every level
        private readonly int _mipMapCount;

        // The color-palette for non-compressed pictures
#if SkiaSharpColorPalette
        private readonly SKColor[] _colorPalette;
#else
        private readonly uint[] _colorPalette;
#endif

        private readonly byte[] _jpgHeaderData; // Shared header when using mipMaps (usually contains markers SOI, DHT, DQT, and a part of SOF0)

        private Stream _baseStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlpFile"/> class.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> that contains the BLP file.</param>
        public BlpFile(Stream stream)
        {
            _baseStream = stream;

            using (var reader = new BinaryReader(_baseStream, Encoding.ASCII, true))
            {
                // Checking for correct Magic-Code
                _fileFormatVersion = reader.ReadInt32<FileFormatVersion>();
                if (_fileFormatVersion == FileFormatVersion.BLP0)
                {
                    throw new NotSupportedException("Unable to open BLP0 file.");
                }

                // Reading type
                _formatVersion = reader.ReadInt32<FileContent>();

                if (_fileFormatVersion == FileFormatVersion.BLP2)
                {
                    // Reading _colorEncoding, _alphaDepth, _alphaEncoding, and _hasMipMaps.
                    _colorEncoding = reader.ReadByte();
                    _alphaDepth = reader.ReadByte();
                    _alphaEncoding = reader.ReadByte();
                    _hasMipMaps = reader.ReadByte();
                }
                else
                {
                    // Should be 0, 1, 4, or 8, and default to 0 if invalid.
                    _alphaDepth = reader.ReadUInt32();

                    if (_formatVersion != FileContent.JPG)
                    {
                        _colorEncoding = 1;
                    }
                }

                // Reading width and height
                _width = reader.ReadInt32();
                _height = reader.ReadInt32();

                if (_fileFormatVersion != FileFormatVersion.BLP2)
                {
                    // http://www.wc3c.net/tools/specs/
                    // flag for alpha channel and team colors (usually 3, 4 or 5)
                    // 3 and 4 means color and alpha information for paletted files
                    // 5 means only color information, if >=5 on 'unit' textures, it won't show the team color.
                    _extra = reader.ReadUInt32();
                    _hasMipMaps = reader.ReadUInt32();
                }

                const int mipMaps = 16;
                _mipMapCount = 0;

                // Reading MipMapOffset Array
                _mipMapOffsets = new uint[mipMaps];
                for (var i = 0; i < mipMaps; i++)
                {
                    _mipMapOffsets[i] = reader.ReadUInt32();

                    if (_mipMapOffsets[i] != 0)
                    {
                        _mipMapCount++;
                    }
                }

                // Reading MipMapSize Array
                _mipMapSizes = new uint[mipMaps];
                for (var i = 0; i < mipMaps; i++)
                {
                    _mipMapSizes[i] = reader.ReadUInt32();
                }

                // When encoding is 1, there is no image compression and we have to read a color palette
                // This palette always exists when the formatVersion is set to FileContent.Direct, even when it's not used.
                if (_colorEncoding == 1)
                {
                    const int paletteSize = 256;
#if SkiaSharpColorPalette
                    _colorPalette = new SKColor[paletteSize];
#else
                    _colorPalette = new uint[paletteSize];
#endif

                    // Reading palette
                    for (var i = 0; i < paletteSize; i++)
                    {
#if SkiaSharpColorPalette
                        _colorPalette[i] = new SKColor(reader.ReadUInt32());
#else
                        _colorPalette[i] = reader.ReadUInt32();
#endif
                    }
                }
                else if (_fileFormatVersion == FileFormatVersion.BLP1 && _formatVersion == FileContent.JPG)
                {
                    var jpgHeaderSize = reader.ReadUInt32(); // max 624 bytes
                    _jpgHeaderData = reader.ReadBytes((int)jpgHeaderSize);
                }
            }
        }

        public int Width => _width;

        public int Height => _height;

        /// <summary>
        /// Gets the amount of MipMaps in this <see cref="BlpFile"/>.
        /// </summary>
        public int MipMapCount => _mipMapCount;

        /// <summary>
        /// Converts the BLP to a <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="mipMapLevel">The desired MipMap-Level. If the given level is invalid, the smallest available level is chosen.</param>
        /// <returns>A new <see cref="BitmapSource"/> instance representing the BLP image.</returns>
        public BitmapSource GetBitmapSource(int mipMapLevel = 0)
        {
            byte[] pixelData;

            switch (_formatVersion)
            {
                case FileContent.JPG:
                    // TODO: test whether or not using SkiaSharp to decode is faster
                    var jpgData = GetJpegFileBytes(mipMapLevel);
                    var decoder = new JpegBitmapDecoder(new MemoryStream(jpgData), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

                    // return decoder.Frames[0];

                    var bitmap = decoder.Frames[0];
                    var bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
                    var stride = bitmap.PixelWidth * bytesPerPixel;

                    pixelData = new byte[stride * bitmap.PixelHeight];
                    bitmap.CopyPixels(pixelData, stride, 0);

                    InvertChannelValues(pixelData);

                    return BitmapSource.Create(bitmap.PixelWidth, bitmap.PixelHeight, bitmap.DpiX, bitmap.DpiY, bitmap.Format, null, pixelData, stride);

                case FileContent.Direct:
                    var bgra = _colorEncoding != 3;

                    pixelData = GetPixelsDirect(mipMapLevel, out var width, out var height, bgra);

                    return BitmapSource.Create(width, height, 96d, 96d, bgra ? PixelFormats.Bgra32 : PixelFormats.Rgb24, null, pixelData, width * 4);

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Returns array of pixels in BGRA or RGBA order.
        /// </summary>
        /// <param name="mipMapLevel">The desired MipMap-Level. If the given level is invalid, the smallest available level is chosen.</param>
        /// <param name="w">Contains the width of the image at the chosen <paramref name="mipMapLevel"/>.</param>
        /// <param name="h">Contains the height of the image at the chosen <paramref name="mipMapLevel"/>.</param>
        /// <param name="bgra">If true, the returned byte array is in BGRA order. Otherwise, it's in RGBA order. Only works for <see cref="FileContent.Direct"/>.</param>
        /// <returns>A byte array representing the BLP image.</returns>
        public byte[] GetPixels(int mipMapLevel, out int w, out int h, bool bgra = true)
        {
            switch (_formatVersion)
            {
                case FileContent.JPG: return GetPixelsJPG(mipMapLevel, out w, out h);
                case FileContent.Direct: return GetPixelsDirect(mipMapLevel, out w, out h, bgra);

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        private byte[] GetPixelsJPG(int mipMapLevel, out int w, out int h)
        {
            var bitmapSource = GetBitmapSource(mipMapLevel);
            w = bitmapSource.PixelWidth;
            h = bitmapSource.PixelHeight;

            var pixels = new byte[4 * w * h];
            bitmapSource.CopyPixels(pixels, 4 * w, 0);
            return pixels;
        }

        private byte[] GetPixelsDirect(int mipMapLevel, out int w, out int h, bool bgra = true)
        {
            var data = GetPixelsPictureData(mipMapLevel);

            var scale = (int)Math.Pow(2, mipMapLevel);
            w = _width / scale;
            h = _height / scale;

            // This byte array stores the Pixel-Data
            var pic = GetImageBytes(w, h, data);

            if (bgra)
            {
                ConvertBetweenRgbAndBgr(pic, 4);
            }

            return pic;
        }

        /// <summary>
        /// Runs <see cref="Close"/>.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Closes the <see cref="_baseStream"/>.
        /// </summary>
        public void Close()
        {
            if (_baseStream != null)
            {
                _baseStream.Close();
                _baseStream = null;
            }
        }

        // Swap red and blue colour channels in a byte array.
        private static void ConvertBetweenRgbAndBgr(byte[] pixelData, int bytesPerPixel)
        {
            for (var i = 0; i < pixelData.Length; i += bytesPerPixel)
            {
                var tmp = pixelData[i];
                pixelData[i] = pixelData[i + 2];
                pixelData[i + 2] = tmp;
            }
        }

        // Assumes colour and alpha channels are all 8bpp.
        private static void InvertChannelValues(byte[] pixelData)
        {
            for (var i = 0; i < pixelData.Length; i++)
            {
                pixelData[i] = (byte)(255 - pixelData[i]);
            }
        }

        // Gets the entire jpg file contents (which should start with a marker of type SOI, and end with a marker of type EOI).
        private byte[] GetJpegFileBytes(int mipMapLevel)
        {
            var data = GetPixelsPictureData(mipMapLevel);
            var jpgData = new byte[_jpgHeaderData.Length + data.Length];

            Array.Copy(_jpgHeaderData, 0, jpgData, 0, _jpgHeaderData.Length);
            Array.Copy(data, 0, jpgData, _jpgHeaderData.Length, data.Length);

            return jpgData;
        }

        // Extracts the palettized Image Data from the given MipMap, and returns a byte array in the 32Bit RGBA-Format.
        private byte[] GetPictureUncompressedByteArray(int w, int h, byte[] data)
        {
            var length = w * h;
            var pic = new byte[length * 4];
            for (var i = 0; i < length; i++)
            {
#if SkiaSharpColorPalette
                pic[(i * 4) + 0] = _colorPalette[data[i]].Red;
                pic[(i * 4) + 1] = _colorPalette[data[i]].Green;
                pic[(i * 4) + 2] = _colorPalette[data[i]].Blue;
                pic[(i * 4) + 3] = GetAlpha(data, i, length);
#else
                var color = _colorPalette[data[i]];
                pic[(i * 4) + 0] = (byte)((color >> 0) & 0xFF);
                pic[(i * 4) + 1] = (byte)((color >> 8) & 0xFF);
                pic[(i * 4) + 2] = (byte)((color >> 16) & 0xFF);
                pic[(i * 4) + 3] = GetAlpha(data, i, length);
#endif
            }

            return pic;
        }

        private byte GetAlpha(byte[] data, int index, int alphaStart)
        {
            switch (_alphaDepth)
            {
                default:
                    return 0xFF;
                case 1:
                    {
                        var b = data[alphaStart + (index / 8)];
                        return (byte)((b & (0x01 << (index % 8))) == 0 ? 0x00 : 0xff);
                    }

                case 4:
                    {
                        var b = data[alphaStart + (index / 2)];
                        return (byte)(index % 2 == 0 ? (b & 0x0F) << 4 : b & 0xF0);
                    }

                case 8:
                    return data[alphaStart + index];
            }
        }

        // Returns the raw MipMap Image Data. This data can either be compressed or uncompressed, depending on the Header Data.
        private byte[] GetPictureData(int mipMapLevel)
        {
            if (_baseStream != null)
            {
                var data = new byte[_mipMapSizes[mipMapLevel]];
                _baseStream.Position = _mipMapOffsets[mipMapLevel];
                _baseStream.Read(data, 0, data.Length);
                return data;
            }

            return null;
        }

        // Returns the uncompressed image as a byte array in the 32Bit RGBA-Format.
        private byte[] GetImageBytes(int w, int h, byte[] data)
        {
            switch (_colorEncoding)
            {
                case 1:
                    return GetPictureUncompressedByteArray(w, h, data);
                case 2:
                    var flag = (_alphaDepth > 1) ? ((_alphaEncoding == 7) ? DxtDecompression.DxtFlags.DXT5 : DxtDecompression.DxtFlags.DXT3) : DxtDecompression.DxtFlags.DXT1;
                    return DxtDecompression.DecompressImage(w, h, data, flag);
                case 3:
                    return data;
                default:
                    return Array.Empty<byte>();
            }
        }

        private byte[] GetPixelsPictureData(int mipMapLevel)
        {
            if (mipMapLevel >= MipMapCount)
            {
                mipMapLevel = MipMapCount - 1;
            }

            if (mipMapLevel < 0)
            {
                mipMapLevel = 0;
            }

            return GetPictureData(mipMapLevel);
        }
    }
}