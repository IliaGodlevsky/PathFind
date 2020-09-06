﻿using GraphLibrary.Common.Extensions;
using GraphLibrary.Extensions;
using GraphLibrary.Collection;
using GraphLibrary.Vertex;
using System.Collections.Generic;
using System.Linq;
using GraphLibrary.Common.Extensions.CollectionExtensions;
using System;

namespace GraphLibrary.Algorithm
{
    /// <summary>
    /// A wave algorithm (Li algorithm, or wide path find algorithm). 
    /// Uses queue to move next graph top. Finds the shortest path to
    /// the destination top
    /// </summary>
    public class LiAlgorithm : IPathFindAlgorithm
    {
        public Graph Graph { get; set; }

        public LiAlgorithm()
        {
            neighbourQueue = new Queue<IVertex>();
        }

        public void FindDestionation()
        {
            OnAlgorithmStarted?.Invoke();
            var currentVertex = Graph.Start;
            ProcessVertex(currentVertex);
            while (!IsDestination(currentVertex))
            {
                currentVertex = neighbourQueue.Dequeue();
                if (!currentVertex.IsVisited)
                    ProcessVertex(currentVertex);
            }
            OnAlgorithmFinished?.Invoke();
        }

        private void ExtractNeighbours(IVertex vertex)
        {
            neighbourQueue.EnqueueRange(vertex.GetUnvisitedNeighbours());
        }

        private void SpreadWaves(IVertex vertex)
        {
            vertex.GetUnvisitedNeighbours().ToList().Apply(vert =>
            {
                if (vert.AccumulatedCost == 0)
                {
                    vert.AccumulatedCost = vertex.AccumulatedCost + 1;
                    vert.ParentVertex = vertex;
                }
                return vert;
            });
        }

        private void ProcessVertex(IVertex vertex)
        {
            OnVertexVisited?.Invoke(vertex);
            SpreadWaves(vertex);
            ExtractNeighbours(vertex);
        }

        private bool IsDestination(IVertex vertex)
        {
            if (vertex == null || Graph.End == null)
                return true;
            return vertex.IsEnd || !neighbourQueue.Any();
        }

        protected Queue<IVertex> neighbourQueue;

        public event AlgorithmEventHanlder OnAlgorithmStarted;
        public event Action<IVertex> OnVertexVisited;
        public event AlgorithmEventHanlder OnAlgorithmFinished;
    }
}