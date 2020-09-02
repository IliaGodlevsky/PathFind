﻿using GraphLibrary.Graph;
using GraphLibrary.Vertex;

namespace GraphLibrary
{
    public static class VertexBinder
    {
        public static void ConnectToNeighbours(IVertex vertex)
        {
            if (vertex is null)
                return;
            foreach (var neigbour in vertex.Neighbours)
            {
                if (CanBeNeighbour(neigbour, vertex))
                    neigbour.Neighbours.Add(vertex);
            }
        }

        public static void IsolateVertex(IVertex vertex)
        {
            if (vertex is null)
                return;
            foreach (var neigbour in vertex.Neighbours)
                neigbour.Neighbours.Remove(vertex);
            vertex.Neighbours.Clear();
        }

        private static bool IsWithinGraph(AbstractGraph graph, int width, int height)
        {
            return width >= 0 && width < graph.Width
                && height >= 0 && height < graph.Height;
        }

        private static bool CanBeNeighbour(IVertex vertex, IVertex neighbourCandidate)
        {
            return !neighbourCandidate.IsObstacle && vertex != neighbourCandidate &&
                !vertex.Neighbours.Contains(neighbourCandidate);
        }

        public static void SetNeighbours(AbstractGraph graph, IVertex vertex)
        {
            if (vertex.IsObstacle)
                return;
            var vertexCoordinates = graph.GetIndices(vertex);
            for (int i = vertexCoordinates.X - 1; i <= vertexCoordinates.X + 1; i++)
                for (int j = vertexCoordinates.Y - 1; j <= vertexCoordinates.Y + 1; j++)
                    if (IsWithinGraph(graph, i, j) && CanBeNeighbour(vertex, graph[i, j]))
                        vertex.Neighbours.Add(graph[i, j]);
        }

        public static void ConnectVertices(AbstractGraph graph)
        {
            foreach (IVertex vertex in graph)
                SetNeighbours(graph, vertex);
        }
    }
}
