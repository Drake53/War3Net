// ------------------------------------------------------------------------------
// <copyright file="MapImportedFiles.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Import
{
    public sealed class MapImportedFiles : ImportedFiles
    {
        public const string FileName = "war3map.imp";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapImportedFiles"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapImportedFiles(ImportedFilesFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        internal MapImportedFiles(BinaryReader reader)
            : base(reader)
        {
        }
    }
}