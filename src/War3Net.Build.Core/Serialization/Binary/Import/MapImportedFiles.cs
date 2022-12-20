// ------------------------------------------------------------------------------
// <copyright file="MapImportedFiles.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Import
{
    public sealed partial class MapImportedFiles : ImportedFiles
    {
        internal MapImportedFiles(BinaryReader reader)
            : base(reader)
        {
        }
    }
}