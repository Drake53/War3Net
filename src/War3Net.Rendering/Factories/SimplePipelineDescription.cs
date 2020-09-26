// ------------------------------------------------------------------------------
// <copyright file="SimplePipelineDescription.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Veldrid;

using War3Net.Modeling.Enums;

namespace War3Net.Rendering.Factories
{
    public struct SimplePipelineDescription
    {
        public FilterMode FilterMode { get; set; }

        public FaceType FaceType { get; set; }

        public LayerShading LayerShading { get; set; }

        public SimpleShaderDescription ShaderSettings { get; set; }

        public OutputDescription OutputDescription { get; set; }
    }
}