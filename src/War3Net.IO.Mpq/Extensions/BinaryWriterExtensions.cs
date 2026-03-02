using System.IO;

namespace War3Net.IO.Mpq.Extensions
{
    public static class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, Attributes attributes) => attributes.WriteTo(writer);

        public static void Write(this BinaryWriter writer, Signature signature) => signature.WriteTo(writer);
    }
}