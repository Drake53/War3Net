// ------------------------------------------------------------------------------
// <copyright file="Model.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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

        public INode GetNodeById(uint id)
        {
            return GetNodes().Single(node => node.ObjectId == id);
        }

        public IEnumerable<INode> GetNodes()
        {
            foreach (var bone in Bones ?? Array.Empty<Bone>())
            {
                yield return bone;
            }

            foreach (var helper in Helpers ?? Array.Empty<Helper>())
            {
                yield return helper;
            }

            foreach (var attachment in Attachments ?? Array.Empty<Attachment>())
            {
                yield return attachment;
            }

            foreach (var eventObject in EventObjects ?? Array.Empty<EventObject>())
            {
                yield return eventObject;
            }

            foreach (var collisionShape in CollisionShapes ?? Array.Empty<CollisionShape>())
            {
                yield return collisionShape;
            }

            foreach (var particleEmitter in ParticleEmitters ?? Array.Empty<ParticleEmitter>())
            {
                yield return particleEmitter;
            }

            foreach (var particleEmitter2 in ParticleEmitters2 ?? Array.Empty<ParticleEmitter2>())
            {
                yield return particleEmitter2;
            }

            foreach (var light in Lights ?? Array.Empty<Light>())
            {
                yield return light;
            }

            foreach (var ribbonEmitter in RibbonEmitters ?? Array.Empty<RibbonEmitter>())
            {
                yield return ribbonEmitter;
            }
        }
    }
}