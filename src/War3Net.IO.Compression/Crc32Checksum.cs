using System.IO;
using System.IO.Hashing;

namespace War3Net.IO.Compression
{
    public static class Crc32Checksum
    {
        public static uint Compute(Stream stream)
        {
            var crc = new Crc32();
            crc.Append(stream);
            return crc.GetCurrentHashAsUInt32();
        }
    }
}