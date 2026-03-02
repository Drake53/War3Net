namespace War3Net.Build.Environment
{
    public sealed partial class PreviewIcon
    {
        internal PreviewIcon(BinaryReader reader, MapPreviewIconsFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal void ReadFrom(BinaryReader reader, MapPreviewIconsFormatVersion formatVersion)
        {
            IconType = reader.ReadInt32<PreviewIconType>();
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            Color = reader.ReadColorBgra();
        }

        internal void WriteTo(BinaryWriter writer, MapPreviewIconsFormatVersion formatVersion)
        {
            writer.Write((int)IconType);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Color.ToBgra());
        }
    }
}