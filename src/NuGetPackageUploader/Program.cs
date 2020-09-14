// ------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGetPackageUploader
{
    internal static class Program
    {
        private const string LocalNuGetFeedDirectory = null;
        private const string ApiKey = null;

        [STAThread]
        private static async Task Main(string[] args)
        {
            if (LocalNuGetFeedDirectory is null)
            {
                return;
            }

            // https://github.com/NuGet/Samples/blob/master/PackageDownloadsExample/Program.cs
            const string sourceString = "https://api.nuget.org/v3/index.json";
            var source = new PackageSource(sourceString);
            var providers = Repository.Provider.GetCoreV3();
            var repository = new SourceRepository(source, providers);

            var search = await repository.GetResourceAsync<RawSearchResourceV3>();
            var filter = new SearchFilter(includePrerelease: true);

            var war3netProjectFolders = Directory.EnumerateDirectories(@"..\..\..\..\", "War3Net.*", SearchOption.TopDirectoryOnly);
            var updateableProjects = new HashSet<string>();
            var handledProjects = new HashSet<string>();

            while (true)
            {
                var knownLatestVersions = new Dictionary<string, NuGetVersion>();
                var knownLatestFeedVersions = new Dictionary<string, NuGetVersion>();
                var moveablePackageFilePaths = new Dictionary<string, HashSet<string>>(); // from release folder to local feed
                var uploadablePackageFilePaths = new Dictionary<string, HashSet<string>>(); // from local feed to online feed
                var anyBuildFailed = false;

                foreach (var war3netProjectFolder in war3netProjectFolders)
                {
                    var projectName = new DirectoryInfo(war3netProjectFolder).Name;
                    if (handledProjects.Contains(projectName))
                    {
                        continue;
                    }

                    var projectFilePath = Path.Combine(war3netProjectFolder, $"{projectName}.csproj");

                    // Set PACK=true to enable some property- and itemgroups that have this as condition.
                    var dotnetPackProcess = Process.Start("dotnet", $"pack \"{projectFilePath}\" -nologo -c Release -verbosity:quiet /p:PACK=true");
                    dotnetPackProcess.WaitForExit();

                    if (dotnetPackProcess.ExitCode != 0)
                    {
                        anyBuildFailed = true;
                        Console.WriteLine($"Build failed for project {projectName}.");
                        Console.WriteLine();
                        continue;
                    }

                    // Package location 1 - release folder.
                    var binaryReleaseFolder = Path.Combine(war3netProjectFolder, @"bin\Release");
                    var localPackages = Directory.EnumerateFiles(binaryReleaseFolder, "*.nupkg", SearchOption.TopDirectoryOnly)
                    .OrderBy(file => file.GetNuGetVersion(projectName));

                    // Package location 2 - local NuGet feed.
                    var localFeedFolder = Path.Combine(LocalNuGetFeedDirectory, projectName.ToLowerInvariant());
                    if (!Directory.Exists(localFeedFolder))
                    {
                        Directory.CreateDirectory(localFeedFolder);
                    }

                    var localFeedPackages = Directory.EnumerateFiles(localFeedFolder, "*.nupkg", SearchOption.TopDirectoryOnly)
                    .OrderBy(file => file.GetNuGetVersion(projectName));

                    // Package location 3 - online NuGet feed.
                    var response = await search.Search($"packageid:{projectName}", filter, skip: 0, take: 20, NullLogger.Instance, CancellationToken.None);
                    var result = response
                    .Select(result => result.ToObject<SearchResult>())
                    .SingleOrDefault(result => string.Equals(result.PackageId, projectName, StringComparison.Ordinal));

                    var knownLatestVersion = result?.Versions.Select(version => NuGetVersion.Parse(version.Version)).Max();
                    var knownLatestFeedVersion = knownLatestVersion;

                    // Find packages that can be moved from location 1 to 2.
                    if (localPackages.Any())
                    {
                        var moveablePackages = new HashSet<string>();

                        var latestLocalVersion = localPackages.Last().GetNuGetVersion(projectName);
                        if (latestLocalVersion > knownLatestVersion)
                        {
                            knownLatestVersion = latestLocalVersion;
                        }

                        if (!localFeedPackages.Any())
                        {
                            foreach (var localPackage in localPackages)
                            {
                                var version = localPackage.GetVersion(projectName);
                                Console.WriteLine($"{projectName} v{version} has not been added to the local feed yet.");

                                moveablePackages.Add(localPackage);
                            }
                        }
                        else
                        {
                            var allVersionsUploaded = true;
                            foreach (var localPackage in localPackages)
                            {
                                var version = localPackage.GetVersion(projectName);
                                var localFeedPackage = localFeedPackages.SingleOrDefault(localFeedPackage => string.Equals(localFeedPackage.GetVersion(projectName), version, StringComparison.OrdinalIgnoreCase));
                                if (localFeedPackage != null)
                                {
                                    if (localPackage.GetNuGetVersion(projectName) == latestLocalVersion)
                                    {
                                        using var localStream = File.OpenRead(localPackage);
                                        using var localFeedStream = File.OpenRead(localFeedPackage);

                                        if (!AreEqual(localStream, localFeedStream))
                                        {
                                            Console.WriteLine($"{projectName}'s version should be incremented.");
                                            allVersionsUploaded = false;

                                            updateableProjects.Add(projectName);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"{projectName} v{version} has not been added to the local feed yet.");
                                    allVersionsUploaded = false;

                                    moveablePackages.Add(localPackage);
                                }
                            }

                            if (allVersionsUploaded)
                            {
                                Console.WriteLine($"{projectName} is up-to-date on the local feed.");
                                handledProjects.Add(projectName);
                            }
                        }

                        moveablePackageFilePaths.Add(projectName, moveablePackages);
                    }

                    // Find packages that can be uploaded from location 2 to 3.
                    if (localFeedPackages.Any())
                    {
                        var uploadablePackages = new HashSet<string>();

                        var latestLocalFeedVersion = localFeedPackages.Last().GetNuGetVersion(projectName);
                        if (latestLocalFeedVersion > knownLatestVersion)
                        {
                            knownLatestVersion = latestLocalFeedVersion;
                            knownLatestFeedVersion = latestLocalFeedVersion;
                        }

                        if (result is null)
                        {
                            foreach (var localFeedPackage in localFeedPackages)
                            {
                                var version = localFeedPackage.GetVersion(projectName);
                                Console.WriteLine($"{projectName} v{version} has not been uploaded yet.");

                                uploadablePackages.Add(localFeedPackage);
                            }
                        }
                        else
                        {
                            var allVersionsUploaded = true;
                            foreach (var localFeedPackage in localFeedPackages)
                            {
                                var version = localFeedPackage.GetVersion(projectName);
                                if (!result.Versions.Any(v => string.Equals(v.Version, version, StringComparison.OrdinalIgnoreCase)))
                                {
                                    Console.WriteLine($"{projectName} v{version} has not been uploaded yet.");
                                    allVersionsUploaded = false;

                                    uploadablePackages.Add(localFeedPackage);
                                }
                            }

                            if (allVersionsUploaded)
                            {
                                Console.WriteLine($"{projectName} is up-to-date on NuGet.");
                            }
                        }

                        uploadablePackageFilePaths.Add(projectName, uploadablePackages);
                    }

                    if (knownLatestVersion != null)
                    {
                        knownLatestVersions.Add(projectName, knownLatestVersion);
                        knownLatestFeedVersions.Add(projectName, knownLatestFeedVersion);
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine();

                // Overview of projects for which the version should be incremented.
                if (updateableProjects.Any())
                {
                    Console.WriteLine("Projects that require version increment:");
                    foreach (var updateableProject in updateableProjects)
                    {
                        Console.WriteLine(updateableProject);
                    }

                    Console.WriteLine();
                }

                // Overview of projects that reference an old version of a War3Net package.
                var allProjectFiles = Directory.EnumerateFiles(@"..\..\..\..\..\", "*.csproj", SearchOption.AllDirectories);
                var anyProject = false;
                var anyPackage = false;
                foreach (var projectFile in allProjectFiles)
                {
                    var project = CSharpLua.ProjectHelper.ParseProject(projectFile, "Release");
                    foreach (var packageReference in project.PackageReferences)
                    {
                        if (knownLatestFeedVersions.TryGetValue(packageReference.Name, out var knownLatestFeedVersion))
                        {
                            if (knownLatestFeedVersion > NuGetVersion.Parse(packageReference.Version))
                            {
                                if (!anyProject)
                                {
                                    anyProject = true;
                                    Console.WriteLine("Projects that require package update:");
                                }

                                if (!anyPackage)
                                {
                                    Console.WriteLine(new FileInfo(projectFile).Name);
                                }

                                Console.WriteLine($"  {packageReference.Name}: {packageReference.Version} -> {knownLatestFeedVersion}");
                            }
                        }
                    }

                    anyPackage = false;
                }

                if (anyProject)
                {
                    Console.WriteLine();
                }

                // Overview of packages that have not been moved to the local feed.
                if (moveablePackageFilePaths.Any(pair => pair.Value.Any()))
                {
                    Console.WriteLine("Packages that can be moved to the local feed:");
                    foreach (var pair in moveablePackageFilePaths)
                    {
                        var projectName = pair.Key;
                        foreach (var moveablePackageFilePath in pair.Value)
                        {
                            var v = moveablePackageFilePath.GetNuGetVersion(projectName);
                            if (knownLatestVersions[projectName] > v)
                            {
                                Console.WriteLine($"{new FileInfo(moveablePackageFilePath).Name} (latest is {knownLatestVersions[projectName]})");
                            }
                            else
                            {
                                Console.WriteLine(new FileInfo(moveablePackageFilePath).Name);
                            }
                        }
                    }

                    Console.WriteLine();
                }

                // Overview of packages that exist on the local feed, but not on NuGet.
                if (uploadablePackageFilePaths.Any(pair => pair.Value.Any()))
                {
                    Console.WriteLine("Packages that can be uploaded:");
                    foreach (var pair in uploadablePackageFilePaths)
                    {
                        var projectName = pair.Key;
                        foreach (var uploadablePackageFilePath in pair.Value)
                        {
                            var v = uploadablePackageFilePath.GetNuGetVersion(projectName);
                            if (knownLatestVersions[projectName] > v)
                            {
                                Console.WriteLine($"{new FileInfo(uploadablePackageFilePath).Name} (latest is {knownLatestVersions[projectName]})");
                            }
                            else
                            {
                                Console.WriteLine(new FileInfo(uploadablePackageFilePath).Name);
                            }
                        }
                    }

                    Console.WriteLine();
                }

                if (updateableProjects.Any() || anyProject)
                {
                    Console.WriteLine("Moving packages disabled because project/package versions must be updated in one or more .csproj files.");
                    break;
                }

                Console.WriteLine();
                Console.WriteLine();

                var anyPackageMoved = false;
                foreach (var pair in moveablePackageFilePaths)
                {
                    var projectName = pair.Key;
                    foreach (var moveablePackageFilePath in pair.Value)
                    {
                        var v = moveablePackageFilePath.GetNuGetVersion(projectName);
                        var fileInfo = new FileInfo(moveablePackageFilePath);
                        var fileName = fileInfo.Name;
                        Console.WriteLine($"Move {fileName} to local feed? (Y/N)");

                        if (knownLatestVersions[projectName] > v)
                        {
                            Console.WriteLine($"  Note: latest is v{knownLatestVersions[projectName]}");
                        }
                        else
                        {
                            handledProjects.Add(projectName);
                        }

                        var key = Console.ReadKey().Key;
                        if (key == ConsoleKey.Y)
                        {
                            File.Copy(moveablePackageFilePath, Path.Combine(LocalNuGetFeedDirectory, projectName, fileName));

                            var symbolsFileName = $"{fileName[0..^6]}.snupkg";
                            var symbolsFilePath = Path.Combine(fileInfo.DirectoryName, symbolsFileName);
                            if (File.Exists(symbolsFilePath))
                            {
                                File.Copy(symbolsFilePath, Path.Combine(LocalNuGetFeedDirectory, projectName, symbolsFileName));
                            }

                            anyPackageMoved = true;
                        }

                        Console.WriteLine();
                    }
                }

                Console.WriteLine();

                if (anyBuildFailed || ApiKey is null)
                {
                    Console.WriteLine("Uploading packages disabled because some builds failed, and/or API-key has not been set.");

                    if (anyPackageMoved)
                    {
                        Console.WriteLine();
                        Console.WriteLine("One or more packages have been moved to the local feed, so the program will restart.");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();

                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                Console.WriteLine();
                Console.WriteLine();

                foreach (var pair in uploadablePackageFilePaths)
                {
                    var projectName = pair.Key;
                    foreach (var uploadablePackageFilePath in pair.Value)
                    {
                        var v = uploadablePackageFilePath.GetNuGetVersion(projectName);
                        var fileInfo = new FileInfo(uploadablePackageFilePath);
                        var fileName = fileInfo.Name;
                        Console.WriteLine($"Upload {fileName} to NuGet? (Y/N)");

                        if (knownLatestVersions[projectName] > v)
                        {
                            Console.WriteLine($"  Note: latest is v{knownLatestVersions[projectName]}");
                        }

                        var key = Console.ReadKey().Key;
                        if (key == ConsoleKey.Y)
                        {
                            // https://docs.microsoft.com/en-us/nuget/nuget-org/publish-a-package
                            var dotnetUploadPackageProcess = Process.Start("dotnet", $"nuget push \"{uploadablePackageFilePath}\" --api-key {ApiKey} --source {sourceString}");
                            dotnetUploadPackageProcess.WaitForExit();

                            if (dotnetUploadPackageProcess.ExitCode != 0)
                            {
                                throw new Exception($"Process exited with code: {dotnetUploadPackageProcess.ExitCode}");
                            }
                        }

                        Console.WriteLine();
                    }
                }

                break;
            }
        }

        private static string GetVersion(this string filePath, string projectName)
        {
            return new FileInfo(filePath).Name[(projectName.Length + 1)..^6];
        }

        private static NuGetVersion GetNuGetVersion(this string filePath, string projectName)
        {
            return NuGetVersion.Parse(filePath.GetVersion(projectName));
        }

        private static bool AreEqual(Stream expected, Stream actual)
        {
            var expectedSize = expected.Length;
            var actualSize = actual.Length;

            if (expectedSize != actualSize)
            {
                return false;
            }

            var data1 = new byte[expectedSize];
            if (expected.Read(data1, 0, (int)expectedSize) != expectedSize)
            {
                throw new IOException("Could not fully read expected stream");
            }

            var data2 = new byte[expectedSize];
            if (actual.Read(data2, 0, (int)expectedSize) != expectedSize)
            {
                throw new IOException("Could not fully read actual stream");
            }

            for (var bytesRead = 0; bytesRead < expectedSize; bytesRead++)
            {
                if (data1[bytesRead] != data2[bytesRead])
                {
                    return false;
                }
            }

            return true;
        }
    }
}