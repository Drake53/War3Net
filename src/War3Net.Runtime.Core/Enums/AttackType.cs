namespace War3Net.Runtime.Enums
{
    public sealed class AttackType : Handle
    {
        private static readonly Dictionary<int, AttackType> _types = GetTypes().ToDictionary(t => (int)t, t => new AttackType(t));

        private readonly Type _type;

        private AttackType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Normal = 0,
            Melee = 1,
            Pierce = 2,
            Siege = 3,
            Magic = 4,
            Chaos = 5,
            Hero = 6,
        }

        public static implicit operator Type(AttackType attackType) => attackType._type;

        public static explicit operator int(AttackType attackType) => (int)attackType._type;

        public static AttackType GetAttackType(int i)
        {
            if (!_types.TryGetValue(i, out var attackType))
            {
                attackType = new AttackType((Type)i);
                _types.Add(i, attackType);
            }

            return attackType;
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