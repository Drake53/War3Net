// ------------------------------------------------------------------------------
// <copyright file="ParticleEmitter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

using War3Net.Modeling.Enums;

namespace War3Net.Modeling.DataStructures
{
    public struct ParticleEmitter : INode
    {
        public float EmissionRate { get; set; }

        public float Gravity { get; set; }

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public string SpawnModelFileName { get; set; }

        public float Lifespan { get; set; }

        public float InitialVelocity { get; set; }

        public AnimationChannel<float>? EmissionRates { get; set; }

        public AnimationChannel<float>? Gravities { get; set; }

        public AnimationChannel<float>? Longitudes { get; set; }

        public AnimationChannel<float>? Latitudes { get; set; }

        public AnimationChannel<float>? Lifespans { get; set; }

        public AnimationChannel<float>? Speeds { get; set; }

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