// ------------------------------------------------------------------------------
// <copyright file="GeosetResources.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Veldrid;

namespace War3Net.Rendering
{
    public class GeosetResources : IDisposable
    {
        public uint VertexIndicesCount { get; set; }

        public PrimitiveTopology PrimitiveTopology { get; set; }

        public DeviceBuffer VertexBuffer { get; set; }

        public DeviceBuffer IndexBuffer { get; set; }

        public void Dispose()
        {
            ((IDisposable)VertexBuffer).Dispose();
            ((IDisposable)IndexBuffer).Dispose();
        }
    }
}