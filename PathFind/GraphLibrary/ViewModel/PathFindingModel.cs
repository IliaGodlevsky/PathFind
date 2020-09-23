﻿using System.Diagnostics;
using System.Linq;
using GraphLibrary.Enums;
using GraphLibrary.PathFindingAlgorithm.Interface;
using GraphLibrary.ViewModel.Interface;
using GraphLibrary.Extensions.CustomTypeExtensions;
using GraphLibrary.Extensions.SystemTypeExtensions;
using GraphLibrary.Graphs.Interface;
using GraphLibrary.AlgorithmCreating;

namespace GraphLibrary.ViewModel
{
    public abstract class PathFindingModel : IModel
    {
        public int DelayTime { get; set; } // miliseconds
        public Algorithms Algorithm { get; set; }

        public PathFindingModel(IMainModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            graph = mainViewModel.Graph;
            pathFindStatisticsFormat = LibraryResources.StatisticsFormat;
            badResultMessage = LibraryResources.BadResultMsg;
            DelayTime = 4;
        }

        public virtual void FindPath()
        {
            pathAlgorithm = AlgorithmFactory.CreateAlgorithm(Algorithm, graph);
            PrepareAlgorithm();
            pathAlgorithm.FindPath();
            var path = pathAlgorithm.GetPath();
            mainViewModel.Statistics +=
                string.Format("   " + pathFindStatisticsFormat,
                path.Count(),
                path.Sum(vertex => vertex.Cost),
                graph.NumberOfVisitedVertices);
        }

        protected virtual void PrepareAlgorithm()
        {
            var timer = new Stopwatch();
            pathAlgorithm.OnVertexVisited += (vertex) =>
            {
                if (vertex.IsSimpleVertex())
                    vertex.MarkAsVisited();

                mainViewModel.Statistics = timer.GetTimeInformation(LibraryResources.TimerInfoFormat) +
                string.Format("   " + pathFindStatisticsFormat, 0, 0, graph.NumberOfVisitedVertices);
            };
            pathAlgorithm.OnStarted += (sender, eventArgs) => { timer.Start(); };
            pathAlgorithm.OnFinished += (sender, eventArgs) =>
            {
                timer.Stop();
                mainViewModel.Statistics = timer.GetTimeInformation(LibraryResources.TimerInfoFormat);
                pathAlgorithm.GetPath().DrawPath();
            };
            pathAlgorithm.OnEnqueued += vertex =>
            {
                if (vertex.IsSimpleVertex())
                    vertex.MarkAsEnqueued();
            };
        }

        protected IPathFindingAlgorithm pathAlgorithm;
        protected IGraph graph;
        protected IMainModel mainViewModel;
        protected string badResultMessage;
        protected string pathFindStatisticsFormat;
    }
}