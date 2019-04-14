// <copyright file="BinaryHeapTPriorityTValueTest.Insert.g.cs" company="MSIT">Copyright © MSIT 2007</copyright>
// <auto-generated>
// This file contains automatically generated unit tests.
// Do NOT modify this file manually.
// 
// When Pex is invoked again,
// it might remove or update any previously generated unit tests.
// 
// If the contents of this file becomes outdated, e.g. if it does not
// compile anymore, you may delete this file and invoke Pex again.
// </auto-generated>
using System.Collections.Generic;
using NUnit.Framework;

namespace QuikGraph.Collections
{
    internal partial class BinaryHeapTPriorityTValueTest
    {
[Test]
public void Insert636()
{
    BinaryHeap<int, int> binaryHeap;
    binaryHeap = BinaryHeapFactory.Create(0);
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[0];
    this.Insert<int, int>(binaryHeap, keyValuePairs);
    Assert.IsNotNull((object)binaryHeap);
    Assert.IsNotNull(binaryHeap.PriorityComparison);
    Assert.AreEqual(0, binaryHeap.Capacity);
    Assert.AreEqual(0, binaryHeap.Count);
}
[Test]
public void Insert471()
{
    BinaryHeap<int, int> binaryHeap;
    binaryHeap = BinaryHeapFactory.Create(1);
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[1];
    this.Insert<int, int>(binaryHeap, keyValuePairs);
    Assert.IsNotNull((object)binaryHeap);
    Assert.IsNotNull(binaryHeap.PriorityComparison);
    Assert.AreEqual(1, binaryHeap.Capacity);
    Assert.AreEqual(1, binaryHeap.Count);
}
[Test]
public void Insert47101()
{
    BinaryHeap<int, int> binaryHeap;
    binaryHeap = BinaryHeapFactory.Create(0);
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[1];
    this.Insert<int, int>(binaryHeap, keyValuePairs);
    Assert.IsNotNull((object)binaryHeap);
    Assert.IsNotNull(binaryHeap.PriorityComparison);
    Assert.AreEqual(1, binaryHeap.Capacity);
    Assert.AreEqual(1, binaryHeap.Count);
}
[Test]
public void Insert504()
{
    BinaryHeap<int, int> binaryHeap;
    binaryHeap = BinaryHeapFactory.Create(1);
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[2];
    this.Insert<int, int>(binaryHeap, keyValuePairs);
    Assert.IsNotNull((object)binaryHeap);
    Assert.IsNotNull(binaryHeap.PriorityComparison);
    Assert.AreEqual(3, binaryHeap.Capacity);
    Assert.AreEqual(2, binaryHeap.Count);
}
[Test]
public void Insert50401()
{
    BinaryHeap<int, int> binaryHeap;
    binaryHeap = BinaryHeapFactory.Create(2);
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[2];
    this.Insert<int, int>(binaryHeap, keyValuePairs);
    Assert.IsNotNull((object)binaryHeap);
    Assert.IsNotNull(binaryHeap.PriorityComparison);
    Assert.AreEqual(2, binaryHeap.Capacity);
    Assert.AreEqual(2, binaryHeap.Count);
}
[Test]
public void Insert280()
{
    BinaryHeap<int, int> binaryHeap;
    binaryHeap = BinaryHeapFactory.Create(2);
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[6];
    KeyValuePair<int, int> s0 = new KeyValuePair<int, int>(1, default(int));
    keyValuePairs[0] = s0;
    KeyValuePair<int, int> s1 = new KeyValuePair<int, int>(1073741824, default(int))
      ;
    keyValuePairs[2] = s1;
    KeyValuePair<int, int> s2
       = new KeyValuePair<int, int>(int.MinValue, default(int));
    keyValuePairs[3] = s2;
    KeyValuePair<int, int> s3 = new KeyValuePair<int, int>(int.MinValue, 1);
    keyValuePairs[4] = s3;
    KeyValuePair<int, int> s4 = new KeyValuePair<int, int>(11, 11);
    keyValuePairs[5] = s4;
    this.Insert<int, int>(binaryHeap, keyValuePairs);
    Assert.IsNotNull((object)binaryHeap);
    Assert.IsNotNull(binaryHeap.PriorityComparison);
    Assert.AreEqual(11, binaryHeap.Capacity);
    Assert.AreEqual(6, binaryHeap.Count);
}
    }
}
