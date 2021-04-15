﻿using GraphLib.Common.NullObjects;
using GraphLib.Interfaces;
using System;

namespace Algorithm.Infrastructure.EventArguments
{
    /// <summary>
    /// Represents data for events associated with pathfinding
    /// </summary>
    public class AlgorithmEventArgs : EventArgs
    {
        public AlgorithmEventArgs()
        {
            Vertex = new DefaultVertex();
        }

        public AlgorithmEventArgs(int visitedVertices,
            bool isEndPoint = false, IVertex vertex = null)
        {
            IsEndPoint = isEndPoint;
            Vertex = vertex ?? new DefaultVertex();
            VisitedVertices = visitedVertices;
        }

        /// <summary>
        /// Provides information about vertex that is being processed
        /// </summary>
        public IVertex Vertex { get; }

        public int VisitedVertices { get; set; }

        public bool IsEndPoint { get; }
    }
}
