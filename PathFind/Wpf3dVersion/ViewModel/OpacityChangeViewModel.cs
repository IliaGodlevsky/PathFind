﻿using Common.Interfaces;
using GraphViewModel.Interfaces;
using System;
using Wpf3dVersion.Infrastructure;
using Wpf3dVersion.Model;

namespace Wpf3dVersion.ViewModel
{
    internal class OpacityChangeViewModel : IModel, IViewModel
    {
        public event EventHandler OnWindowClosed;

        public double ObstacleColorOpacity { get; set; }

        public double VisitedVertexColorOpacity { get; set; }

        public double EnqueuedVertexColorOpacity { get; set; }

        public double PathVertexColorOpacity { get; set; }

        public double SimpleVertexColorOpacity { get; set; }

        public RelayCommand ConfirmOpacityChange { get; }

        public RelayCommand CancelOpacityChange { get; }

        public MainWindowViewModel Model { get; set; }

        public OpacityChangeViewModel(IMainModel model)
        {
            ObstacleColorOpacity = WpfVertex3D.ObstacleVertexBrush.Opacity;
            VisitedVertexColorOpacity = WpfVertex3D.VisitedVertexBrush.Opacity;
            EnqueuedVertexColorOpacity = WpfVertex3D.EnqueuedVertexBrush.Opacity;
            PathVertexColorOpacity = WpfVertex3D.PathVertexBrush.Opacity;
            SimpleVertexColorOpacity = WpfVertex3D.SimpleVertexBrush.Opacity;

            Model = model as MainWindowViewModel;
            ConfirmOpacityChange = new RelayCommand(ExecuteChangeVertexOpacity);
            CancelOpacityChange = new RelayCommand(ExecuteCloseChangeVertexOpacity);
        }

        private void ExecuteChangeVertexOpacity(object param)
        {
            WpfVertex3D.EnqueuedVertexBrush.Opacity = EnqueuedVertexColorOpacity;
            WpfVertex3D.ObstacleVertexBrush.Opacity = ObstacleColorOpacity;
            WpfVertex3D.SimpleVertexBrush.Opacity = SimpleVertexColorOpacity;
            WpfVertex3D.PathVertexBrush.Opacity = PathVertexColorOpacity;
            WpfVertex3D.VisitedVertexBrush.Opacity = VisitedVertexColorOpacity;

            OnWindowClosed?.Invoke(this, new EventArgs());
        }

        private void ExecuteCloseChangeVertexOpacity(object param)
        {
            OnWindowClosed?.Invoke(this, new EventArgs());
        }
    }
}
