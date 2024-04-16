using System.Collections.Generic;
using System.Linq;

namespace Asterism.Common.Extension
{
    static public class ListExtension
    {
        public static bool TryAdd<T>(this ICollection<T> list, T item)
        {
            if (item is null)
                return false;

            if (list.Contains(item))
                return false;

            list.Add(item);
            return true;
        }

        public static bool TryAdd<T>(this ICollection<T> list, params T[] items)
        {
            if (items is null)
                return false;

            if (items.Length is 0)
                return false;

            foreach (var item in items)
            {
                if (item is null)
                    continue;

                if (list.Contains(item))
                    continue;

                list.Add(item);
            }

            return true;
        }

        public static bool TryRemove<T>(this ICollection<T> list, T? item)
            where T : struct
        {
            if (item is null)
                return false;

            if (!list.Contains(item.Value))
                return false;

            list.Remove(item.Value);
            return true;
        }

        public static bool TryGet<T>(this IList<T> list, int index, out T result)
        {
            result = default;
            if (index < 0 || index >= list.Count)
                return false;

            result = list[index];
            return true;
        }

        public static bool TryGetFirst<T>(this IEnumerable<T> list, ref T result)
        {
            return list.TryGetFirstOrDefault(ref result);
        }

        public static bool TryGetFirstOrNull<T>(this IEnumerable<T> list, ref T? result)
            where T : struct
        {
            result = null;
            if (list.Count() is 0)
                return false;

            result = list.First();
            return true;
        }
        public static bool TryGetFirstOrNull<T>(this IEnumerable<T> list, ref T result)
            where T : class
        {
            result = null;
            if (list.Count() is 0)
                return false;

            result = list.First();
            return true;
        }

        public static bool TryGetFirstOrDefault<T>(this IEnumerable<T> list, ref T result)
        {
            result = default;
            if (list.Count() is 0)
                return false;

            result = list.First();
            return true;
        }

        public static bool TryGetLast<T>(this IEnumerable<T> list, ref T result)
        {
            return list.TryGetLastOrDefault(ref result);
        }

        public static bool TryGetLastOrNull<T>(this IEnumerable<T> list, ref T? result)
            where T : struct
        {
            result = null;
            if (list.Count() is 0)
                return false;

            result = list.Last();
            return true;
        }

        public static bool TryGetLastOrNull<T>(this IEnumerable<T> list, ref T result)
            where T : class
        {
            result = null;
            if (list.Count() is 0)
                return false;

            result = list.Last();
            return true;
        }

        public static bool TryGetLastOrDefault<T>(this IEnumerable<T> list, ref T result)
        {
            result = default;
            if (list.Count() is 0)
                return false;

            result = list.Last();
            return true;
        }
    }
}
