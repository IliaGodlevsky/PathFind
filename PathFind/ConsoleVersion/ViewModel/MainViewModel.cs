﻿using ConsoleVersion.Attributes;
using ConsoleVersion.Enums;
using ConsoleVersion.InputClass;
using ConsoleVersion.Model;
using ConsoleVersion.View;
using GraphLib.Extensions;
using GraphLib.Graphs;
using GraphViewModel;
using System;
using System.Drawing;
using System.Linq;
using Console = Colorful.Console;

namespace ConsoleVersion.ViewModel
{
    internal class MainViewModel : MainModel
    {
        public MainViewModel() : base()
        {
            VertexEventHolder = new ConsoleVertexEventHolder();
            FieldFactory = new ConsoleGraphFieldFactory();
            InfoConverter = (info) => new ConsoleVertex(info);
        }

        [MenuItem("Make unweighted")]
        public void MakeGraphUnweighted() => Graph.ToUnweighted();

        [MenuItem("Make weighted")] 
        public void MakeGraphWeighted() => Graph.ToWeighted();

        [MenuItem("Create new graph", MenuItemPriority.Highest)]
        public override void CreateNewGraph()
        {
            var model = new GraphCreatingViewModel(this);
            var view = new GraphCreateView(model);

            view.Start();
        }

        [MenuItem("Find path", MenuItemPriority.High)]
        public override void FindPath()
        {
            var model = new PathFindingViewModel(this);
            model.OnPathNotFound += OnPathNotFound;
            var view = new PathFindView(model);

            view.Start();
        }

        [MenuItem("Reverse vertex")]
        public void ReverseVertex()
        {
            if (Graph.Any())
            {
                var upperPossibleXValue = (Graph as Graph2D).Width - 1;
                var upperPossibleYValue = (Graph as Graph2D).Length - 1;

                var point = Input.InputPoint(upperPossibleXValue, upperPossibleYValue);

                (Graph[point] as ConsoleVertex).Reverse();
            }
        }

        [MenuItem("Change vertex cost", MenuItemPriority.Low)]
        public void ChangeVertexCost()
        {
            if (Graph.Any())
            {
                var graph2D = Graph as Graph2D;

                var upperPossibleXValue = graph2D.Width - 1;
                var upperPossibleYValue = graph2D.Length - 1;

                var point = Input.InputPoint(upperPossibleXValue, upperPossibleYValue);

                while (Graph[point].IsObstacle)
                {
                    point = Input.InputPoint(upperPossibleXValue, upperPossibleYValue);
                }

                (Graph[point] as ConsoleVertex).ChangeCost();
            }
        }

        [MenuItem("Clear graph", MenuItemPriority.High)]
        public override void ClearGraph() => base.ClearGraph();

        [MenuItem("Save graph")]
        public override void SaveGraph() => base.SaveGraph();

        [MenuItem("Load graph")]
        public override void LoadGraph() => base.LoadGraph();

        [MenuItem("Quit programm", MenuItemPriority.Lowest)]
        public void Quit() => Environment.Exit(0);

        public void DisplayGraph()
        {
            Console.Clear();
            Console.ForegroundColor = Color.White;
            Console.WriteLine(GraphParametres);
            var field = GraphField as ConsoleGraphField;
            field?.ShowGraphWithFrames();
            Console.WriteLine(PathFindingStatistics);
        }

        protected override string GetSavingPath()
        {
            return GetPath();
        }

        protected override string GetLoadingPath()
        {
            return GetPath();
        }

        private string GetPath()
        {
            Console.Write("Enter path: ");
            return Console.ReadLine();
        }

        private void OnPathNotFound(string message)
        {
            DisplayGraph();
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
