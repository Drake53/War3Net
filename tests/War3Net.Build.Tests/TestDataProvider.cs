// ------------------------------------------------------------------------------
// <copyright file="TestDataProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using War3Net.Build.Providers;

namespace War3Net.Build.Tests
{
    public static class TestDataProvider
    {
        private const string TestDataFolder = "TestData";
        private const string LocalDataFolder = "Local";

        private static readonly ISet<string> _archiveFileExtensions = new HashSet<string>() { ".w3m", ".w3x", ".w3n", };

        public static IEnumerable<object[]> GetDynamicData(string searchPattern, SearchOption searchOption, params string[] directories)
        {
            foreach (var directory in directories.SelectMany(dir => new[] { Path.Combine(TestDataFolder, dir), Path.Combine(TestDataFolder, LocalDataFolder, dir), }))
            {
                if (Directory.Exists(directory))
                {
                    foreach (var file in Directory.EnumerateFiles(directory, searchPattern, searchOption))
                    {
                        yield return new[] { file };
                    }
                }
            }
        }

        public static IEnumerable<object[]> GetDynamicArchiveData(string fileName, SearchOption searchOption, params string[] directories)
        {
            foreach (var directory in directories.SelectMany(dir => new[] { Path.Combine(TestDataFolder, dir), Path.Combine(TestDataFolder, LocalDataFolder, dir), }))
            {
                if (Directory.Exists(directory))
                {
                    foreach (var searchPattern in _archiveFileExtensions.Select(extension => $"*{extension}"))
                    {
                        foreach (var archive in Directory.EnumerateFiles(directory, searchPattern, searchOption))
                        {
                            var file = Path.Combine(archive, fileName);
                            if (FileProvider.FileExists(file))
                            {
                                yield return new[] { file };
                            }
                        }
                    }
                }
            }
        }

        public static string GetSearchPattern(this string fileName)
        {
            return $"*{new FileInfo(fileName).Extension}";
        }
    }
}