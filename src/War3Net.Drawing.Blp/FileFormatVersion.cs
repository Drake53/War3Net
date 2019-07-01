namespace War3Net.Drawing.Blp
{
    enum FileFormatVersion : uint
    {
        /// <summary>
        /// Used in Warcraft III beta.
        /// </summary>
        BLP0 = 0x30504c42,

        /// <summary>
        /// Used in Warcraft III Reign of Chaos and The Frozen Throne.
        /// </summary>
        BLP1 = 0x31504c42,

        /// <summary>
        /// Used in World of Warcraft.
        /// </summary>
        BLP2 = 0x32504c42,
    }
}