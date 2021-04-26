// ------------------------------------------------------------------------------
// <copyright file="BinaryReaderExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Mpq.Extensions
{
    internal static class BinaryReaderExtensions
    {
        internal static Attributes ReadAttributes(this BinaryReader reader) => new Attributes(reader);

        internal static Signature ReadSignature(this BinaryReader reader) => new Signature(reader);
    }
}