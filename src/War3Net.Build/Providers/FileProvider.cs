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
    internal static class FileProvider
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

        public static IEnumerable<(string key, Stream value)> EnumerateFiles(string path)
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

                        yield return (fileName, memoryStream);
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

                    yield return (fileName, File.OpenRead(file));
                }
            }
        }
    }
}