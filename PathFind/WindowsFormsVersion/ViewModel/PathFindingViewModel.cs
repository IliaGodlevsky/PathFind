﻿using Algorithm.AlgorithmCreating;
using Common.Interfaces;
using GraphLib.ViewModel;
using GraphViewModel.Interfaces;
using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsVersion.ViewModel
{
    internal class PathFindingViewModel : PathFindingModel, IViewModel
    {
        public event EventHandler OnWindowClosed;

        public PathFindingViewModel(IMainModel model) : base(model)
        {
            pauseProvider.PauseEvent += () => Application.DoEvents();
        }

        public void PathFind(object sender, EventArgs e)
        {
            if (CanExecuteConfirmPathFindAlgorithmChoice())
            {
                OnWindowClosed?.Invoke(this, new EventArgs());
                FindPath();
            }
        }

        public void CancelPathFind(object sender, EventArgs e)
        {
            OnWindowClosed?.Invoke(this, new EventArgs());
        }

        private bool CanExecuteConfirmPathFindAlgorithmChoice()
        {
            return AlgorithmFactory.AlgorithmsDescriptions.Any(algo => algo == AlgorithmKey);
        }
    }
}