// ------------------------------------------------------------------------------
// <copyright file="StreamReaderExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Mpq.Extensions
{
    public static class StreamReaderExtensions
    {
        public static ListFile ReadListFile(this StreamReader reader) => new ListFile(reader);
    }
}