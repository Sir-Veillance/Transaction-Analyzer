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

namespace Transaction_Analyzer
{
    /// <summary>
    /// Interaction logic for NewDBWindow.xaml
    /// </summary>
    public partial class NewDBWindow : Window
    {
        private static string databaseName;
        public static string DatabaseName
        {
            get { return databaseName; }
        }
        public NewDBWindow()
        {
            InitializeComponent();
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            if (newNameTextBox.Text != "" || newNameTextBox != null)
            {
                databaseName = newNameTextBox.Text.Replace(" ", "_");
            }
            Close();
        }
    }
}
