using System.Numerics;

namespace War3Net.Modeling.DataStructures
{
    public struct Extent
    {
        public float BoundsRadius { get; set; }

        public Vector3 MinimumExtent { get; set; }

        public Vector3 MaximumExtent { get; set; }
    }
}