// ------------------------------------------------------------------------------
// <copyright file="INode.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

using War3Net.Modeling.Enums;

namespace War3Net.Modeling.DataStructures
{
    public interface INode
    {
        public string Name { get; set; }

        public uint ObjectId { get; set; }

        public uint? ParentId { get; set; }

        public NodeFlags Flags { get; set; }

        public AnimationChannel<Vector3>? Translations { get; set; }

        public AnimationChannel<Quaternion>? Rotations { get; set; }

        public AnimationChannel<Vector3>? Scalings { get; set; }
    }
}