namespace War3Net.Rendering.DataStructures
{
    /// <summary>
    /// Graphics resources for a single model instance.
    /// </summary>
    public sealed class InstanceModelResources
    {
        public DeviceBuffer[] NodeBuffers { get; set; }

        public DeviceBuffer TransformationBuffer { get; set; }

        public Pipeline[][] Pipelines { get; set; }

        public ResourceSet[][] ResourceSets { get; set; }
    }
}