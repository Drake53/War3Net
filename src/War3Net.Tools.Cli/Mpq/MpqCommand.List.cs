namespace War3Net.Tools.Cli.Mpq
{
    public static partial class MpqCommand
    {
        private static Command BuildListCommand()
        {
            var detailOptions = new[] { "names", "summary", "full" };
            var formatOptions = new[] { "text", "json", "jsonl" };

            var command = new Command("list", "List the files in an MPQ archive.");
            var archiveArgument = new Argument<string>("archive").AcceptLegalFilePathsOnly();
            var detailOption = StringOption("--detail", null, detailOptions, "summary", "Level of detail to include for each entry.");
            var formatOption = StringOption("--format", null, formatOptions, "text", "Output format.");

            command.Arguments.Add(archiveArgument);
            command.Options.Add(detailOption);
            command.Options.Add(formatOption);

            command.SetAction(parseResult =>
            {
                try
                {
                    return List(
                        parseResult.GetValue(archiveArgument),
                        parseResult.GetOutputDetail(detailOption),
                        parseResult.GetOutputFormat(formatOption));
                }
                catch (Exception exception)
                {
                    Console.Error.Write(exception);
                    return 1;
                }
            });

            return command;
        }

        private static int List(
            string archivePath,
            OutputDetail detail,
            OutputFormat format)
        {
            using var archive = MpqArchive.Open(archivePath, loadListFile: true);

            IEnumerable<(int Index, string? FileName)> EnumerateNames()
            {
                for (var index = 0; index < archive.Count; index++)
                {
                    var entry = archive[index];
                    if (entry.Flags == 0)
                    {
                        continue;
                    }

                    yield return (index, entry.FileName);
                }
            }

            IEnumerable<(int Index, MpqEntry Entry, bool CanRead, IEnumerable<MpqLocale> Locales)> EnumerateEntries()
            {
                var localesByBlockIndex = archive
                    .EnumerateHashes()
                    .ToLookup(hash => hash.BlockIndex, hash => hash.Locale);

                for (var index = 0; index < archive.Count; index++)
                {
                    var entry = archive[index];
                    if (entry.Flags == 0)
                    {
                        continue;
                    }

                    bool canRead;
                    using (var stream = archive.OpenFile(entry))
                    {
                        canRead = stream.CanRead;
                    }

                    yield return (index, entry, canRead, localesByBlockIndex[(uint)index]);
                }
            }

            switch (format)
            {
                case OutputFormat.Base64:
                case OutputFormat.Binary:
                    throw new NotSupportedException("`w3n mpq list` does not support base64/binary format.");

                case OutputFormat.Json:
                {
                    using var writer = new Utf8JsonWriter(Console.OpenStandardOutput());

                    writer.WriteStartArray();

                    if (detail == OutputDetail.Names)
                    {
                        foreach (var (index, fileName) in EnumerateNames())
                        {
                            WriteEntry(writer, index, fileName);
                        }
                    }
                    else
                    {
                        foreach (var (index, entry, canRead, locales) in EnumerateEntries())
                        {
                            WriteEntry(writer, detail, index, entry, canRead, locales);
                        }
                    }

                    writer.WriteEndArray();

                    return 0;
                }

                case OutputFormat.Jsonl:
                {
                    var options = new JsonWriterOptions() { Indented = false };

                    using var writer = new Utf8JsonWriter(Console.OpenStandardOutput(), options);

                    if (detail == OutputDetail.Names)
                    {
                        foreach (var (index, fileName) in EnumerateNames())
                        {
                            WriteEntry(writer, index, fileName);
                            writer.MoveNext();
                        }
                    }
                    else
                    {
                        foreach (var (index, entry, canRead, locales) in EnumerateEntries())
                        {
                            WriteEntry(writer, detail, index, entry, canRead, locales);
                            writer.MoveNext();
                        }
                    }

                    return 0;
                }

                case OutputFormat.Text:
                default:
                {
                    using var writer = new StreamWriter(Console.OpenStandardOutput());

                    if (detail == OutputDetail.Names)
                    {
                        foreach (var (index, fileName) in EnumerateNames())
                        {
                            writer.WriteLine($"{index}\t{fileName ?? "(unknown)"}");
                        }
                    }
                    else
                    {
                        foreach (var (index, entry, canRead, locales) in EnumerateEntries())
                        {
                            writer.Write($"{index}\t{entry.FileName ?? "(unknown)"}\t{entry.FileSize}\t{entry.CompressedSize}\t{canRead}\t{string.Join(',', locales)}");

                            if (detail == OutputDetail.Full)
                            {
                                writer.Write($"\t{entry.IsCompressed}\t{entry.IsEncrypted}\t{entry.IsSingleUnit}\t{entry.FilePosition}");
                            }

                            writer.WriteLine();
                        }
                    }

                    return 0;
                }
            }
        }

        private static void WriteEntry(
            Utf8JsonWriter writer,
            int index,
            string? fileName)
        {
            writer.WriteStartObject();
            writer.WriteNumber("index", index);
            writer.WriteString("name", fileName);
            writer.WriteEndObject();
        }

        private static void WriteEntry(
            Utf8JsonWriter writer,
            OutputDetail detail,
            int index,
            MpqEntry entry,
            bool canRead,
            IEnumerable<MpqLocale> locales)
        {
            writer.WriteStartObject();
            writer.WriteNumber("index", index);
            writer.WriteString("name", entry.FileName);
            writer.WriteNumber("fileSize", entry.FileSize);
            writer.WriteNumber("compressedSize", entry.CompressedSize);
            writer.WriteBoolean("canRead", canRead);

            writer.WriteStartArray("locales");
            foreach (var locale in locales)
            {
                writer.WriteStringValue(locale.ToString());
            }

            writer.WriteEndArray();

            if (detail == OutputDetail.Full)
            {
                writer.WriteBoolean("compressed", entry.IsCompressed);
                writer.WriteBoolean("encrypted", entry.IsEncrypted);
                writer.WriteBoolean("singleUnit", entry.IsSingleUnit);
                writer.WriteNumber("filePosition", entry.FilePosition);
            }

            writer.WriteEndObject();
        }
    }
}