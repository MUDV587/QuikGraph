using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Tests;

namespace QuikGraph.Collections
{
    /// <summary>This class contains parameterized unit tests for ForestDisjointSet`1</summary>
    [TestFixture]
    public partial class ForestDisjointSetTTest
    {
        public void Unions(int elementCount, KeyValuePair<int,int>[] unions)
        {
            Assert.IsTrue(0 < elementCount);
            QuikGraphAssert.TrueForAll(
                unions, 
                u => 0 <= u.Key && u.Key < elementCount && 
                     0 <= u.Value && u.Value < elementCount
                     );

            var target = new ForestDisjointSet<int>();
            // fill up with 0..elementCount - 1
            for (int i = 0; i < elementCount; i++)
            {
                target.MakeSet(i);
                Assert.IsTrue(target.Contains(i));
                Assert.AreEqual(i + 1, target.ElementCount);
                Assert.AreEqual(i + 1, target.SetCount);
            }

            // apply Union for pairs unions[i], unions[i+1]
            for (int i = 0; i < unions.Length; i++)
            {
                var left = unions[i].Key;
                var right= unions[i].Value;

                var setCount = target.SetCount;
                bool unioned = target.Union(left, right);
                // should be in the same set now
                Assert.IsTrue(target.AreInSameSet(left, right));
                // if unioned, the count decreased by 1
                QuikGraphAssert.ImpliesIsTrue(unioned, () => setCount - 1 == target.SetCount);
            }
        }
    }
}
