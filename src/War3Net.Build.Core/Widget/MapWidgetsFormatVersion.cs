#pragma warning disable CA1008
#pragma warning disable SA1300

namespace War3Net.Build.Widget
{
    /// <summary>
    /// File format version for <see cref="MapDoodads"/> and <see cref="MapUnits"/>.
    /// </summary>
    public enum MapWidgetsFormatVersion
    {
        /// <summary>Reign of Chaos beta format.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        v6 = 6,

        /// <summary>Reign of Chaos format.</summary>
        v7 = 7,

        /// <summary>The Frozen Throne format.</summary>
        v8 = 8,

        /// <summary>Definitive Edition 2.0.4.23839 format.</summary>
        v12 = 12,

        /// <summary>Definitive Edition 2.0.4.23919 format.</summary>
        v13 = 13,
    }
}