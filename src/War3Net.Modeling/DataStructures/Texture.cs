using War3Net.Modeling.Enums;

namespace War3Net.Modeling.DataStructures
{
    public struct Texture
    {
        public uint ReplaceableId { get; set; }

        public string FileName { get; set; }

        public TextureFlags Flags { get; set; }
    }
}