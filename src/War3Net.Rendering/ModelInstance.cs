// ------------------------------------------------------------------------------
// <copyright file="ModelInstance.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

using Veldrid;

using War3Net.Modeling.DataStructures;
using War3Net.Modeling.Enums;
using War3Net.Rendering.AnimatedMesh;
using War3Net.Rendering.DataStructures;
using War3Net.Rendering.Factories;

namespace War3Net.Rendering
{
    public sealed class ModelInstance
    {
        private const uint _ushortByteSize = sizeof(ushort);
        private static readonly uint _animatedVertexByteSize = (uint)Unsafe.SizeOf<Vertex>();

        private static readonly Dictionary<string, CachedModelResources> _cachedResources = new Dictionary<string, CachedModelResources>();
        private static readonly Dictionary<string, TextureView> _cachedTextures = new Dictionary<string, TextureView>();

        public static readonly HashSet<ModelInstance> _createdInstances = new HashSet<ModelInstance>();

        private LoadedModelResources _loadedResources;
        private SharedModelResources _sharedResources;
        private InstanceModelResources _instanceResources;

        private Sequence? _sequence;
        private double _previousAnimMilliseconds;
        private float _animationTimeScale;

        private NodeAnimInfo _nodeAnimInfo;
        private Matrix4x4[] _nodeTransformations;

        private Matrix4x4 _transformation;
        private Matrix4x4 _translation;
        private Matrix4x4 _orientation;
        private Matrix4x4 _scaling;

        public ModelInstance(string modelPath)
        {
            CreateResources(modelPath);
            Init();
        }

        public void Draw(CommandList commandList)
        {
            for (var geoset = 0; geoset < _loadedResources.GeosetCount; geoset++)
            {
                commandList.SetVertexBuffer(0, _sharedResources.VertexBuffers[geoset]);
                commandList.SetIndexBuffer(_sharedResources.IndexBuffers[geoset], IndexFormat.UInt16);

                var layerCount = _loadedResources.Materials[_loadedResources.MaterialIds[geoset]].Layers.Length;
                for (var layer = 0; layer < layerCount; layer++)
                {
                    commandList.SetPipeline(_instanceResources.Pipelines[geoset][layer]);
                    commandList.SetGraphicsResourceSet(0, _instanceResources.ResourceSets[geoset][layer]);
                    commandList.DrawIndexed(_loadedResources.IndexCounts[geoset]);
                }
            }
        }

        public void UpdateAnimation(GraphicsDevice graphicsDevice, float deltaMilliseconds)
        {
            throw new NotImplementedException();
        }

        public static void RecreateResources(bool disposeResources)
        {
            if (disposeResources)
            {
                foreach (var cachedTexture in _cachedTextures.Values)
                {
                    cachedTexture.Dispose();
                }
            }

            _cachedTextures.Clear();

            var resourcesDict = new Dictionary<LoadedModelResources, SharedModelResources>();

            foreach (var cachedResources in _cachedResources.Values)
            {
                if (disposeResources)
                {
                    DisposeSharedResources(cachedResources.SharedResources);
                }

                cachedResources.SharedResources = CreateSharedResources(cachedResources.LoadedResources);
                resourcesDict.Add(cachedResources.LoadedResources, cachedResources.SharedResources);
            }

            foreach (var modelInstance in _createdInstances)
            {
                if (disposeResources)
                {
                    modelInstance.DisposeResources();
                }

                modelInstance._sharedResources = resourcesDict.TryGetValue(modelInstance._loadedResources, out var sharedResourced)
                    ? sharedResourced
                    : CreateSharedResources(modelInstance._loadedResources);
                modelInstance._instanceResources = modelInstance.CreateInstanceResources();
            }

            resourcesDict.Clear();
        }

