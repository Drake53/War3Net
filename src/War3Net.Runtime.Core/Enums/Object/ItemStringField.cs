namespace War3Net.Runtime.Enums.Object
{
    public sealed class ItemStringField : Handle
    {
        private static readonly Dictionary<int, ItemStringField> _fields = GetTypes().ToDictionary(t => (int)t, t => new ItemStringField(t));

        private readonly Type _type;

        private ItemStringField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            MODEL_USED = 1768319340,
        }

        public static implicit operator Type(ItemStringField itemStringField) => itemStringField._type;

        public static explicit operator int(ItemStringField itemStringField) => (int)itemStringField._type;

        public static ItemStringField GetItemStringField(int i)
        {
            if (!_fields.TryGetValue(i, out var itemStringField))
            {
                itemStringField = new ItemStringField((Type)i);
                _fields.Add(i, itemStringField);
            }

            return itemStringField;
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