namespace War3Net.Modeling.DataStructures
{
    public struct AnimationChannel<T>
        where T : struct
    {
        public InterpolationType InterpolationType { get; set; }

        public uint GlobalSequenceId { get; set; }

        public Key[] Keys { get; set; }

        public struct Key
        {
            public int Frame { get; set; }

            public T Value { get; set; }

            public T TanIn { get; set; }

            public T TanOut { get; set; }
        }
    }
}