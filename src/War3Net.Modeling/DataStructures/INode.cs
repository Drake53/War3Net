namespace War3Net.Modeling.DataStructures
{
    public interface INode
    {
        public string Name { get; set; }

        public uint ObjectId { get; set; }

        public uint? ParentId { get; set; }

        public NodeFlags Flags { get; set; }

        public AnimationChannel<Vector3>? Translations { get; set; }

        public AnimationChannel<Quaternion>? Rotations { get; set; }

        public AnimationChannel<Vector3>? Scalings { get; set; }
    }
}