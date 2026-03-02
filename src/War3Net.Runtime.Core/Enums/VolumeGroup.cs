namespace War3Net.Runtime.Enums
{
    public sealed class VolumeGroup : Handle
    {
        private static readonly Dictionary<int, VolumeGroup> _groups = GetTypes().ToDictionary(t => (int)t, t => new VolumeGroup(t));

        private readonly Type _type;

        private VolumeGroup(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            UnitMovement = 0,
            UnitSounds = 1,
            Combat = 2,
            Spells = 3,
            UI = 4,
            Music = 5,
            AmbientSounds = 6,
            Fire = 7,
        }

        public static implicit operator Type(VolumeGroup volumeGroup) => volumeGroup._type;

        public static explicit operator int(VolumeGroup volumeGroup) => (int)volumeGroup._type;

        public static VolumeGroup GetVolumeGroup(int i)
        {
            if (!_groups.TryGetValue(i, out var volumeGroup))
            {
                volumeGroup = new VolumeGroup((Type)i);
                _groups.Add(i, volumeGroup);
            }

            return volumeGroup;
        }

        private static IEnumerable<Type> GetTypes()
        {
            foreach (Type type in Enum.GetValues(typeof(Type)))
            {
                yield return type;
            }
        }
    }
}