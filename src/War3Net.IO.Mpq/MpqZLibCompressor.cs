namespace War3Net.IO.Mpq
{
    public class MpqZLibCompressor : IMpqCompressor
    {
        private static readonly Lazy<MpqZLibCompressor> _defaultInstance = new Lazy<MpqZLibCompressor>(() => new MpqZLibCompressor());

        private readonly CompressionLevel _compressionLevel;

        public MpqZLibCompressor(CompressionLevel compressionLevel)
        {
            _compressionLevel = compressionLevel;
        }

        private MpqZLibCompressor()
        {
            _compressionLevel = CompressionLevel.Optimal;
        }

        public static MpqZLibCompressor Default => _defaultInstance.Value;

        public MpqCompressionType CompressionType => MpqCompressionType.ZLib;

        public Stream Compress(Stream stream, int bytesToCompress)
        {
            return ZLibCompression.Compress(stream, bytesToCompress, _compressionLevel, true);
        }
    }
}