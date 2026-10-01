namespace War3Net.Build.Info
{
    public sealed partial class PlayerData
    {
        internal PlayerData(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal void ReadFrom(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            Id = reader.ReadInt32();
            Controller = reader.ReadInt32<PlayerController>();
            Race = reader.ReadInt32<PlayerRace>();
            if (formatVersion >= MapInfoFormatVersion.v39)
            {
                RaceHud = (PlayerRaceHud)reader.ReadInt32();
            }

            Flags = reader.ReadInt32<PlayerFlags>();
            Name = reader.ReadChars();
            StartPosition = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            AllyLowPriorityFlags = reader.ReadBitmask32();
            AllyHighPriorityFlags = reader.ReadBitmask32();

            if (formatVersion >= MapInfoFormatVersion.v31)
            {
                EnemyLowPriorityFlags = reader.ReadBitmask32();
                EnemyHighPriorityFlags = reader.ReadBitmask32();
            }
            else
            {
                EnemyLowPriorityFlags = new Bitmask32(0);
                EnemyHighPriorityFlags = new Bitmask32(0);
            }
        }

        internal void WriteTo(BinaryWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.Write(Id);
            writer.Write((int)Controller);
            writer.Write((int)Race);
            if (formatVersion >= MapInfoFormatVersion.v39)
            {
                writer.Write((int)RaceHud);
            }

            writer.Write((int)Flags);
            writer.WriteString(Name);
            writer.Write(StartPosition.X);
            writer.Write(StartPosition.Y);
            writer.Write(AllyLowPriorityFlags);
            writer.Write(AllyHighPriorityFlags);

            if (formatVersion >= MapInfoFormatVersion.v31)
            {
                writer.Write(EnemyLowPriorityFlags);
                writer.Write(EnemyHighPriorityFlags);
            }
        }
    }
}