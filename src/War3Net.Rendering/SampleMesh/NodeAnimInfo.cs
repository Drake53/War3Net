using System.Numerics;
using System.Runtime.CompilerServices;

// https://github.com/mellinoe/veldrid-samples/blob/master/src/AnimatedMesh/Application/BoneAnimInfo.cs
namespace War3Net.Rendering.AnimatedMesh
{
    public unsafe struct NodeAnimInfo
    {
        public const uint BlittableSize = Matrix4x4ByteSize * MaxMatrixCount;

        private const uint Matrix4x4ByteSize = Matrix4x4FloatsCount * sizeof(float);
        private const uint Matrix4x4FloatsCount = 16U;
        private const uint MaxMatrixCount = 256U;

        public Matrix4x4[] VertexGroupTransformations;

        public Blittable GetBlittable()
        {
            Blittable b;
            fixed (Matrix4x4* ptr = VertexGroupTransformations)
            {
                Unsafe.CopyBlock(&b, ptr, Matrix4x4ByteSize * MaxMatrixCount);
            }

            return b;
        }

        public struct Blittable
        {
            public fixed float Data[(int)(Matrix4x4FloatsCount * MaxMatrixCount)];
        }

        internal static NodeAnimInfo New()
        {
            return new NodeAnimInfo { VertexGroupTransformations = new Matrix4x4[MaxMatrixCount] };
        }
    }
}