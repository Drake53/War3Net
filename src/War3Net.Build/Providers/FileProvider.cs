// ------------------------------------------------------------------------------
// <copyright file="FileProvider.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.IO.Mpq;

namespace War3Net.Build.Providers
{
    public static class FileProvider
    {
        public static FileStream OpenNewWrite(string path)
        {
            /*if (File.Exists(path))
            {
                File.Delete(path);
            }
            else*/ if (!Directory.Exists(new FileInfo(path).DirectoryName))
            {
                Directory.CreateDirectory(new FileInfo(path).DirectoryName);
            }

            return File.Create(path);
        }

        public static Stream GetFile(string path)
        {
            if (File.Exists(path))
            {
                return File.OpenRead(path);
            }
            else
            {
                // Assume file is contained in an mpq archive.
                var subPath = path;
                var fullPath = new FileInfo(path).FullName;
                while (!File.Exists(subPath))
                {
                    subPath = new FileInfo(subPath).DirectoryName;
                }

                var relativePath = fullPath.Substring(subPath.Length + (subPath.EndsWith("\\") ? 0 : 1));

                var memoryStream = new MemoryStream();
                using (var archive = MpqArchive.Open(subPath))
                {
                    archive.OpenFile(relativePath).CopyTo(memoryStream);
                }

                memoryStream.Position = 0;
                return memoryStream;
            }
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
                var pathPrefixLength = path.Length + (path.EndsWith("\\") ? 0 : 1);
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
    }
}