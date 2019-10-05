// #define SYSTEM_DEFLATE
// #define SYSTEM_INFLATE

#define WITH_PKLIB_HEADER
#define LEAVE_UNCOMPRESSED_IF_LARGER

#define USING_DOTNETZIP

#if USING_DOTNETZIP
#undef WITH_PKLIB_HEADER
// #undef LEAVE_UNCOMPRESSED_IF_LARGER
#endif

using System;
using System.IO;

#if SYSTEM_DEFLATE || SYSTEM_INFLATE
using System.IO.Compression;
#endif

#if USING_DOTNETZIP
#endif

#if (!SYSTEM_DEFLATE || !SYSTEM_INFLATE) //&& !USING_DOTNETZIP
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
#endif

namespace War3Net.IO.Compression
{
    public static class Deflate
    {
        public static uint TryCompress(Stream inputStream, Stream outputStream, uint bytes, bool leaveOpen)
        {
            _ = inputStream ?? throw new ArgumentNullException(nameof(inputStream));
            _ = outputStream ?? throw new ArgumentNullException(nameof(outputStream));

#if WITH_PKLIB_HEADER
            var position = outputStream.Position;
            for (var i = 0; i < 8; i++)
            {
                outputStream.WriteByte(0);
            }
#endif

            var offset = inputStream.Position;
            var compressed = new MemoryStream();
            CompressTo(inputStream, compressed, (int)bytes, true);

#if LEAVE_UNCOMPRESSED_IF_LARGER
#if USING_DOTNETZIP
            // Add one because CompressionType byte not written yet.
            if ((compressed.Length + 1) >= bytes)
#else
            if (compressed.Length >= bytes)
#endif
            {
                compressed.Dispose();

                inputStream.Position = offset;
                for (var i = 0; i < bytes; i++)
                {
                    var r = inputStream.ReadByte();
                    if (r == -1)
                    {
                        break;
                    }

                    outputStream.WriteByte((byte)r);
                }
            }
            else
#endif
            {
#if USING_DOTNETZIP
                outputStream.WriteByte((byte)CompressionType.ZLib);
#endif
                compressed.Position = 0;
                compressed.CopyTo(outputStream);
                compressed.Dispose();
            }

            if (!leaveOpen)
            {
                inputStream.Dispose();
            }

            var result = (uint)outputStream.Position;
#if WITH_PKLIB_HEADER
            var compressedSize = result - (uint)position;

            outputStream.Position = position;
            outputStream.WriteByte((byte)CompressionType.PKLib);
            outputStream.WriteByte(0);
            outputStream.WriteByte(0);
            outputStream.WriteByte(0);
            outputStream.WriteByte((byte)(compressedSize & 0xff));
            outputStream.WriteByte((byte)((compressedSize >> 8) & 0xff));
            outputStream.WriteByte((byte)((compressedSize >> 16) & 0xff));
            outputStream.WriteByte((byte)((compressedSize >> 24) & 0xff));

            outputStream.Position = result;
#endif
            return result;
        }

