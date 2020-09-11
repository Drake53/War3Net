// ------------------------------------------------------------------------------
// <copyright file="Vertex.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

namespace War3Net.Rendering.DataStructures
{
    public struct Vertex
    {
        public Vector3 Position;
        public Vector2 UV;
        public uint VertexGroup;
    }
}