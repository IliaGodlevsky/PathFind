﻿using Common.Interface;
using GraphLib.Interfaces.Factories;
using GraphLib.ViewModel;
using GraphViewModel.Interfaces;
using Logging.Interface;
using System;
using System.Collections.Generic;

namespace WindowsFormsVersion.ViewModel
{
    internal class GraphCreatingViewModel : GraphCreatingModel, IViewModel
    {
        public event EventHandler OnWindowClosed;

        public GraphCreatingViewModel(ILog log, IMainModel model,
            IEnumerable<IGraphAssemble> graphAssembles)
            : base(log, model, graphAssembles)
        {

        }

        public void CreateGraph(object sender, EventArgs e)
        {
            if (CanExecuteConfirmGraphAssembleChoice())
            {
                CreateGraph();
                OnWindowClosed?.Invoke(this, EventArgs.Empty);
                OnWindowClosed = null;
            }
        }

        public void CancelCreateGraph(object sender, EventArgs e)
        {
            OnWindowClosed?.Invoke(this, EventArgs.Empty);
            OnWindowClosed = null;
        }

        private bool CanExecuteConfirmGraphAssembleChoice()
        {
            return SelectedGraphAssemble != null
                && Constants.GraphWidthValueRange.Contains(Width)
                && Constants.GraphLengthValueRange.Contains(Length);
        }
    }
}
