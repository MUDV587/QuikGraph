using System.Collections.Generic;
using NUnit.Framework;

namespace QuikGraph
{
    public static partial class AdjacencyGraphFactory
    {
        public static AdjacencyGraph<int, Edge<int>> Create(
            bool allowParralelEdges,
            KeyValuePair<int, int>[] edges)
        {
            Assert.IsNotNull(edges);

            var adjacencyGraph
               = new AdjacencyGraph<int, Edge<int>>(allowParralelEdges);
            if(edges != null && edges.Length <= 3)
                foreach (var edge in edges)
                    adjacencyGraph.AddVerticesAndEdge(new Edge<int>(edge.Key, edge.Value));

            return adjacencyGraph;
        }
    }
}
