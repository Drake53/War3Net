using System.IO;

namespace War3Net.Build.Environment
{
    public sealed partial class MapShadowMap
    {
        internal MapShadowMap(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            Cells.AddRange(reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position)));
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(Cells.ToArray());
        }
    }
}