namespace War3Net.Runtime.Enums.Object
{
    public sealed class ItemRealField : Handle
    {
        private static readonly Dictionary<int, ItemRealField> _fields = GetTypes().ToDictionary(t => (int)t, t => new ItemRealField(t));

        private readonly Type _type;

        private ItemRealField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            SCALING_VALUE = 1769169761,
        }

        public static implicit operator Type(ItemRealField itemRealField) => itemRealField._type;

        public static explicit operator int(ItemRealField itemRealField) => (int)itemRealField._type;

        public static ItemRealField GetItemRealField(int i)
        {
            if (!_fields.TryGetValue(i, out var itemRealField))
            {
                itemRealField = new ItemRealField((Type)i);
                _fields.Add(i, itemRealField);
            }

            return itemRealField;
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