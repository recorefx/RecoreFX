namespace System.Collections.Generic
{
    public static class ListExtensions<T>
    {
        // Fluent version of AddRange
        // The peculiarly named `collection` parameter is to match AddRange
        public static List<T> AppendRange(this List<T> list, IEnumerable<T> collection)
        {
            list.AddRange(collection);
            return list;
        }
    }
}