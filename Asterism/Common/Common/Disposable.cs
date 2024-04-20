using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asterism.Common
{
    internal class Disposables : ICollection<IDisposable>
    {
        private HashSet<IDisposable> _disposables = new();
        public int Count => _disposables.Count;

        public bool IsReadOnly => false;

        public void Add(IDisposable item)
        {
            _disposables.Add(item);
        }

        public void Clear()
        {
            foreach(var disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables.Clear();
        }

        public bool Contains(IDisposable item)
        {
            return _disposables.Contains(item);
        }

        public void CopyTo(IDisposable[] array, int arrayIndex)
        {
            _disposables.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IDisposable> GetEnumerator()
        {
            return _disposables.GetEnumerator();
        }

        public bool Remove(IDisposable item)
        {
            item.Dispose();
            _disposables.Remove(item);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _disposables.GetEnumerator();
        }
    }
}
