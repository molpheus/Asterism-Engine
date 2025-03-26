using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asterism.Common.Collection
{
    public interface ICollectionExtension<T>
    {
        bool TryAdd(T item);
        bool TryAddRange(params T[] items);
        bool TryAddRange(ICollection<T> items);
        bool TryGet(int index, out T item);
        bool TryRemove(T item);
        bool TryRemoveAt(int index);
        bool TryContains(T item);
        bool TryClear();
        bool TryContainsAll();
        bool TryContainsAny();
        bool TryRemoveAll();
        bool TryRemoveRange(params T[] items);
        bool TryRemoveRange(ICollection<T> items);
        bool TryRemoveWhere(Func<T, bool> predicate);
        bool TryRemoveAllWhere(Func<T, bool> predicate);
        bool TryRemoveRangeWhere(Func<T, bool> predicate);
        bool TryRemoveAllRangeWhere(Func<T, bool> predicate);
    }
}
