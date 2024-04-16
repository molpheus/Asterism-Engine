using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static bool TryRemove<T>(this ICollection<T> list, T item)
        {
            if (item is null)
                return false;

            if (!list.Contains(item))
                return false;

            list.Remove(item);
            return true;
        }

        public static bool TryGet<T>(this IList<T> list, int index, ref T result)
        {
            if (index < 0 || index >= list.Count)
                return false;

            result = list[index];
            return true;
        }

        public static bool TryGetFirst<T>(this IEnumerable<T> list, ref T ?result)
        {
            result = list.First();
            return true;
        }

        public static bool TryGetLast<T>(this IEnumerable<T> list, ref T ?result)
        {
            result = list.Last();
            return true;
        }
    }
}
