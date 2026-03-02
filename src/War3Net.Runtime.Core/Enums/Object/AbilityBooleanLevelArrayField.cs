namespace War3Net.Runtime.Enums.Object
{
    public sealed class AbilityBooleanLevelArrayField : Handle
    {
        private static readonly Dictionary<int, AbilityBooleanLevelArrayField> _fields = GetTypes().ToDictionary(t => (int)t, t => new AbilityBooleanLevelArrayField(t));

        private readonly Type _type;

        private AbilityBooleanLevelArrayField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
        }

        public static implicit operator Type(AbilityBooleanLevelArrayField abilityBooleanLevelArrayField) => abilityBooleanLevelArrayField._type;

        public static explicit operator int(AbilityBooleanLevelArrayField abilityBooleanLevelArrayField) => (int)abilityBooleanLevelArrayField._type;

        public static AbilityBooleanLevelArrayField GetAbilityBooleanLevelArrayField(int i)
        {
            if (!_fields.TryGetValue(i, out var abilityBooleanLevelArrayField))
            {
                abilityBooleanLevelArrayField = new AbilityBooleanLevelArrayField((Type)i);
                _fields.Add(i, abilityBooleanLevelArrayField);
            }

            return abilityBooleanLevelArrayField;
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