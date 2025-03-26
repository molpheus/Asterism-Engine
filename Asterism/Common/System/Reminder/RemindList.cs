using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asterism.Common.Collection;

namespace Asterism.System.Reminder
{
    public partial class RemindList : ICollection<RemindData>
    {
        private HashSet<RemindData> _remindList = new();

        public int Count => _remindList.Count;
        public bool IsReadOnly => false;

        public void Add(RemindData item)
            => _remindList.Add(item);

        public void Clear()
            => _remindList.Clear();
        public bool Contains(RemindData item)
            => _remindList.Any(x => x.Time == item.Time && x.Message == item.Message);

        public void CopyTo(RemindData[] array, int arrayIndex)
        {
            for(int i = arrayIndex; i < array.Length; i++)
                _remindList.Add(array[i]);
        }

        public IEnumerator<RemindData> GetEnumerator()
        {
            foreach (var item in _remindList)
                yield return item;
        }

        public bool Remove(RemindData item)
        {
            return _remindList.RemoveWhere(x => x.Time == item.Time && x.Message == item.Message) is not 0;
        }

        public bool RemoveOlderThen(DateTime time)
        {
            return _remindList.RemoveWhere(x => x.Time <= time) is not 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public partial class RemindList : ICollectionExtension<RemindData>
    {
        public bool TryAdd(RemindData item)
        {
            if (item is null)
                return false;

            if (Contains(item))
                return false;

            return _remindList.Add(item);
        }

        public bool TryAddRange(params RemindData[] items)
        {
            bool allAdded = true;
            foreach (var item in items)
            {
                if (item is not null)
                    allAdded &= _remindList.Add(item);
            }
            return allAdded;
        }

        public bool TryAddRange(ICollection<RemindData> items)
        {
            bool allAdded = true;
            foreach (var item in items)
            {
                if (item is not null)
                    allAdded &= _remindList.Add(item);
            }
            return allAdded;
        }

        public bool TryGet(int index, out RemindData item)
        {
            item = default;
            if (index >= 0 && index < _remindList.Count)
            {
                item = _remindList.ElementAt(index);
                return true;
            }
            return false;
        }

        public bool TryRemove(RemindData item)
        {
            if (item is RemindData remindData)
            {
                return _remindList.Remove(remindData);
            }
            return false;
        }

        public bool TryRemoveAt(int index)
        {
            if (index >= 0 && index < _remindList.Count)
            {
                var item = _remindList.ElementAt(index);
                return _remindList.Remove(item);
            }
            return false;
        }

        public bool TryContains(RemindData item)
        {
            if (item is RemindData remindData)
            {
                return _remindList.Contains(remindData);
            }
            return false;
        }

        public bool TryClear()
        {
            _remindList.Clear();
            return true;
        }

        public bool TryContainsAll()
        {
            return _remindList.Count > 0;
        }

        public bool TryContainsAny()
        {
            return _remindList.Count > 0;
        }

        public bool TryRemoveAll()
        {
            _remindList.Clear();
            return true;
        }

        public bool TryRemoveRange(params RemindData[] items)
        {
            bool allRemoved = true;
            foreach (var item in items)
            {
                if (item is RemindData remindData)
                {
                    allRemoved &= _remindList.Remove(remindData);
                }
                else
                {
                    allRemoved = false;
                }
            }
            return allRemoved;
        }

        public bool TryRemoveRange(ICollection<RemindData> items)
        {
            bool allRemoved = true;
            foreach (var item in items)
            {
                if (item is RemindData remindData)
                {
                    allRemoved &= _remindList.Remove(remindData);
                }
                else
                {
                    allRemoved = false;
                }
            }
            return allRemoved;
        }

        public bool TryRemoveWhere(Func<RemindData, bool> predicate)
        {
            var itemsToRemove = _remindList.Where(x => predicate(x)).ToList();
            foreach (var item in itemsToRemove)
            {
                _remindList.Remove(item);
            }
            return itemsToRemove.Count > 0;
        }

        public bool TryRemoveAllWhere(Func<RemindData, bool> predicate)
        {
            var itemsToRemove = _remindList.Where(x => predicate(x)).ToList();
            foreach (var item in itemsToRemove)
            {
                _remindList.Remove(item);
            }
            return itemsToRemove.Count > 0;
        }

        public bool TryRemoveRangeWhere(Func<RemindData, bool> predicate)
        {
            var itemsToRemove = _remindList.Where(x => predicate(x)).ToList();
            foreach (var item in itemsToRemove)
            {
                _remindList.Remove(item);
            }
            return itemsToRemove.Count > 0;
        }

        public bool TryRemoveAllRangeWhere(Func<RemindData, bool> predicate)
        {
            var itemsToRemove = _remindList.Where(x => predicate(x)).ToList();
            foreach (var item in itemsToRemove)
            {
                _remindList.Remove(item);
            }
            return itemsToRemove.Count > 0;
        }
    }
}
