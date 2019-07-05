// ------------------------------------------------------------------------------
// <copyright file="FileFormatVersion.cs" company="Xalcon @ mmowned.com-Forum">
// Copyright (c) 2011 Xalcon @ mmowned.com-Forum. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Drawing.Blp
{
    /// <summary>
    /// Indicates the version of the <see cref="BlpFile"/> image format.
    /// </summary>
    internal enum FileFormatVersion : uint
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