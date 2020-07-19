// ------------------------------------------------------------------------------
// <copyright file="FaceTypeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;

using Veldrid;

using War3Net.Modeling.Enums;

namespace War3Net.Rendering.Extensions
{
    public static class FaceTypeExtensions
    {
        public static PrimitiveTopology ToPrimitiveTopology(this FaceType faceType)
        {
            switch (faceType)
            {
                case FaceType.Points: return PrimitiveTopology.PointList;
                case FaceType.Lines: return PrimitiveTopology.LineList;
                case FaceType.LineStrip: return PrimitiveTopology.LineStrip;
                case FaceType.Triangles: return PrimitiveTopology.TriangleList;
                case FaceType.TriangleStrip: return PrimitiveTopology.TriangleStrip;

                case FaceType.LineLoop:
                case FaceType.TriangleFan:
                case FaceType.Quads:
                case FaceType.QuadStrip:
                case FaceType.Polygons:
                    throw new NotSupportedException();

                default:
                    throw new InvalidEnumArgumentException(nameof(faceType), (int)faceType, typeof(FaceType));
            }
        }
    }
}