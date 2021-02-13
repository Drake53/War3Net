// ------------------------------------------------------------------------------
// <copyright file="FileProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Mpq
{
    public static class FileProvider
    {
        /// <summary>
        /// <see cref="File.Create(string)"/>, and <see cref="DirectoryInfo.Create()"/> if needed.
        /// </summary>
        public static FileStream CreateFileAndFolder(string path)
        {
            var directory = new FileInfo(path).Directory!;
            if (!directory.Exists)
            {
                directory.Create();
            }

            return File.Create(path);
        }
    }
}