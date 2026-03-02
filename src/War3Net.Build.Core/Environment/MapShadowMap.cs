namespace War3Net.Build.Environment
{
    public sealed partial class MapShadowMap
    {
        public const string FileExtension = ".shd";
        public const string FileName = "war3map.shd";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapShadowMap"/> class.
        /// </summary>
        public MapShadowMap()
        {
        }

        // True = 0xff, false = 0x00
        public List<byte> Cells { get; init; } = new();

        public override string ToString() => FileName;
    }
}