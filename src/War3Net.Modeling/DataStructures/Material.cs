using War3Net.Modeling.Enums;

namespace War3Net.Modeling.DataStructures
{
    public struct Material
    {
        public uint PriorityPlane { get; set; }

        public MaterialRenderMode RenderMode { get; set; }

        public Layer[] Layers { get; set; }
    }
}