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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Transaction_Analyzer
{
    /// <summary>
    /// Interaction logic for AnalyzerMenu.xaml
    /// </summary>
    public partial class AnalyzerMenu : Window
    {
        public AnalyzerMenu(MySqlConnection passedConnection)
        {
            MySqlConnection conn = passedConnection;
            InitializeComponent();
        }

        private void ImportButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ManualButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ModButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void AnalysisButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ReturnButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
