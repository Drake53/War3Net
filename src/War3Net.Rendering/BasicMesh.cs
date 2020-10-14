// ------------------------------------------------------------------------------
// <copyright file="BasicMesh.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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