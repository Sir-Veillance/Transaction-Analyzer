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

namespace Transaction_Analyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string databaseName;
        public static string DatabaseName
        {
            get { return databaseName; }
        }
        private static string fileName;
        public static string FileName
        {
            get { return fileName; }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewDBButtonClick(object sender, RoutedEventArgs e)
        {
            NewDBWindow newDBWindow = new();
            newDBWindow.ShowDialog();
            databaseName = NewDBWindow.DatabaseName;
        }

        private void LoadDBButtonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new();
            fileDialog.DefaultExt = ".csv";
            fileDialog.Filter = "CSV Files (*.csv)|*.csv";

            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                fileName = fileDialog.FileName;
                loadDBButton.Content = fileName;
            }
        }
    }
}
