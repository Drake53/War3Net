// ------------------------------------------------------------------------------
// <copyright file="Light.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

using War3Net.Modeling.Enums;

namespace War3Net.Modeling.DataStructures
{
    public struct Light : INode
    {
        public LightType Type { get; set; }

        public float AttenuationStart { get; set; }

        public float AttenuationEnd { get; set; }

        public Vector3 Color { get; set; }

        public float Intensity { get; set; }

        public Vector3 AmbientColor { get; set; }

        public float AmbientIntensity { get; set; }

        public AnimationChannel<uint>? AttenuationStarts { get; set; }

        public AnimationChannel<uint>? AttenuationEnds { get; set; }

        public AnimationChannel<Vector3>? Colors { get; set; }

        public AnimationChannel<float>? Intensities { get; set; }

        public AnimationChannel<float>? AmbientIntensities { get; set; }

        public AnimationChannel<Vector3>? AmbientColors { get; set; }

        public AnimationChannel<float>? Visibilities { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public uint ObjectId { get; set; }

        /// <inheritdoc/>
        public uint? ParentId { get; set; }

        /// <inheritdoc/>
        public NodeFlags Flags { get; set; }

        /// <inheritdoc/>
        public AnimationChannel<Vector3>? Translations { get; set; }

        /// <inheritdoc/>
        public AnimationChannel<Quaternion>? Rotations { get; set; }

        /// <inheritdoc/>
        public AnimationChannel<Vector3>? Scalings { get; set; }
    }
}