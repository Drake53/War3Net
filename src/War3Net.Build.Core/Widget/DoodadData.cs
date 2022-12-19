// ------------------------------------------------------------------------------
// <copyright file="DoodadData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Numerics;

using War3Net.Build.Extensions;

namespace War3Net.Build.Widget
{
    public sealed class DoodadData : WidgetData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoodadData"/> class.
        /// </summary>
        public DoodadData()
        {
        }

        internal DoodadData(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, out bool useNewFormat)
        {
            ReadFrom(reader, formatVersion, subVersion, out useNewFormat);
        }

        public DoodadState State { get; set; }

        // in %, where 0x64 = 100%
        public byte Life { get; set; }

        internal void ReadFrom(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, out bool useNewFormat)
        {
            TypeId = reader.ReadInt32();
            Variation = reader.ReadInt32();
            Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Rotation = reader.ReadSingle();
            Scale = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            // Check if next byte is 'printable' (this also assumes that _flags byte is a low number).
            useNewFormat = reader.PeekChar() >= 0x20;
            SkinId = useNewFormat ? reader.ReadInt32() : TypeId;

            if (formatVersion > MapWidgetsFormatVersion.v6)
            {
                State = (DoodadState)reader.ReadByte();
                if ((int)State >= 8)
                {
                    throw new InvalidDataException($"Invalid doodad state: {State}.");
                }
            }

            Life = reader.ReadByte();

            if (formatVersion == MapWidgetsFormatVersion.v8)
            {
                MapItemTableId = reader.ReadInt32();

                nint droppedItemDataCount = reader.ReadInt32();
                for (nint i = 0; i < droppedItemDataCount; i++)
                {
                    ItemTableSets.Add(reader.ReadRandomItemSet(formatVersion, subVersion, useNewFormat));
                }
            }

            CreationNumber = reader.ReadInt32();
        }

        internal void WriteTo(BinaryWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.Write(TypeId);
            writer.Write(Variation);
            writer.Write(Position.X);
            writer.Write(Position.Y);
            writer.Write(Position.Z);
            writer.Write(Rotation);
            writer.Write(Scale.X);
            writer.Write(Scale.Y);
            writer.Write(Scale.Z);

            if (useNewFormat)
            {
                writer.Write(SkinId);
            }

            if (formatVersion > MapWidgetsFormatVersion.v6)
            {
                writer.Write((byte)State);
            }

            writer.Write(Life);

            if (formatVersion == MapWidgetsFormatVersion.v8)
            {
                writer.Write(MapItemTableId);

                writer.Write(ItemTableSets.Count);
                foreach (var itemSet in ItemTableSets)
                {
                    writer.Write(itemSet, formatVersion, subVersion, useNewFormat);
                }
            }

            writer.Write(CreationNumber);
        }
    }
}