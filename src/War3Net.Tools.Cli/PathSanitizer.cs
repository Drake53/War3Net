namespace War3Net.Tools.Cli
{
    public static class PathSanitizer
    {
        /// <summary>
        /// Turns an archive-internal path into a safe relative path.
        /// </summary>
        public static string SanitizeRelativePath(string fileName)
        {
            var segments = fileName.Split('\\', '/');
            for (var i = 0; i < segments.Length; i++)
            {
                var segment = segments[i].TrimEnd('.', ' ');
                foreach (var invalidChar in Path.GetInvalidFileNameChars())
                {
                    segment = segment.Replace(invalidChar, '_');
                }

                segments[i] = string.IsNullOrEmpty(segment) || Path.IsPathRooted(segment) ? "_" : segment;
            }

            return Path.Combine(segments);
        }

        /// <summary>
        /// Ensures a unique file path for extraction.
        /// </summary>
        /// <returns>
        /// <paramref name="path"/> if not already used,
        /// otherwise <paramref name="path"/> with a numeric suffix before the file extension.
        /// </returns>
        public static string Deduplicate(string path, HashSet<string> usedPaths)
        {
            if (usedPaths.Add(path))
            {
                return path;
            }

            var directory = Path.GetDirectoryName(path);
            var name = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);
            for (var i = 2; ; i++)
            {
                var candidate = Path.Combine(directory, $"{name} ({i}){extension}");
                if (usedPaths.Add(candidate))
                {
                    return candidate;
                }
            }
        }
    }
}