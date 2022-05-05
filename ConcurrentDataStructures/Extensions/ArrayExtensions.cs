using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentDataStructures.Extensions
{
    internal static class ArrayExtensions
    {
        internal static IEnumerable<TType> CastEach<TType>(this Array list)
        {
            foreach (var item in list)
                yield return (TType)item;
        }
    }
}
