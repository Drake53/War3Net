#pragma warning disable CA1008
#pragma warning disable SA1300

using System.ComponentModel;

namespace War3Net.Build.Script
{
    /// <summary>
    /// File format version for <see cref="MapTriggers"/>.
    /// </summary>
    public enum MapTriggersFormatVersion
    {
        /// <summary>Reign of Chaos beta format.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        v3 = 3,

        /// <summary>Reign of Chaos format.</summary>
        v4 = 4,

        /// <summary>The Frozen Throne beta format.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        v6 = 6,

        /// <summary>The Frozen Throne format.</summary>
        v7 = 7,
    }
}