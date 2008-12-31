﻿using System;
using System.Collections.Generic;

using QuickGraph.Algorithms.MaximumFlow;
using System.Diagnostics.Contracts;

namespace QuickGraph.Algorithms
{
    public sealed class MaximumBipartiteMatchingAlgorithm<TVertex,TEdge> 
        : AlgorithmBase<IMutableVertexAndEdgeListGraph<TVertex,TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly VertexFactory<TVertex> vertexFactory;
        private readonly EdgeFactory<TVertex, TEdge> edgeFactory;
        private IList<TEdge> matchedEdges = new List<TEdge>();

        public MaximumBipartiteMatchingAlgorithm(
            IMutableVertexAndEdgeListGraph<TVertex,TEdge> visitedGraph,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex,TEdge> edgeFactory
            )
            :base(visitedGraph)
        {
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);

            this.vertexFactory = vertexFactory;
            this.edgeFactory = edgeFactory;
        }

        public VertexFactory<TVertex> VertexFactory
        {
            get { return this.vertexFactory; }
        }

        public EdgeFactory<TVertex, TEdge> EdgeFactory
        {
            get { return this.edgeFactory; }
        }

        public ICollection<TEdge> MatchedEdges
        {
            get { return this.matchedEdges; }
        }

        protected override void InternalCompute()
        {
            var cancelManager = this.Services.CancelManager;
            this.matchedEdges.Clear();

            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor = null;
            ReversedEdgeAugmentorAlgorithm<TVertex,TEdge> reverser = null;
            try
            {
                if (cancelManager.IsCancelling)
                    return;

                //augmenting graph
                augmentor = new AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge>(
                    this,
                    this.VisitedGraph,
                    this.VertexFactory,
                    this.EdgeFactory);
                augmentor.Compute();

                if (cancelManager.IsCancelling)
                    return;


                // adding reverse edges
                reverser = new ReversedEdgeAugmentorAlgorithm<TVertex,TEdge>(
                    this,
                    this.VisitedGraph,
                    this.EdgeFactory
                    );
                reverser.AddReversedEdges();
                if (cancelManager.IsCancelling)
                    return;


                // compute maxflow
                var flow = new EdmondsKarpMaximumFlowAlgorithm<TVertex, TEdge>(
                    this,
                    this.VisitedGraph,
                    e => 1,
                    reverser.ReversedEdges
                    );
                flow.Compute(augmentor.SuperSource, augmentor.SuperSink);
                
                if (cancelManager.IsCancelling)
                    return;


                foreach (var edge in this.VisitedGraph.Edges)
                {
                    if (flow.ResidualCapacities[edge] == 0)
                        this.matchedEdges.Add(edge);
                }
            }
            finally
            {
                if (reverser!=null && reverser.Augmented)
                {                    
                    reverser.RemoveReversedEdges();
                    reverser=null;
                }
                if (augmentor != null && augmentor.Augmented)
                {
                    augmentor.Rollback();
                    augmentor = null;
                }
            }
        }
    }
}
