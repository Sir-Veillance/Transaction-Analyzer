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
using MySql.Data.MySqlClient;

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
            this.Opacity = 0.5;
            newDBWindow.ShowDialog();
            this.Opacity = 1.0;
            databaseName = NewDBWindow.DatabaseName;

            // TODO: Hardcoded login info, could obviously be replaced with a better system
            MySqlConnection conn = new("Server=localhost;Database=" + databaseName + ";port=3306;User Id=bmckay;password=!DatabasesUser2");

            AnalyzerMenu menu = new(conn);
            this.Opacity = 0.5;
            menu.ShowDialog();
            this.Opacity = 1.0;

            // string query = "SELECT * FROM pc";
            // MySqlCommand cmd = new(query, conn);
            // MySqlDataReader dataReader = cmd.ExecuteReader();
            // dataReader.Read();
            // loadDBButton.Content = dataReader["model"];
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
