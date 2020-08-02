// ------------------------------------------------------------------------------
// <copyright file="BinaryModelParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

using War3Net.Common.Extensions;
using War3Net.Modeling.DataStructures;
using War3Net.Modeling.Enums;

namespace War3Net.Modeling
{
    public static class BinaryModelParser
    {
        private static readonly Dictionary<int, OptionalModelProperty> _modelProperties = GetModelProperties().ToDictionary(property => property.Tag);
        private static readonly Dictionary<int, OptionalModelProperty> _nodeProperties = GetNodeProperties().ToDictionary(property => property.Tag);
        private static readonly Dictionary<int, OptionalModelProperty> _layerProperties = GetLayerProperties().ToDictionary(property => property.Tag);
        private static readonly Dictionary<int, OptionalModelProperty> _textureAnimationProperties = GetTextureAnimationProperties().ToDictionary(property => property.Tag);
        private static readonly Dictionary<int, OptionalModelProperty> _geosetAnimationProperties = GetGeosetAnimationProperties().ToDictionary(property => property.Tag);
        private static readonly Dictionary<int, OptionalModelProperty> _lightProperties = GetLightProperties().ToDictionary(property => property.Tag);
        private static readonly Dictionary<int, OptionalModelProperty> _attachmentProperties = GetAttachmentProperties().ToDictionary(property => property.Tag);
        private static readonly Dictionary<int, OptionalModelProperty> _particleEmitterProperties = GetParticleEmitterProperties().ToDictionary(property => property.Tag);
        private static readonly Dictionary<int, OptionalModelProperty> _particleEmitter2Properties = GetParticleEmitter2Properties().ToDictionary(property => property.Tag);
        private static readonly Dictionary<int, OptionalModelProperty> _ribbonEmitterProperties = GetRibbonEmitterProperties().ToDictionary(property => property.Tag);
        private static readonly Dictionary<int, OptionalModelProperty> _cameraProperties = GetCameraProperties().ToDictionary(property => property.Tag);

        public static Model Parse(Stream input, bool leaveOpen)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            using var reader = new BinaryReader(input, Encoding.UTF8, leaveOpen);

            var header = reader.ReadUInt32();
            if (header != BinaryConstants.Header)
            {
                throw new InvalidDataException();
            }

            var boxedModel = RuntimeHelpers.GetObjectValue(default(Model));
            while (reader.PeekChar() != -1)
            {
                var chunkTag = reader.ReadInt32();
                if (_modelProperties.TryGetValue(chunkTag, out var property))
                {
                    try
                    {
                        property.ParseForObject(boxedModel, reader);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidDataException($"Duplicate model chunk: '{chunkTag.ToRawcode()}'", e);
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown chunk tag: '{chunkTag.ToRawcode()}'");
                    input.Seek(reader.ReadUInt32(), SeekOrigin.Current);
                }
            }

            return (Model)boxedModel;
        }

        private static ModelVersion ParseVersion(BinaryReader reader)
        {
            var chunkSize = reader.ReadUInt32();
            if (chunkSize != BinaryConstants.VersionChunkSize)
            {
                throw new InvalidDataException();
            }

            var modelVersion = new ModelVersion();
            modelVersion.FormatVersion = (FormatVersion)reader.ReadUInt32();

            return modelVersion;
        }

        private static ModelInfo ParseModelInfo(BinaryReader reader)
        {
            var chunkSize = reader.ReadUInt32();
            if (chunkSize != BinaryConstants.ModelInfoChunkSize)
            {
                throw new InvalidDataException();
            }

            var modelInfo = new ModelInfo();
            modelInfo.Name = reader.ReadString(BinaryConstants.FixedStringSizeShort);
            modelInfo.AnimationFileName = reader.ReadString(BinaryConstants.FixedStringSizeLong);
            modelInfo.Extent = ParseExtent(reader);
            modelInfo.BlendTime = reader.ReadSingle();

            return modelInfo;
        }

        private static IEnumerable<Sequence> ParseSequences(BinaryReader reader)
        {
            var chunkSize = reader.ReadUInt32();
            if ((chunkSize % BinaryConstants.SequenceSize) != 0)
            {
                throw new InvalidDataException();
            }

            var chunkEnd = reader.BaseStream.Position + chunkSize;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseSequence(reader);
            }
        }

        private static Sequence ParseSequence(BinaryReader reader)
        {
            var sequence = new Sequence();
            sequence.Name = reader.ReadString(BinaryConstants.FixedStringSizeShort);
            sequence.IntervalStart = reader.ReadUInt32();
            sequence.IntervalEnd = reader.ReadUInt32();
            sequence.MoveSpeed = reader.ReadSingle();
            sequence.Flags = (SequenceFlags)reader.ReadUInt32();
            sequence.Rarity = reader.ReadSingle();
            sequence.SyncPoint = reader.ReadUInt32();
            sequence.Extent = ParseExtent(reader);

            return sequence;
        }

