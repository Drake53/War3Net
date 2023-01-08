// ------------------------------------------------------------------------------
// <copyright file="ImportedFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Import
{
    public sealed partial class ImportedFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportedFile"/> class.
        /// </summary>
        public ImportedFile()
        {
        }

        public ImportedFileFlags Flags { get; set; }

        public string FullPath { get; set; }

        public override string ToString() => FullPath;
    }
}