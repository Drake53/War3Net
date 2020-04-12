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
            if (File.Exists(path))
            {
                return true;
            }

            // Check if file is contained in an mpq archive.
            var subPath = path;
            var fullPath = new FileInfo(path).FullName;
            while (!File.Exists(subPath))
            {
                subPath = new FileInfo(subPath).DirectoryName;
                if (subPath is null)
                {
                    return false;
                }
            }

            var relativePath = fullPath.Substring(subPath.Length + (subPath.EndsWith(@"\", StringComparison.Ordinal) ? 0 : 1));

            using var archive = MpqArchive.Open(subPath);
            return FileExists(archive, relativePath);
        }

        public static bool FileExists(MpqArchive archive, string path)
        {
            if (archive.FileExists(path))
            {
                return true;
            }

            // Check if file is contained in an mpq archive.
            var subPath = path;
            var ignoreLength = new FileInfo(subPath).FullName.Length - path.Length;
            while (!archive.FileExists(subPath))
            {
                var directoryName = new FileInfo(subPath).DirectoryName;
                if (directoryName.Length <= ignoreLength)
                {
                    return false;
                }

                subPath = directoryName.Substring(ignoreLength);
            }

            var relativePath = path.Substring(subPath.Length + (subPath.EndsWith(@"\", StringComparison.Ordinal) ? 0 : 1));

            using var subArchiveStream = archive.OpenFile(subPath);
            using var subArchive = MpqArchive.Open(subArchiveStream);
            return FileExists(subArchive, relativePath);
        }

        /// <exception cref="FileNotFoundException"></exception>
        public static Stream GetFile(string path)
        {
            if (File.Exists(path))
            {
                return File.OpenRead(path);
            }

            // Assume file is contained in an mpq archive.
            var subPath = path;
            var fullPath = new FileInfo(path).FullName;
            while (!File.Exists(subPath))
            {
                subPath = new FileInfo(subPath).DirectoryName;
                if (subPath is null)
                {
                    throw new FileNotFoundException($"File not found: {path}");
                }
            }

            var relativePath = fullPath.Substring(subPath.Length + (subPath.EndsWith(@"\", StringComparison.Ordinal) ? 0 : 1));

            using var archive = MpqArchive.Open(subPath);
            return GetFile(archive, relativePath);
        }

        /// <exception cref="FileNotFoundException"></exception>
        public static Stream GetFile(MpqArchive archive, string path)
        {
            if (archive.FileExists(path))
            {
                return GetArchiveFileStream(archive, path);
            }

            // Assume file is contained in an mpq archive.
            var subPath = path;
            var ignoreLength = new FileInfo(subPath).FullName.Length - path.Length;
            while (!archive.FileExists(subPath))
            {
                var directoryName = new FileInfo(subPath).DirectoryName;
                if (directoryName.Length <= ignoreLength)
                {
                    throw new FileNotFoundException($"File not found: {path}");
                }

                subPath = directoryName.Substring(ignoreLength);
            }

            var relativePath = path.Substring(subPath.Length + (subPath.EndsWith(@"\", StringComparison.Ordinal) ? 0 : 1));

            using var subArchiveStream = archive.OpenFile(subPath);
            using var subArchive = MpqArchive.Open(subArchiveStream);
            return GetArchiveFileStream(subArchive, relativePath);
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