        private static IEnumerable<GlobalSequence> ParseGlobalSequences(BinaryReader reader)
        {
            var chunkSize = reader.ReadUInt32();
            if ((chunkSize % BinaryConstants.GlobalSequenceSize) != 0)
            {
                throw new InvalidDataException();
            }

            var chunkEnd = reader.BaseStream.Position + chunkSize;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseGlobalSequence(reader);
            }
        }

        private static GlobalSequence ParseGlobalSequence(BinaryReader reader)
        {
            var globalSequence = new GlobalSequence();
            globalSequence.Duration = reader.ReadInt32();

            return globalSequence;
        }

        private static IEnumerable<Material> ParseMaterials(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseMaterial(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException($"Invalid size for chunk '{BinaryConstants.MaterialsChunk.ToRawcode()}'.");
            }
        }

        private static Material ParseMaterial(BinaryReader reader)
        {
            var materialEnd = reader.BaseStream.Position + reader.ReadUInt32();

            var material = new Material();
            material.PriorityPlane = reader.ReadUInt32();
            material.RenderMode = (MaterialRenderMode)reader.ReadUInt32();
            material.Layers = ParseLayers(reader).ToArray();

            if (reader.BaseStream.Position != materialEnd)
            {
                throw new InvalidDataException();
            }

            return material;
        }

        private static IEnumerable<Layer> ParseLayers(BinaryReader reader)
        {
            if (reader.ReadUInt32() != BinaryConstants.LayersChunk)
            {
                throw new InvalidDataException();
            }

            var count = reader.ReadUInt32();
            for (var i = 0; i < count; i++)
            {
                yield return ParseLayer(reader);
            }
        }

        private static Layer ParseLayer(BinaryReader reader)
        {
            var layerEnd = reader.BaseStream.Position + reader.ReadUInt32();

            var layer = new Layer();
            layer.FilterMode = (FilterMode)reader.ReadUInt32();
            layer.ShadingFlags = (LayerShading)reader.ReadUInt32();
            layer.TextureId = reader.ReadUInt32();
            layer.TextureAnimationId = reader.ReadUInt32();
            layer.CoordId = reader.ReadUInt32();
            layer.Alpha = reader.ReadSingle();

            while (reader.BaseStream.Position < layerEnd)
            {
                var optionalTag = reader.ReadInt32();
                if (_layerProperties.TryGetValue(optionalTag, out var property))
                {
                    try
                    {
                        property.ParseForObject(layer, reader);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidDataException($"Duplicate layer tag: '{optionalTag.ToRawcode()}'", e);
                    }
                }
                else
                {
                    throw new NotSupportedException($"Unknown layer tag: '{optionalTag.ToRawcode()}'");
                }
            }

            if (reader.BaseStream.Position != layerEnd)
            {
                throw new InvalidDataException();
            }

            return layer;
        }

        private static IEnumerable<Texture> ParseTextures(BinaryReader reader)
        {
            var chunkSize = reader.ReadUInt32();
            if ((chunkSize % BinaryConstants.TextureSize) != 0)
            {
                throw new InvalidDataException();
            }

            var chunkEnd = reader.BaseStream.Position + chunkSize;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseTexture(reader);
            }
        }

        private static Texture ParseTexture(BinaryReader reader)
        {
            var texture = new Texture();
            texture.ReplaceableId = reader.ReadUInt32();
            texture.FileName = reader.ReadString(BinaryConstants.FixedStringSizeLong);
            texture.Flags = (TextureFlags)reader.ReadUInt32();

            return texture;
        }

        private static IEnumerable<TextureAnimation> ParseTextureAnimations(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseTextureAnimation(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static TextureAnimation ParseTextureAnimation(BinaryReader reader)
        {
            var textureAnimationEnd = reader.BaseStream.Position + reader.ReadUInt32();

            var textureAnimation = new TextureAnimation();

            while (reader.BaseStream.Position < textureAnimationEnd)
            {
                var optionalTag = reader.ReadInt32();
                if (_textureAnimationProperties.TryGetValue(optionalTag, out var property))
                {
                    try
                    {
                        property.ParseForObject(textureAnimation, reader);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidDataException($"Duplicate texture animation tag: '{optionalTag.ToRawcode()}'", e);
                    }
                }
                else
                {
                    throw new NotSupportedException($"Unknown texture animation tag: '{optionalTag.ToRawcode()}'");
                }
            }

            if (reader.BaseStream.Position != textureAnimationEnd)
            {
                throw new InvalidDataException();
            }

            return textureAnimation;
        }

        private static IEnumerable<Geoset> ParseGeosets(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseGeoset(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static Geoset ParseGeoset(BinaryReader reader)
        {
            var geosetEnd = reader.BaseStream.Position + reader.ReadUInt32();

            var geoset = new Geoset();
            geoset.Vertices = ParseVertices(reader).ToArray();
            geoset.Normals = ParseNormals(reader).ToArray();
            geoset.FaceTypeGroups = ParseFaceTypeGroups(reader).ToArray();
            geoset.FaceGroups = ParseFaceGroups(reader).ToArray();
            geoset.Faces = ParseFaces(reader).ToArray();
            geoset.VertexGroups = ParseVertexGroups(reader).ToArray();
            geoset.MatrixGroups = ParseMatrixGroups(reader).ToArray();
            geoset.MatrixIndices = ParseMatrixIndices(reader).ToArray();
            geoset.MaterialId = reader.ReadUInt32();
            geoset.SelectionGroup = reader.ReadUInt32();
            geoset.SelectionFlags = (GeosetSelectionFlags)reader.ReadUInt32();
            geoset.Extent = ParseExtent(reader);

            var sequenceExtentCount = reader.ReadUInt32();
            geoset.SequenceExtents = ParseExtents(reader, sequenceExtentCount).ToArray();
            geoset.TextureCoordinateSets = ParseTextureCoordinateSets(reader).ToArray();

            if (reader.BaseStream.Position != geosetEnd)
            {
                throw new InvalidDataException();
            }

            return geoset;
        }

        private static IEnumerable<Vector3> ParseVertices(BinaryReader reader)
        {
            if (reader.ReadInt32() != BinaryConstants.VerticesChunk)
            {
                throw new InvalidDataException($"Expected vertices chunk: '{BinaryConstants.VersionChunk.ToRawcode()}'");
            }

            var vertexCount = reader.ReadUInt32();
            for (var i = 0; i < vertexCount; i++)
            {
                yield return ParseVector3(reader);
            }
        }

        private static IEnumerable<Vector3> ParseNormals(BinaryReader reader)
        {
            if (reader.ReadInt32() != BinaryConstants.NormalsChunk)
            {
                throw new InvalidDataException($"Expected normals chunk: '{BinaryConstants.NormalsChunk.ToRawcode()}'");
            }

            var normalsCount = reader.ReadUInt32();
            for (var i = 0; i < normalsCount; i++)
            {
                yield return ParseVector3(reader);
            }
        }

        private static IEnumerable<FaceType> ParseFaceTypeGroups(BinaryReader reader)
        {
            if (reader.ReadInt32() != BinaryConstants.FaceTypeGroupsChunk)
            {
                throw new InvalidDataException($"Expected face type groups chunk: '{BinaryConstants.FaceTypeGroupsChunk.ToRawcode()}'");
            }

            var faceTypeGroupsCount = reader.ReadUInt32();
            for (var i = 0; i < faceTypeGroupsCount; i++)
            {
                yield return (FaceType)reader.ReadUInt32();
            }
        }

        private static IEnumerable<uint> ParseFaceGroups(BinaryReader reader)
        {
            if (reader.ReadInt32() != BinaryConstants.FaceGroupsChunk)
            {
                throw new InvalidDataException($"Expected face groups chunk: '{BinaryConstants.FaceGroupsChunk.ToRawcode()}'");
            }

            var faceGroupsCount = reader.ReadUInt32();
            for (var i = 0; i < faceGroupsCount; i++)
            {
                yield return reader.ReadUInt32();
            }
        }

        private static IEnumerable<ushort> ParseFaces(BinaryReader reader)
        {
            if (reader.ReadInt32() != BinaryConstants.FacesChunk)
            {
                throw new InvalidDataException($"Expected faces chunk: '{BinaryConstants.FacesChunk.ToRawcode()}'");
            }

            var facesCount = reader.ReadUInt32();
            for (var i = 0; i < facesCount; i++)
            {
                yield return reader.ReadUInt16();
            }
        }

        private static IEnumerable<byte> ParseVertexGroups(BinaryReader reader)
        {
            if (reader.ReadInt32() != BinaryConstants.VertexGroupsChunk)
            {
                throw new InvalidDataException($"Expected vertex groups chunk: '{BinaryConstants.VertexGroupsChunk.ToRawcode()}'");
            }

            var vertexGroupsCount = reader.ReadUInt32();
            for (var i = 0; i < vertexGroupsCount; i++)
            {
                yield return reader.ReadByte();
            }
        }

        private static IEnumerable<uint> ParseMatrixGroups(BinaryReader reader)
        {
            if (reader.ReadInt32() != BinaryConstants.MatrixGroupsChunk)
            {
                throw new InvalidDataException($"Expected matrix groups chunk: '{BinaryConstants.MatrixGroupsChunk.ToRawcode()}'");
            }

            var matrixGroupsCount = reader.ReadUInt32();
            for (var i = 0; i < matrixGroupsCount; i++)
            {
                yield return reader.ReadUInt32();
            }
        }

        private static IEnumerable<uint> ParseMatrixIndices(BinaryReader reader)
        {
            if (reader.ReadInt32() != BinaryConstants.MatrixIndicesChunk)
            {
                throw new InvalidDataException($"Expected matrix indices chunk: '{BinaryConstants.MatrixIndicesChunk.ToRawcode()}'");
            }

            var matrixIndicesCount = reader.ReadUInt32();
            for (var i = 0; i < matrixIndicesCount; i++)
            {
                yield return reader.ReadUInt32();
            }
        }

        private static IEnumerable<Geoset.TextureCoordinateSet> ParseTextureCoordinateSets(BinaryReader reader)
        {
            if (reader.ReadInt32() != BinaryConstants.TextureCoordinateSetsChunk)
            {
                throw new InvalidDataException($"Expected texture coordinate sets chunk: '{BinaryConstants.TextureCoordinateSetsChunk.ToRawcode()}'");
            }

            var textureCoordinateSetsCount = reader.ReadUInt32();
            for (var i = 0; i < textureCoordinateSetsCount; i++)
            {
                yield return ParseTextureCoordinateSet(reader);
            }
        }

        private static Geoset.TextureCoordinateSet ParseTextureCoordinateSet(BinaryReader reader)
        {
            if (reader.ReadInt32() != BinaryConstants.TextureCoordinateSetChunk)
            {
                throw new InvalidDataException($"Expected texture coordinate set chunk: '{BinaryConstants.TextureCoordinateSetChunk.ToRawcode()}'");
            }

            var textureCoordinateSet = new Geoset.TextureCoordinateSet();

            var textureCoordinateSetSize = reader.ReadUInt32();
            textureCoordinateSet.TextureCoordinates = ParseTextureCoordinates(reader, textureCoordinateSetSize).ToArray();

            return textureCoordinateSet;
        }

        private static IEnumerable<Vector2> ParseTextureCoordinates(BinaryReader reader, uint count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return ParseVector2(reader);
            }
        }

        private static IEnumerable<GeosetAnimation> ParseGeosetAnimations(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseGeosetAnimation(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static GeosetAnimation ParseGeosetAnimation(BinaryReader reader)
        {
            var geosetAnimationEnd = reader.BaseStream.Position + reader.ReadUInt32();

            var geosetAnimation = new GeosetAnimation();
            geosetAnimation.Alpha = reader.ReadSingle();
            geosetAnimation.Flags = (GeosetAnimationFlags)reader.ReadUInt32();
            geosetAnimation.Color = ParseVector3(reader);
            geosetAnimation.GeosetId = reader.ReadUInt32();

            while (reader.BaseStream.Position < geosetAnimationEnd)
            {
                var optionalTag = reader.ReadInt32();
                if (_geosetAnimationProperties.TryGetValue(optionalTag, out var property))
                {
                    try
                    {
                        property.ParseForObject(geosetAnimation, reader);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidDataException($"Duplicate geoset animation tag: '{optionalTag.ToRawcode()}'", e);
                    }
                }
                else
                {
                    throw new NotSupportedException($"Unknown geoset animation tag: '{optionalTag.ToRawcode()}'");
                }
            }

            if (reader.BaseStream.Position != geosetAnimationEnd)
            {
                throw new InvalidDataException();
            }

            return geosetAnimation;
        }

        private static IEnumerable<Bone> ParseBones(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseBone(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static Bone ParseBone(BinaryReader reader)
        {
            INode node = new Bone();
            ParseNode(reader, ref node);

            var bone = (Bone)node;
            bone.GeosetId = reader.ReadUInt32();
            bone.GeosetAnimationId = reader.ReadUInt32();

            return bone;
        }

        private static IEnumerable<Light> ParseLights(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseLight(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static Light ParseLight(BinaryReader reader)
        {
            var lightEnd = reader.BaseStream.Position + reader.ReadUInt32();

            INode node = new Light();
            ParseNode(reader, ref node);

            var light = (Light)node;
            light.Type = (LightType)reader.ReadUInt32();
            light.AttenuationStart = reader.ReadSingle();
            light.AttenuationEnd = reader.ReadSingle();
            light.Color = ParseVector3(reader);
            light.Intensity = reader.ReadSingle();
            light.AmbientColor = ParseVector3(reader);
            light.AmbientIntensity = reader.ReadSingle();

            while (reader.BaseStream.Position < lightEnd)
            {
                var optionalTag = reader.ReadInt32();
                if (_lightProperties.TryGetValue(optionalTag, out var property))
                {
                    try
                    {
                        property.ParseForObject(light, reader);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidDataException($"Duplicate light tag: '{optionalTag.ToRawcode()}'", e);
                    }
                }
                else
                {
                    throw new NotSupportedException($"Unknown light tag: '{optionalTag.ToRawcode()}'");
                }
            }

            if (reader.BaseStream.Position != lightEnd)
            {
                throw new InvalidDataException();
            }

            return light;
        }

        private static IEnumerable<Helper> ParseHelpers(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseHelper(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static Helper ParseHelper(BinaryReader reader)
        {
            INode node = new Helper();
            ParseNode(reader, ref node);

            var helper = (Helper)node;

            return helper;
        }

        private static IEnumerable<Attachment> ParseAttachments(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseAttachment(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static Attachment ParseAttachment(BinaryReader reader)
        {
            var attachmentEnd = reader.BaseStream.Position + reader.ReadUInt32();

            INode node = new Attachment();
            ParseNode(reader, ref node);

            var attachment = (Attachment)node;
            attachment.Path = reader.ReadString(BinaryConstants.FixedStringSizeLong);
            attachment.AttachmentId = reader.ReadUInt32();

            while (reader.BaseStream.Position < attachmentEnd)
            {
                var optionalTag = reader.ReadInt32();
                if (_attachmentProperties.TryGetValue(optionalTag, out var property))
                {
                    try
                    {
                        property.ParseForObject(attachment, reader);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidDataException($"Duplicate attachment tag: '{optionalTag.ToRawcode()}'", e);
                    }
                }
                else
                {
                    throw new NotSupportedException($"Unknown attachment tag: '{optionalTag.ToRawcode()}'");
                }
            }

            if (reader.BaseStream.Position != attachmentEnd)
            {
                throw new InvalidDataException();
            }

            return attachment;
        }

        private static IEnumerable<Vector3> ParsePivotPoints(BinaryReader reader)
        {
            var chunkSize = reader.ReadUInt32();
            if ((chunkSize % BinaryConstants.PivotPointsSize) != 0)
            {
                throw new InvalidDataException();
            }

            var chunkEnd = reader.BaseStream.Position + chunkSize;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParsePivotPoint(reader);
            }
        }

        private static Vector3 ParsePivotPoint(BinaryReader reader)
        {
            return ParseVector3(reader);
        }

        private static IEnumerable<ParticleEmitter> ParseParticleEmitters(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseParticleEmitter(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static ParticleEmitter ParseParticleEmitter(BinaryReader reader)
        {
            var particleEmitterEnd = reader.BaseStream.Position + reader.ReadUInt32();

            INode node = new ParticleEmitter();
            ParseNode(reader, ref node);

            var particleEmitter = (ParticleEmitter)node;
            particleEmitter.EmissionRate = reader.ReadSingle();
            particleEmitter.Gravity = reader.ReadSingle();
            particleEmitter.Longitude = reader.ReadSingle();
            particleEmitter.Latitude = reader.ReadSingle();
            particleEmitter.SpawnModelFileName = reader.ReadString(BinaryConstants.FixedStringSizeLong);
            particleEmitter.Lifespan = reader.ReadSingle();
            particleEmitter.InitialVelocity = reader.ReadSingle();

            while (reader.BaseStream.Position < particleEmitterEnd)
            {
                var optionalTag = reader.ReadInt32();
                if (_particleEmitterProperties.TryGetValue(optionalTag, out var property))
                {
                    try
                    {
                        property.ParseForObject(particleEmitter, reader);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidDataException($"Duplicate particle emitter tag: '{optionalTag.ToRawcode()}'", e);
                    }
                }
                else
                {
                    throw new NotSupportedException($"Unknown particle emitter tag: '{optionalTag.ToRawcode()}'");
                }
            }

            if (reader.BaseStream.Position != particleEmitterEnd)
            {
                throw new InvalidDataException();
            }

            return particleEmitter;
        }

        private static IEnumerable<ParticleEmitter2> ParseParticleEmitters2(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseParticleEmitter2(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static ParticleEmitter2 ParseParticleEmitter2(BinaryReader reader)
        {
            var particleEmitter2End = reader.BaseStream.Position + reader.ReadUInt32();

            INode node = new ParticleEmitter2();
            ParseNode(reader, ref node);

            var particleEmitter2 = (ParticleEmitter2)node;
            particleEmitter2.Speed = reader.ReadSingle();
            particleEmitter2.Variation = reader.ReadSingle();
            particleEmitter2.Latitude = reader.ReadSingle();
            particleEmitter2.Gravity = reader.ReadSingle();
            particleEmitter2.Lifespan = reader.ReadSingle();
            particleEmitter2.EmissionRate = reader.ReadSingle();
            particleEmitter2.Length = reader.ReadSingle();
            particleEmitter2.Width = reader.ReadSingle();
            particleEmitter2.FilterMode = (ParticleEmitter2FilterMode)reader.ReadUInt32();
            particleEmitter2.Rows = reader.ReadUInt32();
            particleEmitter2.Columns = reader.ReadUInt32();
            particleEmitter2.HeadOrTail = (ParticleEmitter2FramesFlags)reader.ReadUInt32();
            particleEmitter2.TailLength = reader.ReadSingle();
            particleEmitter2.Time = reader.ReadSingle();

            var segment1 = new ParticleEmitter2.Segment();
            var segment2 = new ParticleEmitter2.Segment();
            var segment3 = new ParticleEmitter2.Segment();

            segment1.SegmentColor = ParseVector3(reader);
            segment2.SegmentColor = ParseVector3(reader);
            segment3.SegmentColor = ParseVector3(reader);

            segment1.SegmentAlpha = reader.ReadByte();
            segment2.SegmentAlpha = reader.ReadByte();
            segment3.SegmentAlpha = reader.ReadByte();

            segment1.SegmentScaling = reader.ReadSingle();
            segment2.SegmentScaling = reader.ReadSingle();
            segment3.SegmentScaling = reader.ReadSingle();

            segment1.HeadInterval = reader.ReadUInt32();
            segment2.HeadInterval = reader.ReadUInt32();
            segment3.HeadInterval = reader.ReadUInt32();

            segment1.HeadDecayInterval = reader.ReadUInt32();
            segment2.HeadDecayInterval = reader.ReadUInt32();
            segment3.HeadDecayInterval = reader.ReadUInt32();

            segment1.TailInterval = reader.ReadUInt32();
            segment2.TailInterval = reader.ReadUInt32();
            segment3.TailInterval = reader.ReadUInt32();

            segment1.TailDecayInterval = reader.ReadUInt32();
            segment2.TailDecayInterval = reader.ReadUInt32();
            segment3.TailDecayInterval = reader.ReadUInt32();

            particleEmitter2.Segments = new[] { segment1, segment2, segment3 };

            particleEmitter2.TextureId = reader.ReadUInt32();
            particleEmitter2.Squirt = reader.ReadUInt32();
            particleEmitter2.PriorityPlane = reader.ReadUInt32();
            particleEmitter2.ReplaceableId = reader.ReadUInt32();

            while (reader.BaseStream.Position < particleEmitter2End)
            {
                var optionalTag = reader.ReadInt32();
                if (_particleEmitter2Properties.TryGetValue(optionalTag, out var property))
                {
                    try
                    {
                        property.ParseForObject(particleEmitter2, reader);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidDataException($"Duplicate particle emitter 2 tag: '{optionalTag.ToRawcode()}'", e);
                    }
                }
                else
                {
                    throw new NotSupportedException($"Unknown particle emitter 2 tag: '{optionalTag.ToRawcode()}'");
                }
            }

            if (reader.BaseStream.Position != particleEmitter2End)
            {
                throw new InvalidDataException();
            }

            return particleEmitter2;
        }

        private static IEnumerable<RibbonEmitter> ParseRibbonEmitters(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseRibbonEmitter(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static RibbonEmitter ParseRibbonEmitter(BinaryReader reader)
        {
            var ribbonEmitterEnd = reader.BaseStream.Position + reader.ReadUInt32();

            INode node = new RibbonEmitter();
            ParseNode(reader, ref node);

            var ribbonEmitter = (RibbonEmitter)node;
            ribbonEmitter.HeightAbove = reader.ReadSingle();
            ribbonEmitter.HeightBelow = reader.ReadSingle();
            ribbonEmitter.Alpha = reader.ReadSingle();
            ribbonEmitter.Color = ParseVector3(reader);
            ribbonEmitter.Lifespan = reader.ReadSingle();
            ribbonEmitter.TextureSlot = reader.ReadUInt32();
            ribbonEmitter.EmissionRate = reader.ReadUInt32();
            ribbonEmitter.Rows = reader.ReadUInt32();
            ribbonEmitter.Columns = reader.ReadUInt32();
            ribbonEmitter.MaterialId = reader.ReadUInt32();
            ribbonEmitter.Gravity = reader.ReadSingle();

            while (reader.BaseStream.Position < ribbonEmitterEnd)
            {
                var optionalTag = reader.ReadInt32();
                if (_ribbonEmitterProperties.TryGetValue(optionalTag, out var property))
                {
                    try
                    {
                        property.ParseForObject(ribbonEmitter, reader);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidDataException($"Duplicate ribbon emitter tag: '{optionalTag.ToRawcode()}'", e);
                    }
                }
                else
                {
                    throw new NotSupportedException($"Unknown ribbon emitter tag: '{optionalTag.ToRawcode()}'");
                }
            }

            if (reader.BaseStream.Position != ribbonEmitterEnd)
            {
                throw new InvalidDataException();
            }

            return ribbonEmitter;
        }

        private static IEnumerable<Camera> ParseCameras(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseCamera(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static Camera ParseCamera(BinaryReader reader)
        {
            var cameraEnd = reader.BaseStream.Position + reader.ReadUInt32();

            var camera = new Camera();
            camera.Name = reader.ReadString(BinaryConstants.FixedStringSizeShort);
            camera.Position = ParseVector3(reader);
            camera.FieldOfView = reader.ReadSingle();
            camera.FarClippingPlane = reader.ReadSingle();
            camera.NearClippingPlane = reader.ReadSingle();
            camera.TargetPosition = ParseVector3(reader);

            while (reader.BaseStream.Position < cameraEnd)
            {
                var optionalTag = reader.ReadInt32();
                if (_cameraProperties.TryGetValue(optionalTag, out var property))
                {
                    try
                    {
                        property.ParseForObject(camera, reader);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidDataException($"Duplicate camera tag: '{optionalTag.ToRawcode()}'", e);
                    }
                }
                else
                {
                    throw new NotSupportedException($"Unknown camera tag: '{optionalTag.ToRawcode()}'");
                }
            }

            if (reader.BaseStream.Position != cameraEnd)
            {
                throw new InvalidDataException();
            }

            return camera;
        }

        private static IEnumerable<EventObject> ParseEventObjects(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseEventObject(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static EventObject ParseEventObject(BinaryReader reader)
        {
            INode node = new EventObject();
            ParseNode(reader, ref node);

            var eventObject = (EventObject)node;

            if (reader.ReadInt32() != BinaryConstants.EventObjectsChunk2)
            {
                throw new InvalidDataException();
            }

            var tracksCount = reader.ReadUInt32();
            eventObject.GlobalSequenceId = reader.ReadUInt32();

            var tracks = new List<uint>();
            for (var i = 0; i < tracksCount; i++)
            {
                tracks.Add(reader.ReadUInt32());
            }

            eventObject.Tracks = tracks.ToArray();

            return eventObject;
        }

        private static IEnumerable<CollisionShape> ParseCollisionShapes(BinaryReader reader)
        {
            var chunkEnd = reader.ReadUInt32() + reader.BaseStream.Position;
            while (reader.BaseStream.Position < chunkEnd)
            {
                yield return ParseCollisionShape(reader);
            }

            if (reader.BaseStream.Position > chunkEnd)
            {
                throw new InvalidDataException();
            }
        }

        private static CollisionShape ParseCollisionShape(BinaryReader reader)
        {
            INode node = new CollisionShape();
            ParseNode(reader, ref node);

            var collisionShape = (CollisionShape)node;
            collisionShape.Type = (CollisionShapeType)reader.ReadUInt32();
            collisionShape.Vertices = collisionShape.Type == CollisionShapeType.Sphere
                ? new[] { ParseVector3(reader), }
                : new[] { ParseVector3(reader), ParseVector3(reader), };

            if (collisionShape.Type == CollisionShapeType.Sphere || collisionShape.Type == CollisionShapeType.Cylinder)
            {
                collisionShape.Radius = reader.ReadSingle();
            }

            return collisionShape;
        }

        private static IEnumerable<Extent> ParseExtents(BinaryReader reader, uint count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return ParseExtent(reader);
            }
        }

        private static Extent ParseExtent(BinaryReader reader)
        {
            var extent = new Extent();
            extent.BoundsRadius = reader.ReadSingle();
            extent.MinimumExtent = ParseVector3(reader);
            extent.MaximumExtent = ParseVector3(reader);

            return extent;
        }

        private static INode ParseNode(BinaryReader reader, ref INode node)
        {
            var nodeEnd = reader.BaseStream.Position + reader.ReadUInt32();

            node.Name = reader.ReadString(BinaryConstants.FixedStringSizeShort);
            node.ObjectId = reader.ReadUInt32();
            node.ParentId = reader.ReadUInt32();
            node.Flags = (NodeFlags)reader.ReadUInt32();

            while (reader.BaseStream.Position < nodeEnd)
            {
                var optionalTag = reader.ReadInt32();
                if (_nodeProperties.TryGetValue(optionalTag, out var property))
                {
                    try
                    {
                        property.ParseForObject(node, reader);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidDataException($"Duplicate node tag: '{optionalTag.ToRawcode()}'", e);
                    }
                }
                else
                {
                    throw new NotSupportedException($"Unknown node tag: '{optionalTag.ToRawcode()}'");
                }
            }

            if (reader.BaseStream.Position != nodeEnd)
            {
                throw new InvalidDataException();
            }

            return node;
        }

        private static AnimationChannel<uint> ParseAnimationChannelUInt32(BinaryReader reader)
        {
            var count = reader.ReadUInt32();

            var channel = new AnimationChannel<uint>();
            channel.InterpolationType = (InterpolationType)reader.ReadUInt32();
            channel.GlobalSequenceId = reader.ReadUInt32();

            var parseTanValues = channel.InterpolationType == InterpolationType.Hermite || channel.InterpolationType == InterpolationType.Bezier;
            channel.Keys = ParseAnimationChannelKeysUInt32(reader, count, parseTanValues).ToArray();

            return channel;
        }

        private static IEnumerable<AnimationChannel<uint>.Key> ParseAnimationChannelKeysUInt32(BinaryReader reader, uint count, bool parseTanValues)
        {
            for (var i = 0; i < count; i++)
            {
                var key = new AnimationChannel<uint>.Key();
                key.Frame = reader.ReadInt32();
                key.Value = reader.ReadUInt32();

                if (parseTanValues)
                {
                    key.TanIn = reader.ReadUInt32();
                    key.TanOut = reader.ReadUInt32();
                }

                yield return key;
            }
        }

        private static AnimationChannel<float> ParseAnimationChannelSingle(BinaryReader reader)
        {
            var count = reader.ReadUInt32();

            var channel = new AnimationChannel<float>();
            channel.InterpolationType = (InterpolationType)reader.ReadUInt32();
            channel.GlobalSequenceId = reader.ReadUInt32();

            var parseTanValues = channel.InterpolationType == InterpolationType.Hermite || channel.InterpolationType == InterpolationType.Bezier;
            channel.Keys = ParseAnimationChannelKeysSingle(reader, count, parseTanValues).ToArray();

            return channel;
        }

        private static IEnumerable<AnimationChannel<float>.Key> ParseAnimationChannelKeysSingle(BinaryReader reader, uint count, bool parseTanValues)
        {
            for (var i = 0; i < count; i++)
            {
                var key = new AnimationChannel<float>.Key();
                key.Frame = reader.ReadInt32();
                key.Value = reader.ReadSingle();

                if (parseTanValues)
                {
                    key.TanIn = reader.ReadSingle();
                    key.TanOut = reader.ReadSingle();
                }

                yield return key;
            }
        }

        private static AnimationChannel<Vector3> ParseAnimationChannelVector3(BinaryReader reader)
        {
            var count = reader.ReadUInt32();

            var channel = new AnimationChannel<Vector3>();
            channel.InterpolationType = (InterpolationType)reader.ReadUInt32();
            channel.GlobalSequenceId = reader.ReadUInt32();

            var parseTanValues = channel.InterpolationType == InterpolationType.Hermite || channel.InterpolationType == InterpolationType.Bezier;
            channel.Keys = ParseAnimationChannelKeysVector3(reader, count, parseTanValues).ToArray();

            return channel;
        }

        private static IEnumerable<AnimationChannel<Vector3>.Key> ParseAnimationChannelKeysVector3(BinaryReader reader, uint count, bool parseTanValues)
        {
            for (var i = 0; i < count; i++)
            {
                var key = new AnimationChannel<Vector3>.Key();
                key.Frame = reader.ReadInt32();
                key.Value = ParseVector3(reader);

                if (parseTanValues)
                {
                    key.TanIn = ParseVector3(reader);
                    key.TanOut = ParseVector3(reader);
                }

                yield return key;
            }
        }

        private static AnimationChannel<Quaternion> ParseAnimationChannelQuaternion(BinaryReader reader)
        {
            var count = reader.ReadUInt32();

            var channel = new AnimationChannel<Quaternion>();
            channel.InterpolationType = (InterpolationType)reader.ReadUInt32();
            channel.GlobalSequenceId = reader.ReadUInt32();

            var parseTanValues = channel.InterpolationType == InterpolationType.Hermite || channel.InterpolationType == InterpolationType.Bezier;
            channel.Keys = ParseAnimationChannelKeysQuaternion(reader, count, parseTanValues).ToArray();

            return channel;
        }

        private static IEnumerable<AnimationChannel<Quaternion>.Key> ParseAnimationChannelKeysQuaternion(BinaryReader reader, uint count, bool parseTanValues)
        {
            for (var i = 0; i < count; i++)
            {
                var key = new AnimationChannel<Quaternion>.Key();
                key.Frame = reader.ReadInt32();
                key.Value = ParseQuaternion(reader);

                if (parseTanValues)
                {
                    key.TanIn = ParseQuaternion(reader);
                    key.TanOut = ParseQuaternion(reader);
                }

                yield return key;
            }
        }

        private static Vector2 ParseVector2(BinaryReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }

        private static Vector3 ParseVector3(BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        private static Vector4 ParseVector4(BinaryReader reader)
        {
            return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        private static Quaternion ParseQuaternion(BinaryReader reader)
        {
            return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        private static IEnumerable<OptionalModelProperty> GetModelProperties()
        {
            yield return new OptionalModelProperty(BinaryConstants.VersionChunk, nameof(Model.Version), reader => ParseVersion(reader));
            yield return new OptionalModelProperty(BinaryConstants.ModelInfoChunk, nameof(Model.ModelInfo), reader => ParseModelInfo(reader));
            yield return new OptionalModelProperty(BinaryConstants.SequencesChunk, nameof(Model.Sequences), reader => ParseSequences(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.GlobalSequencesChunk, nameof(Model.GlobalSequences), reader => ParseGlobalSequences(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.MaterialsChunk, nameof(Model.Materials), reader => ParseMaterials(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.TexturesChunk, nameof(Model.Textures), reader => ParseTextures(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.TextureAnimationsChunk, nameof(Model.TextureAnimations), reader => ParseTextureAnimations(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.GeosetsChunk, nameof(Model.Geosets), reader => ParseGeosets(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.GeosetAnimsChunk, nameof(Model.GeosetAnimations), reader => ParseGeosetAnimations(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.BonesChunk, nameof(Model.Bones), reader => ParseBones(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.LightsChunk, nameof(Model.Lights), reader => ParseLights(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.HelpersChunk, nameof(Model.Helpers), reader => ParseHelpers(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.AttachmentsChunk, nameof(Model.Attachments), reader => ParseAttachments(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.PivotPointsChunk, nameof(Model.PivotPoints), reader => ParsePivotPoints(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.ParticleEmittersChunk, nameof(Model.ParticleEmitters), reader => ParseParticleEmitters(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.ParticleEmitters2Chunk, nameof(Model.ParticleEmitters2), reader => ParseParticleEmitters2(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.RibbonEmittersChunk, nameof(Model.RibbonEmitters), reader => ParseRibbonEmitters(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.CamerasChunk, nameof(Model.Cameras), reader => ParseCameras(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.EventObjectsChunk, nameof(Model.EventObjects), reader => ParseEventObjects(reader).ToArray());
            yield return new OptionalModelProperty(BinaryConstants.CollisionShapesChunk, nameof(Model.CollisionShapes), reader => ParseCollisionShapes(reader).ToArray());
        }

        private static IEnumerable<OptionalModelProperty> GetNodeProperties()
        {
            yield return new OptionalModelProperty(typeof(INode), nameof(INode.Translations), BinaryConstants.NodeTranslationChunkTag, reader => ParseAnimationChannelVector3(reader));
            yield return new OptionalModelProperty(typeof(INode), nameof(INode.Rotations), BinaryConstants.NodeRotationChunkTag, reader => ParseAnimationChannelQuaternion(reader));
            yield return new OptionalModelProperty(typeof(INode), nameof(INode.Scalings), BinaryConstants.NodeScalingChunkTag, reader => ParseAnimationChannelVector3(reader));
        }

        private static IEnumerable<OptionalModelProperty> GetLayerProperties()
        {
            yield return new OptionalModelProperty(typeof(Layer), nameof(Layer.TextureIds), BinaryConstants.LayerTextureIdChunkTag, reader => ParseAnimationChannelUInt32(reader));
            yield return new OptionalModelProperty(typeof(Layer), nameof(Layer.Alphas), BinaryConstants.LayerAlphaChunkTag, reader => ParseAnimationChannelSingle(reader));
        }

        private static IEnumerable<OptionalModelProperty> GetTextureAnimationProperties()
        {
            yield return new OptionalModelProperty(typeof(TextureAnimation), nameof(TextureAnimation.Translations), BinaryConstants.TextureAnimationTranslationChunkTag, reader => ParseAnimationChannelVector3(reader));
            yield return new OptionalModelProperty(typeof(TextureAnimation), nameof(TextureAnimation.Rotations), BinaryConstants.TextureAnimationRotationChunkTag, reader => ParseAnimationChannelQuaternion(reader));
            yield return new OptionalModelProperty(typeof(TextureAnimation), nameof(TextureAnimation.Scalings), BinaryConstants.TextureAnimationScalingChunkTag, reader => ParseAnimationChannelVector3(reader));
        }

        private static IEnumerable<OptionalModelProperty> GetGeosetAnimationProperties()
        {
            yield return new OptionalModelProperty(typeof(GeosetAnimation), nameof(GeosetAnimation.Alphas), BinaryConstants.GeosetAnimationAlphaChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(GeosetAnimation), nameof(GeosetAnimation.Colors), BinaryConstants.GeosetAnimationColorChunkTag, reader => ParseAnimationChannelVector3(reader));
        }

        private static IEnumerable<OptionalModelProperty> GetLightProperties()
        {
            yield return new OptionalModelProperty(typeof(Light), nameof(Light.AttenuationStarts), BinaryConstants.LightAttenuationStartChunkTag, reader => ParseAnimationChannelUInt32(reader));
            yield return new OptionalModelProperty(typeof(Light), nameof(Light.AttenuationEnds), BinaryConstants.LightAttenuationEndChunkTag, reader => ParseAnimationChannelUInt32(reader));
            yield return new OptionalModelProperty(typeof(Light), nameof(Light.Colors), BinaryConstants.LightColorChunkTag, reader => ParseAnimationChannelVector3(reader));
            yield return new OptionalModelProperty(typeof(Light), nameof(Light.Intensities), BinaryConstants.LightIntensityChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(Light), nameof(Light.AmbientIntensities), BinaryConstants.LightAmbientIntensityChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(Light), nameof(Light.AmbientColors), BinaryConstants.LightAmbientColorChunkTag, reader => ParseAnimationChannelVector3(reader));
            yield return new OptionalModelProperty(typeof(Light), nameof(Light.Visibilities), BinaryConstants.LightVisibilityChunkTag, reader => ParseAnimationChannelSingle(reader));
        }

        private static IEnumerable<OptionalModelProperty> GetAttachmentProperties()
        {
            yield return new OptionalModelProperty(typeof(Attachment), nameof(Attachment.Visibilities), BinaryConstants.AttachmentVisibilityChunkTag, reader => ParseAnimationChannelSingle(reader));
        }

        private static IEnumerable<OptionalModelProperty> GetParticleEmitterProperties()
        {
            yield return new OptionalModelProperty(typeof(ParticleEmitter), nameof(ParticleEmitter.EmissionRates), BinaryConstants.ParticleEmitterEmissionRateChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter), nameof(ParticleEmitter.Gravities), BinaryConstants.ParticleEmitterGravityChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter), nameof(ParticleEmitter.Longitudes), BinaryConstants.ParticleEmitterLongitudeChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter), nameof(ParticleEmitter.Latitudes), BinaryConstants.ParticleEmitterLatitudeChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter), nameof(ParticleEmitter.Lifespans), BinaryConstants.ParticleEmitterLifespanChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter), nameof(ParticleEmitter.Speeds), BinaryConstants.ParticleEmitterSpeedChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter), nameof(ParticleEmitter.Visibilities), BinaryConstants.ParticleEmitterVisibilityChunkTag, reader => ParseAnimationChannelSingle(reader));
        }

        private static IEnumerable<OptionalModelProperty> GetParticleEmitter2Properties()
        {
            yield return new OptionalModelProperty(typeof(ParticleEmitter2), nameof(ParticleEmitter2.EmissionRates), BinaryConstants.ParticleEmitter2EmissionRateChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter2), nameof(ParticleEmitter2.Gravities), BinaryConstants.ParticleEmitter2GravityChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter2), nameof(ParticleEmitter2.Latitudes), BinaryConstants.ParticleEmitter2LatitudeChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter2), nameof(ParticleEmitter2.Speeds), BinaryConstants.ParticleEmitter2SpeedChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter2), nameof(ParticleEmitter2.Visibilities), BinaryConstants.ParticleEmitter2VisibilityChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter2), nameof(ParticleEmitter2.Variations), BinaryConstants.ParticleEmitter2VariationChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter2), nameof(ParticleEmitter2.Lengths), BinaryConstants.ParticleEmitter2LengthChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(ParticleEmitter2), nameof(ParticleEmitter2.Widths), BinaryConstants.ParticleEmitter2WidthChunkTag, reader => ParseAnimationChannelSingle(reader));
        }

        private static IEnumerable<OptionalModelProperty> GetRibbonEmitterProperties()
        {
            yield return new OptionalModelProperty(typeof(RibbonEmitter), nameof(RibbonEmitter.Visibilities), BinaryConstants.RibbonEmitterVisibilityChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(RibbonEmitter), nameof(RibbonEmitter.HeightsAbove), BinaryConstants.RibbonEmitterHeightAboveChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(RibbonEmitter), nameof(RibbonEmitter.HeightsBelow), BinaryConstants.RibbonEmitterHeightBelowChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(RibbonEmitter), nameof(RibbonEmitter.Alphas), BinaryConstants.RibbonEmitterAlphaChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(RibbonEmitter), nameof(RibbonEmitter.Colors), BinaryConstants.RibbonEmitterColorChunkTag, reader => ParseAnimationChannelVector3(reader));
            yield return new OptionalModelProperty(typeof(RibbonEmitter), nameof(RibbonEmitter.TextureSlots), BinaryConstants.RibbonEmitterTextureSlotChunkTag, reader => ParseAnimationChannelUInt32(reader));
        }

        private static IEnumerable<OptionalModelProperty> GetCameraProperties()
        {
            yield return new OptionalModelProperty(typeof(Camera), nameof(Camera.Translations), BinaryConstants.CameraTranslationChunkTag, reader => ParseAnimationChannelVector3(reader));
            yield return new OptionalModelProperty(typeof(Camera), nameof(Camera.Rotations), BinaryConstants.CameraRotationChunkTag, reader => ParseAnimationChannelSingle(reader));
            yield return new OptionalModelProperty(typeof(Camera), nameof(Camera.TargetTranslations), BinaryConstants.CameraTargetTranslationChunkTag, reader => ParseAnimationChannelVector3(reader));
        }
    }
}