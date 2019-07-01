/*
 * Copyright (c) <2011> <by Xalcon @ mmowned.com-Forum>
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace War3Net.Drawing.Blp
{
    public sealed class BlpFile : IDisposable
    {
        private readonly FileFormatVersion _fileFormatVersion;
        private readonly FileContent _formatVersion;
        private readonly byte colorEncoding; // 1 = Uncompressed, 2 = DirectX Compressed
        private readonly uint alphaDepth; // 0 = no alpha, 1 = 1 Bit, 4 = Bit (only DXT3), 8 = 8 Bit Alpha
        private readonly byte alphaEncoding; // 0: DXT1 alpha (0 or 1 Bit alpha), 1 = DXT2/3 alpha (4 Bit), 7: DXT4/5 (interpolated alpha)
        private readonly uint hasMipmaps; // If true (1), then there are Mipmaps
        private readonly uint extra; // ?
        private readonly int width; // X Resolution of the biggest Mipmap
        private readonly int height; // Y Resolution of the biggest Mipmap

        private readonly uint[] mipmapOffsets; // Offset for every Mipmap level. If 0 = no more mitmap level
        private readonly uint[] mipMapSizes; // Size for every level
        private readonly ARGBColor8[] paletteBGRA = new ARGBColor8[256]; // The color-palette for non-compressed pictures

        private readonly byte[] jpgHeaderData; // shared header when using mipmaps

        private Stream baseStream;

        public BlpFile(Stream stream)
        {
            // https://www.hiveworkshop.com/threads/blp-specifications-wc3.279306/
            // https://github.com/DrSuperGood/blp-iio-plugin/tree/master/BLP%20IIO%20Plugins/src/com/hiveworkshop/blizzard/blp
            // https://github.com/DrSuperGood/blp-iio-plugin/blob/master/BLP%20IIO%20Plugins/src/com/hiveworkshop/blizzard/blp/BLPStreamMetadata.java
            // https://github.com/DrSuperGood/blp-iio-plugin/blob/master/BLP%20IIO%20Plugins/src/com/hiveworkshop/blizzard/blp/BLPReader.java

            baseStream = stream;

            using (var reader = new BinaryReader(baseStream, Encoding.ASCII, true))
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
                    // Reading encoding, alphaBitDepth, alphaEncoding and hasMipmaps
                    colorEncoding = reader.ReadByte();
                    alphaDepth = reader.ReadByte();
                    alphaEncoding = reader.ReadByte();
                    hasMipmaps = reader.ReadByte();
                }
                else
                {
                    // Should be 0, 1, 4, or 8, and default to 0 if invalid.
                    alphaDepth = reader.ReadUInt32();
                }

                // Reading width and height
                width = (int)reader.ReadUInt32();
                height = (int)reader.ReadUInt32();

                if (_fileFormatVersion != FileFormatVersion.BLP2)
                {
                    extra = reader.ReadUInt32(); // usually 5
                    hasMipmaps = reader.ReadUInt32();
                }

                const int mipmaps = 16;

                // Reading MipmapOffset Array
                mipmapOffsets = new uint[mipmaps];
                for (var i = 0; i < mipmaps; i++)
                {
                    mipmapOffsets[i] = reader.ReadUInt32();
                }

                // Reading MipmapSize Array
                mipMapSizes = new uint[mipmaps];
                for (var i = 0; i < mipmaps; i++)
                {
                    mipMapSizes[i] = reader.ReadUInt32();
                }

                // When encoding is 1, there is no image compression and we have to read a color palette
                // This palette always exists when the formatVersion is set to FileContent.Direct, even when it's not used.
                if (( _fileFormatVersion == FileFormatVersion.BLP2 && colorEncoding == 1 )
                    || ( _fileFormatVersion != FileFormatVersion.BLP2 && _formatVersion == FileContent.Direct ))
                {
                    // Reading palette
                    for (var i = 0; i < 256; i++)
                    {
                        var color = reader.ReadInt32();
                        paletteBGRA[i].blue = (byte)( ( color >> 0 ) & 0xFF );
                        paletteBGRA[i].green = (byte)( ( color >> 8 ) & 0xFF );
                        paletteBGRA[i].red = (byte)( ( color >> 16 ) & 0xFF );
                        paletteBGRA[i].alpha = (byte)( ( color >> 24 ) & 0xFF );
                    }
                }
                else if (_fileFormatVersion == FileFormatVersion.BLP1 && _formatVersion == FileContent.JPG)
                {
                    var jpgHeaderSize = reader.ReadUInt32(); // max 624 bytes
                    jpgHeaderData = new byte[jpgHeaderSize];
                    for (var i = 0; i < jpgHeaderSize; i++)
                    {
                        jpgHeaderData[i] = reader.ReadByte();
                    }
                }
            }
        }

        /// <summary>
        /// Converts the BLP to a System.Drawing.Bitmap
        /// </summary>
        /// <param name="mipmapLevel">The desired Mipmap-Level. If the given level is invalid, the smallest available level is chosen.</param>
        /// <returns>The Bitmap</returns>
        public Bitmap GetBitmap(int mipmapLevel)
        {
            switch (_formatVersion)
            {
                case FileContent.JPG:
                    var data = GetPixelsPictureData( mipmapLevel );
                    var jpgData = new byte[jpgHeaderData.Length + data.Length];
                    Array.Copy(jpgHeaderData, 0, jpgData, 0, jpgHeaderData.Length);
                    Array.Copy(data, 0, jpgData, jpgHeaderData.Length, data.Length);
                    var stream = new MemoryStream( jpgData );
                    //return Image.FromStream( stream, false, true ) as Bitmap;

                    /*var decoder = new System.Windows.Media.Imaging.JpegBitmapDecoder( stream, System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, System.Windows.Media.Imaging.BitmapCacheOption.Default );
                    var bitmapSource = decoder.Frames[0];
                    var bitmap = bitmapSource.ToBitmap( PixelFormat.Format32bppRgb );*/

                    var bitmap = new Bitmap(Image.FromStream(stream));

                    // Colors are inverted for some reason, so fix that.
                    for (var y = 0; y < bitmap.Height; y++)
                    {
                        for (var x = 0; x < bitmap.Width; x++)
                        {
                            var col = bitmap.GetPixel( x, y );
                            bitmap.SetPixel(x, y, Color.FromArgb(255 - col.R, 255 - col.G, 255 - col.B));
                        }
                    }

                    return bitmap;

                case FileContent.Direct:
                    var pic = GetPixels( mipmapLevel, out var w, out var h, colorEncoding == 3 ? false : true );

                    var bmp = new Bitmap( w, h );

                    // Faster bitmap Data copy
                    var bmpdata = bmp.LockBits( new Rectangle( 0, 0, w, h ), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb );
                    Marshal.Copy(pic, 0, bmpdata.Scan0, pic.Length); // copy! :D
                    bmp.UnlockBits(bmpdata);

                    return bmp;

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Extracts the palettized Image-Data from the given Mipmap and returns a byte-Array in the 32Bit RGBA-Format
        /// </summary>
        /// <param name="mipmap">The desired Mipmap-Level. If the given level is invalid, the smallest available level is choosen</param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="data"></param>
        /// <returns>Pixel-data</returns>
        private byte[] GetPictureUncompressedByteArray(int w, int h, byte[] data)
        {
            int length = w * h;
            byte[] pic = new byte[length * 4];
            for (int i = 0; i < length; i++)
            {
                pic[i * 4] = paletteBGRA[data[i]].red;
                pic[i * 4 + 1] = paletteBGRA[data[i]].green;
                pic[i * 4 + 2] = paletteBGRA[data[i]].blue;
                pic[i * 4 + 3] = GetAlpha(data, i, length);
            }
            return pic;
        }

        private byte GetAlpha(byte[] data, int index, int alphaStart)
        {
            switch (alphaDepth)
            {
                default:
                    return 0xFF;
                case 1:
                    {
                        byte b = data[alphaStart + ( index / 8 )];
                        return (byte)( ( b & ( 0x01 << ( index % 8 ) ) ) == 0 ? 0x00 : 0xff );
                    }
                case 4:
                    {
                        byte b = data[alphaStart + ( index / 2 )];
                        return (byte)( index % 2 == 0 ? ( b & 0x0F ) << 4 : b & 0xF0 );
                    }
                case 8:
                    return data[alphaStart + index];
            }
        }

        /// <summary>
        /// Returns the raw Mipmap-Image Data. This data can either be compressed or uncompressed, depending on the Header-Data
        /// </summary>
        /// <param name="mipmapLevel"></param>
        /// <returns></returns>
        private byte[] GetPictureData(int mipmapLevel)
        {
            if (baseStream != null)
            {
                byte[] data = new byte[mipMapSizes[mipmapLevel]];
                baseStream.Position = mipmapOffsets[mipmapLevel];
                baseStream.Read(data, 0, data.Length);
                return data;
            }
            return null;
        }

        /// <summary>
        /// Returns the amount of Mipmaps in this BLP-File
        /// </summary>
        public int MipMapCount
        {
            get
            {
                int i = 0;
                while (mipmapOffsets[i] != 0)
                    i++;
                return i;
            }
        }

        /// <summary>
        /// Returns the uncompressed image as a bytarray in the 32pppRGBA-Format
        /// </summary>
        private byte[] GetImageBytes(int w, int h, byte[] data)
        {
            switch (colorEncoding)
            {
                case 1:
                    return GetPictureUncompressedByteArray(w, h, data);
                case 2:
                    DXTDecompression.DXTFlags flag = ( alphaDepth > 1 ) ? ( ( alphaEncoding == 7 ) ? DXTDecompression.DXTFlags.DXT5 : DXTDecompression.DXTFlags.DXT3 ) : DXTDecompression.DXTFlags.DXT1;
                    return DXTDecompression.DecompressImage(w, h, data, flag);
                case 3:
                    return data;
                default:
                    return new byte[0];
            }
        }

        /// <summary>
        /// Returns array of pixels in BGRA or RGBA order
        /// </summary>
        /// <param name="mipmapLevel"></param>
        /// <returns></returns>
        public byte[] GetPixels(int mipmapLevel, out int w, out int h, bool bgra = true)
        {
            if (mipmapLevel >= MipMapCount)
                mipmapLevel = MipMapCount - 1;
            if (mipmapLevel < 0)
                mipmapLevel = 0;

            int scale = (int)Math.Pow( 2, mipmapLevel );
            w = width / scale;
            h = height / scale;

            byte[] data = GetPictureData( mipmapLevel );
            byte[] pic = GetImageBytes( w, h, data ); // This bytearray stores the Pixel-Data

            if (bgra)
            {
                // when we want to copy the pixeldata directly into the bitmap, we have to convert them into BGRA before doing so
                ARGBColor8.ConvertToBGRA(pic);
            }

            return pic;
        }

        private byte[] GetPixelsPictureData(int mipmapLevel)
        {
            if (mipmapLevel >= MipMapCount)
                mipmapLevel = MipMapCount - 1;
            if (mipmapLevel < 0)
                mipmapLevel = 0;

            return GetPictureData(mipmapLevel);
        }

        /// <summary>
        /// Runs close()
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Closes the Memorystream
        /// </summary>
        public void Close()
        {
            if (baseStream != null)
            {
                baseStream.Close();
                baseStream = null;
            }
        }
    }
}