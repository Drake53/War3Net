// ------------------------------------------------------------------------------
// <copyright file="CachedPipeline.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Veldrid;

namespace War3Net.Rendering.DataStructures
{
    public class CachedPipeline
    {
        public Pipeline Pipeline { get; set; }

        public ResourceLayout ResourceLayout { get; set; }
    }
}