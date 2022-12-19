// ------------------------------------------------------------------------------
// <copyright file="LevelObjectModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed class LevelObjectModification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelObjectModification"/> class.
        /// </summary>
        public LevelObjectModification()
        {
        }

        internal LevelObjectModification(BinaryReader reader, ObjectDataFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        public int OldId { get; set; }

        public int NewId { get; set; }

        public List<int> Unk { get; init; } = new();

        public List<LevelObjectDataModification> Modifications { get; init; } = new();

        public override string ToString() => NewId == 0 ? OldId.ToRawcode() : $"{NewId.ToRawcode()}:{OldId.ToRawcode()}";

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
                Modifications.Add(reader.ReadLevelObjectDataModification(formatVersion));
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