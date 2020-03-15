using Budgeter.FileParsers;
using Budgeter.Wpf.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Budgeter.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TransactionRecordsViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel = new TransactionRecordsViewModel(new CsvParser());
        }

        private async void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ViewModel.GetRecordsAsync(FilePathTxt.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to read the file.\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilePathTxt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox.Text == "File Path")
                txtBox.Text = string.Empty;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == true)
            {
                FilePathTxt.Text = ofd.FileName;
            }
        }

        private void GroupBtn_Click(object sender, RoutedEventArgs e)
        {
            var pieSeriesWnd = new PieChartWindow(ViewModel.TransactionRecordsList);
            pieSeriesWnd.Show();
        }
    }
}
