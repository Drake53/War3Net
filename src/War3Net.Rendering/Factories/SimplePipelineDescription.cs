namespace War3Net.Rendering.Factories
{
    public struct SimplePipelineDescription
    {
        public FilterMode FilterMode { get; set; }

        public FaceType FaceType { get; set; }

        public LayerShading LayerShading { get; set; }

        public SimpleShaderDescription ShaderSettings { get; set; }

        public OutputDescription OutputDescription { get; set; }
    }
}