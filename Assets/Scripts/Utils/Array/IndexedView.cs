using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Array
{
    public class IndexedView<T>: IReadOnlyList<T>
    {
        private readonly T[] _internal;
        private readonly int[] _indices;

        public IndexedView(T[] @internal, int[] indices)
        {
            _indices = indices;
            _internal = @internal;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _indices.Select(index => _internal[index]).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _indices.Length;

        public T this[int index] => _internal[_indices[index]];
    }
}