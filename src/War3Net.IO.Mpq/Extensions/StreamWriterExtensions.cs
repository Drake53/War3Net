// ------------------------------------------------------------------------------
// <copyright file="StreamWriterExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Mpq.Extensions
{
    public static class StreamWriterExtensions
    {
        public static void WriteListFile(this StreamWriter writer, ListFile listFile) => listFile.WriteTo(writer);
    }
}