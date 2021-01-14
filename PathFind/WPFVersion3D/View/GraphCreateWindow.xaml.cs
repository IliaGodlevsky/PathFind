﻿using Common.ValueRanges;
using System.Windows;

namespace WPFVersion3D.View
{
    /// <summary>
    /// Логика взаимодействия для GraphCreateWindow.xaml
    /// </summary>
    public partial class GraphCreateWindow : Window
    {
        public GraphCreateWindow()
        {
            InitializeComponent();
            obstacleSlider.Minimum = Range.ObstaclePercentValueRange.LowerRange;
            obstacleSlider.Maximum = Range.ObstaclePercentValueRange.UpperRange;
        }
    }
}