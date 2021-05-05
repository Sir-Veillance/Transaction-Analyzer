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
        public MainWindow()
        {
            InitializeComponent();
            LoadAvailableDatabases();
        }

        private void LoadAvailableDatabases()
        {
            databaseDropdown.Items.Clear();
            MySqlConnection databasesConnection = new("Server=localhost;port=3306;database=transaction_databases;uid=bmckay;password=!DatabasesUser2");
            databasesConnection.Open();
            string dbquery = "SELECT * FROM dbnames;";
            MySqlCommand getDatabasesCommand = new(dbquery, databasesConnection);
            MySqlDataReader databaseReader = getDatabasesCommand.ExecuteReader();
            while (databaseReader.Read())
            {
                databaseDropdown.Items.Add(databaseReader.GetString("name"));
            }
            databaseReader.Close();
            databasesConnection.Close();
        }

        private void NewDBButtonClick(object sender, RoutedEventArgs e)
        {
            NewDBWindow newDBWindow = new();
            this.Opacity = 0.5;
            newDBWindow.ShowDialog();
            this.Opacity = 1.0;
            databaseName = NewDBWindow.DatabaseName;

            // TODO: Hardcoded login info, could obviously be replaced with a better system
            // MySqlConnection conn = new("Server=localhost;Database=" + databaseName + ";port=3306;User Id=bmckay;password=!DatabasesUser2");
            MySqlConnection conn = new("Server=localhost;port=3306;uid=bmckay;password=!DatabasesUser2");
            conn.Open();
            string query = "SHOW DATABASES LIKE '" + databaseName + "';";
            MySqlCommand existsCommand = new(query, conn);

            // datareader will return a boolean value of whether it found an existing database with the provided name
            MySqlDataReader dataReader = existsCommand.ExecuteReader();
            if (dataReader.Read())
            {
                MessageBox.Show("A database with this name already exists on your MySQL server. If it is not already available in the load menu then the database is being used for some other purpose",
                                                                 "Confirmation",
                                                                 MessageBoxButton.OK,
                                                                 MessageBoxImage.Exclamation);
            } else
            {
                dataReader.Close();
                query = "INSERT INTO transaction_databases.dbnames VALUES ('" + databaseName + "');";
                MySqlCommand databasesInsert = new(query, conn);
                databasesInsert.ExecuteNonQuery();

                // creating fresh database with full schema
                query = "CREATE DATABASE " + databaseName + ";";
                MySqlCommand createCommand = new(query, conn);
                createCommand.ExecuteNonQuery();

                query = "USE " + databaseName + ";";
                MySqlCommand setDatabaseCommand = new(query, conn);
                setDatabaseCommand.ExecuteNonQuery();

                query = "CREATE TABLE Stock_Transaction (" +
                        "TransactionID BIGINT PRIMARY KEY," +
                        "Symbol VARCHAR(5)," +
                        "TransactDate DATE," +
                        "Quantity FLOAT," +
                        "Price FLOAT," +
                        "Type CHAR(1)" +
                        ");";

                MySqlCommand tableCreationCommand = new(query, conn);
                tableCreationCommand.ExecuteNonQuery();

                query = "CREATE TABLE Option_Transaction (" +
                        "TransactionID BIGINT PRIMARY KEY," +
                        "Symbol VARCHAR(5)," +
                        "TransactDate DATE," +
                        "Quantity FLOAT," +
                        "Price FLOAT," +
                        "Type CHAR(1)," +
                        "OptionType CHAR(1)," +
                        "Strike FLOAT," +
                        "Expire DATE" +
                        ");";

                tableCreationCommand = new(query, conn);
                tableCreationCommand.ExecuteNonQuery();

                query = "CREATE TABLE Option_Removal (" +
                        "TransactionID BIGINT PRIMARY KEY," +
                        "Symbol VARCHAR(5)," +
                        "Quantity FLOAT," +
                        "RemovalDate DATE" +
                        ");";

                tableCreationCommand = new(query, conn);
                tableCreationCommand.ExecuteNonQuery();

                query = "CREATE TABLE Company (" +
                        "Symbol VARCHAR(5) PRIMARY KEY," +
                        "Name VARCHAR(50)" +
                        ");";

                tableCreationCommand = new(query, conn);
                tableCreationCommand.ExecuteNonQuery();

                query = "CREATE TABLE Dividend (" +
                        "TransactionID BIGINT PRIMARY KEY," +
                        "Symbol VARCHAR(5)," +
                        "TransactDate DATE," +
                        "Quantity FLOAT" +
                        ");";

                tableCreationCommand = new(query, conn);
                tableCreationCommand.ExecuteNonQuery();

                query = "CREATE TABLE Historical_Price (" +
                        "Symbol VARCHAR(5) PRIMARY KEY," +
                        "Price FLOAT," +
                        "HistoricDate DATE" +
                        ");";

                tableCreationCommand = new(query, conn);
                tableCreationCommand.ExecuteNonQuery();

                AnalyzerMenu menu = new();
                AnalyzerMenu.Conn = conn;
                this.Opacity = 0.5;
                menu.ShowDialog();
                conn.Close();
                LoadAvailableDatabases();
                this.Opacity = 1.0;
            }

            // string query = "SELECT * FROM pc";
            // MySqlCommand cmd = new(query, conn);
            // MySqlDataReader dataReader = cmd.ExecuteReader();
            // dataReader.Read();
            // loadDBButton.Content = dataReader["model"];
        }

        private void LoadDBButtonClick(object sender, RoutedEventArgs e)
        {
            if (databaseDropdown.SelectedItem != null)
            {
                databaseName = databaseDropdown.SelectedItem.ToString();
            } else
            {
                databaseName = null;
            }

            if (databaseName != null)
            {
                MySqlConnection conn = new("Server=localhost;port=3306;database=" + databaseName + ";uid=bmckay;password=!DatabasesUser2");
                conn.Open();
                AnalyzerMenu menu = new();
                AnalyzerMenu.Conn = conn;
                this.Opacity = 0.5;
                menu.ShowDialog();
                conn.Close();
                this.Opacity = 1.0;
            } else
            {
                MessageBox.Show("Please select a database to load from the dropdown menu.",
                                "Confirmation",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
            }
        }
    }
}
