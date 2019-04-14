﻿using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Tests;

namespace QuikGraph.Tests.Algorithms.ConnectedComponents
{
    [TestFixture]
    internal class IncrementalConnectedComponentsAlgorithmTest : QuikGraphUnitTests
    {
        [Test]
        public void IncrementalConnectedComponent()
        {
            var g = new AdjacencyGraph<int, SEquatableEdge<int>>();
            g.AddVertexRange(new int[] { 0, 1, 2, 3 });
            var components = AlgorithmExtensions.IncrementalConnectedComponents(g);

            var current = components();
            Assert.AreEqual(4, current.Key);

            g.AddEdge(new SEquatableEdge<int>(0, 1));
            current = components();
            Assert.AreEqual(3, current.Key);

            g.AddEdge(new SEquatableEdge<int>(2, 3));
            current = components();
            Assert.AreEqual(2, current.Key);

            g.AddEdge(new SEquatableEdge<int>(1, 3));
            current = components();
            Assert.AreEqual(1, current.Key);
        }
    }
}
