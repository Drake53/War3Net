#pragma warning disable CA1008
#pragma warning disable SA1300

namespace War3Net.Build.Environment
{
    /// <summary>
    /// File format version for <see cref="MapRegions"/>.
    /// </summary>
    public enum MapRegionsFormatVersion
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        v2 = 2,

        [EditorBrowsable(EditorBrowsableState.Never)]
        v3 = 3,

        /// <summary>The initial version.</summary>
        v5 = 5,
    }
}