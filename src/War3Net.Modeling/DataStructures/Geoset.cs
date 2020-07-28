// ------------------------------------------------------------------------------
// <copyright file="Geoset.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

using War3Net.Modeling.Enums;

namespace War3Net.Modeling.DataStructures
{
    public struct Geoset
    {
        public Vector3[] Vertices { get; set; }

        public Vector3[] Normals { get; set; }

        public FaceType[] FaceTypeGroups { get; set; }

        public uint[] FaceGroups { get; set; }

        public ushort[] Faces { get; set; }

        public byte[] VertexGroups { get; set; }

        public uint[] MatrixGroups { get; set; }

        public uint[] MatrixIndices { get; set; }

        public uint MaterialId { get; set; }

        public uint SelectionGroup { get; set; }

        public GeosetSelectionFlags SelectionFlags { get; set; }

        public Extent Extent { get; set; }

        public Extent[] SequenceExtents { get; set; }

        public TextureCoordinateSet[] TextureCoordinateSets { get; set; }

        public struct TextureCoordinateSet
        {
            public Vector2[] TextureCoordinates { get; set; }
        }
    }
}