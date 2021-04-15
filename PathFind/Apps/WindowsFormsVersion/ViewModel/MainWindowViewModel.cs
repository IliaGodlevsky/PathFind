﻿using Algorithm.Common.Exceptions;
using Algorithm.Realizations;
using Common.Extensions;
using Common.Interface;
using Common.Logging;
using GraphLib.Base;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using GraphLib.Realizations;
using GraphLib.Serialization.Interfaces;
using GraphViewModel;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WindowsFormsVersion.Extensions;
using WindowsFormsVersion.Forms;
using WindowsFormsVersion.View;

namespace WindowsFormsVersion.ViewModel
{
    internal class MainWindowViewModel : MainModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string graphParametres;
        public override string GraphParametres
        {
            get => graphParametres;
            set { graphParametres = value; OnPropertyChanged(); }
        }

        private string statistics;
        public override string PathFindingStatistics
        {
            get => statistics;
            set { statistics = value; OnPropertyChanged(); }
        }

        private IGraphField graphField;
        public override IGraphField GraphField
        {
            get => graphField;
            set
            {
                graphField = value;
                if (Graph is Graph2D graph2D && graphField is WinFormsGraphField field)
                {
                    int width = (graph2D.Width + Constants.VertexSize) * Constants.VertexSize;
                    int height = (graph2D.Length + Constants.VertexSize) * Constants.VertexSize;
                    field.Size = new Size(width, height);
                    MainWindow.Controls.RemoveBy(ctrl => ctrl.IsGraphField());
                    MainWindow.Controls.Add(field);
                }
            }
        }

        public MainWindow MainWindow { get; set; }

        public MainWindowViewModel(BaseGraphFieldFactory fieldFactory,
            IVertexEventHolder eventHolder,
            IGraphSerializer graphSerializer,
            IGraphAssembler graphFactory,
            IPathInput pathInput) : base(fieldFactory, eventHolder, graphSerializer, graphFactory, pathInput)
        {

        }

        public override void FindPath()
        {
            if (CanStartPathFinding())
            {
                try
                {
                    string loadPath = GetAlgorithmsLoadPath();
                    AlgorithmsFactory.LoadAlgorithms(loadPath);
                    var model = new PathFindingViewModel(this);
                    model.OnEventHappened += OnExternalEventHappened;
                    model.EndPoints = EndPoints;
                    var form = new PathFindingWindow(model);
                    PrepareWindow(model, form);
                }
                catch (NoAlgorithmsLoadedException ex)
                {
                    OnExceptionCaught(ex);
                    Logger.Instance.Warn(ex);
                }
                catch (Exception ex)
                {
                    OnExceptionCaught(ex);
                    Logger.Instance.Error(ex);
                }
            }
        }

        public override void CreateNewGraph()
        {
            try
            {
                var model = new GraphCreatingViewModel(this, graphAssembler);
                var form = new GraphCreatingWindow(model);
                model.OnEventHappened += OnExternalEventHappened;
                PrepareWindow(model, form);
            }
            catch (Exception ex)
            {
                OnExceptionCaught(ex);
                Logger.Instance.Error(ex);
            }
        }

        public void SaveGraph(object sender, EventArgs e)
        {
            if (!Graph.IsDefault())
            {
                base.SaveGraph();
            }
        }

        public void LoadGraph(object sender, EventArgs e)
        {
            base.LoadGraph();
        }

        public void ClearGraph(object sender, EventArgs e)
        {
            base.ClearGraph();
        }

        public void MakeWeighted(object sender, EventArgs e)
        {
            Graph.ToWeighted();
        }

        public void MakeUnweighted(object sender, EventArgs e)
        {
            Graph.ToUnweighted();
        }

        public void StartPathFind(object sender, EventArgs e)
        {
            FindPath();
        }

        public void CreateNewGraph(object sender, EventArgs e)
        {
            CreateNewGraph();
        }

        private void PrepareWindow(IViewModel model, Form window)
        {
            model.OnWindowClosed += (sender, args) => window.Close();
            window.StartPosition = FormStartPosition.CenterScreen;
            window.Show();
        }

        private bool CanStartPathFinding()
        {
            return EndPoints.HasEndPointsSet;
        }

        protected override string GetAlgorithmsLoadPath()
        {
            return ConfigurationManager.AppSettings["pluginsPath"];
        }

        protected override void OnExternalEventHappened(string message)
        {
            MessageBox.Show(message);
        }

        protected override void OnExceptionCaught(Exception ex, string additaionalMessage = "")
        {
            MessageBox.Show(ex.Message + additaionalMessage);
        }
    }
}
