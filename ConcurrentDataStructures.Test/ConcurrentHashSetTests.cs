using ConcurrentDataStructures.Sets;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentDataStructures.Test
{
    public class ConcurrentHashSetTests
    {
        private static readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        [SetUp]
        public void Setup()
        {

        }

        #region Index Test



        #endregion

        #region Iterator Test

        [Test]
        [TestCase(100, 1000)]
        [TestCase(1000, 10000)]
        [TestCase(10000, 100000)]
        [TestCase(100000, 10000000)]
        [TestCase(0, 10000000)]
        public void Iterator(int lowerBound, int upperBound)
        {
            ConcurrentHashSet<int> dict = InsertionTestRandomized(lowerBound, upperBound);
            int count = dict.Count;
            int itCount = 0;
            foreach (var item in dict)
            {
                itCount++;
            }
            Assert.IsTrue(itCount == count, "Iteration failed");
        }

        #endregion

        #region Randomized Test

        [Test]
        [TestCase(100, 1000)]
        [TestCase(1000, 10000)]
        [TestCase(10000, 100000)]
        [TestCase(100000, 10000000)]
        [TestCase(0, 10000000)]
        public void TestParallelEntryAndRemoveRandomized(int lowerBound, int upperBound)
        {
            ConcurrentHashSet<int> dict = InsertionTestRandomized(lowerBound, upperBound);
            DeletionTestRandomized(dict, lowerBound, upperBound);
        }
        private static void DeletionTestRandomized(ConcurrentHashSet<int> dict, int lowerBound, int upperBound)
        {
            int size = dict.Count;
            var removeList = GetRandomData(lowerBound, upperBound).ToList();
            int toRemove = removeList.Distinct().Count(x => dict.ContainsKey(x));
            Parallel.ForEach(removeList, i =>
            {
                dict.TryRemove(i);
            });
            Assert.IsTrue(dict.Count == (size - toRemove < 0 ? 0 : size - toRemove), "Not removed properly");
        }

        private static ConcurrentHashSet<int> InsertionTestRandomized(int lowerBound, int upperBound)
        {
            var dict = new ConcurrentHashSet<int>();
            var insertList = GetRandomData(lowerBound, upperBound).ToList();
            int toInsert = insertList.Distinct().Count();
            Parallel.ForEach(insertList, i =>
            {
                dict.TryAdd(i);
            });
            Assert.IsTrue(dict.Count == toInsert, "Not inserted properly");
            return dict;
        }


        private static IEnumerable<int> GetRandomData(int lowerBound, int upperBound)
        {
            foreach (var i in Enumerable.Range(0, upperBound))
            {
                yield return _random.Next(lowerBound, upperBound);
            }
        }

        #endregion

        #region Regular Test

        [Test]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        [TestCase(1000000)]
        public void TestParallelEntry(int value)
        {
            ConcurrentHashSet<int> dict = InsertionTest(value);
        }

        [Test]
        [TestCase(100, 100)]
        [TestCase(1000, 100)]
        [TestCase(10000, 100)]
        [TestCase(10000, 1000)]
        [TestCase(10000, 100000)]
        public void TestParallelRemove(int insert, int remove)
        {
            ConcurrentHashSet<int> dict = InsertionTest(insert);

            DeletionTest(dict, remove);
        }

        private static void DeletionTest(ConcurrentHashSet<int> dict, int toRemove)
        {
            int size = dict.Count;
            toRemove = toRemove > size ? size : toRemove;
            Parallel.ForEach(Enumerable.Range(0, toRemove), i =>
            {
                Assert.IsTrue(dict.TryRemove(i), "Should be removed!");
            });
            Assert.IsTrue(dict.Count == (size - toRemove < 0 ? 0 : size - toRemove), "Not removed properly");
        }

        private static ConcurrentHashSet<int> InsertionTest(int toInsert)
        {
            var dict = new ConcurrentHashSet<int>();
            Parallel.ForEach(Enumerable.Range(0, toInsert), i =>
            {
                Assert.IsTrue(dict.TryAdd(i), "Should be inserted!");
            });
            Assert.IsTrue(dict.Count == toInsert, "Not inserted properly");
            return dict;
        }

        #endregion
    }
}