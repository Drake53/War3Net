using Veldrid;

namespace War3Net.Rendering.DataStructures
{
    /// <summary>
    /// Graphics resources for a single model, that can be re-used by multiple model instances.
    /// </summary>
    public sealed class SharedModelResources
    {
        public DeviceBuffer[] VertexBuffers { get; set; }

        public DeviceBuffer[] IndexBuffers { get; set; }

        public TextureView[] Textures { get; set; }
    }
}