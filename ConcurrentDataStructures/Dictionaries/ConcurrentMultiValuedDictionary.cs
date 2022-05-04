using ConcurrentDataStructures.Extensions;
using ConcurrentDataStructures.Sets;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ConcurrentDataStructures.Dictionaries
{
    public class ConcurrentMultiValuedDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, ConcurrentHashSet<TValue>>>
    {
        private readonly ConcurrentDictionary<TKey, ConcurrentHashSet<TValue>> _map = new ConcurrentDictionary<TKey, ConcurrentHashSet<TValue>>();

        public ConcurrentMultiValuedDictionary() { }

        public int Count => _map.Count;

        public ConcurrentHashSet<TValue> this[TKey key]
        {
            get
            {
                if (ContainsKey(key))
                    return _map[key];
                throw new KeyNotFoundException("Key not found!");
            }
        }

        public void TryAdd(TKey key, TValue val)
        {
            if (_map.ContainsKey(key))
            {
                _map[key].TryAdd(val);
            }
            else
            {
                var list = new ConcurrentHashSet<TValue>();
                list.TryAdd(val);

                if (_map.ContainsKey(key))
                    _map[key].TryAdd(val);
                else
                    _map.TryAdd(key, list);
            }
        }

        public void TryRemove(TKey key)
        {
            _map.TryRemove(key, out _);
        }

        public void TryRemove(TKey key, TValue val)
        {
            if (_map.ContainsKey(key))
            {
                _map[key].TryRemove(val);
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _map.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, ConcurrentHashSet<TValue>>> GetEnumerator()
        {
            return _map.GetEnumerator().Cast<KeyValuePair<TKey, ConcurrentHashSet<TValue>>>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _map.GetEnumerator();
        }
    }
}
