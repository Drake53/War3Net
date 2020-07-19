// ------------------------------------------------------------------------------
// <copyright file="Vertex.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

using Veldrid;

namespace War3Net.Rendering
{
    struct Vertex
    {
        public const uint SizeInBytes = 8 + 12 + 12 /*sizeof(Vector2) + sizeof(Vector3) + sizeof(Vector3)*/;

        public Vector2 UV;
        public Vector3 Normal;
        public Vector3 Position;

        public static VertexLayoutDescription GetVertexLayoutDescription()
        {
            return new VertexLayoutDescription(
                new VertexElementDescription(nameof(UV), VertexElementFormat.Float2, VertexElementSemantic.TextureCoordinate),
                new VertexElementDescription(nameof(Normal), VertexElementFormat.Float3, VertexElementSemantic.TextureCoordinate),
                new VertexElementDescription(nameof(Position), VertexElementFormat.Float3, VertexElementSemantic.TextureCoordinate));
        }
    }
}