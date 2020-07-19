// ------------------------------------------------------------------------------
// <copyright file="ModelLoader.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

using Veldrid;

using War3Net.Drawing.Blp;
using War3Net.Modeling.Models;
using War3Net.Rendering.Extensions;

namespace War3Net.Rendering
{
    public static class ModelLoader
    {
        public static ModelResources LoadModel(GraphicsDevice graphicsDevice, Model model, params string[] paths)
        {
            var factory = graphicsDevice.ResourceFactory;

            // Translate and scale vertices so they fit in the window (range -1 to 1).
            var minX = float.MaxValue;
            var maxX = float.MinValue;
            var minY = float.MaxValue;
            var maxY = float.MinValue;
            var minZ = float.MaxValue;
            var maxZ = float.MinValue;
            foreach (var geoset in model.Geosets)
            {
                minX = MathF.Min(minX, geoset.Vertices.Min(vertex => vertex.X));
                maxX = MathF.Max(maxX, geoset.Vertices.Max(vertex => vertex.X));
                minY = MathF.Min(minY, geoset.Vertices.Min(vertex => vertex.Y));
                maxY = MathF.Max(maxY, geoset.Vertices.Max(vertex => vertex.Y));
                minZ = MathF.Min(minZ, geoset.Vertices.Min(vertex => vertex.Z));
                maxZ = MathF.Max(maxZ, geoset.Vertices.Max(vertex => vertex.Z));
            }

            var translateX = (minX + maxX) / -2;
            var translateY = (minY + maxY) / -2;
            var translateZ = ((minZ + maxZ) / -2) + (0.1f - minZ);

            var scaleX = 2 / (maxX - minX);
            var scaleY = 2 / (maxY - minY);
            var scale = new[] { scaleX, scaleY }.Min();

            var transform = Matrix4x4.CreateTranslation(translateX, translateY, translateZ) * Matrix4x4.CreateScale(scale);

            var geosetResources = new List<GeosetResources>();
            foreach (var geoset in model.Geosets)
            {
                geosetResources.Add(LoadGeoset(graphicsDevice, geoset));
            }

            // TODO: support multiple layers
            var textureFileName = model.Textures[(int)model.Materials[(int)model.Geosets.First().MaterialId].Layers.First().TextureId].FileName;
            string textureFilePath = null;

            FileInfo textureFileInfo = null;
            foreach (var path in paths)
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    continue;
                }

                textureFilePath = Path.Combine(path, textureFileName);
                textureFileInfo = new FileInfo(textureFilePath);
                if (textureFileInfo.Exists)
                {
                    break;
                }
            }

            if (!(textureFileInfo?.Exists ?? false))
            {
                throw new FileNotFoundException(null, textureFileName);
            }

            var textureFileExtension = textureFileInfo.Extension;
            Veldrid.Texture texture;
            if (string.Equals(textureFileExtension, ".blp", StringComparison.OrdinalIgnoreCase))
            {
                using var fileStream = File.OpenRead(textureFilePath);
                using var blpFile = new BlpFile(fileStream);

                var textureData = blpFile.GetPixels(0, out var width, out var height);
                var textureDescription = TextureDescription.Texture2D((uint)width, (uint)height, /*(uint)blpFile.MipMapCount*/ 1, 1, PixelFormat.B8_G8_R8_A8_UNorm, TextureUsage.Sampled);

                texture = factory.CreateTexture(textureDescription);
                graphicsDevice.UpdateTexture(texture, textureData, 0, 0, 0, (uint)width, (uint)height, 1, 0, 0);
            }
            else
            {
                throw new NotSupportedException("Only BLP textures supported.");
            }

            return new ModelResources
            {
                GeosetResources = geosetResources,
                Texture = texture,
                Transform = transform,
            };
        }

        public static GeosetResources LoadGeoset(GraphicsDevice graphicsDevice, Geoset geoset)
        {
            var factory = graphicsDevice.ResourceFactory;

            var vertexUVs = geoset.TextureCoordinateSets.Single().TextureCoordinates;
            if (vertexUVs.Count != geoset.Normals.Count || vertexUVs.Count != geoset.Vertices.Count)
            {
                throw new InvalidDataException();
            }

            var vertexNormals = geoset.Normals;
            var vertexPositions = geoset.Vertices;

            var vertices = new Vertex[vertexUVs.Count];
            for (var i = 0; i < vertexUVs.Count; i++)
            {
                vertices[i] = new Vertex
                {
                    UV = vertexUVs[i],
                    Normal = vertexNormals[i],
                    Position = vertexPositions[i],
                };
            }

            var vertexIndices = geoset.Faces.ToArray();
            var indexCount = (uint)vertexIndices.Length;

            var vertexBuffer = factory.CreateBuffer(new BufferDescription((uint)vertices.Length * Vertex.SizeInBytes, BufferUsage.VertexBuffer));
            var indexBuffer = factory.CreateBuffer(new BufferDescription((uint)vertexIndices.Length * sizeof(ushort), BufferUsage.IndexBuffer));

            graphicsDevice.UpdateBuffer(vertexBuffer, 0, vertices);
            graphicsDevice.UpdateBuffer(indexBuffer, 0, vertexIndices);

            return new GeosetResources
            {
                VertexIndicesCount = indexCount,
                PrimitiveTopology = geoset.FaceTypeGroups.Single().ToPrimitiveTopology(),
                VertexBuffer = vertexBuffer,
                IndexBuffer = indexBuffer,
            };
        }
    }
}