// ------------------------------------------------------------------------------
// <copyright file="VariationObjectModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;

namespace War3Net.Build.Object
{
    public sealed partial class VariationObjectModification
    {
        internal VariationObjectModification(BinaryReader reader, ObjectDataFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal void ReadFrom(BinaryReader reader, ObjectDataFormatVersion formatVersion)
        {
            OldId = reader.ReadInt32();
            NewId = reader.ReadInt32();

            if (formatVersion >= ObjectDataFormatVersion.v3)
            {
                nint unkCount = reader.ReadInt32();
                for (nint i = 0; i < unkCount; i++)
                {
                    Unk.Add(reader.ReadInt32());
                }
            }

            nint modificationCount = reader.ReadInt32();
            for (nint i = 0; i < modificationCount; i++)
            {
                Modifications.Add(reader.ReadVariationObjectDataModification(formatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer, ObjectDataFormatVersion formatVersion)
        {
            writer.Write(OldId);
            writer.Write(NewId);

            if (formatVersion >= ObjectDataFormatVersion.v3)
            {
                writer.Write(Unk.Count);
                foreach (var unk in Unk)
                {
                    writer.Write(unk);
                }
            }

            writer.Write(Modifications.Count);
            foreach (var modification in Modifications)
            {
                writer.Write(modification, formatVersion);
            }
        }
    }
}