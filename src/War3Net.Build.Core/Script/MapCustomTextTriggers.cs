namespace War3Net.Build.Script
{
    public sealed partial class MapCustomTextTriggers
    {
        public const string FileExtension = ".wct";
        public const string FileName = "war3map.wct";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapCustomTextTriggers"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="subVersion"></param>
        public MapCustomTextTriggers(MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion)
        {
            FormatVersion = formatVersion;
            SubVersion = subVersion;
        }

        public MapCustomTextTriggersFormatVersion FormatVersion { get; set; }

        public MapCustomTextTriggersSubVersion? SubVersion { get; set; }

        public string GlobalCustomScriptComment { get; set; }

        public CustomTextTrigger GlobalCustomScriptCode { get; set; }

        public List<CustomTextTrigger> CustomTextTriggers { get; init; } = new();

        public override string ToString() => FileName;
    }
}