namespace War3Net.IO.Mpq.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static Attributes ReadAttributes(this BinaryReader reader) => new Attributes(reader);

        public static Signature ReadSignature(this BinaryReader reader) => new Signature(reader);
    }
}