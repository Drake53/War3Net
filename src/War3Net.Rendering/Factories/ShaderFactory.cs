// ------------------------------------------------------------------------------
// <copyright file="ShaderFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;

using Veldrid;
using Veldrid.SPIRV;

using War3Net.Rendering.DataStructures;

namespace War3Net.Rendering.Factories
{
    public static class ShaderFactory
    {
        private static readonly Dictionary<ResourceFactory, Dictionary<SimpleShaderDescription, CachedShaderSet>> _cachedShaderSets
            = new Dictionary<ResourceFactory, Dictionary<SimpleShaderDescription, CachedShaderSet>>();

        public static CachedShaderSet GetShaderSet(this ResourceFactory resourceFactory, SimpleShaderDescription simpleShaderDescription)
        {
            if (!_cachedShaderSets.TryGetValue(resourceFactory, out var shaderDictionary))
            {
                shaderDictionary = new Dictionary<SimpleShaderDescription, CachedShaderSet>();
                _cachedShaderSets.Add(resourceFactory, shaderDictionary);
            }

            return shaderDictionary.TryGetValue(simpleShaderDescription, out var value) ? value : resourceFactory.CreateAndCacheShaders(simpleShaderDescription);
        }

        public static void ClearCache(bool disposeResources)
        {
            foreach (var cachedShaderSets in _cachedShaderSets.Values)
            {
                if (disposeResources)
                {
                    foreach (var cachedShaderSet in cachedShaderSets.Values)
                    {
                        cachedShaderSet.ResourceLayout.Dispose();
                    }
                }

                cachedShaderSets.Clear();
            }

            _cachedShaderSets.Clear();
        }

        private static CachedShaderSet CreateAndCacheShaders(this ResourceFactory resourceFactory, SimpleShaderDescription simpleShaderDescription)
        {
            var vertexLayouts = new VertexLayoutDescription(
                new VertexElementDescription(nameof(Vertex.Position), VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3),
                new VertexElementDescription(nameof(Vertex.UV), VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
                new VertexElementDescription(nameof(Vertex.VertexGroup), VertexElementSemantic.TextureCoordinate, VertexElementFormat.UInt1));

            var vertexCode = VertexCode;
            var fragmentCode = FragmentCode;

            var shaders = resourceFactory.CreateFromSpirv(
                new ShaderDescription(ShaderStages.Vertex, Encoding.UTF8.GetBytes(vertexCode), "main"),
                new ShaderDescription(ShaderStages.Fragment, Encoding.UTF8.GetBytes(fragmentCode), "main"));

            var shaderSetDescription = new ShaderSetDescription(new[] { vertexLayouts }, shaders);

            var resourceLayout = resourceFactory.CreateResourceLayout(new ResourceLayoutDescription(
                new ResourceLayoutElementDescription("Projection", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                new ResourceLayoutElementDescription("View", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                new ResourceLayoutElementDescription("World", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                new ResourceLayoutElementDescription("Transform", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                new ResourceLayoutElementDescription("Nodes", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                new ResourceLayoutElementDescription("SurfaceTex", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                new ResourceLayoutElementDescription("SurfaceSampler", ResourceKind.Sampler, ShaderStages.Fragment)));

            var cachedShaderSet = new CachedShaderSet
            {
                ShaderSetDescription = shaderSetDescription,
                ResourceLayout = resourceLayout,
            };

            _cachedShaderSets[resourceFactory].Add(simpleShaderDescription, cachedShaderSet);
            return cachedShaderSet;
        }

        private const string VertexCode = @"
# version 450

layout(set = 0, binding = 0) uniform ProjectionBuffer
{
    mat4 Projection;
};

layout(set = 0, binding = 1) uniform ViewBuffer
{
    mat4 View;
};

layout(set = 0, binding = 2) uniform WorldBuffer
{
    mat4 World;
};

layout(set = 0, binding = 3) uniform TransformBuffer
{
    mat4 Transform;
};

layout(set = 0, binding = 4) uniform NodesBuffer
{
    mat4 NodesTransformations[256];
};

layout(location = 0) in vec3 Position;
layout(location = 1) in vec2 UV;
layout(location = 2) in uint VertexGroup;
layout(location = 0) out vec2 fsin_uv;

void main()
{
    gl_Position = Projection * View * World * Transform * NodesTransformations[VertexGroup] * vec4(Position, 1);
    fsin_uv = UV;
}";

        private const string FragmentCode = @"
# version 450

layout(set = 0, binding = 5) uniform texture2D SurfaceTex;
layout(set = 0, binding = 6) uniform sampler SurfaceSampler;

layout(location = 0) in vec2 fsin_uv;
layout(location = 0) out vec4 fsout_color;

void main()
{
    fsout_color = texture(sampler2D(SurfaceTex, SurfaceSampler), fsin_uv);
}";
    }
}