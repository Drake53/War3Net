#define SYSTEM_DEFLATE
#define SYSTEM_INFLATE

using System;
using System.IO;

#if SYSTEM_DEFLATE || SYSTEM_INFLATE
using System.IO.Compression;
#endif

#if !SYSTEM_DEFLATE || !SYSTEM_INFLATE
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
#endif

namespace War3Net.IO.Compression
{
    public static class Deflate
    {
        public static uint CompressTo(Stream inputStream, Stream outputStream, int bytes, bool leaveOpen)
        {
            _ = inputStream ?? throw new ArgumentNullException(nameof(inputStream));
            _ = outputStream ?? throw new ArgumentNullException(nameof(outputStream));

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

            if (!leaveOpen)
            {
                inputStream.Dispose();
            }

            return (uint)outputStream.Position;
        }

        public static byte[] Decompress(Stream compressedData, int expectedLength)
        {
            var output = new byte[expectedLength];
#if SYSTEM_INFLATE
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