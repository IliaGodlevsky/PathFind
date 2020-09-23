﻿using GraphLibrary.Enums;
using GraphLibrary.Extensions.SystemTypeExtensions;
using GraphLibrary.ValueRanges;
using System;
using System.Linq;
using System.Windows.Forms;
using WinFormsVersion.ViewModel;

namespace WinFormsVersion.View
{
    internal partial class PathFindingWindow : Form
    {
        public PathFindingViewModel Model { get; set; }
        public PathFindingWindow(PathFindingViewModel model)
        {
            InitializeComponent();

            Model = model;

            okButton.Click += Model.PathFind;
            cancelButton.Click += Model.CancelPathFind;

            var dataSource = Enum.GetValues(typeof(Algorithms)).
                Cast<Algorithms>().
                Select(algo => new { Name = algo.GetDescription(), Value = algo }).
                ToList();
            algorithmListBox.DataSource = dataSource;

            var type = dataSource.First().GetType();
            var properties = type.GetProperties();

            algorithmListBox.DisplayMember = properties.First().Name;
            algorithmListBox.ValueMember = properties.Last().Name;

            var bindingAlgorithm = new Binding(nameof(algorithmListBox.SelectedValue), Model, nameof(Model.Algorithm));
            algorithmListBox.DataBindings.Add(bindingAlgorithm);

            var bindingDelaySliderToDelayTextBox = new Binding(nameof(delaySlider.Value), delayTextBox, nameof(delayTextBox.Text),
                true, DataSourceUpdateMode.OnPropertyChanged);
            delaySlider.DataBindings.Add(bindingDelaySliderToDelayTextBox);

            delaySlider.Minimum = Range.DelayValueRange.LowerRange;
            delaySlider.Maximum = Range.DelayValueRange.UpperRange;

            var bindingDelatTextBoxToModel = new Binding(nameof(delayTextBox.Text), Model, nameof(Model.DelayTime),
                true, DataSourceUpdateMode.OnPropertyChanged);
            delayTextBox.DataBindings.Add(bindingDelatTextBoxToModel);

        }
    }
}