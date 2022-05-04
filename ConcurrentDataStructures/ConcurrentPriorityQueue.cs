using System;
using System.Collections.Concurrent;

namespace ConcurrentDataStructures
{
    public class ConcurrentPriorityQueue<TVal>
    {
        //private readonly ConcurrentDictionary<TVal, int> 

        public ConcurrentPriorityQueue()
        {

        }

        public bool TryEnqueue(TVal val, int priority)
        {
            return true;
        }

        public bool TryDequeue(out TVal val, out int priority)
        {
            val = default(TVal);
            priority = 0;
            return true;
        }

        public bool Peek(out TVal val, out int priority)
        {
            val = default(TVal);
            priority = 0;
            return true;
        }

        public bool Pop(out TVal val, out int priority)
        {
            val = default(TVal);
            priority = 0;
            return true;
        }

        public int Count { get; }
    }
}
