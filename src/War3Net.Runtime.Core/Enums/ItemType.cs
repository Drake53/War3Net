namespace War3Net.Runtime.Enums
{
    public sealed class ItemType : Handle
    {
        private static readonly Dictionary<int, ItemType> _types = GetTypes().ToDictionary(t => (int)t, t => new ItemType(t));

        private readonly Type _type;

        private ItemType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Permanent = 0,
            Charged = 1,
            Powerup = 2,
            Artifact = 3,
            Purchasable = 4,
            Campaign = 5,
            Miscellaneous = 6,
            Unknown = 7,
            Any = 8,
        }

        public static implicit operator Type(ItemType itemType) => itemType._type;

        public static explicit operator int(ItemType itemType) => (int)itemType._type;

        public static ItemType GetItemType(int i)
        {
            if (!_types.TryGetValue(i, out var itemType))
            {
                itemType = new ItemType((Type)i);
                _types.Add(i, itemType);
            }

            return itemType;
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