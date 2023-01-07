// ------------------------------------------------------------------------------
// <copyright file="DestructableObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed partial class DestructableObjectData
    {
        internal DestructableObjectData(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<ObjectDataFormatVersion>();

            nint baseDestructablesCount = reader.ReadInt32();
            for (nint i = 0; i < baseDestructablesCount; i++)
            {
                BaseDestructables.Add(reader.ReadSimpleObjectModification(FormatVersion));
            }

            nint newDestructablesCount = reader.ReadInt32();
            for (nint i = 0; i < newDestructablesCount; i++)
            {
                NewDestructables.Add(reader.ReadSimpleObjectModification(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.Write(BaseDestructables.Count);
            foreach (var destructable in BaseDestructables)
            {
                writer.Write(destructable, FormatVersion);
            }

            writer.Write(NewDestructables.Count);
            foreach (var destructable in NewDestructables)
            {
                writer.Write(destructable, FormatVersion);
            }
        }
    }
}