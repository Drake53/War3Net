// #define SYSTEM_DEFLATE

using System;
using System.IO;

#if SYSTEM_DEFLATE
using System.IO.Compression;
#else
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
#endif

namespace War3Net.IO.Compression
{
    public static class Deflate
    {
        public static uint CompressTo(Stream inputStream, Stream outputStream, int bytes)
        {
            _ = inputStream ?? throw new ArgumentNullException(nameof(inputStream));
            _ = outputStream ?? throw new ArgumentNullException(nameof(outputStream));

            using (var stream = new MemoryStream())
            {
#if SYSTEM_DEFLATE
                using (var deflater = new DeflateStream(inputStream, CompressionMode.Compress))
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

                    // First byte in the block indicates the compression algorithm used.
                    outputStream.WriteByte((byte)CompressionType.ZLib);

                    stream.Position = 0;
                    stream.CopyTo(outputStream);
                }
            }

            return (uint)outputStream.Position;
        }
    }
}