        private static SharedModelResources CreateSharedResources(LoadedModelResources loadedResources)
        {
            var sharedResources = new SharedModelResources();

            sharedResources.Textures = new TextureView[loadedResources.Textures.Length];
            for (var textureId = 0; textureId < loadedResources.Textures.Length; textureId++)
            {
                var texturePath = loadedResources.Textures[textureId];
                if (string.IsNullOrEmpty(texturePath))
                {
                    var texture = TextureLoader.GetDummyTexture(GraphicsProvider.GraphicsDevice, GraphicsProvider.ResourceFactory);
                    sharedResources.Textures[textureId] = GraphicsProvider.ResourceFactory.CreateTextureView(texture);
                }
                else
                {
                    if (_cachedTextures.TryGetValue(texturePath, out var textureView))
                    {
                        sharedResources.Textures[textureId] = textureView;
                    }
                    else
                    {
                        using (var textureStream = GraphicsProvider.Path2TextureStream(texturePath))
                        {
                            if (textureStream is null)
                            {
                                throw new FileNotFoundException(texturePath);
                            }

                            var texture = TextureLoader.LoadTexture(GraphicsProvider.GraphicsDevice, GraphicsProvider.ResourceFactory, textureStream);

                            textureView = GraphicsProvider.ResourceFactory.CreateTextureView(texture);
                            sharedResources.Textures[textureId] = textureView;

                            if (!string.IsNullOrEmpty(texturePath))
                            {
                                _cachedTextures.Add(texturePath, textureView);
                            }
                        }
                    }
                }
            }

            sharedResources.VertexBuffers = new DeviceBuffer[loadedResources.GeosetCount];
            sharedResources.IndexBuffers = new DeviceBuffer[loadedResources.GeosetCount];

            for (var geoset = 0; geoset < loadedResources.GeosetCount; geoset++)
            {
                sharedResources.VertexBuffers[geoset] = GraphicsProvider.ResourceFactory.CreateBuffer(new BufferDescription(
                    (uint)loadedResources.Vertices[geoset].Length * _animatedVertexByteSize, BufferUsage.VertexBuffer));
                GraphicsProvider.GraphicsDevice.UpdateBuffer(sharedResources.VertexBuffers[geoset], 0, loadedResources.Vertices[geoset]);

                sharedResources.IndexBuffers[geoset] = GraphicsProvider.ResourceFactory.CreateBuffer(new BufferDescription(
                    loadedResources.IndexCounts[geoset] * _ushortByteSize, BufferUsage.IndexBuffer));
                GraphicsProvider.GraphicsDevice.UpdateBuffer(sharedResources.IndexBuffers[geoset], 0, loadedResources.Indices[geoset]);
            }

            return sharedResources;
        }

        private static void DisposeSharedResources(SharedModelResources sharedResources)
        {
            foreach (var vertexBuffer in sharedResources.VertexBuffers)
            {
                vertexBuffer.Dispose();
            }

            foreach (var indexBuffer in sharedResources.IndexBuffers)
            {
                indexBuffer.Dispose();
            }
        }

        private void CreateResources(LoadedModelResources modelResources)
        {
            CreateResources(modelResources, null);
        }

        private void CreateResources(string modelPath)
        {
            CreateResources(null, modelPath);
        }

        private void CreateResources(LoadedModelResources? modelResources, string? modelPath)
        {
            var isModelCached = _cachedResources.TryGetValue(modelPath ?? string.Empty, out var cachedResources);
            if (isModelCached)
            {
                _loadedResources = cachedResources.LoadedResources;
                _sharedResources = cachedResources.SharedResources;
            }
            else
            {
                if (modelResources is null)
                {
                    using (var modelStream = GraphicsProvider.Path2ModelStream(modelPath))
                    {
                        if (modelStream is null)
                        {
                            throw new FileNotFoundException(modelPath);
                        }

                        _loadedResources = ModelLoader.LoadModel(modelStream);
                    }
                }
                else
                {
                    _loadedResources = modelResources;
                }

                _sharedResources = CreateSharedResources(_loadedResources);

                if (!string.IsNullOrEmpty(modelPath))
                {
                    _cachedResources.Add(modelPath, new CachedModelResources
                    {
                        LoadedResources = _loadedResources,
                        SharedResources = _sharedResources,
                    });
                }
            }

            _instanceResources = CreateInstanceResources();
        }

