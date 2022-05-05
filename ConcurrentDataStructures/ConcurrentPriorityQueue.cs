using ConcurrentDataStructures.Heaps;
using DotThrow.ExceptionExtensions;
using DotThrow.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ConcurrentDataStructures.Extensions;
using System.Linq;

namespace ConcurrentDataStructures
{
    public class ConcurrentPriorityQueue<TKey, TVal> : IProducerConsumerCollection<KeyValuePair<TKey, TVal>> where TKey : IComparable<TKey>
    {
        private readonly object _padLock = new object();
        private readonly BinaryMinHeap<TKey, TVal> _minHeap = new BinaryMinHeap<TKey, TVal>();

        public ConcurrentPriorityQueue()
        {

        }

        public ConcurrentPriorityQueue(IEnumerable<KeyValuePair<TKey, TVal>> collection) : base()
        {
            collection.CreateThrower().ThrowIf(c => c is null, "Collection is empty!");

            foreach (var item in collection)
            {
                _minHeap.Enqueue(item);
            }
        }

        public int Count
        {
            get
            {
                lock (_padLock)
                    return _minHeap.Count;
            }
        }

        public bool IsSynchronized => true;

        public object SyncRoot => _padLock;


        public void Enqueue(TKey priority, TVal val)
        {
            Enqueue(new KeyValuePair<TKey, TVal>(priority, val));
        }

        public void Enqueue(KeyValuePair<TKey, TVal> keyValuePair)
        {
            lock (_padLock)
                _minHeap.Enqueue(keyValuePair);
        }

        public bool TryDequeue(out KeyValuePair<TKey, TVal> result)
        {
            result = default(KeyValuePair<TKey, TVal>);
            lock (_padLock)
            {
                if (_minHeap.Count > 0)
                {
                    result = _minHeap.Pop();
                    return true;
                }
            }
            return false;
        }

        public bool TryPeek(out KeyValuePair<TKey, TVal> result)
        {
            result = default(KeyValuePair<TKey, TVal>);
            lock (_padLock)
            {
                if (_minHeap.Count > 0)
                {
                    result = _minHeap.Peek();
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            lock (_padLock)
                _minHeap.Clear();
        }

        public bool IsEmpty() => _minHeap.Count == 0;

        public void CopyTo(KeyValuePair<TKey, TVal>[] array, int index)
        {
            lock (_padLock)
                _minHeap.Items.CopyTo(array, index);
        }

        public void CopyTo(Array array, int index)
        {
            lock (_padLock)
                _minHeap.Items.CopyTo(array.CastEach<KeyValuePair<TKey, TVal>>().ToArray(), index);
        }

        public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TVal>>)ToArray()).GetEnumerator();
        }

        public KeyValuePair<TKey, TVal>[] ToArray()
        {
            lock (_padLock)
            {
                var clonedHeap = new BinaryMinHeap<TKey, TVal>(_minHeap);
                var result = new KeyValuePair<TKey, TVal>[_minHeap.Count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = clonedHeap.Pop();
                }
                return result;
            }
        }

        public bool TryAdd(KeyValuePair<TKey, TVal> item)
        {
            Enqueue(item);
            return true;
        }

        public bool TryTake([MaybeNullWhen(false)] out KeyValuePair<TKey, TVal> item)
        {
            return TryDequeue(out item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
