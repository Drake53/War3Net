namespace War3Net.Build.Widget
{
    /// <summary>Introduced in patch 3.0.0.</summary>
    public sealed partial class DoodadLightData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoodadLightData"/> class.
        /// </summary>
        public DoodadLightData()
        {
        }

        public uint LightIndex { get; set; }

        public uint IsShadowCasting { get; set; }

        public Color Color { get; set; }

        public float Intensity { get; set; }

        public float ShadowCastingStart { get; set; }

        public float ShadowCastingEnd { get; set; }

        public float QuadraticFalloff { get; set; }

        public float LinearFalloff { get; set; }

        public float Damping { get; set; }
    }
}