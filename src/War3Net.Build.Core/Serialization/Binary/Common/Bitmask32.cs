// ------------------------------------------------------------------------------
// <copyright file="Bitmask32.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Common
{
    public sealed partial class Bitmask32
    {
        internal Bitmask32(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            _mask = reader.ReadInt32();
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(_mask);
        }
    }
}