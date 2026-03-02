using System.IO;

namespace War3Net.IO.Mpq.Extensions
{
    public static class StreamReaderExtensions
    {
        public static ListFile ReadListFile(this StreamReader reader) => new ListFile(reader);
    }
}