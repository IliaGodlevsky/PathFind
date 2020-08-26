﻿using ConsoleVersion.Enums;
using ConsoleVersion.Forms;
using ConsoleVersion.Graph;
using ConsoleVersion.GraphFactory;
using ConsoleVersion.InputClass;
using ConsoleVersion.ViewModel;
using GraphLibrary;
using GraphLibrary.Extensions;
using System.Collections.Generic;
using System.Text;
using Console = Colorful.Console;

namespace ConsoleVersion.View
{
    public class MainView : IView
    {
        private delegate void MenuAction();
        private readonly Dictionary<MenuOption, MenuAction> menuActions;
        private readonly MainViewModel mainModel;
        private readonly string menu;
        public MainView()
        {
            mainModel = new MainViewModel();

            menuActions = new Dictionary<MenuOption, MenuAction>
            {
                { MenuOption.PathFind, mainModel.PathFind },
                { MenuOption.Save, mainModel.SaveGraph },
                { MenuOption.Load, mainModel.LoadGraph },
                { MenuOption.Create, mainModel.CreateNewGraph },
                { MenuOption.Refresh, mainModel.ClearGraph },
                { MenuOption.Reverse, mainModel.Reverse }
            };

            menu = GetMenu();

            var factory = new RandomValuedConsoleGraphFactory(
                percentOfObstacles: 25, width: 25, height: 25);
            mainModel.Graph = factory.GetGraph();
            mainModel.GraphParametres = GraphDataFormatter.
                GetFormattedData(mainModel.Graph, mainModel.Format);
        }

        private string GetMenu()
        {
            const string newLine = "\n";
            const string bigSpace = "  ";
            const string tab = "\t";

            var stringBuilder = new StringBuilder();
            MenuOption menu = default;
            var descriptions = menu.GetDescriptions<MenuOption>();

            foreach (var item in descriptions)
            {
                int numberOf = descriptions.IndexOf(item);
                if (numberOf.IsEven())
                    stringBuilder.Append(newLine);
                else
                    stringBuilder.Append(bigSpace + tab);
                stringBuilder.Append(string.Format(Res.ShowFormat, numberOf, item));
            }

            return stringBuilder.ToString();
        }

        private void DisplayGraph()
        {
            Console.Clear();

            Console.WriteLine(mainModel.GraphParametres);
            GraphShower.ShowGraph(mainModel.Graph as ConsoleGraph);
            Console.WriteLine(mainModel?.Statistics);
        }

        private MenuOption GetOption()
        {
            Console.WriteLine(menu);
            return Input.InputOption();
        }

        public void Start()
        {
            DisplayGraph();
            var option = GetOption();
            while (option != MenuOption.Quit)
            {
                menuActions[option]();
                DisplayGraph();
                option = GetOption();
            }
        }
    }
}
