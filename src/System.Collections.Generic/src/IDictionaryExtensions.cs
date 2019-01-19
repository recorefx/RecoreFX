namespace System.Collections.Generic
{
    public static class IDictionaryExtensions<TKey, TValue>
    {
        // Fluent version of Add
        public static IDictionary<TKey, TValue> Append(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict.Add(key, value);
            return dict;
        }
    }
}