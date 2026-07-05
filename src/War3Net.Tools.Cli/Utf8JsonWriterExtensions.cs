namespace War3Net.Tools.Cli
{
    public static class Utf8JsonWriterExtensions
    {
        /// <summary>
        /// Flushes and resets the <paramref name="writer"/>,
        /// writing a newline for the next top-level JSON object when writing JSONL.
        /// Assumes output is being written to <see cref="Console.Out"/>,
        /// because <see cref="Utf8JsonWriter"/> does not expose its underlying stream.
        /// </summary>
        public static void MoveNext(this Utf8JsonWriter writer)
        {
            writer.Flush();
            Console.Out.Write('\n');
            writer.Reset();
        }
    }
}