// ------------------------------------------------------------------------------
// <copyright file="ModelResources.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Veldrid;

namespace War3Net.Rendering
{
    public class ModelResources : IDisposable
    {
        public IReadOnlyList<GeosetResources> GeosetResources { get; set; }

        public Texture Texture { get; set; }

        public void Dispose()
        {
            foreach (var resources in GeosetResources)
            {
                resources.Dispose();
            }

            ((IDisposable)Texture).Dispose();
        }
    }
}