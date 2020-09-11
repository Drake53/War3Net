// ------------------------------------------------------------------------------
// <copyright file="NodeData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System.Collections.Generic;
using System.Numerics;

using War3Net.Modeling.DataStructures;

namespace War3Net.Rendering.DataStructures
{
    public sealed class NodeData
    {
        public NodeData()
        {
            Children = new List<NodeData>();
        }

        public string Name { get; set; }

        public Vector3 PivotPoint { get; set; }

        public uint ObjectId { get; set; }

        public uint? ParentId { get; set; }

        public NodeData? Parent { get; set; }

        public List<NodeData> Children { get; set; }

        public AnimationChannel<Vector3>? Translations { get; set; }

        public AnimationChannel<Quaternion>? Rotations { get; set; }

        public AnimationChannel<Vector3>? Scalings { get; set; }
    }
}