namespace War3Net.Runtime.Enums.Object
{
    public sealed class AbilityStringField : Handle
    {
        private static readonly Dictionary<int, AbilityStringField> _fields = GetTypes().ToDictionary(t => (int)t, t => new AbilityStringField(t));

        private readonly Type _type;

        private AbilityStringField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            NAME = 1634623853,
            ICON_ACTIVATED = 1635082610,
            ICON_RESEARCH = 1634886002,
            EFFECT_SOUND = 1634035315,
            EFFECT_SOUND_LOOPING = 1634035308,
        }

        public static implicit operator Type(AbilityStringField abilityStringField) => abilityStringField._type;

        public static explicit operator int(AbilityStringField abilityStringField) => (int)abilityStringField._type;

        public static AbilityStringField GetAbilityStringField(int i)
        {
            if (!_fields.TryGetValue(i, out var abilityStringField))
            {
                abilityStringField = new AbilityStringField((Type)i);
                _fields.Add(i, abilityStringField);
            }

            return abilityStringField;
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