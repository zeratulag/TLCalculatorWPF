﻿using System.ComponentModel;
using System.Windows;
using TLCalculator.ViewModels;

namespace TLCalculator.Views
{
    /// <summary>
    /// DebugWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DebugMainWindow : Window
    {
        private readonly DebugWindowViewModel _VM;

        public DebugMainWindow(DebugWindowViewModel vm)
        {
            _VM = vm;
            InitializeComponent();
            BindViewModels();
        }

        public void BindViewModels()
        {
            TLDebugWindow.DataContext = _VM;
            Expander_FightTimeSummary.DataContext = _VM._MVMs.FightTimeSummaryVM;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;  // cancels the window close    
            this.Hide();      // Programmatically hides the window
        }

    }
}
