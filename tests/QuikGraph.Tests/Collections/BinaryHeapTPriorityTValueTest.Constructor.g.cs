// <copyright file="BinaryHeapTPriorityTValueTest.Constructor.g.cs" company="MSIT">Copyright © MSIT 2007</copyright>
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
using System;
using NUnit.Framework;
using QuikGraph.Tests;

namespace QuikGraph.Collections
{
    internal partial class BinaryHeapTPriorityTValueTest
    {
//[Test]
public void ConstructorThrowsContractException338()
{
    QuikGraphAssert.ThrowsContractException(() => this.Constructor<int, Edge<int>>(int.MinValue));
}
[Test]
public void Constructor703()
{
    this.Constructor<int, Edge<int>>(0);
}
[Test]
[Ignore("Was already ignored")]
public void ConstructorThrowsContractException308()
{
    QuikGraphAssert.ThrowsContractException(() => this.Constructor<int, SEdge<int>>(int.MinValue));
}
[Test]
public void Constructor70302()
{
    this.Constructor<int, SEdge<int>>(0);
}
[Test]
public void Constructor70301()
{
    this.Constructor<int, int>(0);
}
[Test]
[Ignore("Was already ignored")]
public void ConstructorThrowsContractException735()
{
    QuikGraphAssert.ThrowsContractException(() => this.Constructor<int, int>(int.MinValue));
}
    }
}
