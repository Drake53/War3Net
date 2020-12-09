// ------------------------------------------------------------------------------
// <copyright file="MpqFileCreateMode.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Mpq
{
    [Flags]
    public enum MpqFileCreateMode
    {
        AddFlag = 0x01,
        RemoveFlag = 0x02,

        /// <summary>
        /// Do not generate the file, but allow it to be added to the <see cref="MpqArchive"/> if it exists among the input files.
        /// </summary>
        None = 0,

        /// <summary>
        /// Do not generate the file, and remove it from the input files if it exists.
        /// </summary>
        Prune = RemoveFlag,

        /// <summary>
        /// Generate the file, but only if it does not exist in the input files.
        /// </summary>
        Generate = AddFlag,

        /// <summary>
        /// Generate the file, even if it's already contained in the input files.
        /// </summary>
        Overwrite = AddFlag | RemoveFlag,
    }
}