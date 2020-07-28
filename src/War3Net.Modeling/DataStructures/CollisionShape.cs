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
        public CollisionShapeType Type { get; set; }

        public Vector3[] Vertices { get; set; }

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