        private InstanceModelResources CreateInstanceResources()
        {
            var instanceResources = new InstanceModelResources();

            instanceResources.TransformationBuffer = GraphicsProvider.ResourceFactory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer | BufferUsage.Dynamic));
            instanceResources.NodeBuffers = new DeviceBuffer[_loadedResources.GeosetCount];
            instanceResources.Pipelines = new Pipeline[_loadedResources.GeosetCount][];
            instanceResources.ResourceSets = new ResourceSet[_loadedResources.GeosetCount][];

            for (var geoset = 0; geoset < _loadedResources.GeosetCount; geoset++)
            {
                instanceResources.NodeBuffers[geoset] = GraphicsProvider.ResourceFactory.CreateBuffer(new BufferDescription(
                    NodeAnimInfo.BlittableSize, BufferUsage.UniformBuffer | BufferUsage.Dynamic));

                var material = _loadedResources.Materials[_loadedResources.MaterialIds[geoset]];
                var layers = material.Layers;
                var hasFSFlag = material.RenderMode.HasFlag(MaterialRenderMode.FullResolution);

                instanceResources.Pipelines[geoset] = new Pipeline[layers.Length];
                instanceResources.ResourceSets[geoset] = new ResourceSet[layers.Length];

                for (var layer = 0; layer < layers.Length; layer++)
                {
                    var cachedPipeline = GraphicsProvider.ResourceFactory.GetPipeline(new SimplePipelineDescription
                    {
                        FilterMode = FilterMode.Blend,
                        FaceType = FaceType.Triangles,
                        OutputDescription = GraphicsProvider.GraphicsDevice.SwapchainFramebuffer.OutputDescription,
                    });

                    instanceResources.Pipelines[geoset][layer] = cachedPipeline.Pipeline;
                    instanceResources.ResourceSets[geoset][layer] = GraphicsProvider.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                        cachedPipeline.ResourceLayout,
                        hasFSFlag ? GraphicsProvider.FullScreenProjectionBuffer : GraphicsProvider.ProjectionBuffer,
                        hasFSFlag ? GraphicsProvider.FullScreenViewBuffer : GraphicsProvider.ViewBuffer,
                        hasFSFlag ? GraphicsProvider.FullScreenWorldBuffer : GraphicsProvider.WorldBuffer,
                        instanceResources.TransformationBuffer,
                        instanceResources.NodeBuffers[geoset],
                        _sharedResources.Textures[layers[layer].TextureId],
                        GraphicsProvider.GraphicsDevice.Aniso4xSampler));
                }
            }

            return instanceResources;
        }

        private void DisposeResources()
        {
            _instanceResources.TransformationBuffer.Dispose();

            for (var geoset = 0; geoset < _loadedResources.GeosetCount; geoset++)
            {
                _instanceResources.NodeBuffers[geoset].Dispose();

                var layerCount = _loadedResources.Materials[_loadedResources.MaterialIds[geoset]].Layers.Length;
                for (var layer = 0; layer < layerCount; layer++)
                {
                    _instanceResources.ResourceSets[geoset][layer].Dispose();
                }
            }
        }

        private void Init()
        {
            _sequence = null;
            _previousAnimMilliseconds = 0d;
            _animationTimeScale = 1f;

            _nodeAnimInfo = NodeAnimInfo.New();
            _nodeTransformations = new Matrix4x4[_loadedResources.Nodes.Length];

            _translation = Matrix4x4.Identity;
            _orientation = Matrix4x4.Identity;
            _scaling = Matrix4x4.Identity;
            UpdateTransformation();

            _createdInstances.Add(this);
        }

        private void UpdateTransformation()
        {
            _transformation = _scaling * _orientation * _translation;
        }

        private static Matrix4x4 CreateRotationMatrix(Quaternion q)
        {
            return Matrix4x4.CreateFromQuaternion(new Quaternion(q.Z, q.X, q.Y, q.W));
        }
    }
}