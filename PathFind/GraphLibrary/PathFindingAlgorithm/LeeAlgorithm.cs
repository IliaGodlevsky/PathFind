﻿using GraphLibrary.Graphs;
using System.Collections.Generic;
using System.Linq;
using System;
using GraphLibrary.EventArguments;
using GraphLibrary.Extensions.SystemTypeExtensions;
using GraphLibrary.Graphs.Interface;
using GraphLibrary.PathFindingAlgorithm.Interface;
using GraphLibrary.Vertex.Interface;
using GraphLibrary.Extensions.CustomTypeExtensions;
using System.ComponentModel;

namespace GraphLibrary.PathFindingAlgorithm
{
    /// <summary>
    /// A wave algorithm (Lee algorithm, or width-first pathfinding algorithm). 
    /// Uses queue to move next vertex. Finds the shortest path (in steps) to
    /// the destination top
    /// </summary>
    [Description("Lee algorithm")]
    public class LeeAlgorithm : IPathFindingAlgorithm
    {
        public event AlgorithmEventHanlder OnStarted;
        public event Action<IVertex> OnVertexVisited;
        public event AlgorithmEventHanlder OnFinished;
        public event Action<IVertex> OnEnqueued;

        public IGraph Graph { get; protected set; }

        public LeeAlgorithm(IGraph graph)
        {
            Graph = graph;
            neighbourQueue = new Queue<IVertex>();
        }

        public void FindPath()
        {
            OnStarted?.Invoke(this, new AlgorithmEventArgs(Graph));
            var currentVertex = Graph.Start;
            ProcessVertex(currentVertex);
            while (currentVertex?.IsEnd == false)
            {
                currentVertex = GetNextVertex();
                ProcessVertex(currentVertex);
            }
            OnFinished?.Invoke(this, new AlgorithmEventArgs(Graph));
        }

        private void ExtractNeighbours(IVertex vertex)
        {
            foreach(var vert in vertex.GetUnvisitedNeighbours())
            {
                OnEnqueued?.Invoke(vert);
                neighbourQueue.Enqueue(vert);
            }
            neighbourQueue = new Queue<IVertex>(neighbourQueue.DistinctBy(vert => vert.Position));
        }

        protected virtual IVertex GetNextVertex()
        {           
            neighbourQueue = new Queue<IVertex>(neighbourQueue.Where(vertex => !vertex.IsVisited));
            return neighbourQueue.DequeueOrNullVertex();
        }

        protected virtual double WaveFunction(IVertex vertex)
        {
            return vertex.AccumulatedCost + 1;
        }

        private void SpreadWaves(IVertex vertex)
        {
            foreach(var vert in vertex.GetUnvisitedNeighbours())
            {
                if (vert.AccumulatedCost == 0)
                {
                    vert.AccumulatedCost = WaveFunction(vertex);
                    vert.ParentVertex = vertex;
                }
            }
        }

        private void ProcessVertex(IVertex vertex)
        {
            vertex.IsVisited = true;
            OnVertexVisited?.Invoke(vertex);
            SpreadWaves(vertex);
            ExtractNeighbours(vertex);
        }

        protected Queue<IVertex> neighbourQueue;
    }
}
