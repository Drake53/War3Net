using System.IO;

namespace War3Net.IO.Mpq
{
    public interface IMpqCompressor
    {
        MpqCompressionType CompressionType { get; }

        Stream Compress(Stream stream, int bytesToCompress);
    }
}