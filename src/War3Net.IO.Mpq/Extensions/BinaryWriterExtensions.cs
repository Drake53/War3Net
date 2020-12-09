// ------------------------------------------------------------------------------
// <copyright file="BinaryWriterExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Mpq.Extensions
{
    internal static class BinaryWriterExtensions
    {
        internal static void Write(this BinaryWriter writer, Attributes attributes) => attributes.WriteTo(writer);
    }
}