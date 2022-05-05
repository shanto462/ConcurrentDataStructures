using ConcurrentDataStructures.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ConcurrentDataStructures.Sets
{
    public class ConcurrentHashSet<TVal> : IEnumerable<TVal>
    {
        private readonly ConcurrentDictionary<TVal, byte> _map = new ConcurrentDictionary<TVal, byte>();

        public ConcurrentHashSet() { }

        public int Count => _map.Count;

        public bool TryAdd(TVal val)
        {
            return _map.TryAdd(val, new byte());
        }

        public bool TryRemove(TVal val)
        {
            return _map.TryRemove(val, out _);
        }

        public bool ContainsKey(TVal val)
        {
            return _map.ContainsKey(val);
        }

        public void Clear()
        {
            _map.Clear();
        }

        public IEnumerator<TVal> GetEnumerator()
        {
            return _map.Keys.GetEnumerator().Cast<TVal>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _map.Keys.GetEnumerator();
        }
    }
}
