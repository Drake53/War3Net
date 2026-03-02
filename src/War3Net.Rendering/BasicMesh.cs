using System.Collections.Generic;
using War3Net.Rendering.DataStructures;

namespace War3Net.Rendering
{
    public class BasicMesh
    {
        public List<Vertex> Vertices { get; set; }

        public List<ushort> Indices { get; set; }

        public List<string> TexturePaths { get; set; }
    }
}