// ------------------------------------------------------------------------------
// <copyright file="CollisionShape.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

using War3Net.Modeling.Enums;

namespace War3Net.Modeling.DataStructures
{
    public struct CollisionShape : INode
    {
        public CollisionShape(string name)
            : this()
        {
            Name = name;
        }

        public CollisionShapeType Type { get; set; }

        /// <summary>
        /// Gets or sets the vertices of the collision shape.
        /// If the shape is a <see cref="CollisionShapeType.Sphere"/> there should be one vertex, otherwise two.
        /// </summary>
        public Vector3[] Vertices { get; set; }

        /// <summary>
        /// Gets or sets the radius of a <see cref="CollisionShapeType.Sphere"/> or <see cref="CollisionShapeType.Cylinder"/>.
        /// </summary>
        public float Radius { get; set; }

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