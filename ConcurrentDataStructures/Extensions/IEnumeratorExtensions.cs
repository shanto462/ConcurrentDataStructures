using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ConcurrentDataStructures.Extensions
{
    public static class IEnumeratorExtensions
    {
        public static IEnumerator<T> Cast<T>(this IEnumerator iterator)
        {
            while (iterator.MoveNext())
            {
                yield return (T)iterator.Current;
            }
        }
    }
}
