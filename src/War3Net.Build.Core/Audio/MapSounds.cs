namespace War3Net.Build.Audio
{
    public sealed partial class MapSounds
    {
        public const string FileExtension = ".w3s";
        public const string FileName = "war3map.w3s";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSounds"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapSounds(MapSoundsFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public MapSoundsFormatVersion FormatVersion { get; set; }

        public List<Sound> Sounds { get; init; } = new();

        public override string ToString() => FileName;
    }
}