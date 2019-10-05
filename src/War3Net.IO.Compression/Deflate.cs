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
        public static uint CompressTo(Stream inputStream, Stream outputStream, int bytes, bool leaveOpen)
        {
            _ = inputStream ?? throw new ArgumentNullException(nameof(inputStream));
            _ = outputStream ?? throw new ArgumentNullException(nameof(outputStream));

            outputStream.WriteByte((byte)CompressionType.ZLib);

#if SYSTEM_DEFLATE
            using (var deflater = new DeflateStream(outputStream, CompressionMode.Compress, true))
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
            }
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
    }
}