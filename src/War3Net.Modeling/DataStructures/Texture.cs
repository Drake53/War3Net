// ------------------------------------------------------------------------------
// <copyright file="Texture.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Modeling.Enums;

namespace War3Net.Modeling.DataStructures
{
    public struct Texture
    {
        public uint ReplaceableId { get; set; }

        public string FileName { get; set; }

        public TextureFlags Flags { get; set; }
    }
}