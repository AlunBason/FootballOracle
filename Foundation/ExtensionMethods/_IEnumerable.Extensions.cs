
namespace System.Collections.Generic
{
    public static class _IEnumerableExtensions
    {
        public static void _ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T value in source)
                action(value);
        }
    }
}