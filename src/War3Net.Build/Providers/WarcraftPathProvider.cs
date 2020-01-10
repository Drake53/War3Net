// ------------------------------------------------------------------------------
// <copyright file="WarcraftPathProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using Microsoft.Win32;

namespace War3Net.Build.Providers
{
    [Obsolete("", true)]
    internal static class WarcraftPathProvider
    {
        public static string GetExePath(bool x86, bool ptr)
        {
            var displayName = ptr ? "Warcraft III Public Test" : "Warcraft III";

            if (FindByDisplayName(displayName, out var path))
            {
                const string exeName = "Warcraft III.exe";

                var exePath = Path.Combine(path, exeName);
                if (File.Exists(exePath))
                {
                    return exePath;
                }

                exePath = Path.Combine(path, x86 ? "x86" : "x86_64", exeName);
                if (File.Exists(exePath))
                {
                    return exePath;
                }
            }

            return null;
        }

        private static bool FindByDisplayName(string displayName, out string path)
        {
            // Solution from: https://social.msdn.microsoft.com/Forums/windows/en-US/afb5012a-30f1-4b96-9931-a143fd76bab5/how-to-find-path-of-installed-programs-in-c?forum=winformssetup
            const string registryUninstall = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";

            var key = Registry.LocalMachine.OpenSubKey( registryUninstall );
            var nameList = key.GetSubKeyNames();

            for (var i = 0; i < nameList.Length; i++)
            {
                var regKey = key.OpenSubKey( nameList[i] );
                if (regKey.GetValue("DisplayName")?.ToString() == displayName)
                {
                    path = regKey.GetValue("InstallLocation").ToString();
                    return true;
                }
            }

            path = null;
            return false;
        }
    }
}