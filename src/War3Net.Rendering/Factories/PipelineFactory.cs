// ------------------------------------------------------------------------------
// <copyright file="PipelineFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Veldrid;

using War3Net.Rendering.DataStructures;
using War3Net.Rendering.Extensions;

namespace War3Net.Rendering.Factories
{
    public static class PipelineFactory
    {
        internal static readonly Dictionary<ResourceFactory, Dictionary<SimplePipelineDescription, CachedPipeline>> _cachedPipelines
            = new Dictionary<ResourceFactory, Dictionary<SimplePipelineDescription, CachedPipeline>>();

        public static CachedPipeline GetPipeline(this ResourceFactory resourceFactory, SimplePipelineDescription simplePipelineDescription)
        {
            if (!_cachedPipelines.TryGetValue(resourceFactory, out var pipelineDictionary))
            {
                pipelineDictionary = new Dictionary<SimplePipelineDescription, CachedPipeline>();
                _cachedPipelines.Add(resourceFactory, pipelineDictionary);
            }

            return pipelineDictionary.TryGetValue(simplePipelineDescription, out var value) ? value : resourceFactory.CreateAndCachePipeline(simplePipelineDescription);
        }

        public static void ClearCache(bool disposeResources)
        {
            foreach (var cachedPipelines in _cachedPipelines.Values)
            {
                if (disposeResources)
                {
                    foreach (var cachedPipeline in cachedPipelines.Values)
                    {
                        cachedPipeline.Pipeline.Dispose();
                    }
                }

                cachedPipelines.Clear();
            }

            _cachedPipelines.Clear();
        }

        private static CachedPipeline CreateAndCachePipeline(this ResourceFactory resourceFactory, SimplePipelineDescription simplePipelineDescription)
        {
            var shaderSet = resourceFactory.GetShaderSet(simplePipelineDescription.ShaderSettings);
            var pipelineDescription = new GraphicsPipelineDescription(
                simplePipelineDescription.FilterMode.ToBlendStateDescription(),
                DepthStencilStateDescription.DepthOnlyLessEqual,
                new RasterizerStateDescription(FaceCullMode.Back, PolygonFillMode.Solid, FrontFace.CounterClockwise, true, false),
                simplePipelineDescription.FaceType.ToPrimitiveTopology(),
                shaderSet.ShaderSetDescription,
                shaderSet.ResourceLayout,
                simplePipelineDescription.OutputDescription);

            var pipeline = resourceFactory.CreateGraphicsPipeline(ref pipelineDescription);

            var cachedPipeline = new CachedPipeline
            {
                Pipeline = pipeline,
                ResourceLayout = shaderSet.ResourceLayout,
            };

            _cachedPipelines[resourceFactory].Add(simplePipelineDescription, cachedPipeline);
            return cachedPipeline;
        }
    }
}