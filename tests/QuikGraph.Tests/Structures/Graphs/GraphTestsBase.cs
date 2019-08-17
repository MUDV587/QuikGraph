using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.AssertHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Base class for graph tests.
    /// </summary>
    [TestFixture]
    internal class GraphTestsBase
    {
        #region Vertices helpers

        protected static void AssertNoVertex<TVertex>([NotNull] IVertexSet<TVertex> graph)
        {
            Assert.IsTrue(graph.IsVerticesEmpty);
            Assert.AreEqual(0, graph.VertexCount);
            CollectionAssert.IsEmpty(graph.Vertices);
        }

        protected static void AssertHasVertices<TVertex>(
            [NotNull] IVertexSet<TVertex> graph,
            [NotNull, ItemNotNull] IEnumerable<TVertex> vertices)
        {
            TVertex[] vertexArray = vertices.ToArray();
            CollectionAssert.IsNotEmpty(vertexArray);

            Assert.IsFalse(graph.IsVerticesEmpty);
            Assert.AreEqual(vertexArray.Length, graph.VertexCount);
            CollectionAssert.AreEquivalent(vertexArray, graph.Vertices);
        }

        #endregion

        #region Edges helpers

        protected static void AssertNoEdge<TVertex, TEdge>([NotNull] IEdgeSet<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.IsEdgesEmpty);
            Assert.AreEqual(0, graph.EdgeCount);
            CollectionAssert.IsEmpty(graph.Edges);
        }

        protected static void AssertHasEdges<TVertex, TEdge>(
            [NotNull] IEdgeSet<TVertex, TEdge> graph,
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsEdgesEmpty);
            Assert.AreEqual(edgeArray.Length, graph.EdgeCount);
            CollectionAssert.AreEquivalent(edgeArray, graph.Edges);
        }

        #endregion

        #region Graph helpers

        protected static void AssertEmptyGraph<TVertex, TEdge>([NotNull] IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            AssertNoVertex(graph);
            AssertNoEdge(graph);
        }

        protected static void AssertNoInEdge<TVertex, TEdge>([NotNull] IBidirectionalIncidenceGraph<TVertex, TEdge> graph, [NotNull] TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.IsInEdgesEmpty(vertex));
            Assert.AreEqual(0, graph.InDegree(vertex));
            CollectionAssert.IsEmpty(graph.InEdges(vertex));
        }

        protected static void AssertHasInEdges<TVertex, TEdge>(
            [NotNull] IBidirectionalIncidenceGraph<TVertex, TEdge> graph,
            [NotNull] TVertex vertex,
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsInEdgesEmpty(vertex));
            Assert.AreEqual(edgeArray.Length, graph.InDegree(vertex));
            CollectionAssert.AreEquivalent(edgeArray, graph.InEdges(vertex));
        }

        protected static void AssertNoOutEdge<TVertex, TEdge>([NotNull] IImplicitGraph<TVertex, TEdge> graph, [NotNull] TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.IsOutEdgesEmpty(vertex));
            Assert.AreEqual(0, graph.OutDegree(vertex));
            CollectionAssert.IsEmpty(graph.OutEdges(vertex));
        }

        protected static void AssertHasOutEdges<TVertex, TEdge>(
            [NotNull] IImplicitGraph<TVertex, TEdge> graph,
            [NotNull] TVertex vertex,
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsOutEdgesEmpty(vertex));
            Assert.AreEqual(edgeArray.Length, graph.OutDegree(vertex));
            CollectionAssert.AreEquivalent(edgeArray, graph.OutEdges(vertex));
        }

        protected static void AssertNoAdjacentEdge<TVertex, TEdge>([NotNull] IImplicitUndirectedGraph<TVertex, TEdge> graph, [NotNull] TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.IsAdjacentEdgesEmpty(vertex));
            Assert.AreEqual(0, graph.AdjacentDegree(vertex));
            CollectionAssert.IsEmpty(graph.AdjacentEdges(vertex));
        }

        protected static void AssertHasAdjacentEdges<TVertex, TEdge>(
            [NotNull] IImplicitUndirectedGraph<TVertex, TEdge> graph,
            [NotNull] TVertex vertex,
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges,
            int degree = -1)    // If not set => equals the count of edges
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsAdjacentEdgesEmpty(vertex));
            Assert.AreEqual(
                degree < 0 ? edgeArray.Length : degree,
                graph.AdjacentDegree(vertex));
            CollectionAssert.AreEquivalent(edgeArray, graph.AdjacentEdges(vertex));
        }

        #endregion

        #region Test helpers

        #region Add Vertices

        protected static void AddVertex_Test(
            [NotNull] IMutableVertexSet<TestVertex> graph)
        {
            int vertexAdded = 0;

            AssertNoVertex(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };

            // Vertex 1
            var vertex1 = new TestVertex("1");
            Assert.IsTrue(graph.AddVertex(vertex1));
            Assert.AreEqual(1, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1 });

            // Vertex 2
            var vertex2 = new TestVertex("2");
            Assert.IsTrue(graph.AddVertex(vertex2));
            Assert.AreEqual(2, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            Assert.IsFalse(graph.AddVertex(vertex1));
            Assert.AreEqual(2, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            var otherVertex1 = new TestVertex("1");
            Assert.IsTrue(graph.AddVertex(otherVertex1));
            Assert.AreEqual(3, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2, otherVertex1 });
        }

        protected static void AddVertex_Throws_Test<TVertex>(
            [NotNull] IMutableVertexSet<TVertex> graph)
            where TVertex : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVertex(null));
            AssertNoVertex(graph);
        }

        protected static void AddVertex_EquatableVertex_Test(
            [NotNull] IMutableVertexSet<EquatableTestVertex> graph)
        {
            int vertexAdded = 0;

            AssertNoVertex(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };

            // Vertex 1
            var vertex1 = new EquatableTestVertex("1");
            Assert.IsTrue(graph.AddVertex(vertex1));
            Assert.AreEqual(1, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1 });

            // Vertex 2
            var vertex2 = new EquatableTestVertex("2");
            Assert.IsTrue(graph.AddVertex(vertex2));
            Assert.AreEqual(2, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            Assert.IsFalse(graph.AddVertex(vertex1));
            Assert.AreEqual(2, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            var otherVertex1 = new EquatableTestVertex("1");
            Assert.IsFalse(graph.AddVertex(otherVertex1));
            Assert.AreEqual(2, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });
        }

        protected static void AddVertexRange_Test(
            [NotNull] IMutableVertexSet<TestVertex> graph)
        {
            int vertexAdded = 0;

            AssertNoVertex(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };

            // Vertex 1, 2, 3
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var vertex3 = new TestVertex("3");
            Assert.AreEqual(3, graph.AddVertexRange(new[] { vertex1, vertex2, vertex3 }));
            Assert.AreEqual(3, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2, vertex3 });

            // Vertex 1, 4
            var vertex4 = new TestVertex("4");
            Assert.AreEqual(1, graph.AddVertexRange(new[] { vertex1, vertex4 }));
            Assert.AreEqual(4, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2, vertex3, vertex4 });
        }

        protected static void AddVertexRange_Throws_Test(
            [NotNull] IMutableVertexSet<TestVertex> graph)
        {
            int vertexAdded = 0;

            AssertNoVertex(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVertexRange(null));
            AssertNoVertex(graph);
            Assert.AreEqual(0, vertexAdded);

            // Vertex 1, 2, 3
            var vertex1 = new TestVertex("1");
            var vertex3 = new TestVertex("3");
            Assert.Throws<ArgumentNullException>(() => graph.AddVertexRange(new[] { vertex1, null, vertex3 }));
            AssertNoVertex(graph);
            Assert.AreEqual(0, vertexAdded);
        }

        #endregion

        #region Add Edges

        protected static void AddEdge_ParallelEdges_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge2));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.IsTrue(graph.AddEdge(edge3));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(4, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge4));
            Assert.AreEqual(5, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge1, edge4 });
        }

        protected static void AddEdge_ParallelEdges_EquatableEdge_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, EquatableEdge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge2));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.IsTrue(graph.AddEdge(edge3));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(4, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge4));
            Assert.AreEqual(5, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge1, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.IsFalse(graph.AddEdge(edge2));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.IsTrue(graph.AddEdge(edge3));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Edge 1 bis
            Assert.IsFalse(graph.AddEdge(edge1));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge4));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_UndirectedGraph_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.IsFalse(graph.AddEdge(edge2));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.IsFalse(graph.AddEdge(edge3));   // Parallel to edge 1
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 1 bis
            Assert.IsFalse(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge4));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, EquatableEdge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.IsFalse(graph.AddEdge(edge2));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.IsTrue(graph.AddEdge(edge3));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Edge 1 bis
            Assert.IsFalse(graph.AddEdge(edge1));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge4));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_UndirectedGraph_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, EquatableEdge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.IsFalse(graph.AddEdge(edge2));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.IsFalse(graph.AddEdge(edge3));   // Parallel to edge 1
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 1 bis
            Assert.IsFalse(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge4));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge4 });
        }

        protected static void AddEdge_Throws_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdge(null));
            AssertNoEdge(graph);

            Assert.Throws<KeyNotFoundException>(() => graph.AddEdge(new Edge<int>(0, 1)));
            AssertNoEdge(graph);
        }

        protected static void AddEdgeRange_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            Assert.AreEqual(3, graph.AddEdgeRange(new[] { edge1, edge2, edge3 }));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            // Edge 1, 4
            var edge4 = new Edge<int>(2, 2);
            Assert.AreEqual(1, graph.AddEdgeRange(new[] { edge1, edge4 })); // Showcase the add of only one edge
            Assert.AreEqual(4, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });
        }

        protected static void AddEdgeRange_Throws_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange(null));
            AssertNoEdge(graph);
            Assert.AreEqual(0, edgeAdded);

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(2, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange(new[] { edge1, null, edge3 }));
            Assert.AreEqual(0, edgeAdded);
            AssertNoEdge(graph);
        }

        #endregion

        #region Add Vertices & Edges

        protected static void AddVerticesAndEdge_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            AssertNoEdge(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.AddVerticesAndEdge(edge1));
            Assert.AreEqual(2, vertexAdded);
            Assert.AreEqual(1, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 3);
            Assert.IsTrue(graph.AddVerticesAndEdge(edge2));
            Assert.AreEqual(3, vertexAdded);
            Assert.AreEqual(2, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 3);
            Assert.IsTrue(graph.AddVerticesAndEdge(edge3));
            Assert.AreEqual(3, vertexAdded);
            Assert.AreEqual(3, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        protected static void AddVerticesAndEdge_Throws_Test<TVertex, TEdge>(
            [NotNull] IMutableVertexAndEdgeSet<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdge(null));
            AssertNoVertex(graph);
            AssertNoEdge(graph);
        }

        protected static void AddVerticesAndEdgeRange_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            AssertNoEdge(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1, 2
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            Assert.AreEqual(2, graph.AddVerticesAndEdgeRange(new[] { edge1, edge2 }));
            Assert.AreEqual(3, vertexAdded);
            Assert.AreEqual(2, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 1, 3
            var edge3 = new Edge<int>(2, 3);
            Assert.AreEqual(1, graph.AddVerticesAndEdgeRange(new[] { edge1, edge3 })); // Showcase the add of only one edge
            Assert.AreEqual(3, vertexAdded);
            Assert.AreEqual(3, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        protected static void AddVerticesAndEdgeRange_Throws_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            AssertNoEdge(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(null));

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(new[] { edge1, null, edge3 }));
            Assert.AreEqual(0, vertexAdded);
            Assert.AreEqual(0, edgeAdded);
            AssertNoVertex(graph);
            AssertNoEdge(graph);
        }

        #endregion

        #region Contains Vertex

        protected static void ContainsVertex_Test(
            [NotNull] IMutableVertexSet<TestVertex> graph)
        {
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var otherVertex1 = new TestVertex("1");

            Assert.IsFalse(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));

            graph.AddVertex(vertex1);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));

            graph.AddVertex(vertex2);
            Assert.IsTrue(graph.ContainsVertex(vertex2));

            graph.AddVertex(otherVertex1);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));
        }

        protected static void ContainsVertex_ImmutableGraph_Test(
            [NotNull] IMutableVertexSet<TestVertex> wrappedGraph,
            [NotNull, InstantHandle] Func<IImplicitVertexSet<TestVertex>> createGraph)
        {
            IImplicitVertexSet<TestVertex> graph = createGraph();

            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var otherVertex1 = new TestVertex("1");

            Assert.IsFalse(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));

            wrappedGraph.AddVertex(vertex1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));

            wrappedGraph.AddVertex(vertex2);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsVertex(vertex2));

            wrappedGraph.AddVertex(otherVertex1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));
        }

        protected static void ContainsVertex_EquatableVertex_Test(
            [NotNull] IMutableVertexSet<EquatableTestVertex> graph)
        {
            var vertex1 = new EquatableTestVertex("1");
            var vertex2 = new EquatableTestVertex("2");
            var otherVertex1 = new EquatableTestVertex("1");

            Assert.IsFalse(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));

            graph.AddVertex(vertex1);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));

            graph.AddVertex(vertex2);
            Assert.IsTrue(graph.ContainsVertex(vertex2));

            graph.AddVertex(otherVertex1);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));
        }

        protected static void ContainsVertex_EquatableVertex_ImmutableGraph_Test(
            [NotNull] IMutableVertexSet<EquatableTestVertex> wrappedGraph,
            [NotNull, InstantHandle] Func<IImplicitVertexSet<EquatableTestVertex>> createGraph)
        {
            IImplicitVertexSet<EquatableTestVertex> graph = createGraph();

            var vertex1 = new EquatableTestVertex("1");
            var vertex2 = new EquatableTestVertex("2");
            var otherVertex1 = new EquatableTestVertex("1");

            Assert.IsFalse(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));

            wrappedGraph.AddVertex(vertex1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));

            wrappedGraph.AddVertex(vertex2);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsVertex(vertex2));

            wrappedGraph.AddVertex(otherVertex1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));
        }

        protected static void ContainsVertex_Throws_Test<TVertex>(
            [NotNull] IImplicitVertexSet<TVertex> graph)
            where TVertex : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => graph.ContainsVertex(null));
        }

        #endregion

        #region Contains Edge

        protected static void ContainsEdge_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));
        }

        protected static void ContainsEdge_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IEdgeSet<int, Edge<int>>> createGraph)
        {
            IEdgeSet<int, Edge<int>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));
        }

        protected static void ContainsEdge_EquatableEdge_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, EquatableEdge<int>> graph)
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));
        }

        protected static void ContainsEdge_EquatableEdge_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, EquatableEdge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IEdgeSet<int, EquatableEdge<int>>> createGraph)
        {
            IEdgeSet<int, EquatableEdge<int>> graph = createGraph();

            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));
        }

        protected static void ContainsEdge_SourceTarget_Test(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);

            Assert.IsFalse(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            graph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            graph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(1, 3));
            Assert.IsFalse(graph.ContainsEdge(3, 1));

            // A vertex is not present in the graph
            Assert.IsFalse(graph.ContainsEdge(1, 4));
            Assert.IsFalse(graph.ContainsEdge(4, 1));
        }

        protected static void ContainsEdge_SourceTarget_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IIncidenceGraph<int, Edge<int>>> createGraph)
        {
            IIncidenceGraph<int, Edge<int>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);

            Assert.IsFalse(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(1, 3));
            Assert.IsFalse(graph.ContainsEdge(3, 1));

            // A vertex is not present in the graph
            Assert.IsFalse(graph.ContainsEdge(1, 4));
            Assert.IsFalse(graph.ContainsEdge(4, 1));
        }

        protected static void ContainsEdge_SourceTarget_UndirectedGraph_Test(
            [NotNull] IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);

            Assert.IsFalse(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            graph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(1, 2));
            Assert.IsTrue(graph.ContainsEdge(2, 1));

            graph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(1, 3));
            Assert.IsTrue(graph.ContainsEdge(3, 1));

            // A vertex is not present in the graph
            Assert.IsFalse(graph.ContainsEdge(1, 4));
            Assert.IsFalse(graph.ContainsEdge(4, 1));
        }

        protected static void ContainsEdge_Throws_Test<TVertex, TEdge>(
            [NotNull] IEdgeSet<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null));
        }

        protected static void ContainsEdge_SourceTarget_Throws_Test(
            [NotNull] IIncidenceGraph<TestVertex, Edge<TestVertex>> graph)
        {
            var vertex = new TestVertex("v1");

            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(vertex, null));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void ContainsEdge_SourceTarget_Throws_UndirectedGraph_Test(
            [NotNull] IImplicitUndirectedGraph<TestVertex, Edge<TestVertex>> graph)
        {
            var vertex = new TestVertex("v1");

            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(vertex, null));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        #region Out Edges

        protected static void OutEdge_Test(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge41 = new Edge<int>(4, 1);

            graph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });

            Assert.AreSame(edge11, graph.OutEdge(1, 0));
            Assert.AreSame(edge13, graph.OutEdge(1, 2));
            Assert.AreSame(edge24, graph.OutEdge(2, 0));
            Assert.AreSame(edge33, graph.OutEdge(3, 0));
            Assert.AreSame(edge41, graph.OutEdge(4, 0));
        }

        protected static void OutEdge_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IImplicitGraph<int, Edge<int>>> createGraph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge41 = new Edge<int>(4, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });
            IImplicitGraph<int, Edge<int>> graph = createGraph();

            Assert.AreSame(edge11, graph.OutEdge(1, 0));
            Assert.AreSame(edge13, graph.OutEdge(1, 2));
            Assert.AreSame(edge24, graph.OutEdge(2, 0));
            Assert.AreSame(edge33, graph.OutEdge(3, 0));
            Assert.AreSame(edge41, graph.OutEdge(4, 0));
        }

        protected static void OutEdge_Throws_Test<TVertex>(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph1,
            [NotNull] IImplicitGraph<TVertex, Edge<TVertex>> graph2)
            where TVertex : class
        {
            const int vertex1 = 1;
            const int vertex2 = 2;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<KeyNotFoundException>(() => graph1.OutEdge(vertex1, 0));

            graph1.AddVertex(vertex1);
            graph1.AddVertex(vertex2);
            AssertIndexOutOfRange(() => graph1.OutEdge(vertex1, 0));

            graph1.AddEdge(new Edge<int>(1, 2));
            AssertIndexOutOfRange(() => graph1.OutEdge(vertex1, 5));

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph2.OutEdge(null, 0));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void OutEdge_Throws_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph1,
            [NotNull, InstantHandle] Func<IImplicitGraph<int, Edge<int>>> createGraph1,
            [NotNull] IImplicitGraph<TestVertex, Edge<TestVertex>> graph2)
        {
            IImplicitGraph<int, Edge<int>> graph1 = createGraph1();

            const int vertex1 = 1;
            const int vertex2 = 2;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<KeyNotFoundException>(() => graph1.OutEdge(vertex1, 0));

            wrappedGraph1.AddVertex(vertex1);
            wrappedGraph1.AddVertex(vertex2);
            graph1 = createGraph1();
            AssertIndexOutOfRange(() => graph1.OutEdge(vertex1, 0));

            wrappedGraph1.AddEdge(new Edge<int>(1, 2));
            graph1 = createGraph1();
            AssertIndexOutOfRange(() => graph1.OutEdge(vertex1, 5));

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph2.OutEdge(null, 0));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void OutEdges_Test(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);

            AssertNoOutEdge(graph, 1);

            graph.AddVertex(1);
            AssertNoOutEdge(graph, 1);

            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            AssertHasOutEdges(graph, 1, new[] { edge12, edge13, edge14 });
            AssertHasOutEdges(graph, 2, new[] { edge24 });
            AssertHasOutEdges(graph, 3, new[] { edge31, edge33 });
            AssertNoOutEdge(graph, 4);
        }

        protected static void OutEdges_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IImplicitGraph<int, Edge<int>>> createGraph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);

            IImplicitGraph<int, Edge<int>> graph = createGraph();

            AssertNoOutEdge(graph, 1);

            wrappedGraph.AddVertex(1);
            graph = createGraph();
            AssertNoOutEdge(graph, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });
            graph = createGraph();

            AssertHasOutEdges(graph, 1, new[] { edge12, edge13, edge14 });
            AssertHasOutEdges(graph, 2, new[] { edge24 });
            AssertHasOutEdges(graph, 3, new[] { edge31, edge33 });
            AssertNoOutEdge(graph, 4);
        }

        protected static void OutEdges_Throws_Test<TVertex>(
            [NotNull] IImplicitGraph<TVertex, Edge<TVertex>> graph)
            where TVertex : class
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.IsOutEdgesEmpty(null));
            Assert.Throws<ArgumentNullException>(() => graph.OutDegree(null));
            Assert.Throws<ArgumentNullException>(() => graph.OutEdges(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Adjacent Edges

        protected static void AdjacentEdge_Test(
            [NotNull] IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge41 = new Edge<int>(4, 1);

            graph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });

            Assert.AreSame(edge11, graph.AdjacentEdge(1, 0));
            Assert.AreSame(edge13, graph.AdjacentEdge(1, 2));
            Assert.AreSame(edge41, graph.AdjacentEdge(1, 3));
            Assert.AreSame(edge13, graph.AdjacentEdge(3, 0));
            Assert.AreSame(edge33, graph.AdjacentEdge(3, 1));
            Assert.AreSame(edge24, graph.AdjacentEdge(4, 0));
        }

        protected static void AdjacentEdge_Throws_Test<TVertex>(
            [NotNull] IMutableUndirectedGraph<int, Edge<int>> graph1,
            [NotNull] IImplicitUndirectedGraph<TVertex, Edge<TVertex>> graph2)
            where TVertex : class
        {
            const int vertex1 = 1;
            const int vertex2 = 2;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<KeyNotFoundException>(() => graph1.AdjacentEdge(vertex1, 0));

            graph1.AddVertex(vertex1);
            graph1.AddVertex(vertex2);
            AssertIndexOutOfRange(() => graph1.AdjacentEdge(vertex1, 0));

            graph1.AddEdge(new Edge<int>(1, 2));
            AssertIndexOutOfRange(() => graph1.AdjacentEdge(vertex1, 5));

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph2.AdjacentEdge(null, 0));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void AdjacentEdges_Test(
            [NotNull] IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);

            AssertNoAdjacentEdge(graph, 1);

            graph.AddVertex(1);
            AssertNoAdjacentEdge(graph, 1);

            graph.AddVertex(5);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            AssertHasAdjacentEdges(graph, 1, new[] { edge12, edge13, edge14, edge31 });
            AssertHasAdjacentEdges(graph, 2, new[] { edge12, edge24 });
            AssertHasAdjacentEdges(graph, 3, new[] { edge13, edge31, edge33 }, 4);  // Has self edge counting twice
            AssertHasAdjacentEdges(graph, 4, new[] { edge14, edge24 });
            AssertNoAdjacentEdge(graph, 5);
        }

        protected static void AdjacentEdges_Throws_Test<TVertex>(
            [NotNull] IImplicitUndirectedGraph<TVertex, Edge<TVertex>> graph)
            where TVertex : class
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.IsAdjacentEdgesEmpty(null));
            Assert.Throws<ArgumentNullException>(() => graph.AdjacentDegree(null));
            Assert.Throws<ArgumentNullException>(() => graph.AdjacentEdges(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region In Edges

        protected static void InEdges_Throws_Test<TVertex>(
            [NotNull] IBidirectionalIncidenceGraph<TVertex, Edge<TVertex>> graph)
            where TVertex : class
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.IsInEdgesEmpty(null));
            Assert.Throws<ArgumentNullException>(() => graph.InDegree(null));
            Assert.Throws<ArgumentNullException>(() => graph.InEdges(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Try Get Edges

        protected static void TryGetEdge_Test(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            Assert.IsFalse(graph.TryGetEdge(0, 1, out Edge<int> _));

            Assert.IsTrue(graph.TryGetEdge(2, 4, out Edge<int> gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsFalse(graph.TryGetEdge(2, 1, out gotEdge));
        }

        protected static void TryGetEdge_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, Edge<int>> graph = createGraph();

            Assert.IsFalse(graph.TryGetEdge(0, 1, out Edge<int> _));

            Assert.IsTrue(graph.TryGetEdge(2, 4, out Edge<int> gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsFalse(graph.TryGetEdge(2, 1, out gotEdge));
        }

        protected static void TryGetEdge_UndirectedGraph_Test(
            [NotNull] IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 2);

            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            Assert.IsFalse(graph.TryGetEdge(0, 1, out Edge<int> _));

            Assert.IsTrue(graph.TryGetEdge(2, 4, out Edge<int> gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            // 1 -> 2 is present in this undirected graph
            Assert.IsTrue(graph.TryGetEdge(2, 1, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(5, 2, out gotEdge));
            Assert.AreSame(edge7, gotEdge);

            // 5 -> 2 is present in this undirected graph
            Assert.IsTrue(graph.TryGetEdge(2, 5, out gotEdge));
            Assert.AreSame(edge7, gotEdge);
        }

        protected static void TryGetEdge_Throws_Test(
            [NotNull] IIncidenceGraph<TestVertex, Edge<TestVertex>> graph)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, new TestVertex("v2"), out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(new TestVertex("v1"), null, out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void TryGetEdge_Throws_UndirectedGraph_Test(
            [NotNull] IImplicitUndirectedGraph<TestVertex, Edge<TestVertex>> graph)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, new TestVertex("v2"), out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(new TestVertex("v1"), null, out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void TryGetEdges_Test(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            Assert.IsFalse(graph.TryGetEdges(0, 1, out IEnumerable<Edge<int>> _));

            Assert.IsTrue(graph.TryGetEdges(2, 4, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(1, 2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);

            Assert.IsFalse(graph.TryGetEdges(2, 1, out gotEdges));
        }

        protected static void TryGetEdges_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, Edge<int>> graph = createGraph();

            Assert.IsFalse(graph.TryGetEdges(0, 1, out IEnumerable<Edge<int>> _));

            Assert.IsTrue(graph.TryGetEdges(2, 4, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(1, 2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);

            Assert.IsFalse(graph.TryGetEdges(2, 1, out gotEdges));
        }

        protected static void TryGetEdges_Throws_Test(
            [NotNull] IIncidenceGraph<TestVertex, Edge<TestVertex>> graph)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdges(null, new TestVertex("v2"), out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdges(new TestVertex("v1"), null, out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdges(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void TryGetOutEdges_Test(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            Assert.IsFalse(graph.TryGetOutEdges(0, out IEnumerable<Edge<int>> _));

            Assert.IsTrue(graph.TryGetOutEdges(3, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge6 }, gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(1, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge3 }, gotEdges);
        }

        protected static void TryGetOutEdges_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IImplicitGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IImplicitGraph<int, Edge<int>> graph = createGraph();

            Assert.IsFalse(graph.TryGetOutEdges(0, out IEnumerable<Edge<int>> _));

            Assert.IsTrue(graph.TryGetOutEdges(3, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge6 }, gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(1, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge3 }, gotEdges);
        }

        protected static void TryGetOutEdges_Throws_Test<TVertex>(
            [NotNull] IImplicitGraph<TVertex, Edge<TVertex>> graph)
            where TVertex : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetOutEdges(null, out _));
        }

        protected static void TryGetInEdges_Throws_Test<TVertex>(
            [NotNull] IBidirectionalIncidenceGraph<TVertex, Edge<TVertex>> graph)
            where TVertex : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetInEdges(null, out _));
        }

        #endregion

        #region Remove Vertices

        protected static void RemoveVertex_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveVertex(5));
            CheckCounters(0, 0);

            Assert.IsTrue(graph.RemoveVertex(3));
            CheckCounters(1, 3);
            AssertHasVertices(graph, new[] { 1, 2, 4 });
            AssertHasEdges(graph, new[] { edge12, edge14, edge24 });

            Assert.IsTrue(graph.RemoveVertex(1));
            CheckCounters(1, 2);
            AssertHasVertices(graph, new[] { 2, 4 });
            AssertHasEdges(graph, new[] { edge24 });

            Assert.IsTrue(graph.RemoveVertex(2));
            CheckCounters(1, 1);
            AssertHasVertices(graph, new[] { 4 });
            AssertNoEdge(graph);

            Assert.IsTrue(graph.RemoveVertex(4));
            CheckCounters(1, 0);
            AssertEmptyGraph(graph);

            #region Local function

            void CheckCounters(int expectedRemovedVertices, int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedVertices, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveVertex_Throws_Test<TVertex>(
            [NotNull] IMutableVertexSet<TVertex> graph)
            where TVertex : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveVertex(null));
        }

        protected static void RemoveVertexIf_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveVertexIf(vertex => vertex > 10));
            CheckCounters(0, 0);

            Assert.AreEqual(2, graph.RemoveVertexIf(vertex => vertex > 2));
            CheckCounters(2, 5);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12 });

            Assert.AreEqual(2, graph.RemoveVertexIf(vertex => true));
            CheckCounters(2, 1);
            AssertEmptyGraph(graph);

            #region Local function

            void CheckCounters(int expectedRemovedVertices, int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedVertices, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveVertexIf_Throws_Test<TVertex>(
            [NotNull] IMutableVertexSet<TVertex> graph)
            where TVertex : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveVertexIf(null));
        }

        #endregion

        #region Remove Edges

        protected static void RemoveEdge_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            var edgeNotInGraph = new Edge<int>(3, 4);
            var edgeNotEquatable = new Edge<int>(1, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveEdge(edgeNotInGraph));
            CheckCounters(0);

            Assert.IsFalse(graph.RemoveEdge(edgeNotEquatable));
            CheckCounters(0);

            Assert.IsTrue(graph.RemoveEdge(edge13Bis));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge31));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge12));
            Assert.IsTrue(graph.RemoveEdge(edge13));
            Assert.IsTrue(graph.RemoveEdge(edge14));
            Assert.IsTrue(graph.RemoveEdge(edge24));
            Assert.IsTrue(graph.RemoveEdge(edge33));
            CheckCounters(5);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EquatableEdge_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, EquatableEdge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new EquatableEdge<int>(1, 2);
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge13Bis = new EquatableEdge<int>(1, 3);
            var edge14 = new EquatableEdge<int>(1, 4);
            var edge24 = new EquatableEdge<int>(2, 4);
            var edge31 = new EquatableEdge<int>(3, 1);
            var edge33 = new EquatableEdge<int>(3, 3);
            var edgeNotInGraph = new EquatableEdge<int>(3, 4);
            var edgeEquatable = new EquatableEdge<int>(1, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveEdge(edgeNotInGraph));
            CheckCounters(0);

            Assert.IsTrue(graph.RemoveEdge(edgeEquatable));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge13Bis));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge31));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge13));
            Assert.IsTrue(graph.RemoveEdge(edge14));
            Assert.IsTrue(graph.RemoveEdge(edge24));
            Assert.IsTrue(graph.RemoveEdge(edge33));
            CheckCounters(4);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_Throws_Test<TVertex, TEdge>(
            [NotNull] IMutableEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdge(null));
        }

        protected static void RemoveEdgeIf_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVertexRange(new[] { 1, 2, 3, 4 });
            graph.AddEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveEdgeIf(edge => edge.Target == 5));
            CheckCounters(0);

            Assert.AreEqual(2, graph.RemoveEdgeIf(edge => edge.Source == 3));
            CheckCounters(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge13Bis, edge14, edge24 });

            Assert.AreEqual(5, graph.RemoveEdgeIf(edge => true));
            CheckCounters(5);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdgeIf_Throws_Test<TVertex, TEdge>(
            [NotNull] IMutableEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdgeIf(null));
        }

        protected static void RemoveOutEdgeIf_Test(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            Assert.AreEqual(0, graph.RemoveOutEdgeIf(1, edge => true));
            CheckCounters(0);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(3, graph.RemoveOutEdgeIf(1, edge => edge.Target >= 3));
            CheckCounters(3);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveOutEdgeIf(3, edge => edge.Target > 5));
            CheckCounters(0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge31, edge33 });

            Assert.AreEqual(2, graph.RemoveOutEdgeIf(3, edge => true));
            CheckCounters(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24 });

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveOutEdgeIf_Throws_Test(
            [NotNull] IMutableIncidenceGraph<TestVertex, Edge<TestVertex>> graph)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(null, edge => true));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void RemoveAdjacentEdgeIf_Test(
            [NotNull] IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            Assert.AreEqual(0, graph.RemoveAdjacentEdgeIf(1, edge => true));
            CheckCounters(0);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(4, graph.RemoveAdjacentEdgeIf(1, edge => edge.Source >= 3 || edge.Target >= 3));
            CheckCounters(4);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge33 });

            Assert.AreEqual(0, graph.RemoveAdjacentEdgeIf(3, edge => edge.Target > 5));
            CheckCounters(0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge33 });

            Assert.AreEqual(1, graph.RemoveAdjacentEdgeIf(3, edge => true));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24 });

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveAdjacentEdgeIf_Throws_Test(
            [NotNull] IMutableUndirectedGraph<TestVertex, Edge<TestVertex>> graph)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveAdjacentEdgeIf(null, edge => true));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveAdjacentEdgeIf(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveAdjacentEdgeIf(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        #endregion
    }
}
