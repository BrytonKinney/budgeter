using Budgeter.Wpf.Entities;
using Budgeter.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Budgeter.Wpf
{
    /// <summary>
    /// Interaction logic for PieChartWindow.xaml
    /// </summary>
    public partial class PieChartWindow : Window
    {
        public PieChartWindowViewModel ViewModel { get; set; }
        public PieChartWindow(List<TransactionRecord> records)
        {
            InitializeComponent();
            DataContext = ViewModel = new PieChartWindowViewModel(records);
        }
    }
}
