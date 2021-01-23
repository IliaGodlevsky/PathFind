﻿using Algorithm.AlgorithmCreating;
using Common.Interfaces;
using GraphLib.ViewModel;
using GraphViewModel.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using WPFVersion3D.Infrastructure;

namespace WPFVersion3D.ViewModel
{
    internal class PathFindingViewModel : PathFindingModel, IViewModel, INotifyPropertyChanged
    {
        public event EventHandler OnWindowClosed;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand ConfirmPathFindAlgorithmChoice { get; }
        public ICommand CancelPathFindAlgorithmChoice { get; }

        public PathFindingViewModel(IMainModel model) : base(model)
        {
            ConfirmPathFindAlgorithmChoice = new RelayCommand(
                ExecuteConfirmPathFindAlgorithmChoice,
                CanExecuteConfirmPathFindAlgorithmChoice);

            CancelPathFindAlgorithmChoice = new RelayCommand(obj => CloseWindow());

            AlgorithmKeys = new ObservableCollection<string>(AlgorithmFactory.AlgorithmsDescriptions);
        }

        protected override void OnAlgorithmIntermitted()
        {
            var frame = new DispatcherFrame();

            var callback = new DispatcherOperationCallback(arg =>
            {
                ((DispatcherFrame)arg).Continue = false;
                return null;
            });

            var priority = DispatcherPriority.Background;

            Dispatcher.CurrentDispatcher.BeginInvoke(priority, callback, frame);
            Dispatcher.PushFrame(frame);
        }

        private void CloseWindow()
        {
            OnWindowClosed?.Invoke(this, new EventArgs());
        }

        private void ExecuteConfirmPathFindAlgorithmChoice(object param)
        {
            CloseWindow();
            base.FindPath();
        }

        private bool CanExecuteConfirmPathFindAlgorithmChoice(object param)
        {
            return AlgorithmFactory.AlgorithmsDescriptions.Any(algo => algo == AlgorithmKey);
        }
    }
}
