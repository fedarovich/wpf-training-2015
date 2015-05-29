﻿using System.Windows;
using MvvmLightDemo.ViewModel;

namespace MvvmLightDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closed += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}