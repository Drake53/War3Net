#pragma warning disable SA1300

namespace War3Net.Build.Environment
{
    /// <summary>
    /// File format version for <see cref="MapCameras"/>.
    /// </summary>
    public enum MapCamerasFormatVersion
    {
        /// <summary>The initial version.</summary>
        v0 = 0,

        /// <summary>Introduced in patch 3.0.0.</summary>
        v3 = 3,
    }
}