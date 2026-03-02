namespace War3Net.Modeling.DataStructures
{
    public struct GeosetAnimation
    {
        public float Alpha { get; set; }

        public GeosetAnimationFlags Flags { get; set; }

        public Vector3 Color { get; set; }

        public uint GeosetId { get; set; }

        public AnimationChannel<float>? Alphas { get; set; }

        public AnimationChannel<Vector3>? Colors { get; set; }
    }
}