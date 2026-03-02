namespace War3Net.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static void SetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
            where TKey : notnull
        {
            if (dict is null)
            {
                throw new ArgumentNullException(nameof(dict));
            }

            if (!dict.TryAdd(key, value))
            {
                dict[key] = value;
            }
        }
    }
}