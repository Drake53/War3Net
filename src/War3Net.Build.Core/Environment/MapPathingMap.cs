namespace War3Net.Build.Environment
{
    public sealed partial class MapPathingMap
    {
        public const string FileExtension = ".wpm";
        public const string FileName = "war3map.wpm"; // can also be a TGA image, war3mapPath.tga, where red=walk, green=fly, blue=build (0=yes, 255=no, alpha always 0)

        public static readonly int FileFormatSignature = "MP3W".FromRawcode();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPathingMap"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapPathingMap(MapPathingMapFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public MapPathingMapFormatVersion FormatVersion { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }

        public List<PathingType> Cells { get; init; } = new();

        public override string ToString() => FileName;
    }
}