// ------------------------------------------------------------------------------
// <copyright file="FileProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

using War3Net.IO.Mpq;

namespace War3Net.Build.Providers
{
    public static class FileProvider
    {
        public static FileStream OpenNewWrite(string path)
        {
            var directory = new FileInfo(path).Directory;
            if (!directory.Exists)
            {
                directory.Create();
            }

            return File.Create(path);
        }

        public static bool FileExists(string path)
        {
            return MpqFile.Exists(path);
        }

        public static bool FileExists(MpqArchive archive, string path)
        {
            return MpqFile.Exists(archive, path);
        }

        /// <exception cref="FileNotFoundException"></exception>
        public static Stream GetFile(string path)
        {
            return MpqFile.OpenRead(path);
        }

        /// <exception cref="FileNotFoundException"></exception>
        public static Stream GetFile(MpqArchive archive, string path)
        {
            return MpqFile.OpenRead(archive, path);
        }

        public static IEnumerable<(string fileName, MpqLocale locale, Stream stream)> EnumerateFiles(string path)
        {
            if (File.Exists(path))
            {
                // Assume file at path is an mpq archive.
                var archive = MpqArchive.Open(path);
                var listfile = archive.OpenFile(ListFile.Key);

                using (var reader = new StreamReader(listfile))
                {
                    while (!reader.EndOfStream)
                    {
                        var fileName = reader.ReadLine();
                        var memoryStream = new MemoryStream();

                        archive.OpenFile(fileName).CopyTo(memoryStream);
                        memoryStream.Position = 0;

                        yield return (fileName, MpqLocale.Neutral, memoryStream);
                    }
                }

                archive.Dispose();
            }
            else if (Directory.Exists(path))
            {
                var pathPrefixLength = path.Length + (path.EndsWith(@"\", StringComparison.Ordinal) ? 0 : 1);
                foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                {
                    var fileName = new FileInfo(file).ToString().Substring(pathPrefixLength);
                    // var memoryStream = new MemoryStream();
                    // File.OpenRead(file).CopyTo(memoryStream);

                    var locale = MpqLocaleProvider.GetPathLocale(fileName, out var filePath);

                    yield return (filePath, locale, File.OpenRead(file));
                }
            }
        }

        private static MemoryStream GetArchiveFileStream(MpqArchive archive, string filePath)
        {
            var memoryStream = new MemoryStream();
            archive.OpenFile(filePath).CopyTo(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}