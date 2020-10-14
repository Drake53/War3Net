// ------------------------------------------------------------------------------
// <copyright file="ModelInstance.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

using Veldrid;

using War3Net.Modeling.DataStructures;
using War3Net.Modeling.Enums;
using War3Net.Rendering.DataStructures;
using War3Net.Rendering.Factories;

namespace War3Net.Rendering
{
    public sealed class ModelInstance
    {
        public sealed class ModelBatch
        {
            public HashSet<ModelInstance> Models { get; set; }

            public void Draw(CommandList commandList)
            {
                var firstModel = Models.FirstOrDefault();
                if (firstModel is null)
                {
                    return;
                }

                for (var geoset = 0; geoset < firstModel._loadedResources.GeosetCount; geoset++)
                {
                    commandList.SetVertexBuffer(0, firstModel._sharedResources.VertexBuffers[geoset]);
                    commandList.SetIndexBuffer(firstModel._sharedResources.IndexBuffers[geoset], IndexFormat.UInt16);

                    var layerCount = firstModel._loadedResources.Materials[firstModel._loadedResources.MaterialIds[geoset]].Layers.Length;
                    for (var layer = 0; layer < layerCount; layer++)
                    {
                        foreach (var model in Models)
                        {
                            commandList.SetPipeline(model._instanceResources.Pipelines[geoset][layer]);
                            commandList.SetGraphicsResourceSet(0, model._instanceResources.ResourceSets[geoset][layer]);

                            if (model._modelType == ModelType.Sky)
                            {
                                var gd = GraphicsProvider.GraphicsDevice;
                                var depth = gd.IsDepthRangeZeroToOne ? 1f : 0f;
                                commandList.SetViewport(0, new Viewport(0, 0, gd.SwapchainFramebuffer.Width, gd.SwapchainFramebuffer.Height, depth, depth));
                                commandList.DrawIndexed(model._loadedResources.IndexCounts[geoset]);
                                commandList.SetViewport(0, new Viewport(0, 0, gd.SwapchainFramebuffer.Width, gd.SwapchainFramebuffer.Height, 0, 1));
                            }
                            else
                            {
                                commandList.DrawIndexed(model._loadedResources.IndexCounts[geoset]);
                            }
                        }
                    }
                }
            }
        }

        private const uint _ushortByteSize = sizeof(ushort);
        private static readonly uint _animatedVertexByteSize = (uint)Unsafe.SizeOf<Vertex>();

        private static readonly Dictionary<string, CachedModelResources> _cachedResources = new Dictionary<string, CachedModelResources>();
        private static readonly Dictionary<string, TextureView> _cachedTextures = new Dictionary<string, TextureView>();

        public static readonly HashSet<ModelInstance> _createdInstances = new HashSet<ModelInstance>();
        public static readonly Dictionary<CachedModelResources, ModelBatch> _modelBatches = new Dictionary<CachedModelResources, ModelBatch>();

        private readonly int _cameraIndex;
        private readonly ModelType _modelType;

        private LoadedModelResources _loadedResources;
        private SharedModelResources _sharedResources;
        private InstanceModelResources _instanceResources;
        private ModelBatch _batch;

        private Sequence? _sequence;
        private double _previousAnimMilliseconds;
        private float _animationTimeScale;

        private NodeAnimInfo _nodeAnimInfo;
        private Matrix4x4[] _nodeTransformations;

        private Matrix4x4 _transformation;
        private Matrix4x4 _translation;
        private Matrix4x4 _orientation;
        private Matrix4x4 _scaling;

        public ModelInstance(string modelPath, ModelType modelType = ModelType.Default)
        {
            _modelType = modelType;

            CreateResources(modelPath);
            Init();
        }

        public ModelInstance(string modelPath, int cameraIndex, ModelType modelType = ModelType.Default)
        {
            _cameraIndex = cameraIndex;
            _modelType = modelType;

            CreateResources(modelPath);
            Init();
        }

