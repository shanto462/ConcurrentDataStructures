using ConcurrentDataStructures.Sets;
using DotThrow.ExceptionExtensions;
using DotThrow.Throwble;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConcurrentDataStructures.Heaps
{
    internal sealed class BinaryMinHeap<TKey, TVal> where TKey : IComparable<TKey>
    {
        private readonly List<KeyValuePair<TKey, TVal>> _list = new List<KeyValuePair<TKey, TVal>>();
        private readonly ExceptionThrower<List<KeyValuePair<TKey, TVal>>> _exceptionThrower;

        public BinaryMinHeap()
        {
            _exceptionThrower = _list.CreateThrower();
        }

        public BinaryMinHeap(BinaryMinHeap<TKey, TVal> minHeap) : base()
        {
            _list = minHeap.Items;
        }

        public int Count => _list.Count;

        public void Clear()
        {
            _list.Clear();
        }

        public void Enqueue(TKey key, TVal val)
        {
            var pair = new KeyValuePair<TKey, TVal>(key, val);

            Enqueue(pair);
        }

        public void Enqueue(KeyValuePair<TKey, TVal> pair)
        {
            _list.Add(pair);

            if (_list.Count == 1)
                return;

            int index = _list.Count - 1;

            while (index > 0)
            {
                int nextIndex = (index - 1) / 2;
                var nextPair = _list[nextIndex];

                if (pair.Key.CompareTo(nextPair.Key) < 0)
                {
                    _list[index] = nextPair;
                    index = nextIndex;
                }
                else
                {
                    break;
                }
            }

            _list[index] = pair;
        }

        public KeyValuePair<TKey, TVal> Peek()
        {
            _exceptionThrower.ThrowIf(l => l.Count == 0, "Heap is empty!");
            return _list[0];
        }

        public KeyValuePair<TKey, TVal> Pop()
        {
            _exceptionThrower.ThrowIf(l => l.Count == 0, "The heap is empty!");
            var toReturn = _list[0];

            if (_list.Count <= 2) _list.RemoveAt(0);

            else
            {
                _list[0] = _list[_list.Count - 1];
                _list.RemoveAt(_list.Count - 1);

                int current = 0, possibleSwap = 0;

                while (true)
                {
                    int leftChildPos = 2 * current + 1;
                    int rightChildPos = leftChildPos + 1;

                    if (leftChildPos < _list.Count)
                    {
                        var entry1 = _list[current];
                        var entry2 = _list[leftChildPos];

                        if (entry2.Key.CompareTo(entry1.Key) < 0) possibleSwap = leftChildPos;
                    }
                    else
                        break;

                    if (rightChildPos < _list.Count)
                    {
                        var entry1 = _list[possibleSwap];
                        var entry2 = _list[rightChildPos];

                        if (entry2.Key.CompareTo(entry1.Key) < 0) possibleSwap = rightChildPos;
                    }

                    if (current != possibleSwap)
                    {
                        var temp = _list[current];
                        _list[current] = _list[possibleSwap];
                        _list[possibleSwap] = temp;
                    }
                    else
                        break;

                    current = possibleSwap;
                }
            }
            return toReturn;
        }

        internal List<KeyValuePair<TKey, TVal>> Items => _list;
    }
}
