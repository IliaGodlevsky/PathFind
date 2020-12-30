﻿using Common.Extensions;
using ConsoleVersion.Attributes;
using ConsoleVersion.Enums;
using ConsoleVersion.InputClass;
using ConsoleVersion.Model;
using ConsoleVersion.View;
using GraphLib.Extensions;
using GraphLib.Graphs;
using GraphViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using Console = Colorful.Console;

namespace ConsoleVersion.ViewModel
{
    internal class MainViewModel : MainModel
    {
        private string[] MethodsDescriptions { get; set; }

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

        [MenuItem("Create new graph", MenuItemPriority.First)]
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

        [MenuItem("Quit programm", MenuItemPriority.Last)]
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

        public string CreateMenu(int columns = 2)
        {
            var menu = new StringBuilder("\n");

            int menuItemNumber = 0;
            var menuItemsNames = GetMenuMethods().Select(GetMenuItemName);
            var longestNameLength = menuItemsNames.Max(str => str.Length);
            var format = ConsoleVersionResources.MenuFormat;

            foreach (var name in menuItemsNames)
            {
                var paddedName = name.PadRight(longestNameLength);
                var separator = (menuItemNumber + 1) % columns == 0 ? "\n" : " ";
                menu.AppendFormat(format + separator, ++menuItemNumber, paddedName);
            }

            return menu.ToString();
        }

        public Dictionary<string, Action> GetMenuActions()
        {
            var dictionary = new Dictionary<string, Action>();

            foreach (var method in GetMenuMethods())
            {
                var action = (Action)method.CreateDelegate(typeof(Action), this);
                var description = GetMenuItemName(method);
                dictionary.Add(description, action);
            }
            MethodsDescriptions = dictionary.Keys.ToArray();

            return dictionary;
        }

        public string ChooseMethodDescription()
        {
            var option = Input.InputNumber(
                ConsoleVersionResources.OptionInputMsg,
                MethodsDescriptions.Length, 1) - 1;

            return MethodsDescriptions[option];
        }

        protected override string GetSavingPath()
        {
            return GetPath();
        }

        protected override string GetLoadingPath()
        {
            return GetPath();
        }

        private IEnumerable<MethodInfo> GetMenuMethods()
        {
            bool IsMenuMethod(MethodInfo method) 
                => method.GetAttribute<MenuItemAttribute>() != null;

            int ByPriority(MethodInfo method) 
                => method.GetAttribute<MenuItemAttribute>().MenuItemPriority.GetValue<int>();

            return typeof(MainViewModel)
                .GetMethods()
                .Where(IsMenuMethod)
                .OrderBy(ByPriority);
        }

        private string GetMenuItemName(MethodInfo method)
        {
            var attribute = method.GetAttribute<MenuItemAttribute>();
            return attribute.MenuItemName;
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