        private static uint CompressTo(Stream inputStream, Stream outputStream, int bytes, bool leaveOpen)
        {
            _ = inputStream ?? throw new ArgumentNullException(nameof(inputStream));
            _ = outputStream ?? throw new ArgumentNullException(nameof(outputStream));

#if USING_DOTNETZIP
            using (var deflater = new Ionic.Zlib.ZlibStream(outputStream, Ionic.Zlib.CompressionMode.Compress, true))
            {
                for (var i = 0; i < bytes; i++)
                {
                    var r = inputStream.ReadByte();
                    if (r == -1)
                    {
                        break;
                    }

                    deflater.WriteByte((byte)r);
                }

                // deflater.FlushMode = Ionic.Zlib.FlushType.Finish;
                // deflater.Flush();
            }
#else
            outputStream.WriteByte((byte)CompressionType.ZLib);
#if SYSTEM_DEFLATE
            const CompressionLevel compressionLevel = CompressionLevel.Optimal;

            // TODO: don't hardcode header bytes
            outputStream.WriteByte(0x78);
            outputStream.WriteByte(compressionLevel == CompressionLevel.Optimal ? (byte)0xDA : throw new NotSupportedException());

            const int mod = 65521;
            var s1 = 1;
            var s2 = 0;

            using (var deflater = new DeflateStream(outputStream, compressionLevel, true))
            {
                for (var i = 0; i < bytes; i++)
                {
                    var r = inputStream.ReadByte();
                    if (r == -1)
                    {
                        break;
                    }

                    deflater.WriteByte((byte)r);

                    s1 += r;
                    if (s1 >= mod)
                    {
                        s1 -= mod;
                    }

                    s2 += s1;
                    if (s2 >= mod)
                    {
                        s2 -= mod;
                    }
                }
            }

            // Adler-32 checksum
            outputStream.WriteByte((byte)((s2 >> 8) & 0xff));
            outputStream.WriteByte((byte)(s2 & 0xff));
            outputStream.WriteByte((byte)((s1 >> 8) & 0xff));
            outputStream.WriteByte((byte)(s1 & 0xff));
#else
            using (var stream = new MemoryStream())
            {
#if SYSTEM_DEFLATE
                using (var deflater = new DeflateStream(stream, CompressionMode.Compress))
#else
                using (var deflater = new DeflaterOutputStream(stream))
#endif
                {
                    for (var i = 0; i < bytes; i++)
                    {
                        var r = inputStream.ReadByte();
                        if (r == -1)
                        {
                            break;
                        }

                        deflater.WriteByte((byte)r);
                    }

#if !SYSTEM_DEFLATE
                    deflater.Finish();
#endif
                    deflater.Flush();

                    stream.Position = 0;
                    stream.CopyTo(outputStream);
                }
            }
#endif
#endif

            if (!leaveOpen)
            {
                inputStream.Dispose();
            }

            return (uint)outputStream.Position;
        }

        public static byte[] Decompress(Stream compressedData, int expectedLength)
        {
            var output = new byte[expectedLength];
#if USING_DOTNETZIP
            using var inflater = new Ionic.Zlib.ZlibStream(compressedData, Ionic.Zlib.CompressionMode.Decompress, true);
#elif SYSTEM_INFLATE
            _ = compressedData.ReadByte() == 0x78 ? (object?)null : throw new InvalidDataException();
            var b2 = compressedData.ReadByte();
            _ = b2 == 0xDA || b2 == 0x9C ? (object?)null : throw new InvalidDataException();
            using var inflater = new DeflateStream(compressedData, CompressionMode.Decompress, true);
#else
            var inflater = new InflaterInputStream(compressedData);
#endif
            var offset = 0;
            while (expectedLength > 0)
            {
                var size = inflater.Read(output, offset, expectedLength);
                if (size == 0)
                {
                    break;
                }

                offset += size;
                expectedLength -= size;
            }

#if SYSTEM_INFLATE
            // Verify Adler-32 checksum.
            const int mod = 65521;
            var s1 = 1;
            var s2 = 0;

            foreach (var b in output)
            {
                s1 += b;
                if (s1 >= mod)
                {
                    s1 -= mod;
                }

                s2 += s1;
                if (s2 >= mod)
                {
                    s2 -= mod;
                }
            }

            compressedData.Seek(-4, SeekOrigin.End);
            _ = compressedData.ReadByte() == ((s2 >> 8) & 0xff) ? (object?)null : throw new InvalidDataException();
            _ = compressedData.ReadByte() == (s2 & 0xff) ? (object?)null : throw new InvalidDataException();
            _ = compressedData.ReadByte() == ((s1 >> 8) & 0xff) ? (object?)null : throw new InvalidDataException();
            _ = compressedData.ReadByte() == (s1 & 0xff) ? (object?)null : throw new InvalidDataException();
#endif

            return output;
        }
    }
}