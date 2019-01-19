namespace System.Collections.Generic
{
    public static class ICollectionExtensions<T>
    {
        // Fluent version of Add
        public static ICollection<T> Append(this ICollection<T> collection, T item)
        {
            collection.Add(item);
            return collection;
        }
    }
}