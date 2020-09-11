// ------------------------------------------------------------------------------
// <copyright file="LoadedModelResources.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Modeling.DataStructures;

namespace War3Net.Rendering.DataStructures
{
    public sealed class LoadedModelResources
    {
        public int GeosetCount { get; set; }

        public uint[] IndexCounts { get; set; }

        public Vertex[][] Vertices { get; set; }

        public ushort[][] Indices { get; set; }

        public string[] Textures { get; set; }

        public uint[][][] VertexGroups { get; set; }

        public uint[] MaterialIds { get; set; }

        public NodeData[] Nodes { get; set; }

        public Material[] Materials { get; set; }

        public Sequence[] Sequences { get; set; }
    }
}