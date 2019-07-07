// ------------------------------------------------------------------------------
// <copyright file="BlpFile.cs" company="Xalcon @ mmowned.com-Forum">
// Copyright (c) 2011 Xalcon @ mmowned.com-Forum. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

#if !NETSTANDARD1_3
using System.Drawing;
using System.Drawing.Imaging;
#endif

#if !NETSTANDARD
using System.Windows.Media.Imaging;
#endif

namespace War3Net.Drawing.Blp
{
    /// <remarks>
    /// Documentation used for <see cref="FileFormatVersion.BLP1"/>:
    /// https://www.hiveworkshop.com/threads/blp-specifications-wc3.279306/
    /// https://github.com/DrSuperGood/blp-iio-plugin/tree/master/BLP%20IIO%20Plugins/src/com/hiveworkshop/blizzard/blp
    /// https://github.com/DrSuperGood/blp-iio-plugin/blob/master/BLP%20IIO%20Plugins/src/com/hiveworkshop/blizzard/blp/BLPStreamMetadata.java
    /// https://github.com/DrSuperGood/blp-iio-plugin/blob/master/BLP%20IIO%20Plugins/src/com/hiveworkshop/blizzard/blp/BLPReader.java.
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
        private readonly ARGBColor8[] _paletteBGRA = new ARGBColor8[256]; // The color-palette for non-compressed pictures

        private readonly byte[] _jpgHeaderData; // shared header when using mipMaps

        private Stream _baseStream;

