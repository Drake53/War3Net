using System.IO;
using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Audio
{
    public sealed partial class MapSounds
    {
        internal MapSounds(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<MapSoundsFormatVersion>();

            nint soundCount = reader.ReadInt32();
            for (nint i = 0; i < soundCount; i++)
            {
                Sounds.Add(reader.ReadSound(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.Write(Sounds.Count);
            foreach (var sound in Sounds)
            {
                writer.Write(sound, FormatVersion);
            }
        }
    }
}