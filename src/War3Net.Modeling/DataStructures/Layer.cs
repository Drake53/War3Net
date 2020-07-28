// ------------------------------------------------------------------------------
// <copyright file="Layer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

using War3Net.Modeling.Enums;

namespace War3Net.Modeling.DataStructures
{
    public struct Layer
    {
        public FilterMode FilterMode { get; set; }

        public LayerShading ShadingFlags { get; set; }

        public uint TextureId { get; set; }

        public uint TextureAnimationId { get; set; }

        public uint CoordId { get; set; }

        public float Alpha { get; set; }

        public AnimationChannel<uint>? TextureIds { get; set; }

        public AnimationChannel<float>? Alphas { get; set; }
    }
}