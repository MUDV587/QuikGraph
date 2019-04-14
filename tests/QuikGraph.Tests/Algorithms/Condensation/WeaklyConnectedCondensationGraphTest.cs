using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Serialization;
using QuikGraph.Tests;

namespace QuikGraph.Algorithms.Condensation
{
    [TestFixture]
    internal class WeaklyConnectedCondensationGraphAlgorithmTest : QuikGraphUnitTests
    {
        [Test]
        public void WeaklyConnectedCondensatAll()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
                this.WeaklyConnectedCondensate(g);
        }

        public void WeaklyConnectedCondensate<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var algo = new CondensationGraphAlgorithm<TVertex,TEdge, AdjacencyGraph<TVertex,TEdge>>(g);
            algo.StronglyConnected = false;
            algo.Compute();
            CheckVertexCount(g, algo);
            CheckEdgeCount(g, algo);
            CheckComponentCount(g, algo);
        }

        private void CheckVertexCount<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g,
            CondensationGraphAlgorithm<TVertex,TEdge, AdjacencyGraph<TVertex,TEdge>> algo)
            where TEdge : IEdge<TVertex>
        {
            int count = 0;
            foreach (var vertices in algo.CondensedGraph.Vertices)
                count += vertices.VertexCount;
            Assert.AreEqual(g.VertexCount, count, "VertexCount does not match");
        }

        private void CheckEdgeCount<TVertex,TEdge>(IVertexAndEdgeListGraph<TVertex,TEdge> g,
            CondensationGraphAlgorithm<TVertex,TEdge, AdjacencyGraph<TVertex,TEdge>> algo)
            where TEdge : IEdge<TVertex>
        {
            // check edge count
            int count = 0;
            foreach (var edges in algo.CondensedGraph.Edges)
                count += edges.Edges.Count;
            foreach (var vertices in algo.CondensedGraph.Vertices)
                count += vertices.EdgeCount;
            Assert.AreEqual(g.EdgeCount, count, "EdgeCount does not match");
        }


        private void CheckComponentCount<TVertex,TEdge>(IVertexAndEdgeListGraph<TVertex,TEdge> g,
            CondensationGraphAlgorithm<TVertex,TEdge, AdjacencyGraph<TVertex,TEdge>> algo)
            where TEdge : IEdge<TVertex>
        {
            // check number of vertices = number of strongly connected components
            int components = g.WeaklyConnectedComponents<TVertex,TEdge>(new Dictionary<TVertex, int>());
            Assert.AreEqual(components, algo.CondensedGraph.VertexCount, "ComponentCount does not match");
        }
    }
}
