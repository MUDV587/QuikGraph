using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Linq;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.ConnectedComponents
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class WeaklyConnectedComponentsAlgorithm<TVertex, TEdge> :
        AlgorithmBase<IVertexListGraph<TVertex, TEdge>>,
        IConnectedComponentAlgorithm<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly IDictionary<TVertex, int> components;
        private readonly Dictionary<int, int> componentEquivalences = new Dictionary<int, int>();
        private int componentCount = 0;
        private int currentComponent = 0;
        private List<BidirectionalGraph<TVertex, TEdge>> graphs;

        public WeaklyConnectedComponentsAlgorithm(IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, int>())
        { }

        public WeaklyConnectedComponentsAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, int> components)
            : this(null, visitedGraph, components)
        { }

        public WeaklyConnectedComponentsAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, int> components)
            : base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(components != null);
#endif

            this.components = components;
        }

        public IDictionary<TVertex, int> Components
        {
            get { return this.components; }
        }

        public int ComponentCount
        {
            get { return this.componentCount; }
        }

        protected override void Initialize()
        {
            this.componentCount = 0;
            this.currentComponent = 0;
            this.componentEquivalences.Clear();
            this.components.Clear();
        }

        public List<BidirectionalGraph<TVertex, TEdge>> Graphs
        {
            get
            {
                int i;
                graphs = new List<BidirectionalGraph<TVertex, TEdge>>(componentCount + 1);
                for (i = 0; i < componentCount; i++)
                {
                    graphs.Add(new BidirectionalGraph<TVertex, TEdge>());
                }
                foreach (TVertex componentName in components.Keys)
                {
                    graphs[components[componentName]].AddVertex(componentName);
                }

                foreach (TVertex vertex in VisitedGraph.Vertices)
                {
                    foreach (TEdge edge in VisitedGraph.OutEdges(vertex))
                    {

                        if (components[vertex] == components[edge.Target])
                        {
                            graphs[components[vertex]].AddEdge(edge);
                        }
                    }
                }
                return graphs;
            }

        }

        protected override void InternalCompute()
        {
#if SUPPORTS_CONTRACTS
            Contract.Ensures(0 <= this.ComponentCount && this.ComponentCount <= this.VisitedGraph.VertexCount);
            Contract.Ensures(Enumerable.All(this.VisitedGraph.Vertices,
                v => 0 <= this.Components[v] && this.Components[v] < this.ComponentCount));
#endif

            // shortcut for empty graph
            if (this.VisitedGraph.IsVerticesEmpty)
                return;

            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(this.VisitedGraph);
            try
            {
                dfs.StartVertex += dfs_StartVertex;
                dfs.TreeEdge += dfs_TreeEdge;
                dfs.ForwardOrCrossEdge += dfs_ForwardOrCrossEdge;

                dfs.Compute();
            }
            finally
            {
                dfs.StartVertex -= dfs_StartVertex;
                dfs.TreeEdge -= dfs_TreeEdge;
                dfs.ForwardOrCrossEdge -= dfs_ForwardOrCrossEdge;
            }

            // updating component numbers
            foreach (var v in this.VisitedGraph.Vertices)
            {
                int component = this.components[v];
                int equivalent = this.GetComponentEquivalence(component);
                if (component != equivalent)
                    this.components[v] = equivalent;
            }
            this.componentEquivalences.Clear();
        }

        void dfs_StartVertex(TVertex v)
        {
            // we are looking on a new tree
            this.currentComponent = this.componentEquivalences.Count;
            this.componentEquivalences.Add(this.currentComponent, this.currentComponent);
            this.componentCount++;
            this.components.Add(v, this.currentComponent);
        }

        void dfs_TreeEdge(TEdge e)
        {
            // new edge, we store with the current component number
            this.components.Add(e.Target, this.currentComponent);
        }

        private int GetComponentEquivalence(int component)
        {
            int equivalent = component;
            int temp = this.componentEquivalences[equivalent];
            bool compress = false;
            while (temp != equivalent)
            {
                equivalent = temp;
                temp = this.componentEquivalences[equivalent];
                compress = true;
            }

            // path compression
            if (compress)
            {
                int c = component;
                temp = this.componentEquivalences[c];
                while (temp != equivalent)
                {
                    temp = this.componentEquivalences[c];
                    this.componentEquivalences[c] = equivalent;
                }
            }

            return equivalent;
        }

        void dfs_ForwardOrCrossEdge(TEdge e)
        {
            // we have touched another tree, updating count and current component
            int otherComponent = this.GetComponentEquivalence(this.components[e.Target]);
            if (otherComponent != this.currentComponent)
            {
                this.componentCount--;
#if SUPPORTS_CONTRACTS
                Contract.Assert(this.componentCount > 0);
#endif
                if (this.currentComponent > otherComponent)
                {
                    this.componentEquivalences[this.currentComponent] = otherComponent;
                    this.currentComponent = otherComponent;
                }
                else
                {
                    this.componentEquivalences[otherComponent] = this.currentComponent;
                }
            }
        }
    }
}