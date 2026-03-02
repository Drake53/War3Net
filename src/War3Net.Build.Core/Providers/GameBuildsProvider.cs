namespace War3Net.Build.Providers
{
    public static class GameBuildsProvider
    {
        private static readonly Lazy<List<GameBuild>> _builds = new(GetGameBuildsFromJson);

        public static List<GameBuild> GetGameBuilds() => _builds.Value;

        public static List<GameBuild> GetGameBuilds(GameExpansion expansion)
        {
            return GetGameBuilds()
                .Where(gameBuild => gameBuild.GameExpansion == expansion)
                .ToList();
        }

        public static List<GameBuild> GetGameBuilds(GameReleaseType releaseType)
        {
            return GetGameBuilds()
                .Where(gameBuild => gameBuild.GameReleaseType == releaseType)
                .ToList();
        }

        public static List<GameBuild> GetGameBuilds(DateTime releaseDate)
        {
            var date = releaseDate.Date;

            return GetGameBuilds()
                .Where(gameBuild => gameBuild.ReleaseDate == date)
                .ToList();
        }

        public static List<GameBuild> GetGameBuilds(GamePatch patch)
        {
            return GetGameBuilds()
                .Where(gameBuild => gameBuild.GamePatch == patch)
                .ToList();
        }

        public static List<GameBuild> GetGameBuilds(Version version)
        {
            return GetGameBuilds()
                .Where(gameBuild => gameBuild.Version == version)
                .ToList();
        }

        public static List<GameBuild> GetGameBuilds(EditorVersion editorVersion)
        {
            return GetGameBuilds()
                .Where(gameBuild => gameBuild.EditorVersion == editorVersion)
                .ToList();
        }

        private static List<GameBuild> GetGameBuildsFromJson()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.General)
            {
                AllowTrailingCommas = true,
            };

            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new JsonStringVersionConverter());

            return JsonSerializer.Deserialize<List<GameBuild>>(Resources.War3Resources.GameBuilds, options) ?? new();
        }
    }
}