        public BlpFile(Stream stream)
        {
            _baseStream = stream;

            using (var reader = new BinaryReader(_baseStream, Encoding.ASCII, true))
            {
                // Checking for correct Magic-Code
                _fileFormatVersion = (FileFormatVersion)reader.ReadUInt32();
                if (!Enum.IsDefined(typeof(FileFormatVersion), _fileFormatVersion))
                {
                    throw new Exception("Invalid BLP Format.");
                }

                if (_fileFormatVersion == FileFormatVersion.BLP0)
                {
                    throw new NotSupportedException("Unable to open BLP0 file.");
                }

                // Reading type
                _formatVersion = (FileContent)reader.ReadUInt32();
                if (!Enum.IsDefined(typeof(FileContent), _formatVersion))
                {
                    throw new Exception("Invalid content type.");
                }

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

                    if (_formatVersion == FileContent.JPG)
                    {
                        if (_alphaDepth != 0 && _alphaDepth != 8)
                        {
                            throw new NotSupportedException();
                        }
                    }
                    else
                    {
                        _colorEncoding = 1;
                    }
                }

                // Reading width and height
                _width = (int)reader.ReadUInt32();
                _height = (int)reader.ReadUInt32();

                if (_fileFormatVersion != FileFormatVersion.BLP2)
                {
                    _extra = reader.ReadUInt32(); // usually 5
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
                    // Reading palette
                    for (var i = 0; i < 256; i++)
                    {
                        var color = reader.ReadInt32();
                        _paletteBGRA[i].blue = (byte)((color >> 0) & 0xFF);
                        _paletteBGRA[i].green = (byte)((color >> 8) & 0xFF);
                        _paletteBGRA[i].red = (byte)((color >> 16) & 0xFF);
                        _paletteBGRA[i].alpha = (byte)((color >> 24) & 0xFF);
                    }
                }
                else if (_fileFormatVersion == FileFormatVersion.BLP1 && _formatVersion == FileContent.JPG)
                {
                    var jpgHeaderSize = reader.ReadUInt32(); // max 624 bytes
                    _jpgHeaderData = new byte[jpgHeaderSize];
                    for (var i = 0; i < jpgHeaderSize; i++)
                    {
                        _jpgHeaderData[i] = reader.ReadByte();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the amount of MipMaps in this <see cref="BlpFile"/>.
        /// </summary>
        public int MipMapCount => _mipMapCount;

        /// <summary>
        /// Converts the BLP to a <see cref="SkiaSharp.SKBitmap"/>.
        /// </summary>
        /// <param name="mipMapLevel">The desired MipMap-Level. If the given level is invalid, the smallest available level is chosen.</param>
        /// <returns></returns>
        public SkiaSharp.SKBitmap GetSKBitmap(int mipMapLevel)
        {
            switch (_formatVersion)
            {
                case FileContent.JPG:
                    var pixelData = GetJpgBitmapBytes(mipMapLevel, out var bitmap);
                    Marshal.Copy(pixelData, 0, bitmap.GetPixels(), pixelData.Length);
                    return bitmap;

                case FileContent.Direct:
                    throw new NotImplementedException();

                default:
                    throw new IndexOutOfRangeException();
            }
        }

#if !NETSTANDARD1_3
        /// <summary>
        /// Converts the BLP to a System.Drawing.Bitmap.
        /// </summary>
        /// <param name="mipMapLevel">The desired MipMap-Level. If the given level is invalid, the smallest available level is chosen.</param>
        /// <returns></returns>
        public Bitmap GetBitmap(int mipMapLevel)
        {
            switch (_formatVersion)
            {
                case FileContent.JPG:
                    var pixelData = GetJpgBitmapBytes(mipMapLevel, out var skBitmap);
                    var bitmap = new Bitmap(skBitmap.Width, skBitmap.Height);

                    var bitmapData = bitmap.LockBits(new Rectangle(0, 0, skBitmap.Width, skBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                    Marshal.Copy(pixelData, 0, bitmapData.Scan0, pixelData.Length);
                    bitmap.UnlockBits(bitmapData);

                    skBitmap.Dispose();

                    return bitmap;

                case FileContent.Direct:
                    var pic = GetPixels(mipMapLevel, out var w, out var h, _colorEncoding == 3 ? false : true);

                    var bmp = new Bitmap(w, h);

                    // Faster bitmap Data copy
                    var bmpdata = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    Marshal.Copy(pic, 0, bmpdata.Scan0, pic.Length); // copy! :D
                    bmp.UnlockBits(bmpdata);

                    return bmp;

                default:
                    throw new IndexOutOfRangeException();
            }
        }
#endif

#if !NETSTANDARD
        public BitmapSource GetBitmapSource(int mipMapLevel)
        {
            switch (_formatVersion)
            {
                case FileContent.JPG:
                    var jpgData = GetJpegFileBytes(mipMapLevel);
                    var decoder = new JpegBitmapDecoder(new MemoryStream(jpgData), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

                    return decoder.Frames[0];

                case FileContent.Direct:
                    throw new NotImplementedException();

                default:
                    throw new IndexOutOfRangeException();
            }
        }
#endif

        /// <summary>
        /// Returns array of pixels in BGRA or RGBA order.
        /// </summary>
        /// <param name="mipMapLevel"></param>
        /// <returns></returns>
        public byte[] GetPixels(int mipMapLevel, out int w, out int h, bool bgra = true)
        {
            if (mipMapLevel >= MipMapCount)
            {
                mipMapLevel = MipMapCount - 1;
            }

            if (mipMapLevel < 0)
            {
                mipMapLevel = 0;
            }

            var scale = (int)Math.Pow(2, mipMapLevel);
            w = _width / scale;
            h = _height / scale;

            var data = GetPictureData(mipMapLevel);
            var pic = GetImageBytes(w, h, data); // This bytearray stores the Pixel-Data

            if (bgra)
            {
                // when we want to copy the pixeldata directly into the bitmap, we have to convert them into BGRA before doing so
                ARGBColor8.ConvertToBGRA(pic);
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
#if NETSTANDARD1_3
                _baseStream.Dispose();
#else
                _baseStream.Close();
#endif
                _baseStream = null;
            }
        }

        private byte[] GetJpegFileBytes(int mipMapLevel)
        {
            var data = GetPixelsPictureData(mipMapLevel);
            var jpgData = new byte[_jpgHeaderData.Length + data.Length];

            Array.Copy(_jpgHeaderData, 0, jpgData, 0, _jpgHeaderData.Length);
            Array.Copy(data, 0, jpgData, _jpgHeaderData.Length, data.Length);

            return jpgData;
        }

        private byte[] GetJpgBitmapBytes(int mipMapLevel, out SkiaSharp.SKBitmap bitmap)
        {
            var jpgData = GetJpegFileBytes(mipMapLevel);

            bitmap = SkiaSharp.SKBitmap.Decode(jpgData);

            var pixelData = new byte[bitmap.ByteCount];
            Marshal.Copy(bitmap.GetPixels(), pixelData, 0, pixelData.Length);

            // Swap red and blue colour channels.
            var bytesPerPixel = bitmap.BytesPerPixel;
            for (var i = 0; i < pixelData.Length; i += bytesPerPixel)
            {
                var tmp = pixelData[i];
                pixelData[i] = pixelData[i + 2];
                pixelData[i + 2] = tmp;
            }

            return pixelData;
        }

        /// <summary>
        /// Extracts the palettized Image-Data from the given MipMap and returns a byte-Array in the 32Bit RGBA-Format
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="data"></param>
        /// <returns>Pixel-data</returns>
        private byte[] GetPictureUncompressedByteArray(int w, int h, byte[] data)
        {
            var length = w * h;
            var pic = new byte[length * 4];
            for (var i = 0; i < length; i++)
            {
                pic[i * 4] = _paletteBGRA[data[i]].red;
                pic[(i * 4) + 1] = _paletteBGRA[data[i]].green;
                pic[(i * 4) + 2] = _paletteBGRA[data[i]].blue;
                pic[(i * 4) + 3] = GetAlpha(data, i, length);
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

        /// <summary>
        /// Returns the raw MipMap-Image Data. This data can either be compressed or uncompressed, depending on the Header-Data
        /// </summary>
        /// <param name="mipMapLevel"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the uncompressed image as a byte array in Format32bppRGBA.
        /// </summary>
        private byte[] GetImageBytes(int w, int h, byte[] data)
        {
            switch (_colorEncoding)
            {
                case 1:
                    return GetPictureUncompressedByteArray(w, h, data);
                case 2:
                    var flag = (_alphaDepth > 1) ? ((_alphaEncoding == 7) ? DxtDecompression.DXTFlags.DXT5 : DxtDecompression.DXTFlags.DXT3) : DxtDecompression.DXTFlags.DXT1;
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