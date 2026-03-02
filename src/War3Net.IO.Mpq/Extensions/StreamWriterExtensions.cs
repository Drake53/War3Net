namespace War3Net.IO.Mpq.Extensions
{
    public static class StreamWriterExtensions
    {
        public static void WriteListFile(this StreamWriter writer, ListFile listFile) => listFile.WriteTo(writer);
    }
}