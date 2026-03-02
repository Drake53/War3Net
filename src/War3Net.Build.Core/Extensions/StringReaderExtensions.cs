using System.IO;
using War3Net.Build.Script;

namespace War3Net.Build.Extensions
{
    public static class StringReaderExtensions
    {
        public static TriggerData ReadTriggerData(this StringReader reader) => new TriggerData(reader);
    }
}