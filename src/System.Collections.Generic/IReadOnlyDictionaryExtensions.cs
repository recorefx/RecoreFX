namespace System.Collections.Generic
{
    public static class IReadOnlyDictionaryExtensions<TKey, TValue>
    {
        public static TValue ValueOrDefault(this IReadOnlyDictionary<TKey, TValue> dict, TKey key)
        {
            if (dict.TryGetValue(key, out TValue value))
            {
                return value;
            }
            else
            {
                return default;
            }
        }
    }
}