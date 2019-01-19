namespace System.Collections.Generic
{
    public static class ListExtensions
    {
        // Fluent version of AddRange
        // The peculiarly named `collection` parameter is to match AddRange
        public static List<T> AppendRange<T>(this List<T> list, IEnumerable<T> collection)
        {
            list.AddRange(collection);
            return list;
        }
    }
}