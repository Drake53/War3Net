// ------------------------------------------------------------------------------
// <copyright file="Model.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

namespace War3Net.Modeling.DataStructures
{
    public struct Model
    {
        public ModelVersion? Version { get; set; }

        public ModelInfo? ModelInfo { get; set; }

        public Sequence[]? Sequences { get; set; }

        public Texture[]? Textures { get; set; }

        public Material[]? Materials { get; set; }

        public Geoset[]? Geosets { get; set; }

        public GeosetAnimation[]? GeosetAnimations { get; set; }

        public Bone[]? Bones { get; set; }

        public Helper[]? Helpers { get; set; }

        public Attachment[]? Attachments { get; set; }

        public Vector3[]? PivotPoints { get; set; }

        public EventObject[]? EventObjects { get; set; }

        public CollisionShape[]? CollisionShapes { get; set; }

        public GlobalSequence[]? GlobalSequences { get; set; }

        public ParticleEmitter[]? ParticleEmitters { get; set; }

        public ParticleEmitter2[]? ParticleEmitters2 { get; set; }

        public Camera[]? Cameras { get; set; }

        public Light[]? Lights { get; set; }

        public RibbonEmitter[]? RibbonEmitters { get; set; }

        public TextureAnimation[]? TextureAnimations { get; set; }
    }
}