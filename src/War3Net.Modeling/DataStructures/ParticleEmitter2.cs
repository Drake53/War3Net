// ------------------------------------------------------------------------------
// <copyright file="ParticleEmitter2.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

using War3Net.Modeling.Enums;

namespace War3Net.Modeling.DataStructures
{
    public struct ParticleEmitter2 : INode
    {
        public float Speed { get; set; }

        public float Variation { get; set; }

        public float Latitude { get; set; }

        public float Gravity { get; set; }

        public float Lifespan { get; set; }

        public float EmissionRate { get; set; }

        public float Length { get; set; }

        public float Width { get; set; }

        public ParticleEmitter2FilterMode FilterMode { get; set; }

        public uint Rows { get; set; }

        public uint Columns { get; set; }

        public ParticleEmitter2FramesFlags HeadOrTail { get; set; }

        public float TailLength { get; set; }

        public float Time { get; set; }

        public Segment[] Segments { get; set; }

        public uint TextureId { get; set; }

        public uint Squirt { get; set; }

        public uint PriorityPlane { get; set; }

        public uint ReplaceableId { get; set; }

        public AnimationChannel<float>? EmissionRates { get; set; }

        public AnimationChannel<float>? Gravities { get; set; }

        public AnimationChannel<float>? Latitudes { get; set; }

        public AnimationChannel<float>? Speeds { get; set; }

        public AnimationChannel<float>? Visibilities { get; set; }

        public AnimationChannel<float>? Variations { get; set; }

        public AnimationChannel<float>? Lengths { get; set; }

        public AnimationChannel<float>? Widths { get; set; }

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

        public struct Segment
        {
            public Vector3 SegmentColor { get; set; }

            public byte SegmentAlpha { get; set; }

            public float SegmentScaling { get; set; }

            public uint HeadInterval { get; set; }

            public uint HeadDecayInterval { get; set; }

            public uint TailInterval { get; set; }

            public uint TailDecayInterval { get; set; }
        }
    }
}