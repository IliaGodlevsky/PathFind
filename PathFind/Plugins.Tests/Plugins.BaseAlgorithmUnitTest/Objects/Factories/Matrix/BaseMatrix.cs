﻿using GraphLib.Interfaces;
using Plugins.BaseAlgorithmUnitTest.Objects.TestObjects;

namespace Plugins.BaseAlgorithmUnitTest.Objects.Factories.Matrix
{
    internal abstract class BaseMatrix<T> : IMatrix
    {
        protected BaseMatrix(TestGraph graph)
        {
            this.graph = graph;
        }

        public void Overlay()
        {
            for (int x = 0; x < graph.Width; x++)
            {
                for (int y = 0; y < graph.Length; y++)
                {
                    var coordinate = new TestCoordinate(x, y);
                    Assign(graph[coordinate], matrix[x, y]);
                }
            }
        }

        protected abstract void Assign(IVertex vertex, T value);

        protected T[,] matrix;
        protected readonly TestGraph graph;
    }
}
