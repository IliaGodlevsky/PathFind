﻿using System;
using System.Linq;
using System.Threading;
using GraphLibrary.Coordinates;
using GraphLibrary.Extensions.CustomTypeExtensions;
using GraphLibrary.GraphCreating;
using GraphLibrary.Graphs;
using GraphLibrary.Graphs.Interface;
using GraphLibrary.PathFindingAlgorithm;
using GraphLibrary.UnitTests.Classes;
using GraphLibrary.Vertex.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphLibrary.UnitTests.Tests.PathFindingAlgorithmsTests
{
    [TestClass]
    public class LeeAlgorithmTests
    {
        [TestMethod]
        public void LeeAlgorithmFindPath_NullGraph_ReturnsEmptyPath()
        {
            var leeAlgorithm = new LeeAlgorithm(NullGraph.Instance);

            leeAlgorithm.FindPath();

            Assert.IsFalse(leeAlgorithm.GetPath().Any());
        }

        [TestMethod]
        public void LeeAlgorithmFindPath_NotNullGraph_ReturnsNotEmptyPath()
        {
            var startVertexPosition = new Coordinate2D(1, 1);
            var endVertexPosition = new Coordinate2D(9, 9);
            var parametres = new GraphParametres(width: 10, height: 10, obstaclePercent: 0);
            var factory = new GraphFactory(parametres);
            var graph = factory.CreateGraph(() => new TestVertex());
            Thread.Sleep(millisecondsTimeout: 230);
            graph.Start = graph[startVertexPosition];
            graph.End = graph[endVertexPosition];
            var leeAlgorithm = new LeeAlgorithm(graph);

            leeAlgorithm.FindPath();

            Assert.IsTrue(leeAlgorithm.GetPath().Any());
        }
    }
}
