// ------------------------------------------------------------------------------
// <copyright file="TestDataProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using War3Net.Build.Providers;

namespace War3Net.Build.Tests
{
    public static class TestDataProvider
    {
        internal const string TestDataFolder = "TestData";
        internal const string LocalDataFolder = "Local";
        internal const string WebCacheDataFolder = "WebCache";

        private static readonly ISet<string> _archiveFileExtensions = new HashSet<string>() { ".w3m", ".w3x", ".w3n", };

        public static bool IsArchiveFile(string fileName, out string extension)
        {
            extension = new FileInfo(fileName).Extension;
            return _archiveFileExtensions.Contains(extension);
        }

        public static IEnumerable<object[]> GetDynamicData(string searchPattern, SearchOption searchOption, params string[] directories)
        {
            foreach (var directory in GetTestDataDirectories(directories))
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
            foreach (var directory in GetTestDataDirectories(directories))
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

        private static IEnumerable<string> GetTestDataDirectories(params string[] directories)
        {
            foreach (var directory in directories)
            {
                DownloadTestData(directory);
            }

            foreach (var directory in directories.SelectMany(directory => new[]
            {
                Path.Combine(TestDataFolder, directory),
#if DEBUG
                Path.Combine(TestDataFolder, LocalDataFolder, directory),
                Path.Combine(TestDataFolder, WebCacheDataFolder, directory),
#endif
            }))
            {
                yield return directory;
            }
        }

        private static void DownloadTestData(string directoryName)
        {
            switch (directoryName)
            {
                case "Maps": DownloadMapsAsync("Maps").Wait(); break;

                default: break;
            }
        }

        private static async Task DownloadMapsAsync(string directoryName)
        {
            var mapIdsToDownload = new HashSet<int>(new[]
            {
                20000,  // Dota2.w3x
                30000,  // Creature Wars (Castle Edition) V1beta.w3x
                142119, // Bomberman 1.74.w3m
                241070, // RabbitsVsSheep3.0.36.w3x
                301324, // Forest Defense 0.21f_p.w3x
                306773, // MM_RPG_V1.12.w3x
                306775, // Spring Liquidation v1_80b.w3x
                306784, // LegendaryResistanceV2.14.w3x
                306890, // OrangeMushroomStory_4.9_english.w3x
                306913, // platform_escape_bw_2.9e.w3x
            });

            var directoryInfo = new DirectoryInfo(Path.Combine(TestDataFolder, WebCacheDataFolder, directoryName));
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            else
            {
                foreach (var map in directoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
                {
                    // Remove maps that have already been downloaded.
                    mapIdsToDownload.Remove(int.Parse(map.Name.Substring(0, map.Name.Length - map.Extension.Length)));
                }
            }

            using var httpClient = new HttpClient();
            foreach (var mapId in mapIdsToDownload)
            {
                try
                {
                    var responseBody = await httpClient.GetStringAsync(new Uri($@"https://www.epicwar.com/maps/{mapId}/"));

                    var downloadLinkMatch = Regex.Match(responseBody, $"<a href=\"/maps/download/{mapId}/[0-9a-f]{{72}}/[\\w\\-. %]+\">");
                    if (downloadLinkMatch.Success)
                    {
                        // Get href value by removing '<a href="' and '">' from the matched string.
                        var hrefValue = downloadLinkMatch.Value.Substring(9, downloadLinkMatch.Value.Length - 11);
                        if (!IsArchiveFile(hrefValue, out var extension))
                        {
                            throw new Exception($"Unexpected file extension: '{extension}'");
                        }

                        var fileUri = new Uri($"https://www.epicwar.com{hrefValue}");
                        var findFile = await httpClient.GetAsync(fileUri);
                        if (findFile.StatusCode == HttpStatusCode.Found)
                        {
                            fileUri = findFile.Headers.Location;
                        }

                        using var mapFile = await httpClient.GetStreamAsync(fileUri);
                        using var fileStream = File.Create(Path.Combine(TestDataFolder, WebCacheDataFolder, directoryName, $"{mapId}{extension}"));

                        await mapFile.CopyToAsync(fileStream);
                    }
                    else
                    {
                        throw new FileNotFoundException($"No regex match for map with id {mapId}.");
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}