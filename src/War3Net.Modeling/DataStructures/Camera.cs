using System.Numerics;

namespace War3Net.Modeling.DataStructures
{
    public struct Camera
    {
        public string Name { get; set; }

        public Vector3 Position { get; set; }

        public float FieldOfView { get; set; }

        public float FarClippingPlane { get; set; }

        public float NearClippingPlane { get; set; }

        public Vector3 TargetPosition { get; set; }

        public AnimationChannel<Vector3>? Translations { get; set; }

        public AnimationChannel<float>? Rotations { get; set; }

        public AnimationChannel<Vector3>? TargetTranslations { get; set; }
    }
}