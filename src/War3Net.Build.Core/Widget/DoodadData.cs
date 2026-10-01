namespace War3Net.Build.Widget
{
    public sealed partial class DoodadData : WidgetData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoodadData"/> class.
        /// </summary>
        public DoodadData()
        {
        }

        public DoodadState State { get; set; }

        // in %, where 0x64 = 100%
        public byte Life { get; set; }

        /// <summary>Introduced in patch 3.0.0.</summary>
        public uint GroupId { get; set; }

        /// <summary>Introduced in patch 3.0.0.</summary>
        public int Unk1 { get; set; }

        /// <summary>Introduced in patch 3.0.0.</summary>
        public float Roll { get; set; }

        /// <summary>Introduced in patch 3.0.0.</summary>
        public float Pitch { get; set; }

        /// <summary>Introduced in patch 3.0.0.</summary>
        public List<DoodadLightData> DoodadLights { get; set; } = new();
    }
}