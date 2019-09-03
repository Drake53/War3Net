// ------------------------------------------------------------------------------
// <copyright file="FileProvider.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Providers
{
    internal static class FileProvider
    {
        public static FileStream OpenNewWrite(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else if (!Directory.Exists(new FileInfo(path).DirectoryName))
            {
                Directory.CreateDirectory(new FileInfo(path).DirectoryName);
            }

            return File.OpenWrite(path);
        }
    }
}