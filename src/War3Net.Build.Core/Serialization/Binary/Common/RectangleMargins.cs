using System.IO;

namespace War3Net.Build.Common
{
    public sealed partial class RectangleMargins
    {
        internal RectangleMargins(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            Left = reader.ReadInt32();
            Right = reader.ReadInt32();
            Bottom = reader.ReadInt32();
            Top = reader.ReadInt32();
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(Left);
            writer.Write(Right);
            writer.Write(Bottom);
            writer.Write(Top);
        }
    }
}