        public ModelInstance(BasicMesh mesh, ModelType modelType = ModelType.Default)
        {
            _modelType = modelType;

            CreateResources(new LoadedModelResources
            {
                GeosetCount = 1,
                IndexCounts = new[] { (uint)mesh.Indices.Count },
                Vertices = new Vertex[1][] { mesh.Vertices.ToArray() },
                Indices = new ushort[1][] { mesh.Indices.ToArray() },
                Textures = mesh.TexturePaths.Select(texture => texture ?? string.Empty).ToArray(),
                VertexGroups = new uint[1][][] { new uint[1][] { new uint[1] { 0U } } },
                MaterialIds = new uint[1] { 0U },
                Nodes = new NodeData[] { new NodeData { Name = "Root", PivotPoint = Vector3.Zero, ObjectId = 0, ParentId = uint.MaxValue } },
                Materials = new Material[] { new Material { RenderMode = 0, Layers = new[] { new Layer() { TextureId = 0 } } } },
                Sequences = Array.Empty<Sequence>(),
            });
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

            foreach (var modelBatch in _modelBatches)
            {
                var cachedResources = modelBatch.Key;
                if (disposeResources)
                {
                    DisposeSharedResources(cachedResources.SharedResources);
                }

                var newSharedResources = CreateSharedResources(cachedResources.LoadedResources);
                cachedResources.SharedResources = newSharedResources;

                foreach (var model in modelBatch.Value.Models)
                {
                    if (disposeResources)
                    {
                        model.DisposeResources();
                    }

                    model._sharedResources = newSharedResources;
                    model._instanceResources = model.CreateInstanceResources();
                }
            }
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

                _batch = _modelBatches[cachedResources];
            }
            else
            {
                if (modelResources is null)
                {
                    using (var modelStream = GraphicsProvider.Path2ModelStream(modelPath!))
                    {
                        if (modelStream is null)
                        {
                            throw new FileNotFoundException(modelPath);
                        }

                        try
                        {
                            _loadedResources = ModelLoader.LoadModel(modelStream);
                        }
                        catch (Exception e)
                        {
                            throw new InvalidDataException($"Unable to load model: {modelPath}", e);
                        }
                    }
                }
                else
                {
                    _loadedResources = modelResources;
                }

                _sharedResources = CreateSharedResources(_loadedResources);

                cachedResources = new CachedModelResources
                {
                    LoadedResources = _loadedResources,
                    SharedResources = _sharedResources,
                };

                _batch = new ModelBatch
                {
                    Models = new HashSet<ModelInstance>(),
                };

                _modelBatches.Add(cachedResources, _batch);

                if (!string.IsNullOrEmpty(modelPath))
                {
                    _cachedResources.Add(modelPath, cachedResources);
                }
            }

            _instanceResources = CreateInstanceResources();

            _batch.Models.Add(this);
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

                DeviceBuffer projection, view, world;
                var modelType = material.RenderMode.HasFlag(MaterialRenderMode.FullResolution) ? ModelType.FullResolution : _modelType;
                switch (modelType)
                {
                    case ModelType.Default:
                    case ModelType.Sky:
                        projection = GraphicsProvider.ProjectionBuffer;
                        view = GraphicsProvider.ViewBuffer;
                        world = GraphicsProvider.WorldBuffer;
                        break;

                    case ModelType.FullResolution:
                        projection = GraphicsProvider.FullResolutionProjectionBuffer;
                        view = GraphicsProvider.FullResolutionViewBuffer;
                        world = GraphicsProvider.FullResolutionWorldBuffer;
                        break;

                    case ModelType.UI:
                        projection = GraphicsProvider.UIProjectionBuffer;
                        view = GraphicsProvider.UIViewBuffer;
                        world = GraphicsProvider.UIWorldBuffer;
                        break;

                    default: throw new InvalidEnumArgumentException(nameof(modelType), (int)modelType, typeof(ModelType));
                }

                var layers = material.Layers;
                instanceResources.Pipelines[geoset] = new Pipeline[layers.Length];
                instanceResources.ResourceSets[geoset] = new ResourceSet[layers.Length];

                for (var layer = 0; layer < layers.Length; layer++)
                {
                    var cachedPipeline = GraphicsProvider.ResourceFactory.GetPipeline(new SimplePipelineDescription
                    {
                        FilterMode = layers[layer].FilterMode,
                        FaceType = FaceType.Triangles,
                        LayerShading = layers[layer].ShadingFlags,
                        OutputDescription = GraphicsProvider.GraphicsDevice.SwapchainFramebuffer.OutputDescription,
                    });

                    instanceResources.Pipelines[geoset][layer] = cachedPipeline.Pipeline;
                    instanceResources.ResourceSets[geoset][layer] = GraphicsProvider.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                        cachedPipeline.ResourceLayout,
                        projection,
                        view,
                        world,
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

        public void Destroy()
        {
            _batch.Models.Remove(this);
            if (_batch.Models.Count == 0)
            {
                // DisposeSharedResources(_sharedResources);
            }

            DisposeResources();

            _createdInstances.Remove(this);
        }

        private void UpdateTransformation()
        {
            _transformation = _scaling * _orientation * _translation;
        }

        public void SetTranslation(float x, float y, float z)
        {
            _translation = Matrix4x4.CreateTranslation(x, y, z);
            UpdateTransformation();
        }

        public void SetOrientation(float yaw, float pitch, float roll)
        {
            _orientation = CreateRotationMatrix(Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll));
            UpdateTransformation();
        }

        public void SetScale(float scale)
        {
            _scaling = Matrix4x4.CreateScale(scale);
            UpdateTransformation();
        }

        public void SetScale(float scaleX, float scaleY, float scaleZ)
        {
            _scaling = Matrix4x4.CreateScale(scaleX, scaleY, scaleZ);
            UpdateTransformation();
        }

        private static Matrix4x4 CreateRotationMatrix(Quaternion q)
        {
            return Matrix4x4.CreateFromQuaternion(new Quaternion(q.Z, q.X, q.Y, q.W));
        }
    }
}