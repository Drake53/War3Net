using System.Numerics;

namespace War3Net.Modeling.DataStructures
{
    public struct TextureAnimation
    {
        public AnimationChannel<Vector3>? Translations { get; set; }

        public AnimationChannel<Quaternion>? Rotations { get; set; }

        public AnimationChannel<Vector3>? Scalings { get; set; }
    }
}