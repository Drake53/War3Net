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

using War3Net.Modeling;
using War3Net.Rendering.DataStructures;
using War3Net.Rendering.Extensions;

namespace War3Net.Rendering
{
    public static class ModelLoader
    {
        public static LoadedModelResources LoadModel(Stream stream, bool leaveOpen = false)
        {
            var model = BinaryModelParser.IsBinaryModel(stream)
                ? BinaryModelParser.Parse(stream, leaveOpen)
                : throw new NotImplementedException();

            var vertices = new Vertex[model.Geosets.Length][];
            var indices = new ushort[model.Geosets.Length][];
            for (var geosetId = 0; geosetId < model.Geosets.Length; geosetId++)
            {
                var geoset = model.Geosets[geosetId];
                var geosetVertices = new List<Vertex>();
                var textureCoordinates = geoset.TextureCoordinateSets.Single().TextureCoordinates;

                for (var i = 0; i < geoset.Vertices.Length; i++)
                {
                    geosetVertices.Add(new Vertex
                    {
                        Position = geoset.Vertices[i],
                        // Normal = geoset.Normals[i],
                        UV = textureCoordinates[i],
                        VertexGroup = geoset.VertexGroups[i],
                    });
                }

                vertices[geosetId] = geosetVertices.ToArray();
                // indices[geosetId] = geoset.Faces;

                var indicesList = new List<ushort>();
                if (geoset.FaceGroups.Length != geoset.FaceTypeGroups.Length)
                {
                    throw new InvalidDataException();
                }

                var start = 0;
                for (var i = 0; i < geoset.FaceGroups.Length; i++)
                {
                    var count = (int)geoset.FaceGroups[i];
                    var end = start + count;
                    indicesList.AddRange(geoset.Faces[start..end].ToTrianglesIndices(geoset.FaceTypeGroups[i]));
                    start += count;
                }

                indices[geosetId] = indicesList.ToArray();
            }

            var boneMatrixGroups = new Dictionary<int, uint[][]>();
            var geosetVertexGroups = new uint[model.Geosets.Length][][];
            for (var geosetIndex = 0; geosetIndex < model.Geosets.Length; geosetIndex++)
            {
                var geoset = model.Geosets[geosetIndex];
                var matrixGroupOffset = 0;
                var vertexGroups = new uint[geoset.MatrixGroups.Length][];
                for (var matrixGroupIndex = 0; matrixGroupIndex < geoset.MatrixGroups.Length; matrixGroupIndex++)
                {
                    var matrixGroupSize = (int)geoset.MatrixGroups[matrixGroupIndex];
                    var matrixIndices = new uint[matrixGroupSize];
                    Buffer.BlockCopy(geoset.MatrixIndices, matrixGroupOffset, matrixIndices, 0, matrixGroupSize * sizeof(uint));
                    vertexGroups[matrixGroupIndex] = matrixIndices;

                    matrixGroupOffset += matrixGroupSize * sizeof(uint);
                }

                boneMatrixGroups.Add(geosetIndex, vertexGroups);
                geosetVertexGroups[geosetIndex] = vertexGroups;
            }

            var highestObjectId = 0U;
            var nodes = new Dictionary<uint, NodeData>();
            foreach (var modelNode in model.GetNodes())
            {
                var node = new NodeData();
                node.Name = modelNode.Name;
                node.PivotPoint = model.PivotPoints[(int)modelNode.ObjectId];
                node.ObjectId = modelNode.ObjectId;
                node.ParentId = modelNode.ParentId;
                node.Translations = modelNode.Translations;
                node.Rotations = modelNode.Rotations;
                node.Scalings = modelNode.Scalings;
                nodes.Add(modelNode.ObjectId, node);

                highestObjectId = highestObjectId < node.ObjectId ? node.ObjectId : highestObjectId;
            }

            if (highestObjectId != nodes.Count - 1)
            {
                throw new InvalidDataException();
            }

            foreach (var node in nodes.Values)
            {
                if (!node.ParentId.HasValue || node.ParentId.Value == uint.MaxValue)
                {
                    continue;
                }

                var parent = nodes[node.ParentId.Value];
                node.Parent = parent;
                parent.Children.Add(node);
            }

            return new LoadedModelResources
            {
                GeosetCount = vertices.Length,
                IndexCounts = indices.Select(indices => (uint)indices.Length).ToArray(),
                Vertices = vertices,
                Indices = indices,
                Textures = model.Textures.Select(texture => texture.FileName ?? string.Empty).ToArray(),
                VertexGroups = geosetVertexGroups,
                MaterialIds = model.Geosets.Select(geoset => geoset.MaterialId).ToArray(),
                Nodes = nodes.Values.OrderBy(node => node.ObjectId).ToArray(),
                Materials = model.Materials,
                Sequences = model.Sequences,
            };
        }
    }
}