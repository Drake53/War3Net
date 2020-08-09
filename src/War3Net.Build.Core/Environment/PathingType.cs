// ------------------------------------------------------------------------------
// <copyright file="PathingType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Environment
{
    public enum PathingType : byte
    {
        Walk = 0x02,
        Fly = 0x04,
        Build = 0x08,

        Blight = 0x20,
        Water = 0x40,
        UNK = 0x80,
    }
}